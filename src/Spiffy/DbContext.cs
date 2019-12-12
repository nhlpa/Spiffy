using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spiffy {
	public class DbContext<TFixture> : IDbContext<TFixture> where TFixture : IDbFixture {
		private readonly TFixture _fixture;

		public DbContext(TFixture fixture) {
			_fixture = fixture;
		}

		public IDbBatch BeginBatch() {
			try {
				var conn = _fixture.NewConnection();
				return conn.BeginBatch();
			} catch (Exception ex) {
				throw new CouldNotOpenConnectionException(ex);
			}
		}

		public int Exec(Func<IDbBatch, int> cmd) {
			var batch = BeginBatch();
			var affected = cmd(batch);
			batch.Commit();
			return affected;
		}

		public T Val<T>(Func<IDbBatch, T> query) where T : struct {
			var batch = BeginBatch();
			var result = query(batch);
			batch.Commit();
			return result;
		}

		public T Query<T>(Func<IDbBatch, T> query) {
			var batch = BeginBatch();
			var result = query(batch);
			batch.Commit();
			return result;
		}
	}
}
