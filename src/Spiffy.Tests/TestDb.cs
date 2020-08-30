using System.Data;
using System.Data.SQLite;
using System.IO;
using Xunit;

namespace Spiffy.Tests
{
    [CollectionDefinition("TestDb")]
    public class TestDbCollection : ICollectionFixture<TestDb>
    {
    }

    public class TestDb
    {
        public readonly TestDbConnectionFactory _connectionFactory;

        public TestDb()
        {
            var connFact = new TestDbConnectionFactory();
            var db = new DbFixture<TestDbConnectionFactory>(connFact);
            
            using var fs = File.OpenRead("schema.sql");
            using var sr = new StreamReader(fs);
            var sql = sr.ReadToEnd();
            db.Exec(sql);

            _connectionFactory = connFact;
        }        
    }

    public class TestDbConnectionFactory : IDbConnectionFactory
    {
        private const string _connectionString = "Data Source=:memory:;Version=3;New=true;";
        public IDbConnection NewConnection() => new SQLiteConnection(_connectionString);
    }
}
