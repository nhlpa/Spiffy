namespace Spiffy;

using System.Data;

/// <summary>
/// IDataReader extension methods
/// </summary>
public static class IDataReaderExtensions
{
    /// <summary>
    /// Map IDataReader to IEnumerable of T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rd"></param>
    /// <param name="map"></param>
    /// <returns></returns>
    public static IEnumerable<T> Map<T>(this IDataReader rd, Func<IDataReader, T> map)
    {
        var records = new List<T>();

        while (rd.Read())
        {
            records.Add(map(rd));
        }

        return records;
    }

    /// <summary>
    /// Map first iteration of IDataReader to T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rd"></param>
    /// <param name="map"></param>
    /// <returns></returns>
    public static T? MapFirst<T>(this IDataReader rd, Func<IDataReader, T> map)
    {
        if (rd.Read())
        {
            return map(rd);
        }
        else
        {
            return default;
        }
    }

    /// <summary>
    /// Map next iteration of IDataReader to IEnumerable of T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rd"></param>
    /// <param name="map"></param>
    /// <returns></returns>
    public static IEnumerable<T> MapNext<T>(this IDataReader rd, Func<IDataReader, T> map) =>
        rd.NextResult() ? rd.Map(map) : [];

    /// <summary>
    /// Map next iteration of IDataReader to T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rd"></param>
    /// <param name="map"></param>
    /// <returns></returns>
    public static T? MapFirstNext<T>(this IDataReader rd, Func<IDataReader, T> map) =>
        rd.NextResult() ? rd.MapFirst(map) : default;

    /// <summary>
    /// /// Read string from IDataReader
    /// </summary>
    public static string? ReadString(this IDataReader rd, string field) => rd.ReadValueByField(field, rd.GetString);

    /// <summary>
    /// Read char from IDataReader
    /// </summary>
    public static char ReadChar(this IDataReader rd, string field) => rd.ReadValueByField(field, rd.GetChar);

    /// <summary>
    /// Read bool from IDataReader
    /// </summary>
    public static bool ReadBoolean(this IDataReader rd, string field) => rd.ReadValueByField(field, rd.GetBoolean);

    /// <summary>
    /// Read bool from IDataReader
    /// </summary>
    public static bool ReadBool(this IDataReader rd, string field) => rd.ReadBoolean(field);

    /// <summary>
    /// Read byte from IDataReader
    /// </summary>
    public static byte ReadByte(this IDataReader rd, string field) => rd.ReadValueByField(field, rd.GetByte);

    /// <summary>
    /// Read short from IDataReader
    /// </summary>
    public static short ReadInt16(this IDataReader rd, string field) => rd.ReadValueByField(field, rd.GetInt16);

    /// <summary>
    /// Read short from IDataReader
    /// </summary>
    public static short ReadShort(this IDataReader rd, string field) => rd.ReadInt16(field);

    /// <summary>
    /// Read int from IDataReader
    /// </summary>
    public static int ReadInt32(this IDataReader rd, string field) => rd.ReadValueByField(field, rd.GetInt32);

    /// <summary>
    /// Read int from IDataReader
    /// </summary>
    public static int ReadInt(this IDataReader rd, string field) => rd.ReadInt32(field);

    /// <summary>
    /// Read long from IDataReader
    /// </summary>
    public static long ReadInt64(this IDataReader rd, string field) => rd.ReadValueByField(field, rd.GetInt64);

    /// <summary>
    /// Read long from IDataReader
    /// </summary>
    public static long ReadLong(this IDataReader rd, string field) => rd.ReadInt64(field);

    /// <summary>
    /// Read decimal from IDataReader
    /// </summary>
    public static decimal ReadDecimal(this IDataReader rd, string field) => rd.ReadValueByField(field, rd.GetDecimal);

    /// <summary>
    /// Read double from IDataReader
    /// </summary>
    public static double ReadDouble(this IDataReader rd, string field) => rd.ReadValueByField(field, rd.GetDouble);

    /// <summary>
    /// Read float from IDataReader
    /// </summary>
    public static float ReadFloat(this IDataReader rd, string field) => rd.ReadValueByField(field, rd.GetFloat);

    /// <summary>
    /// Read Guid from IDataReader
    /// </summary>
    public static Guid ReadGuid(this IDataReader rd, string field) => rd.ReadValueByField(field, rd.GetGuid);

    /// <summary>
    /// Read DateTime from IDataReader
    /// </summary>
    public static DateTime ReadDateTime(this IDataReader rd, string field) => rd.ReadValueByField(field, rd.GetDateTime);

