using System.Collections.Generic;

namespace Spiffy
{
    /// <summary>
    /// A container for database bound parameters.
    /// </summary>
    public class DbParams : Dictionary<string, object>
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

        public static DbParams Empty => new DbParams();
    }
}