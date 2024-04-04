namespace Spiffy.Tests;

using System;
using System.Data;
using Microsoft.Data.Sqlite;
using System.IO;
using Xunit;

[CollectionDefinition("TestDb")]
public class TestDbCollection : ICollectionFixture<TestDb>
{
}

public class TestDb
{
  private const string _dbName = "Spiffy.Tests.db";
  private const string _connectionString = $"Data Source={_dbName}";

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

  public Func<IDbConnection> NewConnection => () => new SqliteConnection(_connectionString);

  public string GenerateRandomString() => Path.GetRandomFileName().Replace(".", "");
}
