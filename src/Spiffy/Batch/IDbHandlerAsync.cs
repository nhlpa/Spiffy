using Spiffy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Source.Sql
{
  /// <summary>
  /// Represents the ability to do work asynchronously against a data source.
  /// </summary>
  public interface IDbHandlerAsync
  {
    /// <summary>
    /// Asynchronously execute parameterized query.
    /// </summary>        
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    Task ExecAsync(string sql, DbParams param = null);

    /// <summary>
    /// Asynchronously execute parameterized query multiple times
    /// </summary>        
    /// <param name="sql"></param>
    /// <param name="paramList"></param>
    /// <returns></returns>
    Task ExecManyAsync(string sql, IEnumerable<DbParams> paramList);

    /// <summary>
    /// Asynchronously execute parameterized query and return single-value.
    /// </summary>              
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<object> ScalarAsync(string sql, DbParams param = null);

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
    /// Asynchronously execute query, enumerate all records and apply mapping.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sql"></param>
    /// <param name="map"></param>
    /// <returns></returns>
    Task<IEnumerable<T>> QueryAsync<T>(string sql, Func<IDataReader, T> map);

    /// <summary>
    /// Asynchronously execute paramterized query, read only first record and apply mapping.
    /// </summary>
    /// <typeparam name="T"></typeparam>        
    /// <param name="sql"></param>
    /// <param name="map"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<T> QuerySingleAsync<T>(string sql, DbParams param, Func<IDataReader, T> map);

    /// <summary>
    /// Asynchronously execute query, read only first record and apply mapping.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sql"></param>
    /// <param name="map"></param>
    /// <returns></returns>
    Task<T> QuerySingleAsync<T>(string sql, Func<IDataReader, T> map);
  }
}

