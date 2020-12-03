using System;
using System.Data;

namespace Spiffy
{
    public interface IDbCommandBuilder
    {
        IDbCommand Build();
        IDbCommandBuilder SetCommandText(string query);        
        IDbCommandBuilder SetCommandType(CommandType commandType);
        IDbCommandBuilder SetCommandTimeout(int commandTimeout);        
        IDbCommandBuilder AddDbParams(DbParams dbParams);
        IDbCommandBuilder UsingTransaction(IDbTransaction transaction);
    }

    public class DbCommandBuilder : IDbCommandBuilder
    {
        private readonly IDbConnection _conn;
        private string _commandText;
        private int? _commandTimeout;
        private CommandType _commandType;        
        private DbParams _dbParams;
        private IDbTransaction _transaction;

        public DbCommandBuilder(IDbConnection conn)
        {
            _conn = conn ?? throw new ArgumentNullException(nameof(conn));
            _commandType = CommandType.Text;
        }

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

        public IDbCommandBuilder SetCommandText(string query)
        {
            _commandText = query;
            return this;
        }

        public IDbCommandBuilder SetCommandType(CommandType commandType)
        {
            _commandType = commandType;
            return this;
        }

        public IDbCommandBuilder SetCommandTimeout(int commandTimeout)
        {
            _commandTimeout = commandTimeout;
            return this;
        }

        public IDbCommandBuilder AddDbParams(DbParams dbParams)
        {
            _dbParams = dbParams;
            return this;
        }

        public IDbCommandBuilder UsingTransaction(IDbTransaction transaction)
        {
            _transaction = transaction;
            return this;
        }
    }
}