namespace Spiffy;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

/// <summary>
/// A container for database bound parameters.
/// </summary>
public sealed class DbParams : Dictionary<string, object>
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
public sealed class DbTypeParam(DbType dbType, object value)
{
    /// <summary>
    /// The DbType of the parameter
    /// </summary>
    public DbType DbType { get; } = dbType;

    /// <summary>
    /// The value of the parameter
    /// </summary>
    public object Value { get; } = value;

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.AnsiString
    /// </summary>
    public static DbTypeParam AnsiString(string v) => new DbTypeParam(DbType.AnsiString, v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Binary
    /// </summary>
    public static DbTypeParam Binary(byte[] v) => new DbTypeParam(DbType.Binary, v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Binary
    /// </summary>
    public static DbTypeParam Bytes(byte[] v) => Binary(v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Byte
    /// </summary>
    public static DbTypeParam Byte(byte v) => new DbTypeParam(DbType.Byte, v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Boolean
    /// </summary>
    public static DbTypeParam Boolean(bool v) => new DbTypeParam(DbType.Boolean, v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Currency
    /// </summary>
    public static DbTypeParam Currency(decimal v) => new DbTypeParam(DbType.Currency, v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Date
    /// </summary>
    public static DbTypeParam Date(DateTime v) => new DbTypeParam(DbType.Date, v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.DateTime
    /// </summary>
    public static DbTypeParam DateTime(DateTime v) => new DbTypeParam(DbType.DateTime, v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Decimal
    /// </summary>
    public static DbTypeParam Decimal(decimal v) => new DbTypeParam(DbType.Decimal, v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Double
    /// </summary>
    public static DbTypeParam Double(double v) => new DbTypeParam(DbType.Double, v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Guid
    /// </summary>
    public static DbTypeParam Guid(Guid v) => new DbTypeParam(DbType.Guid, v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Int16
    /// </summary>
    public static DbTypeParam Int16(short v) => new DbTypeParam(DbType.Int16, v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Int32
    /// </summary>
    public static DbTypeParam Int32(int v) => new DbTypeParam(DbType.Int32, v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Int64
    /// </summary>
    public static DbTypeParam Int64(long v) => new DbTypeParam(DbType.Int64, v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Object
    /// </summary>
    public static DbTypeParam Object(object v) => new DbTypeParam(DbType.Object, v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Single
    /// </summary>
    public static DbTypeParam Float(float v) => new DbTypeParam(DbType.Single, v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.String
    /// </summary>
    public static DbTypeParam String(string v) => new DbTypeParam(DbType.String, v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.UInt16
    /// </summary>
    public static DbTypeParam UInt16(ushort v) => new DbTypeParam(DbType.UInt16, v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.UInt32
    /// </summary>
    public static DbTypeParam UInt32(uint v) => new DbTypeParam(DbType.UInt32, v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.UInt64
    /// </summary>
    public static DbTypeParam UInt64(ulong v) => new DbTypeParam(DbType.UInt64, v);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.AnsiString
    /// </summary>
    public static DbTypeParam NullAnsiString => new DbTypeParam(DbType.AnsiString, DBNull.Value);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Binary
    /// </summary>
    public static DbTypeParam NullBinary => new DbTypeParam(DbType.Binary, DBNull.Value);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Binary
    /// </summary>
    public static DbTypeParam NullBytes => NullBinary;

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Byte
    /// </summary>
    public static DbTypeParam NullByte => new DbTypeParam(DbType.Byte, DBNull.Value);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Boolean
    /// </summary>
    public static DbTypeParam NullBoolean => new DbTypeParam(DbType.Boolean, DBNull.Value);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Currency
    /// </summary>
    public static DbTypeParam NullCurrency => new DbTypeParam(DbType.Currency, DBNull.Value);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Date
    /// </summary>
    public static DbTypeParam NullDate => new DbTypeParam(DbType.Date, DBNull.Value);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.DateTime
    /// </summary>
    public static DbTypeParam NullDateTime => new DbTypeParam(DbType.DateTime, DBNull.Value);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Decimal
    /// </summary>
    public static DbTypeParam NullDecimal => new DbTypeParam(DbType.Decimal, DBNull.Value);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Double
    /// </summary>
    public static DbTypeParam NullDouble => new DbTypeParam(DbType.Double, DBNull.Value);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Guid
    /// </summary>
    public static DbTypeParam NullGuid => new DbTypeParam(DbType.Guid, DBNull.Value);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Int16
    /// </summary>
    public static DbTypeParam NullInt16 => new DbTypeParam(DbType.Int16, DBNull.Value);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Int32
    /// </summary>
    public static DbTypeParam NullInt32 => new DbTypeParam(DbType.Int32, DBNull.Value);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Int64
    /// </summary>
    public static DbTypeParam NullInt64 => new DbTypeParam(DbType.Int64, DBNull.Value);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Object
    /// </summary>
    public static DbTypeParam NullObject => new DbTypeParam(DbType.Object, DBNull.Value);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Single
    /// </summary>
    public static DbTypeParam NullFloat => new DbTypeParam(DbType.Single, DBNull.Value);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.String
    /// </summary>
    public static DbTypeParam NullString => new DbTypeParam(DbType.String, DBNull.Value);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.Time
    /// </summary>
    public static DbTypeParam NullTime => new DbTypeParam(DbType.Time, DBNull.Value);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.UInt16
    /// </summary>
    public static DbTypeParam NullUInt16 => new DbTypeParam(DbType.UInt16, DBNull.Value);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.UInt32
    /// </summary>
    public static DbTypeParam NullUInt32 => new DbTypeParam(DbType.UInt32, DBNull.Value);

    /// <summary>
    /// Initialize a new instance of the DbTypeParam with a null value DbType.UInt64
    /// </summary>
    public static DbTypeParam NullUInt64 => new DbTypeParam(DbType.UInt64, DBNull.Value);
}
