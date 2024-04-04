using System;
using System.Threading.Tasks;

namespace Source.Sql
{
  /// <summary>
  /// Represents an interface to a specific database.
  /// </summary>
  /// <typeparam name="TFixture"></typeparam>
  public interface IDbFixture<TFixture> : IDbHandler, IDbHandlerAsync where TFixture : IDbConnectionFactory
  {
    /// <summary>
    /// Create a new IDbBatch, which represents a database unit of work.
    /// </summary>
    /// <returns></returns>
    IDbBatch NewBatch();

    /// <summary>
    /// Do work in an auto-batch that returns results
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fn"></param>
    /// <returns></returns>
    T Batch<T>(Func<IDbBatch, T> fn);

    /// <summary>
    /// Do work in an auto-batch that returns no results
    /// </summary>        
    /// <param name="fn"></param>
    /// <returns></returns>
    void Batch(Action<IDbBatch> fn);

    /// <summary>
    /// Do asynchronous work in an auto-batch that returns results
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fn"></param>
    /// <returns></returns>
    Task<T> BatchAsync<T>(Func<IDbBatch, Task<T>> fn);

    /// <summary>
    /// Do asynchronous work in an auto-batch that returns no results
    /// </summary>        
    /// <param name="fn"></param>
    /// <returns></returns>
    Task BatchAsync(Func<IDbBatch, Task> fn);
  }
}

