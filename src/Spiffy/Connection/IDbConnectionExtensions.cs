using System;
using System.Data;

namespace Nhlpa.Sql
{
    public static class IDbConnectionExtensions
    {
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
