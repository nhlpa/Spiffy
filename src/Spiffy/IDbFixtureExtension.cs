using System;

namespace Spiffy {
	internal static class IDbFixtureExtensions {
		/// <summary>
		/// Create IDbConnection -> Begin IDbBatch
		/// </summary>
		/// <param name="db"></param>
		/// <returns></returns>
		internal static IDbBatch BeginBatch(this IDbFixture db) {
			try {
				var conn = db.NewConnection();
				return conn.BeginBatch();
			} catch (Exception ex) {
				throw new CouldNotOpenConnectionException(ex);
			}
		}
	}
}
