using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Spiffy
{
public static class IDbConnectionExtensions
{
    /// <summary>
    /// Start a new batch for the connection.
    /// </summary>
    /// <param name="conn"></param>
    /// <returns></returns>
    public static IDbBatch BeginBatch(this IDbConnection conn)
    {
      conn.TryOpenConnection();
      var tran = conn.TryBeginTransaction();
      return new DbBatch(tran);
    }

    /// <summary>
    /// Execute a single parameterized query and return rows affected.
    /// </summary>
    /// <param name="conn"></param>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public static async Task<int> Exec(this IDbConnection conn, string sql, IDbParams param = null)
    {
      var batch = conn.BeginBatch();
      var result = await batch.Exec(sql, param);
      batch.Commit();
      return result;
    }

    /// <summary>
    /// Execute a single parameterized query and return single-value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="conn"></param>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public static async Task<T> Val<T>(this IDbConnection conn, string sql, IDbParams param = null)
    {
      var batch = conn.BeginBatch();
      var result = await batch.Val<T>(sql, param);
      batch.Commit();
      return result;
    }

    /// <summary>
    /// Execute a single parameterized query, enumerate all records and apply mapping.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="conn"></param>
    /// <param name="sql"></param>
    /// <param name="map"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public static async Task<IEnumerable<T>> Query<T>(this IDbConnection conn, string sql, Func<IDataReader, T> map, IDbParams param = null)
    {
      var batch = conn.BeginBatch();
      var results = await batch.Query(sql, map, param);
      batch.Commit();
      return results;
    }

    /// <summary>
    /// Execute single paramterized query, read only first record and apply mapping.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="conn"></param>
    /// <param name="sql"></param>
    /// <param name="map"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public static async Task<T> Read<T>(this IDbConnection conn, string sql, Func<IDataReader, T> map, IDbParams param = null)
    {
      var batch = conn.BeginBatch();
      var result = await batch.Read(sql, map, param);
      batch.Commit();
      return result;
    }

    internal static void TryClose(this IDbConnection conn)
    {
      try
      {
        conn.TryCloseConnection();
      }
      finally
      {
        conn.Dispose();
      }
    }

    private static IDbTransaction TryBeginTransaction(this IDbConnection connection)
    {
      try
      {
        return connection.BeginTransaction();
      }
      catch (Exception ex)
      {
        throw new FailedTransacitonException(ex);
      }
    }

    private static void TryCloseConnection(this IDbConnection conn)
    {
      try
      {
        conn.Close();
      }
      catch (Exception ex)
      {
        throw new CouldNotCloseConnectionException(ex);
      }
    }

    private static void TryOpenConnection(this IDbConnection conn)
    {
      try
      {
        if (conn.State == ConnectionState.Closed)
        {
          conn.Open();
        }
        else
        {
          throw new ConnectionBusyException();
        }
      }
      catch (Exception ex)
      {
        throw new CouldNotOpenConnectionException(ex);
      }
    }
  }
}
