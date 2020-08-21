using System;

namespace Nhlpa.Sql
{
  public interface IDbContext<TFixture> where TFixture : IDbFixture
  {
    IDbBatch BeginBatch();
    int Exec(Func<IDbBatch, int> cmd);
    T Query<T>(Func<IDbBatch, T> query);
  }
}
