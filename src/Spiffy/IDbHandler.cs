namespace Spiffy;

using System;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// Represents the ability to do work against a data source.
/// </summary>
public interface IDbHandler
{
    /// <summary>
    /// Create a new IDbBatch, which represents a database unit of work.
    /// </summary>
    /// <returns></returns>
    IDbBatch NewBatch();

    /// <summary>
    /// Execute parameterized query.
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    void Exec(string sql, DbParams? param = null);

    /// <summary>
    /// Asynchronously execute parameterized query.
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    Task ExecAsync(string sql, DbParams? param = null);

    /// <summary>
    /// Execute parameterized query multiple times
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="paramList"></param>
    /// <returns></returns>
    void ExecMany(string sql, IEnumerable<DbParams> paramList);

    /// <summary>
    /// Asynchronously execute parameterized query multiple times
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="paramList"></param>
    /// <returns></returns>
    Task ExecManyAsync(string sql, IEnumerable<DbParams> paramList);

    /// <summary>
    /// Execute parameterized query and return single-value.
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    object? Scalar(string sql, DbParams? param = null);

    /// <summary>
    /// Asynchronously execute parameterized query and return single-value.
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<object?> ScalarAsync(string sql, DbParams? param = null);

    /// <summary>
    /// Execute parameterized query, enumerate all records and apply mapping.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sql"></param>
    /// <param name="map"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    IEnumerable<T> Query<T>(string sql, DbParams param, Func<IDataReader, T> map);

    /// <summary>
    /// Asynchronously execute parameterized query, enumerate all records and apply mapping.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sql"></param>
    /// <param name="map"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<IEnumerable<T>> QueryAsync<T>(string sql, DbParams param, Func<IDataReader, T> map);

    /// <summary>
    /// Execute parameterized query, enumerate all records and apply mapping.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sql"></param>
    /// <param name="map"></param>
    /// <returns></returns>
    IEnumerable<T> Query<T>(string sql, Func<IDataReader, T> map);

    /// <summary>
    /// Asynchronously execute query, enumerate all records and apply mapping.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sql"></param>
    /// <param name="map"></param>
    /// <returns></returns>
    Task<IEnumerable<T>> QueryAsync<T>(string sql, Func<IDataReader, T> map);

    /// <summary>
    /// Execute paramterized query, read only first record and apply mapping.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sql"></param>
    /// <param name="map"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    T? QuerySingle<T>(string sql, DbParams param, Func<IDataReader, T> map);

    /// <summary>
    /// Asynchronously execute paramterized query, read only first record and apply mapping.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sql"></param>
    /// <param name="map"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<T?> QuerySingleAsync<T>(string sql, DbParams param, Func<IDataReader, T> map);

    /// <summary>
    /// Execute query, read only first record and apply mapping.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sql"></param>
    /// <param name="map"></param>
    /// <returns></returns>
    T? QuerySingle<T>(string sql, Func<IDataReader, T> map);

    /// <summary>
    /// Asynchronously execute query, read only first record and apply mapping.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sql"></param>
    /// <param name="map"></param>
    /// <returns></returns>
    Task<T?> QuerySingleAsync<T>(string sql, Func<IDataReader, T> map);

    /// <summary>
    /// Execute paramterized query and manually cursor IDataReader.
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <param name="read"></param>
    /// <returns></returns>
    T Read<T>(string sql, DbParams param, Func<IDataReader, T> read);

    /// <summary>
    /// Execute paramterized query and manually cursor IDataReader.
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <param name="read"></param>
    /// <returns></returns>
    Task<T> ReadAsync<T>(string sql, DbParams param, Func<IDataReader, T> read);

    /// <summary>
    /// Execute paramterized query and manually cursor IDataReader.
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="read"></param>
    /// <returns></returns>
    T Read<T>(string sql, Func<IDataReader, T> read);

    /// <summary>
    /// Execute paramterized query and manually cursor IDataReader.
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="read"></param>
    /// <returns></returns>
    Task<T> ReadAsync<T>(string sql, Func<IDataReader, T> read);
}
