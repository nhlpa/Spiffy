using System.Data;

namespace Spiffy
{
  internal static class IDbTransactionExtensions
  {
    internal static IDbCommand NewCommand(this IDbTransaction tran, string sql, IDbParams param = null)
    {
      var cmd = tran.Connection.CreateCommand();
      cmd.Transaction = tran;
      cmd.CommandType = CommandType.Text;
      cmd.CommandText = sql;

      foreach (var p in param ?? new DbParams())
      {
        var cmdParam = cmd.CreateParameter();
        cmdParam.ParameterName = p.Key;
        cmdParam.Value = p.Value;
        cmd.Parameters.Add(cmdParam);
      }

      return cmd;
    }
  }
}
