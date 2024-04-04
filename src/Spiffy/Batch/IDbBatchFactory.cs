using System;
using System.Threading.Tasks;

namespace Spiffy
{
  /// <summary>
  /// Represents the ability to create IDbBatch.
  /// </summary>
  public interface IDbBatchFactory
  {
    /// <summary>
    /// Create a new IDbBatch, which represents a database unit of work.
    /// </summary>
    /// <returns></returns>
    IDbBatch NewBatch();
  }
}