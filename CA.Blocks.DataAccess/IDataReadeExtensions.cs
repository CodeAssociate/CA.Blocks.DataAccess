//===============================================================================
// Copyright (C) 2002-2020 Ravin Enterprises Ltd. 
// All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Data;

namespace CA.Blocks.DataAccess
{
    //TODO we need to benchmark some of these procedure https://www.nuget.org/packages/BenchmarkDotNet/
    // https://stackoverflow.com/questions/1170756/casting-vs-converting-an-object-tostring-when-object-really-is-a-string

    public static class IDataReaderExtensions
    {
        private static void ThrowExceptionIfIsNull(object obj, string sColumnName, string typeDescription)
        {
            if (obj == null || obj == DBNull.Value)
            {
                throw new ArgumentNullException(
                    $"Tried to get {sColumnName} from row as non-nullable {typeDescription}, however value is NULL.");
            }
        }

        private static void ThrowExceptionIfIsNull(object obj, int columnIndex, string typeDescription)
        {
            if (obj == null || obj == DBNull.Value)
            {
                throw new ArgumentNullException(
                    $"Tried to get col in position {columnIndex} from row as non-nullable {typeDescription}, however value is NULL.");
            }
        }


        //#region Binary
        //public static byte[] AsBinary(this DataRow dr, string colName)
        //{
        //    return DataHelper.GetValueFromRowAsBinary(dr, colName);
        //}

        //public static byte[] AsBinary(this DataRow dr, int columnIndex)
        //{
        //    return DataHelper.GetValueFromRowAsBinary(dr, columnIndex);
        //}

        //public static byte[] AsBinary(this DataRow dr, DataColumn column)
        //{
        //    return DataHelper.GetValueFromRowAsBinary(dr, column);
        //}

        //#endregion

        #region bool

        public static bool AsBool(this IDataReader dr, string colName)
        {
            bool? val = dr.AsNullBool(colName);
            ThrowExceptionIfIsNull(val, colName, "bool");
            return val.Value;
        }


        public static bool AsBool(this IDataReader dr, int columnIndex)
        {
            bool? val = dr.AsNullBool(columnIndex);
            ThrowExceptionIfIsNull(val, columnIndex, "bool");
            return val.Value;
        }


        // Nulls
        public static bool? AsNullBool(this IDataReader dr, string colName)
        {
            if (dr.IsDBNull(dr.GetOrdinal(colName)))
                return null;
            else
            {
                return Convert.ToBoolean(dr[colName]);
            }
        }

        public static bool? AsNullBool(this IDataReader dr, int columnIndex)
        {
            if (dr.IsDBNull(columnIndex))
                return null;
            else
            {
                return Convert.ToBoolean(dr[columnIndex]);
            }
        }

        #endregion

        #region Byte
        public static byte AsByte(this IDataReader dr, string colName)
        {
            byte? val = dr.AsNullByte(colName);
            ThrowExceptionIfIsNull(val, colName, "byte");
            return val.Value;
        }


        public static byte AsByte(this IDataReader dr, int columnIndex)
        {
            byte? val = dr.AsNullByte(columnIndex);
            ThrowExceptionIfIsNull(val, columnIndex, "byte");
            return val.Value;
        }


        // Nulls
        public static byte? AsNullByte(this IDataReader dr, string colName)
        {
            if (dr.IsDBNull(dr.GetOrdinal(colName)))
                return null;
            else
            {
                return Convert.ToByte(dr[colName]);
            }
        }

        public static byte? AsNullByte(this IDataReader dr, int columnIndex)
        {
            if (dr.IsDBNull(columnIndex))
                return null;
            else
            {
                return Convert.ToByte(dr[columnIndex]);
            }
        }
        #endregion

        #region Char

        public static char AsChar(this IDataReader dr, string colName)
        {
            char? val = dr.AsNullChar(colName);
            ThrowExceptionIfIsNull(val, colName, "char");
            return val.Value;
        }


        public static char AsChar(this IDataReader dr, int columnIndex)
        {
            char? val = dr.AsNullChar(columnIndex);
            ThrowExceptionIfIsNull(val, columnIndex, "char");
            return val.Value;
        }

        // Nulls
        public static char? AsNullChar(this IDataReader dr, string colName)
        {
            if (dr.IsDBNull(dr.GetOrdinal(colName)))
                return null;
            else
            {
                return Convert.ToChar(dr[colName]);
            }
        }

