using System;
using System.Collections.Generic;
using System.Data;

namespace Spiffy {
	public interface IDbBatch {
		void Commit();
		int Exec(string sql, IDbParams param = null);
		IEnumerable<T> Query<T>(string sql, Func<IDataReader, T> map, IDbParams param = null);
		T Read<T>(string sql, Func<IDataReader, T> map, IDbParams param = null);
		void Rollback();
		T Val<T>(string sql, IDbParams param = null) where T : struct;		
	}
}
