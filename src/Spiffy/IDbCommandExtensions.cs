using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Spiffy
{
  internal static class IDbCommandExtensions
  {
    internal static async Task<int> Exec(this IDbCommand cmd)
    {
      return await cmd.TryExecuteNonQueryAsync();
    }

    internal static async Task<T> Val<T>(this IDbCommand cmd)
    {
      return await cmd.TryExecuteScalarAsync<T>();
    }

    internal static async Task<IEnumerable<T>> Query<T>(this IDbCommand cmd, Func<IDataReader, T> map)
    {
      using (var rd = await cmd.TryExecuteReaderAsync())
      {
        var records = new HashSet<T>();

        while (rd.Read())
        {
          records.Add(map(rd));
        }

        rd.Dispose();

        return records;
      }
    }

    internal static async Task<T> Read<T>(this IDbCommand cmd, Func<IDataReader, T> map)
    {
      using (var rd = await cmd.TryExecuteReaderAsync())
      {
        T result;
        if (rd.Read())
        {
          result = map(rd);
        }
        else
        {
          result = default(T);
        }

        rd.Dispose();

        return result;
      }
    }

    private static Task<int> TryExecuteNonQueryAsync(this IDbCommand cmd)
    {
      return Task.Run(() => cmd.TryExecuteNonQuery());
    }

    private static Task<IDataReader> TryExecuteReaderAsync(this IDbCommand cmd)
    {
      return Task.Run(() => cmd.TryExecuteReader());
    }

    private static Task<T> TryExecuteScalarAsync<T>(this IDbCommand cmd)
    {
      return Task.Run(() => cmd.TryExecuteScalar<T>());
    }

    private static int TryExecuteNonQuery(this IDbCommand cmd)
    {
      try
      {
        return cmd.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        throw new FailedExecutionException(DbErrorCode.CouldNotExecuteNonQuery, cmd.CommandText, ex);
      }
    }

    private static T TryExecuteScalar<T>(this IDbCommand cmd)
    {
      try
      {
        var result = cmd.ExecuteScalar();
        return result != null ? (T)Convert.ChangeType(result, typeof(T)) : default(T);
      }
      catch (Exception ex)
      {
        throw new FailedExecutionException(DbErrorCode.CouldNotExecuteScalar, cmd.CommandText, ex);
      }
    }

    private static IDataReader TryExecuteReader(this IDbCommand cmd)
    {
      try
      {
        return cmd.ExecuteReader();
      }
      catch (Exception ex)
      {
        throw new FailedExecutionException(DbErrorCode.CouldNotExecuteReader, cmd.CommandText, ex);
      }
    }
  }
}
