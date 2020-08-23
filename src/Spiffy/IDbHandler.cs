using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Spiffy
{
    public interface IDbHandler
    {
        /// <summary>
        /// Execute parameterized query and return rows affected.
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        int Exec(string sql, DbCommandParams param = null);

        /// <summary>
        /// Execute parameterized query, enumerate all records and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="batch"></param>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        IEnumerable<T> Query<T>(string sql, Func<IDataReader, T> map, DbCommandParams param = null);

        /// <summary>
        /// Execute paramterized query, read only first record and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="batch"></param>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        T QuerySingle<T>(string sql, Func<IDataReader, T> map, DbCommandParams param = null);

        /// <summary>
        /// Execute parameterized query and return single-value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="batch"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        T Scalar<T>(string sql, DbCommandParams param = null);

        /// <summary>
        /// Asynchronously execute parameterized query and return rows affected.
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> ExecAsync(string sql, DbCommandParams param = null);

        /// <summary>
        /// Asynchronously execute parameterized query, enumerate all records and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="batch"></param>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> QueryAsync<T>(string sql, Func<IDataReader, T> map, DbCommandParams param = null);

        /// <summary>
        /// Asynchronously execute paramterized query, read only first record and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="batch"></param>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<T> QuerySingleAsync<T>(string sql, Func<IDataReader, T> map, DbCommandParams param = null);

        /// <summary>
        /// Asynchronously execute parameterized query and return single-value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="batch"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<T> ScalarAsync<T>(string sql, DbCommandParams param = null);
    }
}