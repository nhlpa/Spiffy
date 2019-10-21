using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Spiffy
{
  public static class IDbBatchExtensions
  {
    /// <summary>
    /// Execute parameterized query and return rows affected.
    /// </summary>
    /// <param name="batch"></param>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public static async Task<int> Exec(this IDbBatch batch, string sql, IDbParams param = null)
      => await batch.Transaction.NewCommand(sql, param).Exec();

    /// <summary>
    /// Execute parameterized query and return single-value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="batch"></param>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public static async Task<T> Val<T>(this IDbBatch batch, string sql, IDbParams param = null)
      => await batch.Transaction.NewCommand(sql, param).Val<T>();
    
    /// <summary>
    /// Execute parameterized query, enumerate all records and apply mapping.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="batch"></param>
    /// <param name="sql"></param>
    /// <param name="map"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public static async Task<IEnumerable<T>> Query<T>(this IDbBatch batch, string sql, Func<IDataReader, T> map, IDbParams param = null)
      => await batch.Transaction.NewCommand(sql, param).Query(map);


    /// <summary>
    /// Execute paramterized query, read only first record and apply mapping.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="batch"></param>
    /// <param name="sql"></param>
    /// <param name="map"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public static async Task<T> Read<T>(this IDbBatch batch, string sql, Func<IDataReader, T> map, IDbParams param = null)
      => await batch.Transaction.NewCommand(sql, param).Read(map);
  }

}
