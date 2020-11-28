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
            var rowsAffected = batch.Exec(sql, param);
            batch.Commit();
            Assert.Equal(1, rowsAffected);
        }

        [Fact]
        public void CanExecWithRollback()
        {
            var descripton = _testDb.GenerateRandomString();
            var sql = "INSERT INTO test_values (description) VALUES (@description);";
            var param = new DbParams("description", descripton);

            using var conn = _testDb.NewConnection();
            var batch = conn.NewBatch();
            var rowsAffected = batch.Exec(sql, param);
            batch.Rollback();

            var exists = conn.QuerySingle(
                "SELECT description FROM test_values WHERE description = @description;",
                param,
                rd => rd.GetString("description"));

            Assert.Null(exists);
        }

        [Fact]
        public void CanScalar()
        {
            var expected = _testDb.GenerateRandomString();
            var sql = "SELECT @description";
            var param = new DbParams("description", expected);

            using var conn = _testDb.NewConnection();
            var batch = conn.NewBatch();
            var result = batch.Scalar(sql, param);
            batch.Commit();

            Assert.Equal(expected, Convert.ToString(result) ?? "");
        }

        [Fact]
        public void CanQuery()
        {
            var expected = _testDb.GenerateRandomString();
            var sql = "SELECT @description AS description";
            var param = new DbParams("description", expected);

            using var conn = _testDb.NewConnection();
            var batch = conn.NewBatch();
            
            var result = batch.Query(sql, param, rd => rd.GetString("description"));
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
            var result = batch.QuerySingle(sql, param, rd => rd.GetString("description"));
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
            ";

            var now = DateTime.Now;
            var param = new DbParams(){
                { "p_GetString", "spiffy" },
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
                { "p_GetNullableBoolean", null },
                { "p_GetNullableByte", null },
                { "p_GetNullableInt16", null },
                { "p_GetNullableInt32", null },
                { "p_GetNullableInt64", null },
                { "p_GetNullableDecimal", null },
                { "p_GetNullableDouble", null },
                { "p_GetNullableFloat", null },
                { "p_GetNullableGuid", null },
                { "p_GetNullableDateTime", null }
            };

            using var conn = _testDb.NewConnection();
            var result = conn.QuerySingle(sql, param, rd =>
            {
                Assert.Equal("spiffy", rd.GetString("p_GetString"));
                Assert.Equal('s', rd.GetChar("p_GetChar"));
                Assert.True(rd.GetBoolean("p_GetBoolean"));
                Assert.Equal(1, rd.GetByte("p_GetByte"));
                Assert.Equal(1, rd.GetInt16("p_GetInt16"));
                Assert.Equal(1, rd.GetInt32("p_GetInt32"));
                Assert.Equal(1L, rd.GetInt64("p_GetInt64"));
                Assert.Equal(1.0M, rd.GetDecimal("p_GetDecimal"));
                Assert.Equal(1.0, rd.GetDouble("p_GetDouble"));
                Assert.Equal(1.0, rd.GetFloat("p_GetFloat"));
                Assert.Equal(Guid.Empty, rd.GetGuid("p_GetGuid"));
                Assert.Equal(now, rd.GetDateTime("p_GetDateTime"));
                
                Assert.Null(rd.GetNullableBoolean("p_GetNullableBoolean"));
                Assert.Null(rd.GetNullableByte("p_GetNullableByte"));
                Assert.Null(rd.GetNullableInt16("p_GetNullableInt16"));
                Assert.Null(rd.GetNullableInt32("p_GetNullableInt32"));
                Assert.Null(rd.GetNullableInt64("p_GetNullableInt64"));
                Assert.Null(rd.GetNullableDecimal("p_GetNullableDecimal"));
                Assert.Null(rd.GetNullableDouble("p_GetNullableDouble"));
                Assert.Null(rd.GetNullableFloat("p_GetNullableFloat"));
                Assert.Null(rd.GetNullableGuid("p_GetNullableGuid"));
                Assert.Null(rd.GetNullableDateTime("p_GetNullableDateTime"));  
                
                Assert.True(rd.GetNullableBoolean("p_GetNullableBoolean_value"));
                Assert.Equal<byte?>(1, rd.GetNullableByte("p_GetNullableByte_value"));
                Assert.Equal<short?>(1, rd.GetNullableInt16("p_GetNullableInt16_value"));
                Assert.Equal(1, rd.GetNullableInt32("p_GetNullableInt32_value"));
                Assert.Equal(1L, rd.GetNullableInt64("p_GetNullableInt64_value"));
                Assert.Equal(1.0M, rd.GetNullableDecimal("p_GetNullableDecimal_value"));
                Assert.Equal(1.0, rd.GetNullableDouble("p_GetNullableDouble_value"));                
                Assert.Equal(Guid.Empty, rd.GetNullableGuid("p_GetNullableGuid_value"));
                Assert.Equal(now, rd.GetNullableDateTime("p_GetNullableDateTime_value"));

                return 1;
            });

            Assert.Equal(1, result);
        }
    }
}