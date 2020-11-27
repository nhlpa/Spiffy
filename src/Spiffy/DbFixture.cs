using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Spiffy
{
    /// <summary>
    /// Represents the ability to obtain new unit instances to perform 
    /// database-bound tasks transactionally, as well as query
    /// the database directly.
    /// </summary>
    /// <typeparam name="TConn">The IDbConnectionFactory to use for connection creation.</typeparam>
    public class DbFixture<TConn> : IDbFixture<TConn> where TConn : IDbConnectionFactory
    {
        private readonly TConn _connectionFactory;

        /// <summary>
        /// Constitute a DbFixture from a IDbConnectionFactory
        /// </summary>
        /// <param name="connectionFactory"></param>
        public DbFixture(TConn connectionFactory)
        {
            if (connectionFactory == null)
                throw new ArgumentNullException(nameof(connectionFactory));

            _connectionFactory = connectionFactory;
        }

        /// <summary>
        /// Create a new IDbBatch, which represents a database unit of work.
        /// </summary>
        /// <returns></returns>
        public IDbBatch NewBatch()
        {
            var conn = _connectionFactory.NewConnection();
            conn.TryOpenConnection();
            var tran = conn.TryBeginTransaction();
            return new DbBatch(conn, tran);
        }

        /// <summary>
        /// Execute parameterized query and return rows affected.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int Exec(string sql, DbParams param = null) =>
            Batch(b => b.Exec(sql, param));

        /// <summary>
        /// Execute parameterized query multiple times
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public void ExecMany(string sql, IEnumerable<DbParams> param) =>
            Do(b => b.ExecMany(sql, param));

        /// <summary>
        /// Execute parameterized query and return single-value.
        /// </summary>        
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public object Scalar(string sql, DbParams param = null) =>
            Batch(b => b.Scalar(sql, param));

        /// <summary>
        /// Execute parameterized query, enumerate all records and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string sql, DbParams param, Func<IDataReader, T> map) =>
            Batch(b => b.Query(sql, param, map));

        /// <summary>
        /// Execute query, enumerate all records and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string sql, Func<IDataReader, T> map) =>
            Batch(b => b.Query(sql, map));

        /// <summary>
        /// Execute paramterized query, read only first record and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T QuerySingle<T>(string sql, DbParams param, Func<IDataReader, T> map) =>
            Batch(b => b.QuerySingle(sql, param, map));

        /// <summary>
        /// Execute query, read only first record and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>        
        /// <returns></returns>
        public T QuerySingle<T>(string sql, Func<IDataReader, T> map) =>
            Batch(b => b.QuerySingle(sql, map));

        /// <summary>
        /// Asynchronously execute parameterized query and return rows affected.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task<int> ExecAsync(string sql, DbParams param = null) =>
            BatchAsync(b => b.ExecAsync(sql, param));

        /// <summary>
        /// Asynchronously execute parameterized query and return rows affected.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramList"></param>
        /// <returns></returns>
        public Task ExecManyAsync(string sql, IEnumerable<DbParams> paramList) =>
            DoAsync(b => b.ExecManyAsync(sql, paramList));

        /// <summary>
        /// Asynchronously execute parameterized query and return single-value.
        /// </summary>        
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task<object> ScalarAsync(string sql, DbParams param = null) =>
            BatchAsync(b => b.ScalarAsync(sql, param));

        /// <summary>
        /// Asynchronously execute parameterized query, enumerate all records and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task<IEnumerable<T>> QueryAsync<T>(string sql, DbParams param, Func<IDataReader, T> map) =>
            BatchAsync(b => b.QueryAsync(sql, param, map));

        /// <summary>
        /// Asynchronously execute query, enumerate all records and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>        
        /// <returns></returns>
        public Task<IEnumerable<T>> QueryAsync<T>(string sql, Func<IDataReader, T> map) =>
            BatchAsync(b => b.QueryAsync(sql, map));

        /// <summary>
        /// Asynchronously execute paramterized query, read only first record and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task<T> QuerySingleAsync<T>(string sql, DbParams param, Func<IDataReader, T> map) =>
            BatchAsync(b => b.QuerySingleAsync(sql, param, map));

        /// <summary>
        /// Asynchronously execute query, read only first record and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>        
        /// <returns></returns>
        public Task<T> QuerySingleAsync<T>(string sql, Func<IDataReader, T> map) =>
            BatchAsync(b => b.QuerySingleAsync(sql, map));

        private T Batch<T>(Func<IDbBatch, T> func)
        {
            var batch = NewBatch();
            var result = func(batch);
            batch.Commit();
            return result;
        }

        private async Task<T> BatchAsync<T>(Func<IDbBatch, Task<T>> func)
        {
            var batch = NewBatch();
            var result = await func(batch);
            batch.Commit();
            return result;
        }

        private void Do(Action<IDbBatch> func)
        {
            var batch = NewBatch();
            func(batch);
            batch.Commit();            
        }

        private async Task DoAsync(Func<IDbBatch, Task> func)
        {
            var batch = NewBatch();
            await func(batch);
            batch.Commit();
        }
    }
}
