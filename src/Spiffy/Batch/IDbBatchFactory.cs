namespace Spiffy;

using System;
using System.Threading.Tasks;

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
