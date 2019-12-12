using System.Collections.Generic;

namespace Spiffy {
	/// <summary>
	/// Container for sending key/value pairs via `IDbCommand`
	/// </summary>
	public class DbParams : Dictionary<string, object>, IDbParams {
		public DbParams() { }
		public DbParams(string key, object value) => this[key] = value;
	}
}
