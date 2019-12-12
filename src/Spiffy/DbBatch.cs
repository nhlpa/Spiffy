using System;
using System.Collections.Generic;
using System.Data;

namespace Spiffy {
	/// <summary>
	/// An `IDbTransaction` wrapper to make dealing with transactions
	/// a lot simlper.
	/// </summary>
	public class DbBatch : IDbBatch {
		private readonly IDbConnection _connection;
		private readonly IDbTransaction _transaction;

		public DbBatch(IDbConnection conn, IDbTransaction tran) {
			_connection = conn ?? throw new ArgumentNullException(nameof(conn));
			_transaction = tran ?? throw new ArgumentNullException(nameof(tran));
		}
				
		/// <summary>
		/// Attempt to commit the work within the batch and dispose of volatile resources.
		/// </summary>
		public void Commit() {
			try {
				_transaction.Commit();
			} catch (Exception ex) {
				_transaction.Rollback();
				throw new FailedCommitBatchException(ex);
			} finally {
				Reset();
			}
		}

		/// <summary>
		/// Execute parameterized query and return rows affected.
		/// </summary>
		/// <param name="batch"></param>
		/// <param name="sql"></param>
		/// <param name="param"></param>
		/// <returns></returns>
		public int Exec(string sql, IDbParams param = null) {
			try {
				return _transaction.NewCommand(sql, param).Exec();
			} catch (FailedExecutionException) {
				Rollback();
				throw;
			}
		}

		/// <summary>
		/// Execute parameterized query, enumerate all records and apply mapping.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="batch"></param>
		/// <param name="sql"></param>
		/// <param name="map"></param>
		/// <param name="param"></param>
		/// <returns></returns>
		public IEnumerable<T> Query<T>(string sql, Func<IDataReader, T> map, IDbParams param = null) {
			try {
				return _transaction.NewCommand(sql, param).Query(map);
			} catch (FailedExecutionException) {
				Rollback();
				throw;
			}
		}

		/// <summary>
		/// Execute paramterized query, read only first record and apply mapping.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="batch"></param>
		/// <param name="sql"></param>
		/// <param name="map"></param>
		/// <param name="param"></param>
		/// <returns></returns>
		public T Read<T>(string sql, Func<IDataReader, T> map, IDbParams param = null) {
			try {
				return _transaction.NewCommand(sql, param).Read(map);
			} catch (FailedExecutionException) {
				Rollback();
				throw;
			}
		}

		/// <summary>
		/// Undo changes made within the  
		/// Called automatically from Commit() when exception is thrown during execution.
		/// </summary>
		public void Rollback() {
			try {
				_transaction.Rollback();
			} finally {
				Reset();
			}
		}

		/// <summary>
		/// Execute parameterized query and return single-value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="batch"></param>
		/// <param name="sql"></param>
		/// <param name="param"></param>
		/// <returns></returns>
		public T Val<T>(string sql, IDbParams param = null) where T : struct {
			try {
				return _transaction.NewCommand(sql, param).Val<T>();
			} catch (FailedExecutionException) {
				Rollback();
				throw;
			}
		}

		/// <summary>
		/// Dispose of volatile resources.
		/// </summary>
		private void Reset() {
			_connection.TryClose();
			_transaction.Dispose();
		}
	}
}
