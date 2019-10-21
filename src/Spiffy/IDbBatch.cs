using System.Data;

namespace Spiffy
{
  public interface IDbBatch
  {   
    IDbTransaction Transaction { get; }

    void Commit();
    void Rollback();
  }
}
