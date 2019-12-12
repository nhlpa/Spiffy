using System;

namespace Spiffy {
	public interface IDbContext<TFixture> where TFixture : IDbFixture {
		IDbBatch BeginBatch();
		int Exec(Func<IDbBatch, int> cmd);
		T Query<T>(Func<IDbBatch, T> query);
		T Val<T>(Func<IDbBatch, T> query) where T : struct;
	}
}
