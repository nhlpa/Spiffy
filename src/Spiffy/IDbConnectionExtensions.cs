using System;
using System.Data;

namespace Spiffy {
	public static class IDbConnectionExtensions {
		/// <summary>
		/// Start a new batch for the connection.
		/// Open IDbConnection -> Begin IDbTransaction -> Create IDbBatch
		/// </summary>
		/// <param name="conn"></param>
		/// <returns></returns>
		public static IDbBatch BeginBatch(this IDbConnection conn) {
			conn.TryOpen();
			var tran = conn.TryBeginTransaction();
			return new DbBatch(conn, tran);
		}

		internal static void TryClose(this IDbConnection conn) {
			try {
				conn.Close();
			} finally {
				conn.Dispose();
			}
		}

		internal static void TryOpen(this IDbConnection conn) {
			try {
				if (conn.State == ConnectionState.Closed) {
					conn.Open();
				} else {
					throw new ConnectionBusyException();
				}
			} catch (Exception ex) {
				throw new CouldNotOpenConnectionException(ex);
			}
		}

		private static IDbTransaction TryBeginTransaction(this IDbConnection connection) {
			try {
				return connection.BeginTransaction();
			} catch (Exception ex) {
				throw new FailedTransacitonException(ex);
			}
		}
	}
}
