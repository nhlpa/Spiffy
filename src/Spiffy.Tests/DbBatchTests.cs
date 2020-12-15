using System;
using System.Linq;
using Xunit;

namespace Spiffy.Tests
{
    [Collection("TestDb")]
    public class DbBatchTests
    {
        private readonly TestDb _testDb;

        public DbBatchTests(TestDb testDb)
        {
            _testDb = testDb;
        }

        [Fact]
        public void CanExec()
        {
            var descripton = _testDb.GenerateRandomString();
            var sql = "INSERT INTO test_values (description) VALUES (@description);";
            var param = new DbParams("description", descripton);

            using var conn = _testDb.NewConnection();
            var batch = conn.NewBatch();
            batch.Exec(sql, param);
            var exists = batch.QuerySingleAsync(
                "SELECT description FROM test_values WHERE description = @description;",
                param,
                rd => rd.ReadString("description"));
            batch.Commit();

            Assert.NotNull(exists);
        }

        [Fact]
        public void CanExecWithRollback()
        {
            var descripton = _testDb.GenerateRandomString();
            var sql = "INSERT INTO test_values (description) VALUES (@description);";
            var param = new DbParams("description", descripton);

            using var conn = _testDb.NewConnection();
            var batch = conn.NewBatch();
            batch.Exec(sql, param);
            batch.Rollback();

            var exists = conn.QuerySingle(
                "SELECT description FROM test_values WHERE description = @description;",
                param,
                rd => rd.ReadString("description"));

            Assert.Null(exists);
        }

        [Fact]
        public void CanQuery()
        {
            var expected = _testDb.GenerateRandomString();
            var sql = "SELECT @description AS description";
            var param = new DbParams("description", expected);

            using var conn = _testDb.NewConnection();
            var batch = conn.NewBatch();
            
            var result = batch.Query(sql, param, rd => rd.ReadString("description"));
            batch.Commit();

            Assert.Equal(expected, result.First());
        }

