using System;
using System.Collections.Generic;
using System.Data;

namespace Nhlpa.Sql
{
  public class DbBatch : IDbBatch
  {
    private IDbConnection _connection;
    private IDbTransaction _transaction;

    public DbBatch(IDbConnection connection, IDbTransaction transaction)
    {
      _transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
      _connection = connection ?? throw new ArgumentNullException(nameof(connection));
    }

    public void Commit()
    {
      try
      {
        _transaction.Commit();
      }
      catch (Exception ex)
      {
        _transaction.Rollback();
        throw new FailedCommitBatchException(ex);
      }
      finally
      {
        Reset();
      }
    }

    public void Rollback()
    {
      try
      {
        _transaction.Rollback();
      }
      finally
      {
        Reset();
      }
    }

    public int Exec(string sql, IDbParams param = null)
    {
      try
      {
        return _transaction.NewCommand(sql, param).Exec();
      }
      catch (FailedExecutionException)
      {
        Rollback();
        throw;
      }
    }

    public IEnumerable<T> Query<T>(string sql, Func<IDataReader, T> map, IDbParams param = null)
    {
      try
      {
        return _transaction.NewCommand(sql, param).Query(map);
      }
      catch (FailedExecutionException)
      {
        Rollback();
        throw;
      }
    }

    public T QuerySingle<T>(string sql, Func<IDataReader, T> map, IDbParams param = null)
    {
      try
      {
        return _transaction.NewCommand(sql, param).QuerySingle(map);
      }
      catch (FailedExecutionException)
      {
        Rollback();
        throw;
      }
    }

    public IDataReader Read(string sql, IDbParams param = null)
    {
      try
      {
        return _transaction.NewCommand(sql, param).Read();
      }
      catch (FailedExecutionException)
      {
        Rollback();
        throw;
      }
    }

    public T Val<T>(string sql, IDbParams param = null)
    {
      try
      {
        return _transaction.NewCommand(sql, param).Val<T>();
      }
      catch (FailedExecutionException)
      {
        Rollback();
        throw;
      }
    }

    private void Reset()
    {
      _connection.Close();
      _connection.Dispose();
      _transaction.Dispose();
    }
  }
}