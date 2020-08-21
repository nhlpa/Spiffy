using System;
using System.Data;

namespace Nhlpa.Sql
{
  public static class IDbConnectionExtensions
  {
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
