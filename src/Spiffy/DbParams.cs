using System.Collections.Generic;
using System.Linq;

namespace Spiffy
{
    /// <summary>
    /// A container for database bound parameters.
    /// </summary>
    public class DbParams : Dictionary<string, object>
    {
      /// <summary>
      /// Initialize a new instance of the DbParams class
      /// </summary>
      public DbParams()
      {
      }

      /// <summary>
      /// Initialize a new instance of the DbParams class from a key and value
      /// </summary>
      public DbParams(string key, object value)
      {
          if (!this.ContainsKey(key))
          {
              this[key] = value;
          }
      }
    }

    /// <summary>
    /// Extensions for DbParams
    /// </summary>
    public static class DbParamsExtensions
    {

      /// <summary>
      /// Returns a new dictionary of this, others merged leftward.
      /// </summary>
      public static DbParams Add(this DbParams p1, DbParams p2)
      {
        p2.ToList()
          .ForEach(x => {
            if(!p1.ContainsKey(x.Key))
            {
              p1.Add(x.Key, x.Value);
            }
        });

        return p1;
      }
    }
}