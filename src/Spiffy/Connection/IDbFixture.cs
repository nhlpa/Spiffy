using System.Data;

namespace Nhlpa.Sql
{
    /// <summary>
    /// Represents a connection fixture to a specific data source.
    /// </summary>
    public interface IDbFixture
    {
        IDbConnection NewConnection();
    }
}