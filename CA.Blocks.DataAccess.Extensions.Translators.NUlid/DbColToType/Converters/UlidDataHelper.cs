using System.Data;
using NUlid;

namespace CA.Blocks.DataAccess.Extensions.Translators.NUlid.DbColToType.Converters;

public static class UlidDataHelper
{
    private static Ulid? ObjectAsUlid(object? dbDataValue)
    {
        if (dbDataValue == null)
            return null;

        if (dbDataValue is string value)
        {
            if (Ulid.TryParse(value, out var UlidFromString))
            {
                return UlidFromString;
            }
            else
            {
                throw new InvalidDataException("Data not a valid Ulid");
            }
        }
        if (dbDataValue is Guid gvalue)
        {
            return new Ulid(gvalue);
        }
        else
        {
            // if not stored as string assumes it is stored as binary
            return new Ulid((byte[])dbDataValue);
        }
    }

    public static Ulid GetValueFromRowAsUlid(DataRow dr, string sColumnName)
    {
        Ulid? val = GetValueFromRowAsNullUlid(dr, sColumnName);
        return DataHelper.ThrowExceptionIfIsNull(val, sColumnName, "Ulid");
    }

    public static Ulid GetValueFromRowAsUlid(DataRow dr, int columnIndex)
    {
        Ulid? val = GetValueFromRowAsNullUlid(dr, columnIndex);
        return DataHelper.ThrowExceptionIfIsNull(val, columnIndex, "Ulid");
    }

    public static Ulid GetValueFromRowAsUlid(DataRow dr, DataColumn dc)
    {
        Ulid? val = GetValueFromRowAsNullUlid(dr, dc);
        return DataHelper.ThrowExceptionIfIsNull(val, dc.ColumnName, "Ulid");
    }

    public static Ulid? GetValueFromRowAsNullUlid(DataRow dr, string sColumnName)
    {
        return ObjectAsUlid(DataHelper.GetValueFromRow(dr, sColumnName));
    }

    public static Ulid? GetValueFromRowAsNullUlid(DataRow dr, int columnIndex)
    {
        return ObjectAsUlid(DataHelper.GetValueFromRow(dr, columnIndex));
    }

    public static Ulid? GetValueFromRowAsNullUlid(DataRow dr, DataColumn dc)
    {
        return ObjectAsUlid(DataHelper.GetValueFromRow(dr, dc));
    }

    public static Ulid AsUlid(this DataRow dr, string colName)
    {
        return GetValueFromRowAsUlid(dr, colName);
    }

    public static Ulid AsUlid(this DataRow dr, int columnIndex)
    {
        return GetValueFromRowAsUlid(dr, columnIndex);
    }

    public static Ulid AsUlid(this DataRow dr, DataColumn column)
    {
        return GetValueFromRowAsUlid(dr, column);
    }

    // Nulls
    public static Ulid? AsNullUlid(this DataRow dr, string colName)
    {
        return GetValueFromRowAsNullUlid(dr, colName);
    }

    public static Ulid? AsNullUlid(this DataRow dr, int columnIndex)
    {
        return GetValueFromRowAsNullUlid(dr, columnIndex);
    }

    public static Ulid? AsNullUlid(this DataRow dr, DataColumn column)
    {
        return GetValueFromRowAsNullUlid(dr, column);
    }

    // Data Reader
    public static Ulid AsUlid(this IDataReader dr, string colName)
    {
        var val = dr.AsNullUlid(colName);
        return DataHelper.ThrowExceptionIfIsNull(val, colName, "Ulid");
    }

    public static Ulid AsUlid(this IDataReader dr, int columnIndex)
    {
        var val = dr.AsNullUlid(columnIndex);
        return DataHelper.ThrowExceptionIfIsNull(val, columnIndex, "Ulid");
    }

    // Nulls
    public static Ulid? AsNullUlid(this IDataReader dr, string colName)
    {
        return dr.IsDBNull(dr.GetOrdinal(colName)) ? null : ObjectAsUlid(dr[colName]);
    }

    public static Ulid? AsNullUlid(this IDataReader dr, int columnIndex)
    {
        return dr.IsDBNull(columnIndex) ? null : ObjectAsUlid(dr[columnIndex]);
    }
}