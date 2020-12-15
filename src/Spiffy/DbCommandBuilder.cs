using System;
using System.Data;

namespace Spiffy
{    
    /// <summary>
    /// A fluent API for generating IDbCommand instances
    /// </summary>
    public class DbCommandBuilder
    {
        private readonly IDbConnection _conn;
        private string _commandText;
        private int? _commandTimeout;
        private CommandType _commandType;        
        private DbParams _dbParams;
        private IDbTransaction _transaction;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conn"></param>
        public DbCommandBuilder(IDbConnection conn)
        {
            _conn = conn ?? throw new ArgumentNullException(nameof(conn));
            _commandType = CommandType.Text;
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
                command.AddDbParams(_dbParams);

            if (_transaction != null)
                command.Transaction = _transaction;
            
            return command;
        }

        /// <summary>
        /// Set statement text
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DbCommandBuilder SetCommandText(string query)
        {
            _commandText = query;
            return this;
        }

        /// <summary>
        /// Set command type (default: CommandType.Text)
        /// </summary>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public DbCommandBuilder SetCommandType(CommandType commandType)
        {
            _commandType = commandType;
            return this;
        }

        /// <summary>
        /// Set command timeout
        /// </summary>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public DbCommandBuilder SetCommandTimeout(int commandTimeout)
        {
            _commandTimeout = commandTimeout;
            return this;
        }

        /// <summary>
        /// Add DbParams
        /// </summary>
        /// <param name="dbParams"></param>
        /// <returns></returns>
        public DbCommandBuilder AddDbParams(DbParams dbParams)
        {
            _dbParams = dbParams;
            return this;
        }

        /// <summary>
        /// Set transaction
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public DbCommandBuilder UsingTransaction(IDbTransaction transaction)
        {
            _transaction = transaction;
            return this;
        }
    }
}