using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Spiffy
{
    /// <summary>
    /// Database unit of work.
    /// </summary>
    public class DbBatch : IDbBatch
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        /// <summary>
        /// Constitute a new DbBatch from IDbConnection and IDbTransaction
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public DbBatch(IDbConnection connection, IDbTransaction transaction)
        {
            _transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

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
                _transaction.Rollback();
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
        /// Execute parameterized query and return rows affected.
        /// </summary>        
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public void Exec(string sql, DbParams param = null) =>
            DoVoid(sql, param, cmd => cmd.Exec());

        /// <summary>
        /// Execute parameterized query and return rows affected.
        /// </summary>        
        /// <param name="sql"></param>
        /// <param name="paramList"></param>
        /// <returns></returns>
        public void ExecMany(string sql, IEnumerable<DbParams> paramList) =>
            DoMany(sql, paramList, cmd => cmd.Exec());

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
        public T QuerySingle<T>(string sql, DbParams param, Func<IDataReader, T> map) =>
            Do(sql, param, cmd => cmd.QuerySingle(map));

        /// <summary>
        /// Execute query, read only first record and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public T QuerySingle<T>(string sql, Func<IDataReader, T> map) =>
            QuerySingle(sql, new DbParams(), map);

        /// <summary>
        /// Execute paramterized query and manually cursor IDataReader.
        /// </summary>        
        /// <param name="sql"></param>		
        /// <param name="param"></param>
        /// <returns></returns>
        public IDataReader Read(string sql, DbParams param = null) =>
            Do(sql, param, cmd => cmd.Read());

        /// <summary>
        /// Asynchronously execute parameterized query and return rows affected.
        /// </summary>        
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task ExecAsync(string sql, DbParams param = null) =>
            DoVoidAsync(sql, param, cmd => cmd.ExecAsync());

        /// <summary>
        /// Execute parameterized query and return rows affected.
        /// </summary>        
        /// <param name="sql"></param>
        /// <param name="paramList"></param>
        /// <returns></returns>
        public Task ExecManyAsync(string sql, IEnumerable<DbParams> paramList) =>
            DoManyAsync(sql, paramList, cmd => cmd.ExecAsync());

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
            DoAsync(sql, new DbParams(), cmd => cmd.QueryAsync(map));

        /// <summary>
        /// Asynchronously execute paramterized query, read only first record and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>        
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task<T> QuerySingleAsync<T>(string sql, DbParams param, Func<IDataReader, T> map) =>
            DoAsync(sql, param, cmd => cmd.QuerySingleAsync(map));

        /// <summary>
        /// Asynchronously execute query, read only first record and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public Task<T> QuerySingleAsync<T>(string sql, Func<IDataReader, T> map) =>
            DoAsync(sql, new DbParams(), cmd => cmd.QuerySingleAsync(map));

        /// <summary>
        /// Execute paramterized query and manually cursor IDataReader.
        /// </summary>        
        /// <param name="sql"></param>		
        /// <param name="param"></param>
        /// <returns></returns>
        public Task<IDataReader> ReadAsync(string sql, DbParams param = null) =>
            DoAsync(sql, param, cmd => cmd.ReadAsync());

        /// <summary>
        /// Ensure the DbConnection and DbTransaction resources are properly disposed
        /// </summary>
        public void Dispose()
        {
            _connection.Close();
            _transaction.Dispose();
        }

        private T Do<T>(string sql, DbParams param, Func<IDbCommand, T> func)
        {
            try
            {
                var cmdBuilder = new DbCommandBuilder(_connection)
                    .UsingTransaction(_transaction)
                    .SetCommandText(sql)
                    .AddDbParams(param);

                using (var cmd = cmdBuilder.Build())
                {
                    return func(cmd);
                }
            }
            catch (FailedExecutionException)
            {
                Rollback();
                throw;
            }
        }

        private async Task<T> DoAsync<T>(string sql, DbParams param, Func<DbCommand, Task<T>> func)
        {
            try
            {
                var cmdBuilder = new DbCommandBuilder(_connection).SetCommandText(sql).AddDbParams(param);
                using (var cmd = cmdBuilder.Build() as DbCommand)
                {
                    return await func(cmd);
                }
            }
            catch (FailedExecutionException)
            {
                Rollback();
                throw;
            }
        }

        private void DoVoid(string sql, DbParams param, Action<IDbCommand> func)
        {
            try
            {
                var cmdBuilder = new DbCommandBuilder(_connection)
                    .UsingTransaction(_transaction)
                    .SetCommandText(sql)
                    .AddDbParams(param);

                using (var cmd = cmdBuilder.Build())
                {
                    func(cmd);
                }
            }
            catch (FailedExecutionException)
            {
                Rollback();
                throw;
            }
        }

        private async Task DoVoidAsync(string sql, DbParams param, Func<DbCommand, Task> func)
        {
            try
            {
                var cmdBuilder = new DbCommandBuilder(_connection).SetCommandText(sql).AddDbParams(param);
                using (var cmd = cmdBuilder.Build() as DbCommand)
                {
                    await func(cmd);
                }
            }
            catch (FailedExecutionException)
            {
                Rollback();
                throw;
            }
        }

        private void DoMany(string sql, IEnumerable<DbParams> paramList, Action<IDbCommand> func)
        {
            try
            {
                var cmdBuilder = new DbCommandBuilder(_connection).UsingTransaction(_transaction).SetCommandText(sql);

                using (var cmd = cmdBuilder.Build())
                {
                    foreach (var param in paramList)
                    {
                        cmd.Parameters.Clear();
                        cmd.AddDbParams(param);
                        func(cmd);
                    }
                }
            }
            catch (FailedExecutionException)
            {
                Rollback();
                throw;
            }
        }

        private async Task DoManyAsync(string sql, IEnumerable<DbParams> paramList, Func<DbCommand, Task> func)
        {
            try
            {
                var cmdBuilder = new DbCommandBuilder(_connection).UsingTransaction(_transaction).SetCommandText(sql);

                using (var cmd = cmdBuilder.Build() as DbCommand)
                {
                    foreach (var param in paramList)
                    {
                        cmd.Parameters.Clear();
                        cmd.AddDbParams(param);
                        await func(cmd);
                    }
                }
            }
            catch (FailedExecutionException)
            {
                Rollback();
                throw;
            }
        }
    }
}