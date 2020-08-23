using System.Data;
using System.Data.SQLite;
using System.IO;
using Xunit;

namespace Spiffy.Tests
{
    [CollectionDefinition("DbFixture")]
    internal class DbCollection : ICollectionFixture<TestDbFixture>
    {
        //private readonly IDbContext<TestDbFixture> _db;

        //internal DbCollection()
        //{
        //    _db = new DbContext<TestDbFixture>(new TestDbFixture());
        //    Setup();
        //}

        //private void Setup(){
        //    using var fs = File.OpenRead("schema.sql");
        //    using var sr = new StreamReader(fs);
        //    var sql = sr.ReadToEnd();
        //    _db.Exec(sql);
        //}
    }

    
}
