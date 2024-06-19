namespace Spiffy;

using System;

/// <summary>
/// Errors thrown during database interaction.
/// </summary>
public enum DbErrorCode
{
    /// <summary>
    /// Could Not Open Connection
    /// </summary>
    CouldNotOpenConnection = 1000,
    /// <summary>
    /// Could Not Close Connection
    /// </summary>
    CouldNotCloseConnection = 1001,
    /// <summary>
    /// Connection Busy
    /// </summary>
    ConnectionBusy = 1002,

    /// <summary>
    /// Could Not Begin Transaction
    /// </summary>
    CouldNotBeginTransaction = 2000,

    /// <summary>
    /// Could Not Begin Batch
    /// </summary>
    CouldNotBeginBatch = 3000,
    /// <summary>
    /// Could Not Commit Batch
    /// </summary>
    CouldNotCommitBatch = 3001,

    /// <summary>
    /// Could Not Execute Non Query
    /// </summary>
    CouldNotExecuteNonQuery = 4000,
    /// <summary>
    /// Could Not Execute Scalar
    /// </summary>
    CouldNotExecuteScalar = 4001,
    /// <summary>
    /// Could Not Execute Reader
    /// </summary>
    CouldNotExecuteReader = 4002,

    /// <summary>
    /// Could Not Cast Value
    /// </summary>
    CouldNotCastValue = 5000,
}

/// <summary>
/// Represents errors that occur during database interactions.
/// </summary>
/// <remarks>
/// Initializes a new instance of the DbException class
/// </remarks>
/// <param name="errorCode"></param>
/// <param name="message"></param>
/// <param name="innerEx"></param>
public class DbException(DbErrorCode errorCode, string message, Exception? innerEx = null) : Exception(message, innerEx)
{
    /// <summary>
    /// The error DbErrorCode
    /// </summary>
    public readonly DbErrorCode ErrorCode = errorCode;
}

/// <summary>
/// Represents a failure to open a database connection
/// </summary>
/// <remarks>
/// Initializes a new instance of the CouldNotOpenConnectionException class
/// </remarks>
/// <param name="ex"></param>
public class CouldNotOpenConnectionException(Exception ex) : DbException(DbErrorCode.CouldNotOpenConnection, $"Could not establish database connection.", ex)
{
}

/// <summary>
/// Represents a failure due to a busy database connection
/// </summary>
public class ConnectionBusyException : DbException
{
    /// <summary>
    /// Initializes a new instance of the ConnectionBusyException class
    /// </summary>
    public ConnectionBusyException()
      : base(DbErrorCode.ConnectionBusy, $"The connection is not currently available to open.")
    {
    }
}

/// <summary>
/// Represents a failure to begin a new database transaction
/// </summary>
/// <remarks>
/// Initializes a new instance of the FailedTransacitonException class
/// </remarks>
/// <param name="ex"></param>
public class FailedTransacitonException(Exception ex) : DbException(DbErrorCode.CouldNotBeginTransaction, $"Could not begin transaction.", ex)
{
}

/// <summary>
/// Represents a failure to begin a database batch
/// </summary>
/// <remarks>
/// Initializes a new instance of the FailedBeginBatchException class
/// </remarks>
/// <param name="ex"></param>
public class FailedBeginBatchException(Exception ex) : DbException(DbErrorCode.CouldNotBeginBatch, $"Could not begin batch.", ex)
{
}

/// <summary>
/// Represents a failure to commit a database transaction
/// </summary>
/// <remarks>
/// Initializes a new instance of the FailedCommitBatchException class
/// </remarks>
/// <param name="ex"></param>
public class FailedCommitBatchException(Exception ex) : DbException(DbErrorCode.CouldNotCommitBatch, $"Could not commit batch.", ex)
{
}

/// <summary>
/// Represents a failure to execute a database command.
/// </summary>
/// <remarks>
/// Initializes a new instance of the FailedExecutionException class.
/// </remarks>
/// <param name="errorCode"></param>
/// <param name="sql"></param>
/// /// <param name="ex"></param>
public class FailedExecutionException(DbErrorCode errorCode, string sql, Exception ex) : DbException(errorCode, sql, ex)
{
}
