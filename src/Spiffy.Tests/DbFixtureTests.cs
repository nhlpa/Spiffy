using System;
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
            _db = new DbFixture<TestDbConnectionFactory>(_testDb._connectionFactory);
        }

        [Fact]
        public void CanCreateNewBatch()
        {
            var unit = _db.NewBatch();
            Assert.NotNull(unit);
            unit.Rollback();
        }
    }
}