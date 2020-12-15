using System;

namespace Spiffy
{
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
    public class DbException : Exception
    {
        /// <summary>
        /// The error DbErrorCode
        /// </summary>
        public readonly DbErrorCode ErrorCode;

        /// <summary>
        /// Initializes a new instance of the DbException class
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="message"></param>
        /// <param name="innerEx"></param>
        public DbException(DbErrorCode errorCode, string message, Exception innerEx = null)
          : base(message, innerEx)
        {
            ErrorCode = errorCode;
        }
    }

    /// <summary>
    /// Represents a failure to open a database connection
    /// </summary>
    public class CouldNotOpenConnectionException : DbException
    {
        /// <summary>
        /// Initializes a new instance of the CouldNotOpenConnectionException class
        /// </summary>
        /// <param name="ex"></param>
        public CouldNotOpenConnectionException(Exception ex)
          : base(DbErrorCode.CouldNotOpenConnection, $"Could not establish database connection.", ex) { }
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
    public class FailedTransacitonException : DbException
    {
        /// <summary>
        /// Initializes a new instance of the FailedTransacitonException class
        /// </summary>
        /// <param name="ex"></param>
        public FailedTransacitonException(Exception ex)
          : base(DbErrorCode.CouldNotBeginTransaction, $"Could not begin transaction.", ex) { }
    }

    /// <summary>
    /// Represents a failure to begin a database batch
    /// </summary>
    public class FailedBeginBatchException : DbException
    {
        /// <summary>
        /// Initializes a new instance of the FailedBeginBatchException class
        /// </summary>
        /// <param name="ex"></param>
        public FailedBeginBatchException(Exception ex)
          : base(DbErrorCode.CouldNotBeginBatch, $"Could not begin batch.", ex) { }
    }

    /// <summary>
    /// Represents a failure to commit a database transaction
    /// </summary>
    public class FailedCommitBatchException : DbException
    {
        /// <summary>
        /// Initializes a new instance of the FailedCommitBatchException class
        /// </summary>
        /// <param name="ex"></param>
        public FailedCommitBatchException(Exception ex)
          : base(DbErrorCode.CouldNotCommitBatch, $"Could not commit batch.", ex) { }
    }

    /// <summary>
    /// Represents a failure to execute a database command.
    /// </summary>
    public class FailedExecutionException : DbException
    {
        /// <summary>
        /// Initializes a new instance of the FailedExecutionException class.
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="sql"></param>
        /// /// <param name="ex"></param>
        public FailedExecutionException(DbErrorCode errorCode, string sql, Exception ex)
          : base(errorCode, sql, ex) { }
    }
}
