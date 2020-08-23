using System;
using System.Data;

namespace Spiffy
{
    public static class IDataReaderExtensions
    {
        /// <summary>
        /// Safely retrieve and convert value. 
        /// Throws IndexOutOfRangeException on missing fieldName.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rd"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static T Get<T>(this IDataReader rd, string fieldName)
        {
            return Common.ChangeType<T>(rd.GetValue(fieldName));
        }

        private static object GetValue(this IDataReader rd, string fieldName)
        {
            if (rd == null)
            {
                return null;
            }

            try
            {
                var i = rd.GetOrdinal(fieldName);

                if (rd.IsDBNull(i))
                {
                    return null;
                }

                return rd.GetValue(i);
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new IndexOutOfRangeException($"The column '{fieldName}' was not found.", ex);
            }
        }
    }
}
