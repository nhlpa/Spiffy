using Spiffy;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Source.Sql
{
  /// <summary>
  /// Database unit of work.
  /// </summary>
  public interface IDbBatch : IDbHandler, IDbHandlerAsync, IDisposable
  {
    /// <summary>
    /// Commit unit of work.
    /// </summary>
    void Commit();

    /// <summary>
    /// Rollback unit of work.
    /// </summary>
    void Rollback();

    /// <summary>
    /// Execute paramterized query and manually cursor IDataReader.
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    IDataReader Read(string sql, DbParams param = null);

    /// <summary>
    /// Asynchronously execute paramterized query and manually cursor IDataReader.
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<IDataReader> ReadAsync(string sql, DbParams param = null);
  }
}
