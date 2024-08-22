namespace Spiffy;

using System;
using System.Data;
using System.Threading.Tasks;

/// <summary>
/// Database unit of work.
/// </summary>
public interface IDbBatch : IDbHandler, IDisposable
{
    /// <summary>
    /// Commit unit of work.
    /// </summary>
    void Commit();

    /// <summary>
    /// Rollback unit of work.
    /// </summary>
    void Rollback();
}
