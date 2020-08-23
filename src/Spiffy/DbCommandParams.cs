using System.Collections.Generic;

namespace Spiffy
{
    /// <summary>
    /// A container for database bound parameters.
    /// </summary>
    public class DbCommandParams : Dictionary<string, object>
    {
        public DbCommandParams()
        {
        }

        public DbCommandParams(string key, object value)
        {
            if (!this.ContainsKey(key))
            {
                this[key] = value;
            }
        }
    }
}