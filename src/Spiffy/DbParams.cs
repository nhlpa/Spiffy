using System.Collections.Generic;

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
}