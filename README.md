# Spiffy - simple data access for .NET 

[![NuGet Version](https://img.shields.io/nuget/v/Spiffy.svg)](https://www.nuget.org/packages/Spiffy)
[![Build Status](https://travis-ci.org/pimbrouwers/Spiffy.svg?branch=master)](https://travis-ci.org/pimbrouwers/Spiffy)

Spiffy is a well-tested library that aims to make working with [ADO.NET](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/ado-net-overview) a lot simpler. 

It extends `IDbTransaction` to provide a basic "batch" model, which encourages performing database-related work in **units**. It also extends your `IDbConnection` interface to enable a simple API for performing queries that will be automatically batched for you.

Spiffy **is not an ORM**, encouraging you to take back control of your mappings. However, Spiffy does extend the `IDataReader` interface with several helpers covering most primitive types to make retrieving values safer and more direct.

## Key Features

- [Batch](#batches) model, to enable unit of work pattern.
- Safe value reading via `IDataReader` [extensions](#idatareader-extension-methods).
- Enhanced exception output.
- Asynchronous capabilities.

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
using Microsoft.Data.SQLite;
using Spiffy;

namespace SpiffyQuickStart
{
  class Program
  {
    const connectionString = "{your connection string}";

    static void Main(string[] args)
    {            
      var sql = "SELECT author_id, full_name FROM author WHERE author_id = @author_id";
      var param = new DbParams("author_id", 1);

      using var connection = new SqliteConnection(connectionString);            
      using var cmd = new DbCommandBuilder(connection, sql, param).Build();
            
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
var sql = "SELECT author_id, full_name FROM author";

using var cmd = new DbCommandBuilder(connection, sql).Build();

var authors = cmd.Query(cmd, Author.FromDataReader);
```

### Query for a single strongly-type result

```csharp
var sql = "SELECT author_id, full_name FROM author WHERE author_id = @author_id";

var param = new DbParams("author_id", authorId);

using var cmd = new DbCommandBuilder(connection, sql, param).Build();

// This method is optimized to dispose the `IDataReader` after safely reading the first `IDataRecord
var author = connection.QuerySingle(cmd, Author.FromDataReader);
```

### Execute a statement multiple times

```csharp
var sql = "INSERT INTO author (full_name) VALUES (@full_name)";

var paramList = authors.Select(a => new DbParams("full_name", a.FullName));

using var cmd = new DbCommandBuilder(connection, sql).Build();

connection.ExecMany(cmd, paramList);
```

### Execute a statement transactionally

```csharp
var sql = "UPDATE author SET full_name = @full_name where author_id = @author_id";

var param = new DbParams() {
    { "author_id", author.AuthorId },
    { "full_name", author.FullName }
};

using var transaction = connection.TryBeginTransaction();

using var cmd = new DbCommandBuilder(tran, sql, param).Build();

transaction.Exec(cmd);

transaction.TryCommit();
```

### Asynchronously execute a scalar command (single value)

```csharp
var sql = "SELECT COUNT(*) AS author_count FROM author";

using var cmd = new DbCommandBuilder(connection, sql).Build();

var count = await cmd.QuerySingleAsync(rd => rd.ReadInt32("author_count"));
```

> Async versions of all data access methods are available: `ExecAsync, ExecManyAsync, QueryAsync, QuerySingleAsync, ReadAsync`

## `IDataReader` Extension Methods

To make obtaining values from reader more straight-forward, 2 sets of extension methods are available for:

- Get value, automatically defaulted
- Get value as Nullable<'a>

Assume we have an active IDataReader called rd and are currently reading a row, the following extension methods are available to simplify reading values:

```csharp
public static string ReadString(this IDataReader rd, string field) = // ...
public static char ReadChar(this IDataReader rd, string field) = // ...
public static bool ReadBoolean(this IDataReader rd, string field) = // ...
public static byte ReadByte(this IDataReader rd, string field) = // ...
public static short ReadInt16(this IDataReader rd, string field) = // ...
public static short ReadShort(this IDataReader rd, string field) = // ...
public static int ReadInt32(this IDataReader rd, string field) = // ...
public static int ReadInt(this IDataReader rd, string field) = // ...
public static long ReadInt64(this IDataReader rd, string field) = // ...
public static long ReadLong(this IDataReader rd, string field) = // ...
public static decimal ReadDecimal(this IDataReader rd, string field) = // ...
public static double ReadDouble(this IDataReader rd, string field) = // ...
public static float ReadFloat(this IDataReader rd, string field) = // ...
public static Guid ReadGuid(this IDataReader rd, string field) = // ...
public static DateTime ReadDateTime(this IDataReader rd, string field) = // ...

public static bool? ReadNullableBoolean(this IDataReader rd, string field) = // ...
public static byte? ReadNullableByte(this IDataReader rd, string field) = // ...
public static short? ReadNullableInt16(this IDataReader rd, string field) = // ...
public static short? ReadNullableShort(this IDataReader rd, string field) = // ...
public static int? ReadNullableInt32(this IDataReader rd, string field) = // ...
public static int? ReadNullableInt(this IDataReader rd, string field) = // ...
public static long? ReadNullableInt64(this IDataReader rd, string field) = // ...
public static long? ReadNullableLong(this IDataReader rd, string field) = // ...
public static decimal? ReadNullableDecimal(this IDataReader rd, string field) = // ...
public static double? ReadNullableDouble(this IDataReader rd, string field) = // ...
public static float? ReadNullableFloat(this IDataReader rd, string field) = // ...
public static Guid? ReadNullableGuid(this IDataReader rd, string field) = // ...
public static DateTime? ReadNullableDateTime(this IDataReader rd, string field) = // ...
```

## Errors

_Docs comming soon_

## Why no automatic mapping?

No matter how you slice it (cached or not) reflection is slow, brittle and hard to debug. As such, the library encourages you to define your mappings manually and aims to help you do this by extending the `IDataReader` interface with helpers to make retrieving values safer and more direct.

## Why "Spiffy"?
It's an homage to [Dapper](https://github.com/StackExchange/Dapper) which was transformative in it's approach to database-bound workloads for .NET.

## Find a bug?

There's an [issue](https://github.com/pimbrouwers/Spiffy/issues) for that.

## License

Built with ♥ by [Pim Brouwers](https://github.com/pimbrouwers) in Toronto, ON. Licensed under [Apache License 2.0](https://github.com/pimbrouwers/Spiffy/blob/master/LICENSE).
