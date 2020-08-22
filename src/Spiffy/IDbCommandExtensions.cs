using System;
using System.Collections.Generic;
using System.Data;

namespace Nhlpa.Sql
{
    internal static class IDbCommandExtensions
    {

        /// <summary>
        /// Execute parameterized query and return rows affected.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        internal static int Exec(this IDbCommand cmd) =>
          cmd.TryExecuteNonQuery();

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
        internal static T Val<T>(this IDbCommand cmd) =>
          cmd.TryExecuteScalar<T>();

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
                return Common.ChangeType<T>(cmd.ExecuteScalar());
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
