# Spiffy - simple data access for .NET 

Spiffy is a library that aims to make working with [ADO.NET](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/ado-net-overview) a little bit simpler. 

At it's core is a batch model, which encourages performing database-related work in **units**. It also extends your `IDbConnection` interface to enable a simple API for performing queries that will be automatically batched for you.

Spiffy **is not an ORM**, encouraging you to take back control of your mappings. However, Spiffy does extend the `IDataReader` interface with 2 helpers to make retrieving values safer and more direct.

## Individual queries

### Execute a query and map the results to a strongly typed collection

```csharp
public static async Task<IEnumerable<T>> Query<T>(this IDbConnection conn, string sql, Func<IDataReader, T> map, IDbParams param = null)
```

Example:
```csharp
public class Person
{
    public int PersonId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public static Person FromReader (IDataReader rd)
    {
        return new Person() {
            PersonId = rd.GetValue<int>("person_id"),
            FirstName = rd.GetValue<string>("first_name"),
            LastName = rd.GetValue<string("last_name")
        }
    }
}
IDbConnection db = ... // connection creation code
var people = await db.Query("SELECT person_id, first_name, last_name FROM person", Person.FromReader);
```

To add parameters:
```csharp
// typed Dictionary<string, object>
var param = new DbParams() {
    { "last_name", "smith" }
};
var people = await db.Query("SELECT person_id, first_name, last_name FROM person WHERE last_name = @last_name", Person.FromReader, param);
```

### Execute a query and return **only one** strongly typed object
```csharp
public static async Task<T> Read<T>(this IDbConnection conn, string sql, Func<IDataReader, T> map, IDbParams param = null)
```
Example: 
```csharp
var param = new DbParams() {
    { "person_id", 1 }
};
var person = await db.Read("SELECT person_id first_name, last_name FROM person WHERE person_id = @person_id", Person.FromReader, param);
```

> This method is optimized to dispose the `IDataReader` after safely reading the first `IDataRecord`.

### Execute a query that returns no results
```csharp
public static async Task<int> Exec(this IDbConnection conn, string sql, IDbParams param = null)
```

Example:
```csharp
var param = new DbParams() {
    { "first_name", "John" },
    { "last_name", "Doe" }
};

int recordsAffected = await db.Exec("INSERT INTO person (first_name, last_name) VALUES (@first_name, @last_name)", param);
```

### Execute a query that returns a single value
```csharp
public static async Task<T> Val<T>(this IDbConnection conn, string sql, IDbParams param = null)
```

Example:
```csharp
var param = new DbParams() {
    { "first_name", "John" },
    { "last_name", "Doe" }
};

int personId = await db.Val<int>("INSERT INTO person (first_name, last_name) VALUES (@first_name, @last_name); SELECT SCOPE_IDENTITY();", param);
```

## Batches

The heart and soul of Spiffy is the `IDbBatch`, which provides a simple API for implementing the unit of work pattern. 

> Batches are also ideal for situations which would benefit from reusing resources like, connection and transaction.


```csharp
public static async Task<int> Exec(this IDbBatch batch, string sql, IDbParams param = null) { ... }
public static async Task<T> Val<T>(this IDbBatch batch, string sql, IDbParams param = null) { ... }
public static async Task<IEnumerable<T>> Query<T>(this IDbBatch batch, string sql, Func<IDataReader, T> map, IDbParams param = null) { ... }
public static async Task<T> Read<T>(this IDbBatch batch, string sql, Func<IDataReader, T> map, IDbParams param = null) { ... }
```

Examples:
```csharp
IDbConnection db = ... // connection creation code
var batch = db.BeginBatch();

// ... work in the batch ...

bool failed = //something error'd or didn't go according to plan

if(failed){
    batch.Rollback();
}

batch.Commit();
```

On commit batches will automatically take care of cleaning up all volatile resources (`IDbConnection`, `IDbTransaction`). So `Commit()` should be seen as a terminal command.

## Why no automatic mapping?

No matter how you slice it (cached or not) reflection is slow, brittle and hard to debug. As such, the library encourages you to define your mappings manually and aims to help you do this by extending the `IDataReader` interface with 2 helpers (`GetValue<T>()` & `GetNullableValue<T>()`) to make retrieving values safer and more direct.

## Find a bug?

There's an [issue](https://github.com/pimbrouwers/Spiffy/issues) for that.

## License

Built with ♥ by [Pim Brouwers](https://github.com/pimbrouwers) in Toronto, ON. Licensed under [GNU](https://github.com/pimbrouwers/Spiffy/blob/master/LICENSE).