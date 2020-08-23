using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Spiffy
{
    public class DbFixture<TConn> : IDbFixture<TConn> where TConn : IDbConnectionFactory
    {
        private readonly TConn _fixture;

        public DbFixture(TConn fixture)
        {
            if (fixture == null)
                throw new ArgumentNullException(nameof(fixture));

            _fixture = fixture;
        }

        /// <summary>
        /// Create a new IDbBatch, which represents a database unit of work.
        /// </summary>
        /// <returns></returns>
        public IDbUnit NewBatch()
        {
            var conn = _fixture.NewConnection();
            conn.TryOpenConnection();
            var tran = conn.TryBeginTransaction();
            return new DbUnit(conn, tran);
        }

        /// <summary>
        /// Execute parameterized query and return rows affected.
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int Exec(string sql, DbCommandParams param = null) =>
            Do(b => b.Exec(sql, param));

        /// <summary>
        /// Execute parameterized query, enumerate all records and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="batch"></param>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string sql, Func<IDataReader, T> map, DbCommandParams param = null) =>
            Do(b => b.Query(sql, map, param));

        /// <summary>
        /// Execute paramterized query, read only first record and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="batch"></param>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T QuerySingle<T>(string sql, Func<IDataReader, T> map, DbCommandParams param = null) =>
            Do(b => b.QuerySingle(sql, map, param));

        /// <summary>
        /// Execute parameterized query and return single-value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="batch"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T Scalar<T>(string sql, DbCommandParams param = null) =>
            Do(b => b.Scalar<T>(sql, param));

        /// <summary>
        /// Asynchronously execute parameterized query and return rows affected.
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task<int> ExecAsync(string sql, DbCommandParams param = null) =>
            throw new NotImplementedException();

        /// <summary>
        /// Asynchronously execute parameterized query, enumerate all records and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="batch"></param>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task<IEnumerable<T>> QueryAsync<T>(string sql, Func<IDataReader, T> map, DbCommandParams param = null) =>
            throw new NotImplementedException();

        /// <summary>
        /// Asynchronously execute paramterized query, read only first record and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="batch"></param>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task<T> QuerySingleAsync<T>(string sql, Func<IDataReader, T> map, DbCommandParams param = null) =>
            throw new NotImplementedException();

        /// <summary>
        /// Asynchronously execute parameterized query and return single-value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="batch"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task<T> ScalarAsync<T>(string sql, DbCommandParams param = null) =>
            throw new NotImplementedException();

        private T Do<T>(Func<IDbUnit, T> func)
        {
            var batch = NewBatch();
            var result = func(batch);
            batch.Commit();
            return result;
        }

        private async Task<T> DoAsync<T>(Func<IDbUnit, Task<T>> func)
        {
            var batch = NewBatch();
            var result = await func(batch);
            batch.Commit();
            return result;
        }
    }
}