        [Fact]
        public void CanQuerySingle()
        {
            var expected = _testDb.GenerateRandomString();
            var sql = "SELECT @description AS description";
            var param = new DbParams("description", expected);

            using var conn = _testDb.NewConnection();
            var batch = conn.NewBatch();
            var result = batch.QuerySingle(sql, param, rd => rd.ReadString("description"));
            batch.Commit();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SelectAllTypes()
        {
            var sql = @"
            SELECT  @p_GetString AS p_GetString
                  , @p_GetChar AS p_GetChar
                  , @p_GetBoolean AS p_GetBoolean
                  , @p_GetByte AS p_GetByte
                  , @p_GetInt16 AS p_GetInt16
                  , @p_GetInt32 AS p_GetInt32
                  , @p_GetInt64 AS p_GetInt64
                  , @p_GetDecimal AS p_GetDecimal
                  , @p_GetDouble AS p_GetDouble
                  , @p_GetFloat AS p_GetFloat
                  , @p_GetGuid AS p_GetGuid
                  , @p_GetDateTime AS p_GetDateTime
                  , @p_GetBytes AS p_GetBytes
                  , @p_GetNullableBoolean AS p_GetNullableBoolean
                  , @p_GetNullableByte AS p_GetNullableByte
                  , @p_GetNullableInt16 AS p_GetNullableInt16
                  , @p_GetNullableInt32 AS p_GetNullableInt32
                  , @p_GetNullableInt64 AS p_GetNullableInt64
                  , @p_GetNullableDecimal AS p_GetNullableDecimal
                  , @p_GetNullableDouble AS p_GetNullableDouble
                  , @p_GetNullableFloat AS p_GetNullableFloat
                  , @p_GetNullableGuid AS p_GetNullableGuid
                  , @p_GetNullableDateTime AS p_GetNullableDateTime
                  , @p_GetNullableBytes AS p_GetNullableBytes
                  , @p_GetString AS p_GetNullableString_value
                  , @p_GetChar AS p_GetNullableChar_value
                  , @p_GetBoolean AS p_GetNullableBoolean_value
                  , @p_GetByte AS p_GetNullableByte_value
                  , @p_GetInt16 AS p_GetNullableInt16_value
                  , @p_GetInt32 AS p_GetNullableInt32_value
                  , @p_GetInt64 AS p_GetNullableInt64_value
                  , @p_GetDecimal AS p_GetNullableDecimal_value
                  , @p_GetDouble AS p_GetNullableDouble_value
                  , @p_GetFloat AS p_GetNullableFloat_value
                  , @p_GetGuid AS p_GetNullableGuid_value
                  , @p_GetDateTime AS p_GetNullableDateTime_value
                  , @p_GetBytes AS p_GetNullableBytes_value
            ";

            var now = DateTime.Now;
            var param = new DbParams(){
                { "p_GetString", "Nhlpa.Sql" },
                { "p_GetChar", 's' },
                { "p_GetBoolean", true },
                { "p_GetByte", 1 },
                { "p_GetInt16", 1 },
                { "p_GetInt32", 1 },
                { "p_GetInt64", 1L },
                { "p_GetDecimal", 1.0M },
                { "p_GetDouble", 1.0 },
                { "p_GetFloat", 1.0 },
                { "p_GetGuid", Guid.Empty },
                { "p_GetDateTime", now },
                { "p_GetBytes", new byte[]{ } },
                { "p_GetNullableBoolean", null },
                { "p_GetNullableByte", null },
                { "p_GetNullableInt16", null },
                { "p_GetNullableInt32", null },
                { "p_GetNullableInt64", null },
                { "p_GetNullableDecimal", null },
                { "p_GetNullableDouble", null },
                { "p_GetNullableFloat", null },
                { "p_GetNullableGuid", null },
                { "p_GetNullableDateTime", null },
                { "p_GetNullableBytes", null }
            };

            using var conn = _testDb.NewConnection();
            var result = conn.QuerySingle(sql, param, rd =>
            {
                Assert.Equal("Nhlpa.Sql", rd.ReadString("p_GetString"));
                Assert.Equal('s', rd.ReadChar("p_GetChar"));
                Assert.True(rd.ReadBoolean("p_GetBoolean"));
                Assert.Equal(1, rd.ReadByte("p_GetByte"));
                Assert.Equal(1, rd.ReadInt16("p_GetInt16"));
                Assert.Equal(1, rd.ReadInt32("p_GetInt32"));
                Assert.Equal(1L, rd.ReadInt64("p_GetInt64"));
                Assert.Equal(1.0M, rd.ReadDecimal("p_GetDecimal"));
                Assert.Equal(1.0, rd.ReadDouble("p_GetDouble"));
                Assert.Equal(1.0, rd.ReadFloat("p_GetFloat"));
                Assert.Equal(Guid.Empty, rd.ReadGuid("p_GetGuid"));
                Assert.Equal(now, rd.ReadDateTime("p_GetDateTime"));
                Assert.Equal(new byte[] { }, rd.ReadBytes("p_GetBytes"));

                Assert.Null(rd.ReadNullableBoolean("p_GetNullableBoolean"));
                Assert.Null(rd.ReadNullableByte("p_GetNullableByte"));
                Assert.Null(rd.ReadNullableInt16("p_GetNullableInt16"));
                Assert.Null(rd.ReadNullableInt32("p_GetNullableInt32"));
                Assert.Null(rd.ReadNullableInt64("p_GetNullableInt64"));
                Assert.Null(rd.ReadNullableDecimal("p_GetNullableDecimal"));
                Assert.Null(rd.ReadNullableDouble("p_GetNullableDouble"));
                Assert.Null(rd.ReadNullableFloat("p_GetNullableFloat"));
                Assert.Null(rd.ReadNullableGuid("p_GetNullableGuid"));
                Assert.Null(rd.ReadNullableDateTime("p_GetNullableDateTime"));
                Assert.Null(rd.ReadBytes("p_GetNullableBytes"));

                Assert.True(rd.ReadNullableBoolean("p_GetNullableBoolean_value"));
                Assert.Equal<byte?>(1, rd.ReadNullableByte("p_GetNullableByte_value"));
                Assert.Equal<short?>(1, rd.ReadNullableInt16("p_GetNullableInt16_value"));
                Assert.Equal(1, rd.ReadNullableInt32("p_GetNullableInt32_value"));
                Assert.Equal(1L, rd.ReadNullableInt64("p_GetNullableInt64_value"));
                Assert.Equal(1.0M, rd.ReadNullableDecimal("p_GetNullableDecimal_value"));
                Assert.Equal(1.0, rd.ReadNullableDouble("p_GetNullableDouble_value"));
                Assert.Equal(Guid.Empty, rd.ReadNullableGuid("p_GetNullableGuid_value"));
                Assert.Equal(now, rd.ReadNullableDateTime("p_GetNullableDateTime_value"));
                Assert.Equal(new byte[] { }, rd.ReadBytes("p_GetNullableBytes_value"));

                return 1;
            });

            Assert.Equal(1, result);
        }
    }
}