        public static char? AsNullChar(this IDataReader dr, int columnIndex)
        {
            if (dr.IsDBNull(columnIndex))
                return null;
            else
            {
                return Convert.ToChar(dr[columnIndex]);
            }
        }

        #endregion

        #region DateTime

        public static DateTime AsDateTime(this IDataReader dr, string colName)
        {
            DateTime? val = dr.AsNullDateTime(colName);
            ThrowExceptionIfIsNull(val, colName, "DateTime");
            return val.Value;
        }


        public static DateTime AsDateTime(this IDataReader dr, int columnIndex)
        {
            DateTime? val = dr.AsNullDateTime(columnIndex);
            ThrowExceptionIfIsNull(val, columnIndex, "DateTime");
            return val.Value;
        }

        // Nulls
        public static DateTime? AsNullDateTime(this IDataReader dr, string colName)
        {
            if (dr.IsDBNull(dr.GetOrdinal(colName)))
                return null;
            else
            {
                return Convert.ToDateTime(dr[colName]);
            }
        }

        public static DateTime? AsNullDateTime(this IDataReader dr, int columnIndex)
        {
            if (dr.IsDBNull(columnIndex))
                return null;
            else
            {
                return Convert.ToDateTime(dr[columnIndex]);
            }
        }

        #endregion

        #region Double

        public static double AsDouble(this IDataReader dr, string colName)
        {
            double? val = dr.AsNullDouble(colName);
            ThrowExceptionIfIsNull(val, colName, "double");
            return val.Value;
        }


        public static double AsDouble(this IDataReader dr, int columnIndex)
        {
            double? val = dr.AsNullDouble(columnIndex);
            ThrowExceptionIfIsNull(val, columnIndex, "double");
            return val.Value;
        }


        // Nulls
        public static double? AsNullDouble(this IDataReader dr, string colName)
        {
            if (dr.IsDBNull(dr.GetOrdinal(colName)))
                return null;
            else
            {
                return Convert.ToDouble(dr[colName]);
            }
        }

        public static double? AsNullDouble(this IDataReader dr, int columnIndex)
        {
            if (dr.IsDBNull(columnIndex))
                return null;
            else
            {
                return Convert.ToDouble(dr[columnIndex]);
            }
        }
        #endregion

        #region Single / float  a float is a single The use of "float" in C# seems to be a throwback to its C/C++ heritage. a float" still maps to the System.Single type in C# so use single where you can

        public static Single AsSingle(this IDataReader dr, string colName)
        {
            Single? val = dr.AsNullSingle(colName);
            ThrowExceptionIfIsNull(val, colName, "single");
            return val.Value;
        }


        public static Single AsSingle(this IDataReader dr, int columnIndex)
        {
            Single? val = dr.AsNullSingle(columnIndex);
            ThrowExceptionIfIsNull(val, columnIndex, "single");
            return val.Value;
        }


        // Nulls
        public static Single? AsNullSingle(this IDataReader dr, string colName)
        {
            if (dr.IsDBNull(dr.GetOrdinal(colName)))
                return null;
            else
            {
                return Convert.ToSingle(dr[colName]);
            }
        }

        public static Single? AsNullSingle(this IDataReader dr, int columnIndex)
        {
            if (dr.IsDBNull(columnIndex))
                return null;
            else
            {
                return Convert.ToSingle(dr[columnIndex]);
            }
        }
        #endregion

        #region Decimal

        public static decimal AsDecimal(this IDataReader dr, string colName)
        {
            decimal? val = dr.AsNullDecimal(colName);
            ThrowExceptionIfIsNull(val, colName, "decimal");
            return val.Value;
        }


        public static decimal AsDecimal(this IDataReader dr, int columnIndex)
        {
            decimal? val = dr.AsNullDecimal(columnIndex);
            ThrowExceptionIfIsNull(val, columnIndex, "decimal");
            return val.Value;
        }


        // Nulls
        public static decimal? AsNullDecimal(this IDataReader dr, string colName)
        {
            if (dr.IsDBNull(dr.GetOrdinal(colName)))
                return null;
            else
            {
                return Convert.ToDecimal(dr[colName]);
            }
        }

        public static decimal? AsNullDecimal(this IDataReader dr, int columnIndex)
        {
            if (dr.IsDBNull(columnIndex))
                return null;
            else
            {
                return Convert.ToDecimal(dr[columnIndex]);
            }
        }


        #endregion

        #region Guid

