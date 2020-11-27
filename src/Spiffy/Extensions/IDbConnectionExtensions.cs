using System;
using System.Data;

namespace Spiffy
{    
    internal static class IDbConnectionExtensions
    {
        /// <summary>
        /// Create a new IDbCommand.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        internal static IDbCommand NewCommand(this IDbConnection conn, IDbTransaction trans, string sql, DbParams param = null)
        {
            var cmd = conn.CreateCommand();
            cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;

            if (param != null && param.Count > 0)
                cmd.AddDbParams(param);

            return cmd;
        }

        /// <summary>
        /// Attempt to open the IDbConnection if it is not already open.
        /// </summary>
        /// <param name="conn"></param>
        internal static void TryOpenConnection(this IDbConnection conn)
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

        /// <summary>
        /// Attempt to begin a IDbTransaction.
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        internal static IDbTransaction TryBeginTransaction(this IDbConnection connection)
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
    }
}
