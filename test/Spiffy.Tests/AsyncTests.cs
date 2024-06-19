namespace Spiffy.Tests;

using System.Linq;
using System.Threading.Tasks;
using Xunit;

[Collection("TestDb")]
public class DbCommandExtensionsTests(TestDb testDb)
{

    private readonly TestDb _testDb = testDb;

    [Fact]
    public async Task CanExecAsync()
    {
        var descripton = _testDb.GenerateRandomString();

        var sql = @"
            INSERT INTO test_values (description) VALUES (@description);
            SELECT description FROM test_values WHERE description = @description;";
        var param = new DbParams("description", descripton);

        using var conn = _testDb.NewConnection();
        using var cmd = new DbCommandBuilder(conn, sql, param).Build();

        var exists = await cmd.QuerySingleAsync(rd => rd.ReadString("description"));

        Assert.NotNull(exists);
    }

    [Fact]
    public async Task CanExecTranAsync()
    {
        var descripton = _testDb.GenerateRandomString();

        var sql = @"
            INSERT INTO test_values (description) VALUES (@description);
            SELECT description FROM test_values WHERE description = @description;";
        var param = new DbParams("description", descripton);

        using var conn = _testDb.NewConnection();
        using var tran = conn.TryBeginTransaction();
        using var cmd = new DbCommandBuilder(tran, sql, param).Build();

        var exists = await cmd.QuerySingleAsync(rd => rd.ReadString("description"));

        tran.TryCommit();

        Assert.NotNull(exists);
    }

    [Fact]
    public async Task CanExecWithRollbackAsync()
    {
        var descripton = _testDb.GenerateRandomString();

        var sql = "INSERT INTO test_values (description) VALUES (@description);";
        var param = new DbParams("description", descripton);

        using var conn = _testDb.NewConnection();
        using var tran = conn.TryBeginTransaction();
        using var cmd = new DbCommandBuilder(tran, sql, param).Build();
        await cmd.ExecAsync();
        tran.Rollback();

        sql = "SELECT description FROM test_values WHERE description = @description;";
        using var existsCmd = new DbCommandBuilder(conn, sql, param).Build();
        var exists = await existsCmd.QuerySingleAsync(rd => rd.ReadString("description"));
        Assert.Null(exists);
    }

    [Fact]
    public async Task CanScalarAsync()
    {
        var expected = _testDb.GenerateRandomString();

        var sql = "SELECT @description AS description";
        var param = new DbParams("description", expected);

        using var conn = _testDb.NewConnection();
        using var cmd = new DbCommandBuilder(conn, sql, param).Build();

        var result = (await cmd.ScalarAsync()) as string;

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task CanQueryAsync()
    {
        var expected = _testDb.GenerateRandomString();

        var sql = "SELECT @description AS description";
        var param = new DbParams("description", expected);

        using var conn = _testDb.NewConnection();
        using var cmd = new DbCommandBuilder(conn, sql, param).Build();

        var result = await cmd.QueryAsync(rd => rd.ReadString("description"));

        Assert.Equal(expected, result.First());
    }

    [Fact]
    public async Task CanQuerySingleAsync()
    {
        var expected = _testDb.GenerateRandomString();
        var sql = "SELECT @description AS description";
        var param = new DbParams("description", expected);

        using var conn = _testDb.NewConnection();
        using var cmd = new DbCommandBuilder(conn, sql, param).Build();

        var result = await cmd.QuerySingleAsync(rd => rd.ReadString("description"));

        Assert.Equal(expected, result);
    }
}
