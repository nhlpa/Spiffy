using System;
using System.Data;

namespace Spiffy
{
    public static class IDataReaderExtensions
    {
        public static string GetString(this IDataReader rd, string field) => rd.GetValueByField(field, rd.GetString);
        public static char GetChar(this IDataReader rd, string field) => rd.GetValueByField(field, rd.GetChar);
        public static bool GetBoolean(this IDataReader rd, string field) => rd.GetValueByField(field, rd.GetBoolean);
        public static byte GetByte(this IDataReader rd, string field) => rd.GetValueByField(field, rd.GetByte);
        public static short GetInt16(this IDataReader rd, string field) => rd.GetValueByField(field, rd.GetInt16);
        public static short GetShort(this IDataReader rd, string field) => rd.GetInt16(field);
        public static int GetInt32(this IDataReader rd, string field) => rd.GetValueByField(field, rd.GetInt32);
        public static long GetInt64(this IDataReader rd, string field) => rd.GetValueByField(field, rd.GetInt64);
        public static long GetLong(this IDataReader rd, string field) => rd.GetInt64(field);
        public static decimal GetDecimal(this IDataReader rd, string field) => rd.GetValueByField(field, rd.GetDecimal);
        public static double GetDouble(this IDataReader rd, string field) => rd.GetValueByField(field, rd.GetDouble);
        public static float GetFloat(this IDataReader rd, string field) => rd.GetValueByField(field, rd.GetFloat);
        public static Guid GetGuid(this IDataReader rd, string field) => rd.GetValueByField(field, rd.GetGuid);
        public static DateTime GetDateTime(this IDataReader rd, string field) => rd.GetValueByField(field, rd.GetDateTime);

        public static bool? GetNullableBoolean(this IDataReader rd, string field) => rd.GetNullableValueByField(field, rd.GetBoolean);
        public static byte? GetNullableByte(this IDataReader rd, string field) => rd.GetNullableValueByField(field, rd.GetByte);
        public static short? GetNullableInt16(this IDataReader rd, string field) => rd.GetNullableValueByField(field, rd.GetInt16);
        public static short? GetNullableShort(this IDataReader rd, string field) => rd.GetNullableInt16(field);
        public static int? GetNullableInt32(this IDataReader rd, string field) => rd.GetNullableValueByField(field, rd.GetInt32);
        public static long? GetNullableInt64(this IDataReader rd, string field) => rd.GetNullableValueByField(field, rd.GetInt64);
        public static long? GetNullableLong(this IDataReader rd, string field) => rd.GetNullableInt64(field);
        public static decimal? GetNullableDecimal(this IDataReader rd, string field) => rd.GetNullableValueByField(field, rd.GetDecimal);
        public static double? GetNullableDouble(this IDataReader rd, string field) => rd.GetNullableValueByField(field, rd.GetDouble);
        public static float? GetNullableFloat(this IDataReader rd, string field) => rd.GetNullableValueByField(field, rd.GetFloat);
        public static Guid? GetNullableGuid(this IDataReader rd, string field) => rd.GetNullableValueByField(field, rd.GetGuid);
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
