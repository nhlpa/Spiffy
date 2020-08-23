using System;
using System.Collections.Generic;
using System.Data;

namespace Spiffy
{
    public class DbFixture<TConn> : IDbFixture<TConn> where TConn : IDbConnectionFactory
    {
        private readonly TConn _fixture;

        public DbFixture(TConn fixture)
        {
            if (fixture == null)
                throw new ArgumentNullException(nameof(fixture));

            _fixture = fixture;
        }

        public IDbUnit NewBatch()
        {
            var conn = _fixture.NewConnection();
            conn.TryOpenConnection();
            var tran = conn.TryBeginTransaction();
            return new DbUnit(conn, tran);
        }

        public int Exec(string sql, DbCommandParams param = null) =>
            Batch(b => b.Exec(sql, param));

        public T Val<T>(string sql, DbCommandParams param = null) =>
            Batch(b => b.Val<T>(sql, param));

        public IEnumerable<T> Query<T>(string sql, Func<IDataReader, T> map, DbCommandParams param = null) =>
            Batch(b => b.Query(sql, map, param));

        public T QuerySingle<T>(string sql, Func<IDataReader, T> map, DbCommandParams param = null) =>
            Batch(b => b.QuerySingle(sql, map, param));

        private T Batch<T>(Func<IDbUnit, T> func)
        {
            var batch = NewBatch();
            var result = func(batch);
            batch.Commit();
            return result;
        }
    }
}
