using System.Data;

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
        internal static IDbCommand NewCommand(this IDbTransaction tran, string sql, DbParams param = null)
        {            
            var cmd = tran.Connection.CreateCommand();
            cmd.Transaction = tran;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;

            if (param != null && param.Count > 0)
                cmd.AddDbParams(param);

            return cmd;
        }
    }
}
