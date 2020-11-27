using System;
using System.Data;

namespace Spiffy
{
    /// <summary>
    /// IDataReader extension methods
    /// </summary>
    public static class IDataReaderExtensions
    {
        /// <summary>
        /// Get string from IDataReader
        /// </summary>
        public static string GetString(this IDataReader rd, string field) => rd.GetValueByField(field, rd.GetString);

        /// <summary>
        /// Get char from IDataReader
        /// </summary>
        public static char GetChar(this IDataReader rd, string field) => rd.GetValueByField(field, rd.GetChar);

        /// <summary>
        /// Get bool from IDataReader
        /// </summary>
        public static bool GetBoolean(this IDataReader rd, string field) => rd.GetValueByField(field, rd.GetBoolean);

        /// <summary>
        /// Get byte from IDataReader
        /// </summary>
        public static byte GetByte(this IDataReader rd, string field) => rd.GetValueByField(field, rd.GetByte);

        /// <summary>
        /// Get short from IDataReader
        /// </summary>
        public static short GetInt16(this IDataReader rd, string field) => rd.GetValueByField(field, rd.GetInt16);

        /// <summary>
        /// Get short from IDataReader
        /// </summary>
        public static short GetShort(this IDataReader rd, string field) => rd.GetInt16(field);

        /// <summary>
        /// Get int from IDataReader
        /// </summary>
        public static int GetInt32(this IDataReader rd, string field) => rd.GetValueByField(field, rd.GetInt32);

        /// <summary>
        /// Get long from IDataReader
        /// </summary>
        public static long GetInt64(this IDataReader rd, string field) => rd.GetValueByField(field, rd.GetInt64);

        /// <summary>
        /// Get long from IDataReader
        /// </summary>
        public static long GetLong(this IDataReader rd, string field) => rd.GetInt64(field);

        /// <summary>
        /// Get decimal from IDataReader
        /// </summary>
        public static decimal GetDecimal(this IDataReader rd, string field) => rd.GetValueByField(field, rd.GetDecimal);

        /// <summary>
        /// Get double from IDataReader
        /// </summary>
        public static double GetDouble(this IDataReader rd, string field) => rd.GetValueByField(field, rd.GetDouble);

        /// <summary>
        /// Get float from IDataReader
        /// </summary>
        public static float GetFloat(this IDataReader rd, string field) => rd.GetValueByField(field, rd.GetFloat);

        /// <summary>
        /// Get Guid from IDataReader
        /// </summary>
        public static Guid GetGuid(this IDataReader rd, string field) => rd.GetValueByField(field, rd.GetGuid);

        /// <summary>
        /// Get DateTime from IDataReader
        /// </summary>
        public static DateTime GetDateTime(this IDataReader rd, string field) => rd.GetValueByField(field, rd.GetDateTime);

        /// <summary>
        /// Get bool? from IDataReader
        /// </summary>
        public static bool? GetNullableBoolean(this IDataReader rd, string field) => rd.GetNullableValueByField(field, rd.GetBoolean);

        /// <summary>
        /// Get byte? from IDataReader
        /// </summary>
        public static byte? GetNullableByte(this IDataReader rd, string field) => rd.GetNullableValueByField(field, rd.GetByte);

        /// <summary>
        /// Get short? from IDataReader
        /// </summary>
        public static short? GetNullableInt16(this IDataReader rd, string field) => rd.GetNullableValueByField(field, rd.GetInt16);

        /// <summary>
        /// Get short? from IDataReader
        /// </summary>
        public static short? GetNullableShort(this IDataReader rd, string field) => rd.GetNullableInt16(field);

        /// <summary>
        /// Get int? from IDataReader
        /// </summary>
        public static int? GetNullableInt32(this IDataReader rd, string field) => rd.GetNullableValueByField(field, rd.GetInt32);

        /// <summary>
        /// Get long? from IDataReader
        /// </summary>
        public static long? GetNullableInt64(this IDataReader rd, string field) => rd.GetNullableValueByField(field, rd.GetInt64);

        /// <summary>
        /// Get long? from IDataReader
        /// </summary>
        public static long? GetNullableLong(this IDataReader rd, string field) => rd.GetNullableInt64(field);

        /// <summary>
        /// Get decimal? from IDataReader
        /// </summary>
        public static decimal? GetNullableDecimal(this IDataReader rd, string field) => rd.GetNullableValueByField(field, rd.GetDecimal);

        /// <summary>
        /// Get double? from IDataReader
        /// </summary>
        public static double? GetNullableDouble(this IDataReader rd, string field) => rd.GetNullableValueByField(field, rd.GetDouble);

        /// <summary>
        /// Get float? from IDataReader
        /// </summary>
        public static float? GetNullableFloat(this IDataReader rd, string field) => rd.GetNullableValueByField(field, rd.GetFloat);

        /// <summary>
        /// Get Guid? from IDataReader
        /// </summary>
        public static Guid? GetNullableGuid(this IDataReader rd, string field) => rd.GetNullableValueByField(field, rd.GetGuid);

        /// <summary>
        /// Get DateTime? from IDataReader
        /// </summary>
        public static DateTime? GetNullableDateTime(this IDataReader rd, string field) => rd.GetNullableValueByField(field, rd.GetDateTime);

        private static T GetValueByField<T>(this IDataReader rd, string fieldName, Func<int, T> map)
        {
            var i = rd.GetOrdinal(fieldName);

            if (rd.IsDBNull(i))
            {
                return default;
            }

            return map(i);
        }

        private static T? GetNullableValueByField<T>(this IDataReader rd, string fieldName, Func<int, T> map)
            where T : struct
        {
            var i = rd.GetOrdinal(fieldName);

            if (rd.IsDBNull(i))
            {
                return null;
            }

            return map(i);
        }
    }
}
