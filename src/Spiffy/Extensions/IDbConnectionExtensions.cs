using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Spiffy
{    
    /// <summary>
    /// IDbConnection extension methods
    /// </summary>
    public static class IDbConnectionExtensions
    {
        /// <summary>
        /// Create a new IDbBatch, which represents a database unit of work.
        /// </summary>
        /// <returns></returns>
        public static IDbBatch NewBatch(this IDbConnection conn)
        {            
            conn.TryOpenConnection();
            var tran = conn.TryBeginTransaction();
            return new DbBatch(conn, tran);
        }

        /// <summary>
        /// Execute parameterized query and return rows affected.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static int Exec(this IDbConnection conn, string sql, DbParams param = null) =>
            conn.Batch(b => b.Exec(sql, param));

        /// <summary>
        /// Execute parameterized query multiple times
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static void ExecMany(this IDbConnection conn, string sql, IEnumerable<DbParams> param) =>
            conn.Do(b => b.ExecMany(sql, param));

        /// <summary>
        /// Execute parameterized query and return single-value.
        /// </summary>        
        /// <param name="conn"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static object Scalar(this IDbConnection conn, string sql, DbParams param = null) =>
            conn.Batch(b => b.Scalar(sql, param));

        /// <summary>
        /// Execute parameterized query, enumerate all records and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static IEnumerable<T> Query<T>(this IDbConnection conn, string sql, DbParams param, Func<IDataReader, T> map) =>
            conn.Batch(b => b.Query(sql, param, map));

        /// <summary>
        /// Execute query, enumerate all records and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public static IEnumerable<T> Query<T>(this IDbConnection conn, string sql, Func<IDataReader, T> map) =>
            conn.Batch(b => b.Query(sql, map));

        /// <summary>
        /// Execute paramterized query, read only first record and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static T QuerySingle<T>(this IDbConnection conn, string sql, DbParams param, Func<IDataReader, T> map) =>
            conn.Batch(b => b.QuerySingle(sql, param, map));

        /// <summary>
        /// Execute query, read only first record and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="sql"></param>
        /// <param name="map"></param>        
        /// <returns></returns>
        public static T QuerySingle<T>(this IDbConnection conn, string sql, Func<IDataReader, T> map) =>
            conn.Batch(b => b.QuerySingle(sql, map));

        /// <summary>
        /// Asynchronously execute parameterized query and return rows affected.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static Task<int> ExecAsync(this IDbConnection conn, string sql, DbParams param = null) =>
            conn.BatchAsync(b => b.ExecAsync(sql, param));

        /// <summary>
        /// Asynchronously execute parameterized query and return rows affected.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="sql"></param>
        /// <param name="paramList"></param>
        /// <returns></returns>
        public static Task ExecManyAsync(this IDbConnection conn, string sql, IEnumerable<DbParams> paramList) =>
            conn.DoAsync(b => b.ExecManyAsync(sql, paramList));

        /// <summary>
        /// Asynchronously execute parameterized query and return single-value.
        /// </summary>        
        /// <param name="conn"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static Task<object> ScalarAsync(this IDbConnection conn, string sql, DbParams param = null) =>
            conn.BatchAsync(b => b.ScalarAsync(sql, param));

        /// <summary>
        /// Asynchronously execute parameterized query, enumerate all records and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static Task<IEnumerable<T>> QueryAsync<T>(this IDbConnection conn, string sql, DbParams param, Func<IDataReader, T> map) =>
            conn.BatchAsync(b => b.QueryAsync(sql, param, map));

        /// <summary>
        /// Asynchronously execute query, enumerate all records and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="sql"></param>
        /// <param name="map"></param>        
        /// <returns></returns>
        public static Task<IEnumerable<T>> QueryAsync<T>(this IDbConnection conn, string sql, Func<IDataReader, T> map) =>
            conn.BatchAsync(b => b.QueryAsync(sql, map));

        /// <summary>
        /// Asynchronously execute paramterized query, read only first record and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static Task<T> QuerySingleAsync<T>(this IDbConnection conn, string sql, DbParams param, Func<IDataReader, T> map) =>
            conn.BatchAsync(b => b.QuerySingleAsync(sql, param, map));

        /// <summary>
        /// Asynchronously execute query, read only first record and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="sql"></param>
        /// <param name="map"></param>        
        /// <returns></returns>
        public static Task<T> QuerySingleAsync<T>(this IDbConnection conn, string sql, Func<IDataReader, T> map) =>
            conn.BatchAsync(b => b.QuerySingleAsync(sql, map));

        /// <summary>
        /// Attempt to open the IDbConnection if it is not already open.
        /// </summary>
        /// <param name="conn"></param>
        internal static void TryOpenConnection(this IDbConnection conn)
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                else
                {
                    throw new ConnectionBusyException();
                }
            }
            catch (Exception ex)
            {
                throw new CouldNotOpenConnectionException(ex);
            }
        }

        /// <summary>
        /// Attempt to begin a IDbTransaction.
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        internal static IDbTransaction TryBeginTransaction(this IDbConnection connection)
        {
            try
            {
                return connection.BeginTransaction();
            }
            catch (Exception ex)
            {
                throw new FailedTransacitonException(ex);
            }
        }

        private static T Batch<T>(this IDbConnection conn, Func<IDbBatch, T> func)
        {
            var batch = conn.NewBatch();
            var result = func(batch);
            batch.Commit();
            return result;
        }

        private static async Task<T> BatchAsync<T>(this IDbConnection conn, Func<IDbBatch, Task<T>> func)
        {
            var batch = conn.NewBatch();
            var result = await func(batch);
            batch.Commit();
            return result;
        }

        private static void Do(this IDbConnection conn, Action<IDbBatch> func)
        {
            var batch = conn.NewBatch();
            func(batch);
            batch.Commit();
        }

        private static async Task DoAsync(this IDbConnection conn, Func<IDbBatch, Task> func)
        {
            var batch = conn.NewBatch();
            await func(batch);
            batch.Commit();
        }
    }
}
