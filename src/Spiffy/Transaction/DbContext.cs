using System;

namespace Nhlpa.Sql
{
    public class DbContext<TFixture> : IDbContext<TFixture> where TFixture : IDbFixture
    {
        private readonly TFixture _fixture;

        public DbContext(TFixture fixture)
        {
            _fixture = fixture;
        }

        public IDbBatch BeginBatch()
        {
            var conn = _fixture.NewConnection();
            conn.TryOpenConnection();
            var tran = conn.TryBeginTransaction();
            return new DbBatch(conn, tran);
        }

        public int Exec(Func<IDbBatch, int> cmd)
        {
            var batch = BeginBatch();
            var rowsAffected = cmd(batch);
            batch.Commit();
            return rowsAffected;
        }

        public T Query<T>(Func<IDbBatch, T> query)
        {
            var batch = BeginBatch();
            var result = query(batch);
            batch.Commit();
            return result;
        }
    }
}
