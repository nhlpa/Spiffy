namespace Spiffy;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

/// <summary>
/// Database unit of work.
/// </summary>
/// <remarks>
/// Constitute a new DbBatch from IDbConnection and IDbTransaction
/// </remarks>
/// <param name="connection"></param>
/// <param name="transaction"></param>
public class DbBatch(IDbConnection connection, IDbTransaction transaction) : IDbBatch
{
    private readonly IDbConnection _connection = connection ?? throw new ArgumentNullException(nameof(connection));
    private readonly IDbTransaction _transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));

    /// <summary>
    /// Commit unit of work and cleanup.
    /// Rolls back transaction and throws FailedCommitBatchException on failure
    /// </summary>
    public void Commit()
    {
        try
        {
            _transaction.Commit();
        }
        catch (Exception ex)
        {
            Rollback();
            throw new FailedCommitBatchException(ex);
        }
        finally
        {
            Dispose();
        }
    }

    /// <summary>
    /// Rollback unit of work and cleanup.
    /// </summary>
    public void Rollback()
    {
        try
        {
            _transaction.Rollback();
        }
        finally
        {
            Dispose();
        }
    }

    /// <summary>
    /// Create a new IDbBatch, which represents a database unit of work.
    /// </summary>
    /// <returns></returns>
    public IDbBatch NewBatch()
    {
        var transaction = _connection.TryBeginTransaction();
        return new DbBatch(_connection, transaction);
    }

    /// <summary>
    /// Execute parameterized query.
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public void Exec(string sql, DbParams? param = null) =>
        Do(sql, param, cmd => cmd.Exec());

    /// <summary>
    /// Execute parameterized query.
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="paramList"></param>
    /// <returns></returns>
    public void ExecMany(string sql, IEnumerable<DbParams> paramList) =>
        DoMany(sql, paramList, cmd => cmd.Exec());

    /// <summary>
    /// Execute parameterized query and return single-value.
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public object? Scalar(string sql, DbParams? param = null) =>
        Do(sql, param, cmd => cmd.Scalar());

    /// <summary>
    /// Execute parameterized query, enumerate all records and apply mapping.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sql"></param>
    /// <param name="map"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public IEnumerable<T> Query<T>(string sql, DbParams param, Func<IDataReader, T> map) =>
        Do(sql, param, cmd => cmd.Query(map));

    /// <summary>
    /// Execute query, enumerate all records and apply mapping.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sql"></param>
    /// <param name="map"></param>
    /// <returns></returns>
    public IEnumerable<T> Query<T>(string sql, Func<IDataReader, T> map) =>
        Query(sql, new DbParams(), map);

    /// <summary>
    /// Execute paramterized query, read only first record and apply mapping.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sql"></param>
    /// <param name="map"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public T? QuerySingle<T>(string sql, DbParams param, Func<IDataReader, T> map) =>
        Do(sql, param, cmd => cmd.QuerySingle(map));

    /// <summary>
    /// Execute query, read only first record and apply mapping.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sql"></param>
    /// <param name="map"></param>
    /// <returns></returns>
    public T? QuerySingle<T>(string sql, Func<IDataReader, T> map) =>
        QuerySingle(sql, new DbParams(), map);

    /// <summary>
    /// Execute paramterized query and manually cursor IDataReader.
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <param name="read"></param>
    /// <returns></returns>
    public T Read<T>(string sql, DbParams param, Func<IDataReader, T> read) =>
        Do(sql, param, cmd => cmd.Read(read));

    /// <summary>
    /// Execute paramterized query and manually cursor IDataReader.
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="read"></param>
    /// <returns></returns>
    public T Read<T>(string sql, Func<IDataReader, T> read) =>
        Do(sql, null, cmd => cmd.Read(read));

    /// <summary>
    /// Asynchronously execute parameterized query.
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public Task ExecAsync(string sql, DbParams? param = null) =>
        DoAsync(sql, param, cmd => cmd.ExecAsync());

    /// <summary>
    /// Execute parameterized query.
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="paramList"></param>
    /// <returns></returns>
    public Task ExecManyAsync(string sql, IEnumerable<DbParams> paramList) =>
        DoManyAsync(sql, paramList, cmd => cmd.ExecAsync());


    /// <summary>
    /// Asynchronously execute parameterized query and return single-value.
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public Task<object?> ScalarAsync(string sql, DbParams? param = null) =>
        DoAsync(sql, param, cmd => cmd.ScalarAsync());

    /// <summary>
    /// Asynchronously execute parameterized query, enumerate all records and apply mapping.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sql"></param>
    /// <param name="map"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public Task<IEnumerable<T>> QueryAsync<T>(string sql, DbParams param, Func<IDataReader, T> map) =>
        DoAsync(sql, param, cmd => cmd.QueryAsync(map));

    /// <summary>
    /// Asynchronously execute query, enumerate all records and apply mapping.///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sql"></param>
    /// <param name="map"></param>
    /// <returns></returns>
    public Task<IEnumerable<T>> QueryAsync<T>(string sql, Func<IDataReader, T> map) =>
        DoAsync(sql, [], cmd => cmd.QueryAsync(map));

    /// <summary>
    /// Asynchronously execute paramterized query, read only first record and apply mapping.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sql"></param>
    /// <param name="map"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public Task<T?> QuerySingleAsync<T>(string sql, DbParams param, Func<IDataReader, T> map) =>
        DoAsync(sql, param, cmd => cmd.QuerySingleAsync(map));

    /// <summary>
    /// Asynchronously execute query, read only first record and apply mapping.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sql"></param>
    /// <param name="map"></param>
    /// <returns></returns>
    public Task<T?> QuerySingleAsync<T>(string sql, Func<IDataReader, T> map) =>
        DoAsync(sql, [], cmd => cmd.QuerySingleAsync(map));

    /// <summary>
    /// Execute paramterized query and manually cursor IDataReader.
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <param name="read"></param>
    /// <returns></returns>
    public Task<T> ReadAsync<T>(string sql, DbParams param, Func<IDataReader, T> read) =>
        DoAsync(sql, param, cmd => cmd.ReadAsync(read));

    /// <summary>
    /// Execute paramterized query and manually cursor IDataReader.
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="read"></param>
    /// <returns></returns>
    public Task<T> ReadAsync<T>(string sql, Func<IDataReader, T> read) =>
        DoAsync(sql, null, cmd => cmd.ReadAsync(read));

    /// <summary>
    /// Ensure the DbConnection and DbTransaction resources are properly disposed
    /// </summary>
    public void Dispose()
    {
        _connection.Close();
        _transaction.Dispose();
    }

    private void Do(string sql, DbParams? param, Action<IDbCommand> action) =>
        action(new DbCommandBuilder(_transaction, sql, param ?? []).Build());

    private T Do<T>(string sql, DbParams? param, Func<IDbCommand, T> func) =>
        func(new DbCommandBuilder(_transaction, sql, param ?? []).Build());

    private Task DoAsync(string sql, DbParams? param, Func<DbCommand, Task> func) =>
        func(new DbCommandBuilder(_transaction).CommandText(sql).DbParams(param ?? []).Build().ToDbCommand());

    private Task<T> DoAsync<T>(string sql, DbParams? param, Func<DbCommand, Task<T>> func) =>
        func(new DbCommandBuilder(_transaction).CommandText(sql).DbParams(param ?? []).Build().ToDbCommand());

    private void DoMany(string sql, IEnumerable<DbParams> paramList, Action<IDbCommand> func)
    {
        var cmd = new DbCommandBuilder(_transaction, sql).Build();

        foreach (var param in paramList)
        {
            cmd.Parameters.Clear();
            cmd.SetDbParams(param);
            func(cmd);
        }
    }

    private async Task DoManyAsync(string sql, IEnumerable<DbParams> paramList, Func<DbCommand, Task> func)
    {
        var cmd = new DbCommandBuilder(_transaction, sql).Build().ToDbCommand();

        foreach (var param in paramList)
        {
            cmd.Parameters.Clear();
            cmd.SetDbParams(param);
            await func(cmd);
        }
    }
}
