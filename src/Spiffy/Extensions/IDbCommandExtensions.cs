using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Spiffy
{
    internal static class IDbCommandExtensions
    {
        internal static void AddDbParams(this IDbCommand cmd, DbParams param)
        {
            foreach (var p in param)
            {
                var cmdParam = cmd.CreateParameter();
                cmdParam.ParameterName = p.Key;
                cmdParam.Value = p.Value ?? DBNull.Value;
                cmd.Parameters.Add(cmdParam);
            }
        }

        /// <summary>
        /// Execute parameterized query and return rows affected.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        internal static int Exec(this IDbCommand cmd)
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

        /// <summary>
        /// Execute parameterized query, enumerate all records and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmd"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        internal static IEnumerable<T> Query<T>(this IDbCommand cmd, Func<IDataReader, T> map)
        {
            using (var rd = cmd.TryExecuteReader())
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
        /// Execute paramterized query, read only first record and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmd"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        internal static T QuerySingle<T>(this IDbCommand cmd, Func<IDataReader, T> map)
        {
            using (var rd = cmd.TryExecuteReader())
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
        /// Execute paramterized query and manually cursor IDataReader.
        /// </summary>
        internal static IDataReader Read(this IDbCommand cmd) =>
          cmd.TryExecuteReader();

        /// <summary>
        /// Execute parameterized query and return single-value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmd"></param>
        /// <returns></returns>
        internal static object Scalar(this IDbCommand cmd)
        {
            try
            {
                return cmd.ExecuteScalar();
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
