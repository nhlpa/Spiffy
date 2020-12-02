using System;
using System.Linq;
using Xunit;

namespace Spiffy.Tests
{
    [Collection("TestDb")]
    public class IDbConnectionExtensionTests
    {
        private readonly TestDb _testDb;
        
        public IDbConnectionExtensionTests(TestDb testDb)
        {
            _testDb = testDb;            
        }

        [Fact]
        public void CanCreateNewBatch()
        {
            using var conn = _testDb.NewConnection();
            var batch = conn.NewBatch();
            Assert.NotNull(batch);
            batch.Rollback();
        }

        [Fact]
        public void CanExec() 
        {
            var descripton = _testDb.GenerateRandomString();
            var sql = "INSERT INTO test_values (description) VALUES (@description);";
            var param = new DbParams("description", descripton);
            
            using var conn = _testDb.NewConnection();
            var rowsAffected = conn.Exec(sql, param);
            Assert.Equal(1, rowsAffected);
        }

        [Fact]
        public void CanScalar() {
            var expected = _testDb.GenerateRandomString();
            var sql = "SELECT @description";
            var param = new DbParams("description", expected);
            using var conn = _testDb.NewConnection();
            var result = conn.Scalar(sql, param);

            Assert.Equal(expected, Convert.ToString(result) ?? "");
        }

        [Fact]
        public void CanQuery() {
            var expected = _testDb.GenerateRandomString();
            var sql = "SELECT @description AS description";
            var param = new DbParams("description", expected);
            using var conn = _testDb.NewConnection();
            var result = conn.Query(sql, param, rd => rd.ReadString("description"));

            Assert.Equal(expected, result.First());
        }

        [Fact]
        public void CanQuerySingle()
        {
            var expected = _testDb.GenerateRandomString();
            var sql = "SELECT @description AS description";
            var param = new DbParams("description", expected);
            using var conn = _testDb.NewConnection();
            var result = conn.QuerySingle(sql, param, rd => rd.ReadString("description"));

            Assert.Equal(expected, result);
        }
    }
}