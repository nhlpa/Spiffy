using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Spiffy
{
    internal static class DbCommandExtensions
    {        
        /// <summary>
        /// Execute parameterized query and return rows affected.
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
    }
}
