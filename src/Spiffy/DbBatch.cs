﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spiffy
{
  public class DbBatch : IDbBatch
  {
    public DbBatch(IDbConnection conn, IDbTransaction tran)
    {
      Connection = conn ?? throw new ArgumentNullException(nameof(conn));
      Transaction = tran ?? throw new ArgumentNullException(nameof(tran));
    }

    public IDbConnection Connection { get; }

    public IDbTransaction Transaction { get; }

    /// <summary>
    /// Attempt to commit the work within the batch and dispose of volatile resources.
    /// </summary>
    public void Commit()
    {
      try
      {
        Transaction.Commit();
      }
      catch (Exception ex)
      {
        Transaction.Rollback();
        throw new FailedCommitBatchException(ex);
      }
      finally
      {
        Reset();
      }
    }

    /// <summary>
    /// Undo changes made within the batch. 
    /// Called automatically from Commit() when exception is thrown during execution.
    /// </summary>
    public void Rollback()
    {
      try
      {
        Transaction.Rollback();
      }
      finally
      {
        Reset();
      }
    }

    /// <summary>
    /// Dispose of volatile resources.
    /// </summary>
    private void Reset()
    {
      Connection.TryClose();
      Transaction.Dispose();
    }
  }
}
