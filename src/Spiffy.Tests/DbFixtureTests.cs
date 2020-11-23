using System;
using System.Linq;
using Xunit;

namespace Spiffy.Tests
{
    [Collection("TestDb")]
    public class DbFixtureTests
    {
        private readonly TestDb _testDb;
        private readonly DbFixture<TestDbConnectionFactory> _db;

        public DbFixtureTests(TestDb testDb)
        {
            _testDb = testDb;
            _db = testDb.Db;
        }

        [Fact]
        public void CanCreateNewBatch()
        {
            var batch = _db.NewBatch();
            Assert.NotNull(batch);
            batch.Rollback();
        }

        [Fact]
        public void CanExec() 
        {
            var descripton = _testDb.GenerateRandomString();
            var sql = "INSERT INTO test_values (description) VALUES (@description);";
            var param = new DbParams("description", descripton);

            var rowsAffected = _db.Exec(sql, param);
            Assert.Equal(1, rowsAffected);
        }

        [Fact]
        public void CanScalar() {
            var expected = _testDb.GenerateRandomString();
            var sql = "SELECT @description";
            var param = new DbParams("description", expected);
            var result = _db.Scalar(sql, param);

            Assert.Equal(expected, Convert.ToString(result) ?? "");
        }

        [Fact]
        public void CanQuery() {
            var expected = _testDb.GenerateRandomString();
            var sql = "SELECT @description AS description";
            var param = new DbParams("description", expected);
            var result = _db.Query(sql, param, rd => rd.GetString("description"));

            Assert.Equal(expected, result.First());
        }

        [Fact]
        public void CanQuerySingle()
        {
            var expected = _testDb.GenerateRandomString();
            var sql = "SELECT @description AS description";
            var param = new DbParams("description", expected);
            var result = _db.QuerySingle(sql, param, rd => rd.GetString("description"));

            Assert.Equal(expected, result);
        }
    }
}