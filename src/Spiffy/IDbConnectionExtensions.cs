using System;
using System.Data;

namespace Spiffy
{
    /// <summary>
    /// IDbConnection extension methods
    /// </summary>
    public static class IDbConnectionExtensions
    {
        /// <summary>
        /// Attempt to open the IDbConnection if it is not already open.
        /// </summary>
        /// <param name="conn"></param>
        public static void TryOpenConnection(this IDbConnection conn)
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
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
        public static IDbTransaction TryBeginTransaction(this IDbConnection connection)
        {
            try
            {
                connection.TryOpenConnection();
                return connection.BeginTransaction();
            }
            catch (Exception ex)
            {
                throw new FailedTransacitonException(ex);
            }
        }
    }
}
