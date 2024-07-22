namespace Spiffy;

using System.Data;

/// <summary>
/// Represents the ability to obtain new unit instances to perform
/// database-bound tasks transactionally, as well as query
/// the database directly.
/// </summary>
public interface IDbFixture : IDbConnectionFactory, IDbHandler
{
}
