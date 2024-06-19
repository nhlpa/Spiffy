namespace Spiffy;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

/// <summary>
/// A container for database bound parameters.
/// </summary>
public sealed class DbParams : Dictionary<string, object?>
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
    public DbParams(string key, object? value)
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
          .ForEach(x =>
          {
              if (!p1.ContainsKey(x.Key))
              {
                  p1.Add(x.Key, x.Value);
              }
          });

        return p1;
    }
}

/// <summary>
/// A container for database bound parameters with DbType
/// </summary>
/// <remarks>
/// Initialize a new instance of the DbTypeParam class
/// </remarks>
/// <param name="dbType"></param>
/// <param name="value"></param>
public sealed class DbTypeParam(DbType dbType, object? value = null)
{
    /// <summary>
    /// The DbType of the parameter
    /// </summary>
    public DbType DbType { get; } = dbType;

    /// <summary>
    /// The value of the parameter
    /// </summary>
    public object Value { get; } = value ?? DBNull.Value;

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.AnsiString
    /// </summary>
    public static DbTypeParam AnsiString(string v) => new(DbType.AnsiString, v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Binary
    /// </summary>
    public static DbTypeParam Binary(byte[] v) => new(DbType.Binary, v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Binary
    /// </summary>
    public static DbTypeParam Bytes(byte[] v) => Binary(v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Byte
    /// </summary>
    public static DbTypeParam Byte(byte v) => new(DbType.Byte, v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Boolean
    /// </summary>
    public static DbTypeParam Boolean(bool v) => new(DbType.Boolean, v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Currency
    /// </summary>
    public static DbTypeParam Currency(decimal v) => new(DbType.Currency, v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Date
    /// </summary>
    public static DbTypeParam Date(DateTime v) => new(DbType.Date, v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.DateTime
    /// </summary>
    public static DbTypeParam DateTime(DateTime v) => new(DbType.DateTime, v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Decimal
    /// </summary>
    public static DbTypeParam Decimal(decimal v) => new(DbType.Decimal, v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Double
    /// </summary>
    public static DbTypeParam Double(double v) => new(DbType.Double, v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Guid
    /// </summary>
    public static DbTypeParam Guid(Guid v) => new(DbType.Guid, v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Int16
    /// </summary>
    public static DbTypeParam Int16(short v) => new(DbType.Int16, v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Int32
    /// </summary>
    public static DbTypeParam Int32(int v) => new(DbType.Int32, v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Int64
    /// </summary>
    public static DbTypeParam Int64(long v) => new(DbType.Int64, v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Object
    /// </summary>
    public static DbTypeParam Object(object v) => new(DbType.Object, v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Single
    /// </summary>
    public static DbTypeParam Float(float v) => new(DbType.Single, v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.String
    /// </summary>
    public static DbTypeParam String(string v) => new(DbType.String, v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.UInt16
    /// </summary>
    public static DbTypeParam UInt16(ushort v) => new(DbType.UInt16, v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.UInt32
    /// </summary>
    public static DbTypeParam UInt32(uint v) => new(DbType.UInt32, v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.UInt64
    /// </summary>
    public static DbTypeParam UInt64(ulong v) => new(DbType.UInt64, v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.AnsiString
    /// </summary>
    public static DbTypeParam NullAnsiString => new(DbType.AnsiString);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Binary
    /// </summary>
    public static DbTypeParam NullBinary => new(DbType.Binary);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Binary
    /// </summary>
    public static DbTypeParam NullBytes => NullBinary;

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Byte
    /// </summary>
    public static DbTypeParam NullByte => new(DbType.Byte);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Boolean
    /// </summary>
    public static DbTypeParam NullBoolean => new(DbType.Boolean);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Currency
    /// </summary>
    public static DbTypeParam NullCurrency => new(DbType.Currency);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Date
    /// </summary>
    public static DbTypeParam NullDate => new(DbType.Date);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.DateTime
    /// </summary>
    public static DbTypeParam NullDateTime => new(DbType.DateTime);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Decimal
    /// </summary>
    public static DbTypeParam NullDecimal => new(DbType.Decimal);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Double
    /// </summary>
    public static DbTypeParam NullDouble => new(DbType.Double);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Guid
    /// </summary>
    public static DbTypeParam NullGuid => new(DbType.Guid);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Int16
    /// </summary>
    public static DbTypeParam NullInt16 => new(DbType.Int16);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Int32
    /// </summary>
    public static DbTypeParam NullInt32 => new(DbType.Int32);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Int64
    /// </summary>
    public static DbTypeParam NullInt64 => new(DbType.Int64);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Object
    /// </summary>
    public static DbTypeParam NullObject => new(DbType.Object);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Single
    /// </summary>
    public static DbTypeParam NullFloat => new(DbType.Single);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.String
    /// </summary>
    public static DbTypeParam NullString => new(DbType.String);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Time
    /// </summary>
    public static DbTypeParam NullTime => new(DbType.Time);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.UInt16
    /// </summary>
    public static DbTypeParam NullUInt16 => new(DbType.UInt16);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.UInt32
    /// </summary>
    public static DbTypeParam NullUInt32 => new(DbType.UInt32);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.UInt64
    /// </summary>
    public static DbTypeParam NullUInt64 => new(DbType.UInt64);
}
