using System;
using System.Collections.Generic;
using System.Data;

namespace Nhlpa.Sql
{
    public class DbContext<TFixture> : IDbContext<TFixture> where TFixture : IDbFixture
    {
        private readonly TFixture _fixture;

        public DbContext(TFixture fixture)
        {
            if (fixture == null)
                throw new ArgumentNullException(nameof(fixture));

            _fixture = fixture;
        }

        public IDbBatch NewBatch()
        {
            var conn = _fixture.NewConnection();
            conn.TryOpenConnection();
            var tran = conn.TryBeginTransaction();
            return new DbBatch(conn, tran);
        }

        public int Exec(string sql, DbParams param = null) =>
            Batch(b => b.Exec(sql, param));

        public T Val<T>(string sql, DbParams param = null) =>
            Batch(b => b.Val<T>(sql, param));

        public IEnumerable<T> Query<T>(string sql, Func<IDataReader, T> map, DbParams param = null) =>
            Batch(b => b.Query(sql, map, param));

        public T QuerySingle<T>(string sql, Func<IDataReader, T> map, DbParams param = null) =>
            Batch(b => b.QuerySingle(sql, map, param));

        private T Batch<T>(Func<IDbBatch, T> func)
        {
            var batch = NewBatch();
            var result = func(batch);
            batch.Commit();
            return result;
        }
    }
}
