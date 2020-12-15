using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Spiffy
{
    /// <summary>
    /// DbCommand extension methods for async workloads
    /// </summary>
    public static class DbCommandExtensions
    {
        /// <summary>
        /// Asynchronously execute parameterized query with no results
        /// </summary>
        /// <param name="dbCommand"></param>
        /// <returns></returns>
        public async static Task ExecAsync(this DbCommand dbCommand) =>
            await dbCommand.DoVoidAsync(async cmd => await cmd.ExecuteNonQueryAsync());

        /// <summary>
        /// Asynchronously execute parameterized query many times with no results
        /// </summary>
        /// <param name="dbCommand"></param>
        /// <param name="paramList"></param>
        /// <returns></returns>
        public async static Task ExecManyAsync(this DbCommand dbCommand, IEnumerable<DbParams> paramList) =>
            await dbCommand.DoManyAsync(paramList, async cmd => await cmd.ExecuteNonQueryAsync());

        /// <summary>
        /// Asynchronously execute parameterized query, enumerate all records and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbCommand"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public async static Task<IEnumerable<T>> QueryAsync<T>(this DbCommand dbCommand, Func<IDataReader, T> map) =>
            await dbCommand.DoAsync(async cmd =>
            {
                using (var rd = await dbCommand.TryExecuteReaderAsync())
                {
                    var records = new HashSet<T>();

                    while (rd.Read())
                    {
                        records.Add(map(rd));
                    }

                    return records;
                }
            });

        /// <summary>
        /// Asynchronously execute paramterized query, read only first record and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbCommand"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public async static Task<T> QuerySingleAsync<T>(this DbCommand dbCommand, Func<IDataReader, T> map) =>
            await dbCommand.DoAsync(async cmd =>
            {
                using (var rd = await dbCommand.TryExecuteReaderAsync())
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
            });

        /// <summary>
        /// Asynchronously execute paramterized query and manually cursor IDataReader.
        /// </summary>
        public async static Task<IDataReader> ReadAsync(this DbCommand dbCommand) =>
          await dbCommand.TryExecuteReaderAsync();

        private static async Task<T> DoAsync<T>(this DbCommand cmd, Func<DbCommand, Task<T>> func)
        {
            try
            {
                cmd.Connection.TryOpenConnection();
                return await func(cmd);
            }
            catch (Exception ex)
            {
                cmd.TryRollback();
                throw new FailedExecutionException(DbErrorCode.CouldNotExecuteNonQuery, cmd.CommandText, ex);
            }
        }

        private static async Task DoVoidAsync(this DbCommand cmd, Func<DbCommand, Task> func)
        {
            try
            {
                cmd.Connection.TryOpenConnection();
                await func(cmd);
            }
            catch (DbException ex)
            {
                cmd.TryRollback();
                throw new FailedExecutionException(DbErrorCode.CouldNotExecuteNonQuery, cmd.CommandText, ex);
            }
        }

        private static async Task DoManyAsync(this DbCommand cmd, IEnumerable<DbParams> paramList, Func<DbCommand, Task> func)
        {
            try
            {
                cmd.Connection.TryOpenConnection();
                foreach (var param in paramList)
                {
                    cmd.Parameters.Clear();
                    cmd.SetDbParams(param);
                    await cmd.DoVoidAsync(func);
                }
            }
            catch (FailedExecutionException)
            {
                cmd.TryRollback();
                throw;
            }
        }

        private async static Task<IDataReader> TryExecuteReaderAsync(this DbCommand cmd)
        {
            try
            {
                cmd.Connection.TryOpenConnection();
                return await cmd.ExecuteReaderAsync();
            }
            catch (Exception ex)
            {
                throw new FailedExecutionException(DbErrorCode.CouldNotExecuteReader, cmd.CommandText, ex);
            }
        }
    }
}
