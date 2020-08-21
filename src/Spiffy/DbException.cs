using System;

namespace Nhlpa.Sql
{
  public enum DbErrorCode
  {
    CouldNotOpenConnection = 1000,
    CouldNotCloseConnection = 1001,
    ConnectionBusy = 1002,

    CouldNotBeginTransaction = 2000,

    CouldNotBeginBatch = 3000,
    CouldNotCommitBatch = 3001,

    CouldNotExecuteNonQuery = 4000,
    CouldNotExecuteScalar = 4001,
    CouldNotExecuteReader = 4002
  }

  public class DbException : Exception
  {
    public readonly DbErrorCode ErrorCode;

    public DbException(DbErrorCode errorCode, string message, Exception innerEx = null)
      : base(message, innerEx)
    {
      ErrorCode = errorCode;
    }
  }

  public class CouldNotOpenConnectionException : DbException
  {
    public CouldNotOpenConnectionException(Exception ex)
      : base(DbErrorCode.CouldNotOpenConnection, $"Could not establish database connection.", ex) { }
  }

  public class ConnectionBusyException : DbException
  {
    public ConnectionBusyException()
      : base(DbErrorCode.ConnectionBusy, $"The connection is not currently available to open.")
    {
    }
  }

  public class FailedTransacitonException : DbException
  {
    public FailedTransacitonException(Exception ex)
      : base(DbErrorCode.CouldNotBeginTransaction, $"Could not begin transaction.", ex) { }
  }

  public class FailedBeginBatchException : DbException
  {
    public FailedBeginBatchException(Exception ex)
      : base(DbErrorCode.CouldNotBeginBatch, $"Could not begin batch.", ex) { }
  }

  public class FailedCommitBatchException : DbException
  {
    public FailedCommitBatchException(Exception ex)
      : base(DbErrorCode.CouldNotCommitBatch, $"Could not commit batch.", ex) { }
  }

  public class FailedExecutionException : DbException
  {
    public FailedExecutionException(DbErrorCode errorCode, string sql, Exception ex)
      : base(errorCode, sql, ex) { }
  }
}
