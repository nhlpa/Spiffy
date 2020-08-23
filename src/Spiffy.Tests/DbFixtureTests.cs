using System;
using Xunit;

namespace Spiffy.Tests
{
    public class DbFixtureTests
    {
        private readonly TestDbConnectionFactory _connectionFactory;

        public DbFixtureTests()
        {
            _connectionFactory = new TestDbConnectionFactory();
        }
    }
}