namespace Spiffy;

using System;
using System.Data;

/// <summary>
/// A fluent API for generating IDbCommand instances
/// </summary>
/// <remarks>
/// Create an empty DbCommandBuilder
/// </remarks>
/// <param name="conn"></param>
public sealed class DbCommandBuilder(IDbConnection conn)
{
    private readonly IDbConnection _conn = conn ?? throw new ArgumentNullException(nameof(conn));
    private string? _commandText;
    private int? _commandTimeout;
    private CommandType _commandType = System.Data.CommandType.Text;
    private DbParams _dbParams = [];
    private IDbTransaction? _transaction;

    /// <summary>
    /// Create an initialized DbCommandBuilder
    /// </summary>
    /// <param name="conn"></param>
    /// <param name="commandText"></param>
    /// <param name="param"></param>
    public DbCommandBuilder(IDbConnection conn, string commandText, DbParams? param = null) : this(conn)
    {
        CommandText(commandText);
        DbParams(param);
    }

    /// <summary>
    /// Create an empty DbCommandBuilder from IDbTransaction
    /// </summary>
    /// <param name="tran"></param>
    public DbCommandBuilder(IDbTransaction tran) : this(tran.Connection!)
    {
        Transaction(tran);
    }

    /// <summary>
    /// Create an initialized DbCommandBuilder
    /// </summary>
    /// <param name="tran"></param>
    /// <param name="commandText"></param>
    /// <param name="param"></param>
    public DbCommandBuilder(IDbTransaction tran, string commandText, DbParams? param = null) : this(tran)
    {
        CommandText(commandText);
        DbParams(param);
    }

    /// <summary>
    /// Generate IDbCommand
    /// </summary>
    /// <returns></returns>
    public IDbCommand Build()
    {
        var command = _conn.CreateCommand();
        command.CommandType = _commandType;
        command.CommandText = _commandText;

        if (_commandTimeout.HasValue)
            command.CommandTimeout = _commandTimeout.Value;

        if (_dbParams != null)
            command.SetDbParams(_dbParams);

        if (_transaction != null)
            command.Transaction = _transaction;

        return command;
    }

    /// <summary>
    /// Set statement text
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public DbCommandBuilder CommandText(string query)
    {
        _commandText = query;
        return this;
    }

    /// <summary>
    /// Set command type (default: CommandType.Text)
    /// </summary>
    /// <param name="commandType"></param>
    /// <returns></returns>
    public DbCommandBuilder CommandType(CommandType commandType)
    {
        _commandType = commandType;
        return this;
    }

    /// <summary>
    /// Set command timeout
    /// </summary>
    /// <param name="commandTimeout"></param>
    /// <returns></returns>
    public DbCommandBuilder Timeout(int commandTimeout)
    {
        _commandTimeout = commandTimeout;
        return this;
    }

    /// <summary>
    /// Add DbParams
    /// </summary>
    /// <param name="dbParams"></param>
    /// <returns></returns>
    public DbCommandBuilder DbParams(DbParams? dbParams)
    {
        _dbParams = dbParams ?? [];
        return this;
    }

    /// <summary>
    /// Set transaction
    /// </summary>
    /// <param name="transaction"></param>
    /// <returns></returns>
    public DbCommandBuilder Transaction(IDbTransaction transaction)
    {
        _transaction = transaction;
        return this;
    }
}