    /// <summary>
    /// Read byte[] from IDataReader
    /// </summary>
    /// <param name="rd"></param>
    /// <param name="field"></param>
    /// <returns></returns>
    public static byte[] ReadBytes(this IDataReader rd, string field) => rd.ReadValueByField(field, rd.StreamBytes) ?? [];

    /// <summary>
    /// Read bool? from IDataReader
    /// </summary>
    public static bool? ReadNullableBoolean(this IDataReader rd, string field) => rd.ReadNullableValueByField(field, rd.GetBoolean);

    /// <summary>
    /// Read bool? from IDataReader
    /// </summary>
    public static bool? ReadNullableBool(this IDataReader rd, string field) => rd.ReadNullableBoolean(field);

    /// <summary>
    /// Read byte? from IDataReader
    /// </summary>
    public static byte? ReadNullableByte(this IDataReader rd, string field) => rd.ReadNullableValueByField(field, rd.GetByte);

    /// <summary>
    /// Read short? from IDataReader
    /// </summary>
    public static short? ReadNullableInt16(this IDataReader rd, string field) => rd.ReadNullableValueByField(field, rd.GetInt16);

    /// <summary>
    /// Read short? from IDataReader
    /// </summary>
    public static short? ReadNullableShort(this IDataReader rd, string field) => rd.ReadNullableInt16(field);

    /// <summary>
    /// Read int? from IDataReader
    /// </summary>
    public static int? ReadNullableInt32(this IDataReader rd, string field) => rd.ReadNullableValueByField(field, rd.GetInt32);

    /// <summary>
    /// Read int? from IDataReader
    /// </summary>
    public static int? ReadNullableInt(this IDataReader rd, string field) => rd.ReadNullableInt32(field);

    /// <summary>
    /// Read long? from IDataReader
    /// </summary>
    public static long? ReadNullableInt64(this IDataReader rd, string field) => rd.ReadNullableValueByField(field, rd.GetInt64);

    /// <summary>
    /// Read long? from IDataReader
    /// </summary>
    public static long? ReadNullableLong(this IDataReader rd, string field) => rd.ReadNullableInt64(field);

    /// <summary>
    /// Read decimal? from IDataReader
    /// </summary>
    public static decimal? ReadNullableDecimal(this IDataReader rd, string field) => rd.ReadNullableValueByField(field, rd.GetDecimal);

    /// <summary>
    /// Read double? from IDataReader
    /// </summary>
    public static double? ReadNullableDouble(this IDataReader rd, string field) => rd.ReadNullableValueByField(field, rd.GetDouble);

    /// <summary>
    /// Read float? from IDataReader
    /// </summary>
    public static float? ReadNullableFloat(this IDataReader rd, string field) => rd.ReadNullableValueByField(field, rd.GetFloat);

    /// <summary>
    /// Read Guid? from IDataReader
    /// </summary>
    public static Guid? ReadNullableGuid(this IDataReader rd, string field) => rd.ReadNullableValueByField(field, rd.GetGuid);

    /// <summary>
    /// Read DateTime? from IDataReader
    /// </summary>
    public static DateTime? ReadNullableDateTime(this IDataReader rd, string field) => rd.ReadNullableValueByField(field, rd.GetDateTime);

    private static T? ReadValueByField<T>(this IDataReader rd, string fieldName, Func<int, T> map)
    {
        var i = rd.GetOrdinal(fieldName);

        if (rd.IsDBNull(i))
        {
            return default;
        }

        try
        {
            return map(i);
        }
        catch (InvalidCastException ex)
        {
            throw new FailedExecutionException(DbErrorCode.CouldNotCastValue, fieldName, ex);
        }
    }

    private static T? ReadNullableValueByField<T>(this IDataReader rd, string fieldName, Func<int, T> map) where T : struct
    {
        var i = rd.GetOrdinal(fieldName);

        if (rd.IsDBNull(i))
        {
            return null;
        }

        try
        {
            return map(i);
        }
        catch (InvalidCastException ex)
        {
            throw new FailedExecutionException(DbErrorCode.CouldNotCastValue, fieldName, ex);
        }
    }

    private static byte[] StreamBytes(this IDataReader rd, int ordinal)
    {
        var bufferSize = 1024;
        var buffer = new byte[bufferSize];
        long bytesReturned;
        var startIndex = 0;

        using var ms = new MemoryStream();
        bytesReturned = rd.GetBytes(ordinal, startIndex, buffer, 0, bufferSize);
        while (bytesReturned == bufferSize)
        {
            ms.Write(buffer, 0, (int)bytesReturned);
            ms.Flush();

            startIndex += bufferSize;
            bytesReturned = rd.GetBytes(ordinal, startIndex, buffer, 0, bufferSize);
        }

        ms.Write(buffer, 0, (int)bytesReturned);
        ms.Flush();

        return ms.ToArray();

    }
}
