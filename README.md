# Spiffy - simple data access for .NET

[![NuGet Version](https://img.shields.io/nuget/v/Spiffy.svg)](https://www.nuget.org/packages/Spiffy)
[![build](https://github.com/nhlpa/Spiffy/actions/workflows/build.yml/badge.svg)](https://github.com/nhlpa/Spiffy/actions/workflows/build.yml)


Spiffy is a well-tested library that aims to make working with [ADO.NET](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/ado-net-overview) from C# *a lot* simpler.

The library is delivered as a fluent API for building `IDbCommand` instances, and `IDbCommand` extension methods to support execution. Spiffy **is not an ORM**, encouraging you to take back control of your mappings. However, Spiffy does extend the `IDataReader` interface with several helpers covering most primitive types to make retrieving values safer and more direct.

## Key Features

- Fluent API for build `IDbCommand` instances.
- Simple execution model, delivered as `IDbCommand` extension methods.
- Safe value reading via `IDataReader` [extensions](#idatareader-extension-methods).
- [Enhanced](#exceptions) exception output.
- Asynchronous capabilities.

## Design Goals

- Appear "native", augmenting the base ADO.NET functionality as little as possible and adhering to internal naming conventions.
- Encourage manual mappings by providing a succinct and safe methodology to obtain values from tabular data.
- Provide an easy to reason about execution model.
- Support asynchronous database workloads.

## Getting Started

Install the Spiffy NuGet package:

```
PM>  Install-Package Spiffy
```

Or using the dotnet CLI

```
dotnet add package Spiffy
```

### Quick Start

```csharp
using System;
using System.Data.Sqlite;
using Spiffy;

namespace SpiffyQuickStart
{
  class Program
  {
    const connectionString = "{your connection string}";

    static void Main(string[] args)
    {
      using var connection = new SqliteConnection(connectionString);

      using var cmd = new DbCommandBuilder(connection)
        .CommandText("SELECT author_id, full_name FROM author WHERE author_id = @author_id")
        .DbParams(new DbParams("author_id", 1))
        .Build();

      cmd.Query(cmd, rd =>
        Console.WriteLine("Hello {0}" rd.ReadString("full_name")));
    }
  }
}


```

## An Example using SQLite

For this example, assume we have an `IDbConnection` named `connection`:

```csharp
using var connection = new SqliteConnection("Data Source=hello.db");
```

Consider the following domain model:

```csharp
public class Author
{
  public int AuthorId { get; set; }
  public string FullName { get; set; }

  public static Author FromDataReader (IDataReader rd)
  {
    return new Author() {
      AuthorId = rd.ReadInt32("person_id"),
      FullName = rd.ReadString("full_name")
    }
  }
}
```

### Query for multiple strongly-type results

```csharp
using var cmd = new DbCommandBuilder(connection)
  .CommandText("SELECT author_id, full_name FROM author")
  .Build();

var authors = cmd.Query(Author.FromDataReader);
```

### Query for a single strongly-type result

```csharp
using var cmd = new DbCommandBuilder(connection)
  .CommandText("SELECT author_id, full_name FROM author WHERE author_id = @author_id")
  .DbParams(new DbParams("author_id", authorId))
  .Build();

// This method is optimized to dispose the `IDataReader` after safely reading the first `IDataRecord
var author = cmd.QuerySingle(Author.FromDataReader);
```

### Execute a statement multiple times

```csharp
using var cmd = new DbCommandBuilder(connection)
  .CommandText("INSERT INTO author (full_name) VALUES (@full_name)")
  .Build();

var paramList = authors.Select(a => new DbParams("full_name", a.FullName));
cmd.ExecMany(paramList);
```

### Execute a statement transactionally

```csharp
using var transaction = connection.TryBeginTransaction();

using var cmd = new DbCommandBuilder(tran)
  .CommandText("UPDATE author SET full_name = @full_name where author_id = @author_id")
  .DbParams(new DbParams() {
    { "author_id", author.AuthorId },
    { "full_name", author.FullName }
  })
  .Build();

cmd.Exec();

transaction.TryCommit();
```

### Asynchronously execute a scalar command (single value)

```csharp
using var cmd = new DbCommandBuilder(connection)
  .CommandText("SELECT COUNT(*) AS author_count FROM author")
  .Build();

var count = await cmd.QuerySingleAsync(rd => rd.ReadInt32("author_count"));
```

> Async versions of all data access methods are available: `ExecAsync, ExecManyAsync, QueryAsync, QuerySingleAsync, ReadAsync`

## `IDataReader` Extension Methods

To make obtaining values from reader more straight-forward, 2 sets of extension methods are available for:

- Get value, automatically defaulted
- Get value as Nullable<'a>

Assume we have an active IDataReader called rd and are currently reading a row, the following extension methods are available to simplify reading values:

```csharp
public static string ReadString(this IDataReader rd, string field);
public static char ReadChar(this IDataReader rd, string field);
public static bool ReadBoolean(this IDataReader rd, string field);
public static bool ReadBool(this IDataReader rd, string field);
public static byte ReadByte(this IDataReader rd, string field);
public static short ReadInt16(this IDataReader rd, string field);
public static short ReadShort(this IDataReader rd, string field);
public static int ReadInt32(this IDataReader rd, string field);
public static int ReadInt(this IDataReader rd, string field);
public static int ReadInt(this IDataReader rd, string field);
public static long ReadInt64(this IDataReader rd, string field);
public static long ReadLong(this IDataReader rd, string field);
public static decimal ReadDecimal(this IDataReader rd, string field);
public static double ReadDouble(this IDataReader rd, string field);
public static float ReadFloat(this IDataReader rd, string field);
public static Guid ReadGuid(this IDataReader rd, string field);
public static DateTime ReadDateTime(this IDataReader rd, string field);

public static bool? ReadNullableBoolean(this IDataReader rd, string field);
public static bool? ReadNullableBool(this IDataReader rd, string field);
public static byte? ReadNullableByte(this IDataReader rd, string field);
public static short? ReadNullableInt16(this IDataReader rd, string field);
public static short? ReadNullableShort(this IDataReader rd, string field);
public static int? ReadNullableInt32(this IDataReader rd, string field);
public static int? ReadNullableInt(this IDataReader rd, string field);
public static int? ReadNullableInt(this IDataReader rd, string field);
public static long? ReadNullableInt64(this IDataReader rd, string field);
public static long? ReadNullableLong(this IDataReader rd, string field);
public static decimal? ReadNullableDecimal(this IDataReader rd, string field);
public static double? ReadNullableDouble(this IDataReader rd, string field);
public static float? ReadNullableFloat(this IDataReader rd, string field);
public static Guid? ReadNullableGuid(this IDataReader rd, string field);
public static DateTime? ReadNullableDateTime(this IDataReader rd, string field);
```

## Exceptions

_Docs comming soon_

## Why no automatic mapping?

No matter how you slice it (cached or not) reflection is slow, brittle and hard to debug. As such, the library encourages you to define your mappings manually and aims to help you do this by extending the `IDataReader` interface with helpers to make retrieving values safer and more direct.

## Why "Spiffy"?
It's an homage to [Dapper](https://github.com/StackExchange/Dapper) which was transformative in it's approach to database-bound workloads for .NET.

## Find a bug?

There's an [issue](https://github.com/nhlpa/Spiffy/issues) for that.

## License

Built with ♥ by [NHLPA Engineering](https://github.com/nhlpa) in Toronto, ON. Licensed under [MIT](https://github.com/nhlpa/Spiffy/blob/master/LICENSE).