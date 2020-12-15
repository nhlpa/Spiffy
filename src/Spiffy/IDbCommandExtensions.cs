using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Spiffy
{
    /// <summary>
    /// IDbCommand extension methods
    /// </summary>
    public static class IDbCommandExtensions
    {
        /// <summary>
        /// Execute parameterized query with no results
        /// </summary>
        /// <param name="dbCommand"></param>
        /// <returns></returns>
        public static void Exec(this IDbCommand dbCommand) =>
            dbCommand.DoVoid(cmd => cmd.ExecuteNonQuery());

        /// <summary>
        /// Execute parameterized query many times with no results
        /// </summary>
        /// <param name="dbCommand"></param>
        /// <param name="paramList"></param>
        /// <returns></returns>
        public static void ExecMany(this IDbCommand dbCommand, IEnumerable<DbParams> paramList) =>
            dbCommand.DoMany(paramList, cmd => cmd.ExecuteNonQuery());

        /// <summary>
        /// Execute parameterized query, enumerate all records and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbCommand"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public static IEnumerable<T> Query<T>(this IDbCommand dbCommand, Func<IDataReader, T> map) =>
            dbCommand.Do(cmd =>
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
            });

        /// <summary>
        /// Execute paramterized query, read only first record and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbCommand"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public static T QuerySingle<T>(this IDbCommand dbCommand, Func<IDataReader, T> map) =>
            dbCommand.Do(cmd =>
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
            });

        /// <summary>
        /// Execute paramterized query and manually cursor IDataReader.
        /// </summary>
        public static IDataReader Read(this IDbCommand cmd) =>
          cmd.TryExecuteReader();

        //
        // Async

        /// <summary>
        /// Asynchronously execute parameterized query with no results
        /// </summary>
        /// <param name="dbCommand"></param>
        /// <returns></returns>
        public static Task ExecAsync(this IDbCommand dbCommand) =>
            (dbCommand as DbCommand).ExecAsync();

        /// <summary>
        /// Asynchronously execute parameterized query many times with no results
        /// </summary>
        /// <param name="dbCommand"></param>
        /// <param name="paramList"></param>
        /// <returns></returns>
        public static Task ExecManyAsync(this IDbCommand dbCommand, IEnumerable<DbParams> paramList) =>
            (dbCommand as DbCommand).ExecManyAsync(paramList); 

        /// <summary>
        /// Asynchronously execute parameterized query, enumerate all records and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbCommand"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public static Task<IEnumerable<T>> QueryAsync<T>(this IDbCommand dbCommand, Func<IDataReader, T> map) =>
            (dbCommand as DbCommand).QueryAsync(map);

        /// <summary>
        /// Asynchronously execute paramterized query, read only first record and apply mapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbCommand"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public static Task<T> QuerySingleAsync<T>(this IDbCommand dbCommand, Func<IDataReader, T> map) =>
            (dbCommand as DbCommand).QuerySingleAsync(map);

        /// <summary>
        /// Asynchronously execute paramterized query and manually cursor IDataReader.
        /// </summary>
        public static Task<IDataReader> ReadAsync(this IDbCommand dbCommand) =>
          (dbCommand as DbCommand).ReadAsync();

        internal static void SetDbParams(this IDbCommand cmd, DbParams param)
        {
            if (param != null)
            {
                foreach (var p in param)
                {
                    var cmdParam = cmd.CreateParameter();
                    cmdParam.ParameterName = p.Key;
                    cmdParam.Value = p.Value ?? DBNull.Value;
                    cmd.Parameters.Add(cmdParam);
                }
            }
        }

        internal static void TryRollback(this IDbCommand cmd)
        {
            if (cmd.Transaction != null)
            {
                cmd.Transaction.Rollback();
            }
        }

        private static T Do<T>(this IDbCommand cmd, Func<IDbCommand, T> func)
        {
            try
            {
                cmd.Connection.TryOpenConnection();
                return func(cmd);
            }
            catch (Exception ex)
            {
                cmd.TryRollback();
                throw new FailedExecutionException(DbErrorCode.CouldNotExecuteNonQuery, cmd.CommandText, ex);
            }
        }

        private static void DoVoid(this IDbCommand cmd, Action<IDbCommand> func)
        {
            try
            {
                cmd.Connection.TryOpenConnection();
                func(cmd);
            }
            catch (Exception ex)
            {
                cmd.TryRollback();
                throw new FailedExecutionException(DbErrorCode.CouldNotExecuteNonQuery, cmd.CommandText, ex);
            }
        }

        private static void DoMany(this IDbCommand cmd, IEnumerable<DbParams> paramList, Action<IDbCommand> func)
        {
            try
            {
                cmd.Connection.TryOpenConnection();

                foreach (var param in paramList)
                {
                    cmd.Parameters.Clear();
                    cmd.SetDbParams(param);
                    cmd.DoVoid(func);
                }
            }
            catch (Exception ex)
            {
                cmd.TryRollback();
                throw new FailedExecutionException(DbErrorCode.CouldNotExecuteNonQuery, cmd.CommandText, ex);
            }
        }

        private static IDataReader TryExecuteReader(this IDbCommand cmd)
        {
            try
            {
                cmd.Connection.TryOpenConnection();
                return cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw new FailedExecutionException(DbErrorCode.CouldNotExecuteReader, cmd.CommandText, ex);
            }
        }
    }
}
