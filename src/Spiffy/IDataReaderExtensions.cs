using System;
using System.Data;

namespace Spiffy {
	public static class IDataReaderExtensions {
		/// <summary>
		/// Safely retrieve and convert value. 
		/// Throws ArgumentNullException on NULL value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="rd"></param>
		/// <param name="fieldname"></param>
		/// <returns></returns>
		public static T Get<T>(this IDataReader rd, string fieldname) {
			var value = rd.GetValue(fieldname);

			if (value == null)
				return default;

			return (T)Convert.ChangeType(value, typeof(T));
		}

		/// <summary>
		/// Safely retrieve and convert nullable value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="rd"></param>
		/// <param name="fieldname"></param>
		/// <returns></returns>
		public static T? GetNullable<T>(this IDataReader rd, string fieldname)
			where T : struct {
			var value = rd.GetValue(fieldname);

			if (value == null)
				return default;

			return (T?)Convert.ChangeType(value, typeof(T));
		}

		private static object GetValue(this IDataReader rd, string fieldname) {
			if (rd == null)
				return null;

			try {
				var i = rd.GetOrdinal(fieldname);

				if (rd.IsDBNull(i)) {
					return null;
				}

				return rd.GetValue(i);
			} catch (IndexOutOfRangeException ex) {
				throw new IndexOutOfRangeException(
					$"The column '{fieldname}' was not found.", ex);
			}
		}
	}
}
