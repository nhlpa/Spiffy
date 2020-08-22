using System;

namespace Nhlpa.Sql
{    
    /// <summary>
    /// Represents an interface to a specific database.
    /// </summary>
    /// <typeparam name="TFixture"></typeparam>
    public interface IDbContext<TFixture> : IDbHandler where TFixture : IDbFixture
    {
        /// <summary>
        /// Create a new IDbBatch, which represents a database unit of work.
        /// </summary>
        /// <returns></returns>
        IDbBatch NewBatch();        
    }
}
