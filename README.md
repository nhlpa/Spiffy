# Spiffy - simple data access for .NET 

[![NuGet Version](https://img.shields.io/nuget/v/Spiffy.svg)](https://www.nuget.org/packages/Spiffy)
[![Build Status](https://travis-ci.org/pimbrouwers/Spiffy.svg?branch=master)](https://travis-ci.org/pimbrouwers/Spiffy)

Spiffy is a well-tested library that aims to make working with [ADO.NET](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/ado-net-overview) a lot simpler. 

At it's core is a batch model, which encourages performing database-related work in **units**. It also extends your `IDbConnection` interface to enable a simple API for performing queries that will be automatically batched for you.

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
        static void Main(string[] args)
        {
            var connectionString = "{your connection string}";
            using var connection = new SqliteConnection(connectionString);

            var sql = @"
            SELECT  author_id
                  , full_name 
            FROM    author 
            WHERE   author_id = @author_id";

            var param = new DbParams("author_id", 1)
            
            connection.Query(sql, param, rd => {
                Console.WriteLine("Hello {0}" rd.GetString("full_name"));
            })
        }
    }
}


```

## An Example using SQLite

For this example, assume we have an `IDbConnection` named `connection`:

```csharp
var connection = new SqliteConnection("Data Source=hello.db");
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
            AuthorId = rd.GetInt32("person_id"),
            FullName = rd.GetString("full_name")
        }
    }
}
```

### Query for multiple strongly-type results

```csharp
var sql = "SELECT author_id, full_name FROM author";
var authors = connection.Query(sql, Author.FromDataReader);
```

### Query for a single strongly-type result

```csharp
var sql = "SELECT author_id, full_name FROM author WHERE author_id = @author_id";
var param = new DbParams("author_id", authorId)
// This method is optimized to dispose the `IDataReader` after safely reading the first `IDataRecord
var author = connection.QuerySingle(sql, param, Author.FromDataReader);
```

### Execute a statement multiple times

```csharp
var sql = "INSERT INTO author (full_name)";
var paramList = authors.Select(a => new DbParams("full_name", a.FullName));
connection.ExecMany(sql, paramList);
```

### Execute a statement transactionally

```csharp
var batch = connection.BeginBatch();
var sql = "UPDATE author SET full_name = @full_name where author_id = @author_id";
var param = new DbParams() {
    { "author_id", author.AuthorId },
    { "full_name", author.FullName }
}
batch.Exec(sql, param);
batch.Commit();
```

> The `IDbBatch` facilitates the unit-of-work programming model.

### Asynchronously execute a scalar command (single value)

```csharp
var sql = "SELECT COUNT(*) FROM author";
var countObj = _db.ScalarAsync(sql);                
var count = Convert.ToInt32(countObj);
```

> Async versions of all data access methods are available: `ExecAsync, ExecManyAsync, ScalarAsync, QueryAsync, QuerySingleAsync, ReadAsync`

## Batches

The heart and soul of Spiffy is the `IDbBatch`, which provides a simple API for implementing the unit of work pattern and are also ideal for situations which would benefit from reusing resources like, connection and transaction.

```csharp
IDbConnection conn = ... // connection creation code
var batch = connection.BeginBatch();

// ... work in the batch ...

bool failed = //something error'd or didn't go according to plan

if(failed){
    batch.Rollback();
}

batch.Commit();
```

On commit batches will automatically take care of cleaning up all volatile resources (`IDbConnection`, `IDbTransaction`). So `Commit()` should be seen as a terminal command.

## `IDataReader` Extension Methods

To make obtaining values from reader more straight-forward, 2 sets of extension methods are available for:

- Get value, automatically defaulted
- Get value as Nullable<'a>

Assume we have an active IDataReader called rd and are currently reading a row, the following extension methods are available to simplify reading values:

```csharp
public static string GetString(this IDataReader rd, string field) = // ...
public static char GetChar(this IDataReader rd, string field) = // ...
public static bool GetBoolean(this IDataReader rd, string field) = // ...
public static byte GetByte(this IDataReader rd, string field) = // ...
public static short GetInt16(this IDataReader rd, string field) = // ...
public static short GetShort(this IDataReader rd, string field) = // ...
public static int GetInt32(this IDataReader rd, string field) = // ...
public static long GetInt64(this IDataReader rd, string field) = // ...
public static long GetLong(this IDataReader rd, string field) = // ...
public static decimal GetDecimal(this IDataReader rd, string field) = // ...
public static double GetDouble(this IDataReader rd, string field) = // ...
public static float GetFloat(this IDataReader rd, string field) = // ...
public static Guid GetGuid(this IDataReader rd, string field) = // ...
public static DateTime GetDateTime(this IDataReader rd, string field) = // ...

public static bool? GetNullableBoolean(this IDataReader rd, string field) = // ...
public static byte? GetNullableByte(this IDataReader rd, string field) = // ...
public static short? GetNullableInt16(this IDataReader rd, string field) = // ...
public static short? GetNullableShort(this IDataReader rd, string field) = // ...
public static int? GetNullableInt32(this IDataReader rd, string field) = // ...
public static long? GetNullableInt64(this IDataReader rd, string field) = // ...
public static long? GetNullableLong(this IDataReader rd, string field) = // ...
public static decimal? GetNullableDecimal(this IDataReader rd, string field) = // ...
public static double? GetNullableDouble(this IDataReader rd, string field) = // ...
public static float? GetNullableFloat(this IDataReader rd, string field) = // ...
public static Guid? GetNullableGuid(this IDataReader rd, string field) = // ...
public static DateTime? GetNullableDateTime(this IDataReader rd, string field) = // ...
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
