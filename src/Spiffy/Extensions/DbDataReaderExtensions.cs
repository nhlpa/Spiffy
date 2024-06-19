using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Spiffy
{
    /// <summary>
    /// DbDataReader extension methods
    /// </summary>
    public static class DbDataReaderExtensions
    {
        /// <summary>
        /// Asynchronously map DbDataReader to IEnumerable of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rd"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<T>> MapAsync<T>(this DbDataReader rd, Func<DbDataReader, T> map)
        {
            var records = new List<T>();

            while (await rd.ReadAsync())
            {
                records.Add(map(rd));
            }

            return records;
        }

        /// <summary>
        /// Asynchronously map first iteration of DbDataReader to T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rd"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public static async Task<T> MapFirstAsync<T>(this DbDataReader rd, Func<DbDataReader, T> map)
        {
            if (await rd.ReadAsync())
            {
                return map(rd);
            }
            else
            {
                return default;
            }
        }

        /// <summary>
        /// Asynchronously map next iteration of DbDataReader to IEnumerable of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rd"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<T>> MapNextAsync<T>(this DbDataReader rd, Func<DbDataReader, T> map) =>
            await (rd.NextResult() ? rd.MapAsync(map) : Task.FromResult(Enumerable.Empty<T>()));

        /// <summary>
        /// Asynchronously map next iteration of DbDataReader to T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rd"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public static async Task<T> MapFirstNextAsync<T>(this DbDataReader rd, Func<DbDataReader, T> map) =>
            await (rd.NextResult() ? rd.MapFirstAsync(map) : Task.FromResult<T>(default));
    }
}