        public static Guid AsGuid(this IDataReader dr, string colName)
        {
            Guid? val = dr.AsNullGuid(colName);
            ThrowExceptionIfIsNull(val, colName, "Guid");
            return val.Value;
        }


        public static Guid AsGuid(this IDataReader dr, int columnIndex)
        {
            Guid? val = dr.AsNullGuid(columnIndex);
            ThrowExceptionIfIsNull(val, columnIndex, "Guid");
            return val.Value;
        }


        // Nulls
        public static Guid? AsNullGuid(this IDataReader dr, string colName)
        {
            if (dr.IsDBNull(dr.GetOrdinal(colName)))
                return null;
            else
            {
                return (Guid)(dr[colName]);
            }
        }

        public static Guid? AsNullGuid(this IDataReader dr, int columnIndex)
        {
            if (dr.IsDBNull(columnIndex))
                return null;
            else
            {
                return (Guid)(dr[columnIndex]);
            }
        }

        #endregion

        #region Int

        public static int AsInt(this IDataReader dr, string colName)
        {
            int? val = dr.AsNullInt(colName);
            ThrowExceptionIfIsNull(val, colName, "int");
            return val.Value;
        }


        public static int AsInt(this IDataReader dr, int columnIndex)
        {
            int? val = dr.AsNullInt(columnIndex);
            ThrowExceptionIfIsNull(val, columnIndex, "int");
            return val.Value;
        }


        // Nulls
        public static int? AsNullInt(this IDataReader dr, string colName)
        {
            if (dr.IsDBNull(dr.GetOrdinal(colName)))
                return null;
            else
            {
                return Convert.ToInt32(dr[colName]);
            }
        }

        public static int? AsNullInt(this IDataReader dr, int columnIndex)
        {
            if (dr.IsDBNull(columnIndex))
                return null;
            else
            {
                return Convert.ToInt32(dr[columnIndex]);
            }
        }

        #endregion

        #region Long

        public static long AsLong(this IDataReader dr, string colName)
        {
            long? val = dr.AsNullLong(colName);
            ThrowExceptionIfIsNull(val, colName, "long");
            return val.Value;
        }


        public static long AsLong(this IDataReader dr, int columnIndex)
        {
            long? val = dr.AsNullLong(columnIndex);
            ThrowExceptionIfIsNull(val, columnIndex, "long");
            return val.Value;
        }


        // Nulls
        public static long? AsNullLong(this IDataReader dr, string colName)
        {
            if (dr.IsDBNull(dr.GetOrdinal(colName)))
                return null;
            else
            {
                return Convert.ToInt64(dr[colName]);
            }
        }

        public static long? AsNullLong(this IDataReader dr, int columnIndex)
        {
            if (dr.IsDBNull(columnIndex))
                return null;
            else
            {
                return Convert.ToInt64(dr[columnIndex]);
            }
        }

        #endregion 

        //#region ulong
        //public static ulong AsULong(this DataRow dr, string colName)
        //{
        //    return DataHelper.GetValueFromRowAsULong(dr, colName);
        //}

        //public static ulong AsULong(this DataRow dr, int columnIndex)
        //{
        //    return DataHelper.GetValueFromRowAsULong(dr, columnIndex);
        //}

        //public static ulong AsULong(this DataRow dr, DataColumn column)
        //{
        //    return DataHelper.GetValueFromRowAsULong(dr, column);
        //}

        //// Nulls
        //public static ulong? AsNullULong(this DataRow dr, string colName)
        //{
        //    return DataHelper.GetValueFromRowAsNullULong(dr, colName);
        //}

        //public static ulong? AsNullULong(this DataRow dr, int columnIndex)
        //{
        //    return DataHelper.GetValueFromRowAsNullULong(dr, columnIndex);
        //}

        //public static ulong? AsNullULong(this DataRow dr, DataColumn column)
        //{
        //    return DataHelper.GetValueFromRowAsNullULong(dr, column);
        //}

        //#endregion

        //#region Sbyte


        //public static sbyte AsSbyte(this DataRow dr, string colName)
        //{
        //    return DataHelper.GetValueFromRowAsSbyte(dr, colName);
        //}

        //public static sbyte AsSbyte(this DataRow dr, int columnIndex)
        //{
        //    return DataHelper.GetValueFromRowAsSbyte(dr, columnIndex);
        //}

        //public static sbyte AsSbyte(this DataRow dr, DataColumn column)
        //{
        //    return DataHelper.GetValueFromRowAsSbyte(dr, column);
        //}

