using System.Data;

namespace Spiffy
{
  /// <summary>
  /// Represents the ability to create new IDbConnection instances.
  /// </summary>
  public interface IDbConnectionFactory
  {
    /// <summary>
    /// Create a new instance of an IDbConnection implementation.
    /// </summary>
    /// <returns></returns>
    IDbConnection NewConnection();
  }
}
