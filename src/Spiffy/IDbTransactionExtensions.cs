using System;
using System.Data;

namespace Spiffy
{
    /// <summary>
    /// IDbTransaction extension methods
    /// </summary>
    public static class IDbTransactionExtensions
    {
        /// <summary>
        /// Commit unit of work and cleanup.
        /// Rolls back transaction and throws FailedCommitBatchException on failure
        /// </summary>
        public static void TryCommit(this IDbTransaction tran)
        {
            try
            {
                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw new FailedCommitBatchException(ex);
            }
        }
    }
}
