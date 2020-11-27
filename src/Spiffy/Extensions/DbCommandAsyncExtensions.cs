using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Spiffy
{
    internal static class DbCommandAsyncExtensions
    {        
        /// <summary>
        /// Asynchronously execute parameterized query and return rows affected.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        internal async static Task<int> ExecAsync(this DbCommand cmd)
        {
            try
            {
                return await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new FailedExecutionException(DbErrorCode.CouldNotExecuteNonQuery, cmd.CommandText, ex);
            }
        }

        /// <summary>
        /// Asynchronously execute parameterized query, enumerate all records and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmd"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        internal async static Task<IEnumerable<T>> QueryAsync<T>(this DbCommand cmd, Func<IDataReader, T> map)
        {
            using (var rd = await cmd.TryExecuteReaderAsync())
            {
                var records = new HashSet<T>();

                while (rd.Read())
                {
                    records.Add(map(rd));
                }

                return records;
            }
        }

        /// <summary>
        /// Asynchronously execute paramterized query, read only first record and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmd"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        internal async static Task<T> QuerySingleAsync<T>(this DbCommand cmd, Func<IDataReader, T> map)
        {
            using (var rd = await cmd.TryExecuteReaderAsync())
            {
                if (rd.Read())
                {
                    return map(rd);
                }
                else
                {
                    return default;
                }
            }
        }

        /// <summary>
        /// Asynchronously execute paramterized query and manually cursor IDataReader.
        /// </summary>
        internal async static Task<IDataReader> ReadAsync(this DbCommand cmd) =>
          await cmd.TryExecuteReaderAsync();

        /// <summary>
        /// Asynchronously execute parameterized query and return single-value.
        /// </summary>        
        /// <param name="cmd"></param>
        /// <returns></returns>
        internal async static Task<object> ScalarAsync(this DbCommand cmd)
        {
            try
            {
                return await cmd.ExecuteScalarAsync();
            }
            catch (Exception ex)
            {
                throw new FailedExecutionException(DbErrorCode.CouldNotExecuteScalar, cmd.CommandText, ex);
            }
        }

        private async static Task<IDataReader> TryExecuteReaderAsync(this DbCommand cmd)
        {
            try
            {
                return await cmd.ExecuteReaderAsync();
            }
            catch (Exception ex)
            {
                throw new FailedExecutionException(DbErrorCode.CouldNotExecuteReader, cmd.CommandText, ex);
            }
        }
    }
}
