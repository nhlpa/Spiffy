using System.Data;

namespace Spiffy
{
  public interface IDbBatch
  {
    IDbConnection Connection { get; }
    IDbTransaction Transaction { get; }
    void Commit();
    void Rollback();
  }
}
