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

    public class TestDb : IDisposable
    {
        private const string _connectionString = "Data Source=.\\testdb.db;Version=3;New=true;";
        
        public TestDb()
        {            
            using var conn = NewConnection();

            var cmdBuilder = new DbCommandBuilder(conn);

            using var fs = File.OpenRead("schema.sql");
            using var sr = new StreamReader(fs);
            var sql = sr.ReadToEnd();

            using var cmd = cmdBuilder.CommandText(sql).Build();
            cmd.Exec();
        }

        public Func<IDbConnection> NewConnection => () => new SQLiteConnection(_connectionString);

        public string GenerateRandomString() => Path.GetRandomFileName().Replace(".", "");

        public void Dispose() => File.Delete("test.db");
    }
}
