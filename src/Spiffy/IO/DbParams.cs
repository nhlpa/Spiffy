using System.Collections.Generic;

namespace Nhlpa.Sql
{
  public class DbParams : Dictionary<string, object>, IDbParams
  {
    public DbParams()
    {
    }

    public DbParams(string key, object value)
    {
      if (!this.ContainsKey(key))
      {
        this[key] = value;
      }
    }
  }
}