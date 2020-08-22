using System;
using System.Data;

namespace Nhlpa.Sql
{
    /// <summary>
    /// Database unit of work.
    /// </summary>
    public interface IDbBatch : IDbHandler, IDisposable
    {
        /// <summary>
        /// Commit transaction & cleanup.
        /// </summary>
        void Commit();

        /// <summary>
        /// Rollback transaction & cleanup.
        /// <summary>
        void Rollback();

        /// <summary>
        /// Execute paramterized query and manually cursor IDataReader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>		
        /// <param name="param"></param>
        /// <returns></returns>
        IDataReader Read(string sql, DbParams param = null);
    }
}