using System;
using System.Collections.Generic;
using System.Data;

namespace Spiffy {
	internal static class IDbCommandExtensions {

		internal static int Exec(this IDbCommand cmd) =>
			cmd.TryExecuteNonQuery();

		internal static T Val<T>(this IDbCommand cmd) where T : struct =>
			cmd.TryExecuteScalar<T>();

		internal static IEnumerable<T> Query<T>(this IDbCommand cmd, Func<IDataReader, T> map) {
			using (var rd = cmd.TryExecuteReader()) {
				var records = new HashSet<T>();

				while (rd.Read()) {
					records.Add(map(rd));
				}

				return records;
			}
		}

		internal static T Read<T>(this IDbCommand cmd, Func<IDataReader, T> map) {
			using (var rd = cmd.TryExecuteReader()) {
				if (rd.Read()) {
					return map(rd);
				} else {
					return default;
				}
			}
		}

		private static int TryExecuteNonQuery(this IDbCommand cmd) {
			try {
				return cmd.ExecuteNonQuery();
			} catch (Exception ex) {
				throw new FailedExecutionException(DbErrorCode.CouldNotExecuteNonQuery, cmd.CommandText, ex);
			}
		}

		private static T TryExecuteScalar<T>(this IDbCommand cmd) {
			try {
				var result = cmd.ExecuteScalar();
				return result != null ? (T)Convert.ChangeType(result, typeof(T)) : default;
			} catch (Exception ex) {
				throw new FailedExecutionException(DbErrorCode.CouldNotExecuteScalar, cmd.CommandText, ex);
			}
		}

		private static IDataReader TryExecuteReader(this IDbCommand cmd) {
			try {
				return cmd.ExecuteReader();
			} catch (Exception ex) {
				throw new FailedExecutionException(DbErrorCode.CouldNotExecuteReader, cmd.CommandText, ex);
			}
		}
	}
}
