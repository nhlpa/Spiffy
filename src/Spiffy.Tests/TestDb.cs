using System;
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

    public class TestDb //: IDisposable
    {
        public readonly DbFixture<TestDbConnectionFactory> Db;

        public TestDb()
        {
            var connFact = new TestDbConnectionFactory();
            var db = new DbFixture<TestDbConnectionFactory>(connFact);

            using var fs = File.OpenRead("schema.sql");
            using var sr = new StreamReader(fs);
            var sql = sr.ReadToEnd();
            db.Exec(sql);

            Db = db;
        }

        public string GenerateRandomString() => Path.GetRandomFileName().Replace(".", "");

        //public void Dispose() => File.Delete("test.db");
    }

    public class TestDbConnectionFactory : IDbConnectionFactory
    {
        private const string _connectionString = "Data Source=.\\testdb.db;Version=3;New=true;";
        public IDbConnection NewConnection() => new SQLiteConnection(_connectionString);
    }
}
