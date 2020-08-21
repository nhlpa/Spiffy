using System.Data;

namespace Nhlpa.Sql
{
  public interface IDbFixture
  {
    IDbConnection NewConnection();
  }
}