        //// Nulls
        //public static sbyte? AsNullSbyte(this DataRow dr, string colName)
        //{
        //    return DataHelper.GetValueFromRowAsNullSbyte(dr, colName);
        //}

        //public static sbyte? AsNullSbyte(this DataRow dr, int columnIndex)
        //{
        //    return DataHelper.GetValueFromRowAsNullSbyte(dr, columnIndex);
        //}

        //public static sbyte? AsNullSbyte(this DataRow dr, DataColumn column)
        //{
        //    return DataHelper.GetValueFromRowAsNullSbyte(dr, column);
        //}

        //#endregion

        #region Short

        public static short AsShort(this IDataReader dr, string colName)
        {
            short? val = dr.AsNullShort(colName);
            ThrowExceptionIfIsNull(val, colName, "short");
            return val.Value;
        }


        public static short AsShort(this IDataReader dr, int columnIndex)
        {
            short? val = dr.AsNullShort(columnIndex);
            ThrowExceptionIfIsNull(val, columnIndex, "short");
            return val.Value;
        }


        // Nulls
        public static short? AsNullShort(this IDataReader dr, string colName)
        {
            if (dr.IsDBNull(dr.GetOrdinal(colName)))
                return null;
            else
            {
                return Convert.ToInt16(dr[colName]);
            }
        }

        public static short? AsNullShort(this IDataReader dr, int columnIndex)
        {
            if (dr.IsDBNull(columnIndex))
                return null;
            else
            {
                return Convert.ToInt16(dr[columnIndex]);
            }
        }


        #endregion

        #region String

        public static string AsString(this IDataReader dr, string colName, bool returnNullAsEmptyString = false)
        {
            if (dr.IsDBNull(dr.GetOrdinal(colName)))
                return returnNullAsEmptyString ? string.Empty : null;
            else
            {
                return (string)(dr[colName]);
            }
        }


        public static string AsString(this IDataReader dr, int columnIndex, bool returnNullAsEmptyString = false)
        {
            if (dr.IsDBNull(columnIndex))
                return returnNullAsEmptyString ? string.Empty : null;
            else
            {
                return (string)(dr[columnIndex]);
            }

        }

        #endregion

        #region TimeSpan


        public static TimeSpan AsTimeSpan(this IDataReader dr, string colName)
        {
            TimeSpan? val = dr.AsNullTimeSpan(colName);
            ThrowExceptionIfIsNull(val, colName, "TimeSpan");
            return val.Value;
        }


        public static TimeSpan AsTimeSpan(this IDataReader dr, int columnIndex)
        {
            TimeSpan? val = dr.AsNullTimeSpan(columnIndex);
            ThrowExceptionIfIsNull(val, columnIndex, "TimeSpan");
            return val.Value;
        }


        // Nulls
        public static TimeSpan? AsNullTimeSpan(this IDataReader dr, string colName)
        {
            if (dr.IsDBNull(dr.GetOrdinal(colName)))
                return null;
            else
            {
                return (TimeSpan)(dr[colName]);
            }
        }

        public static TimeSpan? AsNullTimeSpan(this IDataReader dr, int columnIndex)
        {
            if (dr.IsDBNull(columnIndex))
                return null;
            else
            {
                return (TimeSpan)(dr[columnIndex]);
            }
        }

        //public static TimeSpan AsTimeSpan(this DataRow dr, string colName)
        //{
        //    return DataHelper.GetValueFromRowAsTimeSpan(dr, colName);
        //}

        //public static TimeSpan AsTimeSpan(this DataRow dr, int columnIndex)
        //{
        //    return DataHelper.GetValueFromRowAsTimeSpan(dr, columnIndex);
        //}

        //public static TimeSpan AsTimeSpan(this DataRow dr, DataColumn column)
        //{
        //    return DataHelper.GetValueFromRowAsTimeSpan(dr, column);
        //}
        //// Nulls
        //public static TimeSpan? AsNullTimeSpan(this DataRow dr, string colName)
        //{
        //    return DataHelper.GetValueFromRowAsNullTimeSpan(dr, colName);
        //}

        //public static TimeSpan? AsNullTimeSpan(this DataRow dr, int columnIndex)
        //{
        //    return DataHelper.GetValueFromRowAsNullTimeSpan(dr, columnIndex);
        //}

        //public static TimeSpan? AsNullTimeSpan(this DataRow dr, DataColumn column)
        //{
        //    return DataHelper.GetValueFromRowAsNullTimeSpan(dr, column);
        //}
        #endregion
    }
}