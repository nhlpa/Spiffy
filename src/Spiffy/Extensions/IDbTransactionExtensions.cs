using System;
using System.Data;
using System.Data.Common;

namespace Spiffy
{
    internal static class IDbTransactionExtensions
    {
        /// <summary>
        /// Create a new IDbCommand.
        /// </summary>
        /// <param name="tran"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        internal static DbCommand NewCommand(this IDbTransaction tran, string sql, DbCommandParams param = null)
        {            
            var cmd = tran.Connection.CreateCommand();
            cmd.Transaction = tran;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;

            foreach (var p in param ?? new DbCommandParams())
            {
                var cmdParam = cmd.CreateParameter();
                cmdParam.ParameterName = p.Key;
                cmdParam.Value = p.Value ?? DBNull.Value;
                cmd.Parameters.Add(cmdParam);
            }

            return cmd as DbCommand;
        }
    }
}
