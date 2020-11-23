using System;

namespace Spiffy
{    
    /// <summary>
    /// Represents an interface to a specific database.
    /// </summary>
    /// <typeparam name="TFixture"></typeparam>
    public interface IDbFixture<TFixture> : IDbHandler where TFixture : IDbConnectionFactory
    {
        /// <summary>
        /// Create a new IDbBatch, which represents a database unit of work.
        /// </summary>
        /// <returns></returns>
        IDbBatch NewBatch();        
    }
}
