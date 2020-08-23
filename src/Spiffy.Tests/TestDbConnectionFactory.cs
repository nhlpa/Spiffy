using System.Data;
using System.Data.SQLite;
using System.IO;
using Xunit;

namespace Spiffy.Tests
{
    internal class TestDbConnectionFactory : IDbConnectionFactory
    {
        private const string _connectionString = "Data Source=:memory:;Version=3;New=true;";

        public IDbConnection NewConnection() =>
            new SQLiteConnection(_connectionString);
    }
}
