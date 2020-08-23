using System;
using System.Collections.Generic;
using System.Data;

namespace Spiffy
{
    public class DbUnit : IDbUnit
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        public DbUnit(IDbConnection connection, IDbTransaction transaction)
        {
            _transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

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

        public int Exec(string sql, DbCommandParams param = null)
        {
            try
            {
                return _transaction.NewCommand(sql, param).Exec();
            }
            catch (FailedExecutionException)
            {
                Rollback();
                throw;
            }
        }

        public IEnumerable<T> Query<T>(string sql, Func<IDataReader, T> map, DbCommandParams param = null)
        {
            try
            {
                return _transaction.NewCommand(sql, param).Query(map);
            }
            catch (FailedExecutionException)
            {
                Rollback();
                throw;
            }
        }

        public T QuerySingle<T>(string sql, Func<IDataReader, T> map, DbCommandParams param = null)
        {
            try
            {
                return _transaction.NewCommand(sql, param).QuerySingle(map);
            }
            catch (FailedExecutionException)
            {
                Rollback();
                throw;
            }
        }

        /// <summary>
        /// Execute paramterized query and manually cursor IDataReader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>		
        /// <param name="param"></param>
        /// <returns></returns>
        public IDataReader Read(string sql, DbCommandParams param = null)
        {
            try
            {
                return _transaction.NewCommand(sql, param).Read();
            }
            catch (FailedExecutionException)
            {
                Rollback();
                throw;
            }
        }

        public T Val<T>(string sql, DbCommandParams param = null)
        {
            try
            {
                return _transaction.NewCommand(sql, param).Val<T>();
            }
            catch (FailedExecutionException)
            {
                Rollback();
                throw;
            }
        }

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
            _transaction.Dispose();
        }
    }
}