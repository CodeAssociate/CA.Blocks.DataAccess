//===============================================================================
// Copyright (C) 2002-2020 Ravin Enterprises Ltd. 
// All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Data;

namespace CA.Blocks.DataAccess
{
    //TODO we need to benchmark some of these procedure https://www.nuget.org/packages/BenchmarkDotNet/
    // https://stackoverflow.com/questions/1170756/casting-vs-converting-an-object-tostring-when-object-really-is-a-string

    public static class IDataReaderExtensions
    {
        
        private static T ThrowExceptionIfIsNull<T>(T? obj, string sColumnName, string typeDescription)  where T : struct
        {
            if (obj == null)
            {
                throw new ArgumentNullException(
                    $"Tried to get {sColumnName} from row as non-nullable {typeDescription}, however value is NULL.");
            }
            return obj.Value;
        }

        private static T ThrowExceptionIfIsNull<T>(T? obj, int columnIndex, string typeDescription) where T : struct
        {
            if (obj == null)
            {
                throw new ArgumentNullException(
                    $"Tried to get col in position {columnIndex} from row as non-nullable {typeDescription}, however value is NULL.");
            }
            return obj.Value;
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
            var val = dr.AsNullBool(colName);
            return ThrowExceptionIfIsNull(val, colName, "bool");
        }


        public static bool AsBool(this IDataReader dr, int columnIndex)
        {
            var val = dr.AsNullBool(columnIndex);
            return ThrowExceptionIfIsNull(val, columnIndex, "bool");
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
            var val = dr.AsNullByte(colName);
            return ThrowExceptionIfIsNull(val, colName, "byte");
        }
        
        public static byte AsByte(this IDataReader dr, int columnIndex)
        {
            var val = dr.AsNullByte(columnIndex);
            return ThrowExceptionIfIsNull(val, columnIndex, "byte");
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

        #region Binary
        public static byte[] AsBinary(this IDataReader dr, string colName)
        {
            if (dr.IsDBNull(dr.GetOrdinal(colName)))
                return null;
            else
            {

                return (byte[])(dr[colName]);
            }
        }

        public static byte[] AsBinary(this IDataReader dr, int columnIndex)
        {
            if (dr.IsDBNull(columnIndex))
                return null;
            else
            {
                return (byte[])(dr[columnIndex]);
            }
        }

        #endregion
        #region SByte
        public static sbyte AsSbyte(this IDataReader dr, string colName)
        {
            var val = dr.AsNullSbyte(colName);
            return ThrowExceptionIfIsNull(val, colName, "byte");
        }


        public static sbyte AsSbyte(this IDataReader dr, int columnIndex)
        {
            var val = dr.AsNullSbyte(columnIndex);
            return ThrowExceptionIfIsNull(val, columnIndex, "byte");
        }


        // Nulls
        public static sbyte? AsNullSbyte(this IDataReader dr, string colName)
        {
            if (dr.IsDBNull(dr.GetOrdinal(colName)))
                return null;
            else
            {
                return Convert.ToSByte(dr[colName]);
            }
        }

        public static sbyte? AsNullSbyte(this IDataReader dr, int columnIndex)
        {
            if (dr.IsDBNull(columnIndex))
                return null;
            else
            {
                return Convert.ToSByte(dr[columnIndex]);
            }
        }
        #endregion

        #region Char

        public static char AsChar(this IDataReader dr, string colName)
        {
            var val = dr.AsNullChar(colName);
            return ThrowExceptionIfIsNull(val, colName, "char");
        }


        public static char AsChar(this IDataReader dr, int columnIndex)
        {
            var val = dr.AsNullChar(columnIndex);
            return ThrowExceptionIfIsNull(val, columnIndex, "char");
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
            var val = dr.AsNullDateTime(colName);
            return ThrowExceptionIfIsNull(val, colName, "DateTime");
        }


        public static DateTime AsDateTime(this IDataReader dr, int columnIndex)
        {
            var val = dr.AsNullDateTime(columnIndex);
            return ThrowExceptionIfIsNull(val, columnIndex, "DateTime");
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

        #region DateTimeOffset
        public static DateTimeOffset AsDateTimeOffset(this IDataReader dr, string colName)
        {
            var val = dr.AsNullDateTimeOffset(colName);
            return ThrowExceptionIfIsNull(val, colName, "DateTimeOffSet");
        }

        public static DateTimeOffset AsDateTimeOffset(this IDataReader dr, int columnIndex)
        {
            var val = dr.AsNullDateTimeOffset(columnIndex);
            return ThrowExceptionIfIsNull(val, columnIndex, "DateTimeOffSet");
        }

        // Nulls
        public static DateTimeOffset? AsNullDateTimeOffset(this IDataReader dr, string colName)
        {
            if (dr.IsDBNull(dr.GetOrdinal(colName)))
                return null;
            else
            {
                return (DateTimeOffset)(dr[colName]);
            }
        }

        public static DateTimeOffset? AsNullDateTimeOffset(this IDataReader dr, int columnIndex)
        {
            if (dr.IsDBNull(columnIndex))
                return null;
            else
            {
                return (DateTimeOffset)(dr[columnIndex]);
            }
        }
        #endregion

        #region Double

        public static double AsDouble(this IDataReader dr, string colName)
        {
            var val = dr.AsNullDouble(colName);
            return ThrowExceptionIfIsNull(val, colName, "double");
        }


        public static double AsDouble(this IDataReader dr, int columnIndex)
        {
            var val = dr.AsNullDouble(columnIndex);
            return ThrowExceptionIfIsNull(val, columnIndex, "double");
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
            var val = dr.AsNullSingle(colName);
            return ThrowExceptionIfIsNull(val, colName, "single");
        }


        public static Single AsSingle(this IDataReader dr, int columnIndex)
        {
            var val = dr.AsNullSingle(columnIndex);
            return ThrowExceptionIfIsNull(val, columnIndex, "single");
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
            var val = dr.AsNullDecimal(colName);
            return ThrowExceptionIfIsNull(val, colName, "decimal");
        }


        public static decimal AsDecimal(this IDataReader dr, int columnIndex)
        {
            var val = dr.AsNullDecimal(columnIndex);
            return ThrowExceptionIfIsNull(val, columnIndex, "decimal");
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
            var val = dr.AsNullGuid(colName);
            return ThrowExceptionIfIsNull(val, colName, "Guid");
        }
        
        public static Guid AsGuid(this IDataReader dr, int columnIndex)
        {
            var val = dr.AsNullGuid(columnIndex);
            return ThrowExceptionIfIsNull(val, columnIndex, "Guid");
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
            var val = dr.AsNullInt(colName);
            return ThrowExceptionIfIsNull(val, colName, "int");
        }


        public static int AsInt(this IDataReader dr, int columnIndex)
        {
            var val = dr.AsNullInt(columnIndex);
            return ThrowExceptionIfIsNull(val, columnIndex, "int");
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


        #region UInt

        public static uint AsUInt(this IDataReader dr, string colName)
        {
            var val = dr.AsNullUInt(colName);
            return ThrowExceptionIfIsNull(val, colName, "uint");
        }


        public static uint AsUInt(this IDataReader dr, int columnIndex)
        {
            var val = dr.AsNullUInt(columnIndex);
            return ThrowExceptionIfIsNull(val, columnIndex, "uint");
        }


        // Nulls
        public static uint? AsNullUInt(this IDataReader dr, string colName)
        {
            if (dr.IsDBNull(dr.GetOrdinal(colName)))
                return null;
            else
            {
                return Convert.ToUInt32(dr[colName]);
            }
        }

        public static uint? AsNullUInt(this IDataReader dr, int columnIndex)
        {
            if (dr.IsDBNull(columnIndex))
                return null;
            else
            {
                return Convert.ToUInt32(dr[columnIndex]);
            }
        }

        #endregion


        #region Long

        public static long AsLong(this IDataReader dr, string colName)
        {
            var val = dr.AsNullLong(colName);
            return ThrowExceptionIfIsNull(val, colName, "long");
        }


        public static long AsLong(this IDataReader dr, int columnIndex)
        {
            var val = dr.AsNullLong(columnIndex);
            return ThrowExceptionIfIsNull(val, columnIndex, "long");
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

        #region ulong

        public static ulong AsULong(this IDataReader dr, string colName)
        {
            var val = dr.AsNullULong(colName);
            return ThrowExceptionIfIsNull(val, colName, "short");
        }
        
        public static ulong AsULong(this IDataReader dr, int columnIndex)
        {
            var val = dr.AsNullULong(columnIndex);
            return ThrowExceptionIfIsNull(val, columnIndex, "short");
        }
        // Nulls
        public static ulong? AsNullULong(this IDataReader dr, string colName)
        {
            if (dr.IsDBNull(dr.GetOrdinal(colName)))
                return null;
            else
            {
                return Convert.ToUInt64(dr[colName]);
            }
        }

        public static ulong? AsNullULong(this IDataReader dr, int columnIndex)
        {
            if (dr.IsDBNull(columnIndex))
                return null;
            else
            {
                return Convert.ToUInt64(dr[columnIndex]);
            }
        }

        #endregion

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
            var val = dr.AsNullShort(colName);
            return ThrowExceptionIfIsNull(val, colName, "short");
        }


        public static short AsShort(this IDataReader dr, int columnIndex)
        {
            var val = dr.AsNullShort(columnIndex);
            return ThrowExceptionIfIsNull(val, columnIndex, "short");
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

        #region UShort

        public static ushort AsUShort(this IDataReader dr, string colName)
        {
            var val = dr.AsNullUShort(colName);
            return ThrowExceptionIfIsNull(val, colName, "short");
        }


        public static ushort AsUShort(this IDataReader dr, int columnIndex)
        {
            var val = dr.AsNullUShort(columnIndex);
            return ThrowExceptionIfIsNull(val, columnIndex, "short");
        }


        // Nulls
        public static ushort? AsNullUShort(this IDataReader dr, string colName)
        {
            if (dr.IsDBNull(dr.GetOrdinal(colName)))
                return null;
            else
            {
                return Convert.ToUInt16(dr[colName]);
            }
        }

        public static ushort? AsNullUShort(this IDataReader dr, int columnIndex)
        {
            if (dr.IsDBNull(columnIndex))
                return null;
            else
            {
                return Convert.ToUInt16(dr[columnIndex]);
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
                //TO DO CAST vrs convert? 
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

        /// <summary>
        /// The AsToString is more forgiving of type conversions 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="colName"></param>
        /// <param name="returnNullAsEmptyString"></param>
        /// <returns></returns>
        public static string AsToString(this IDataReader dr, string colName, bool returnNullAsEmptyString = false)
        {
            if (dr.IsDBNull(dr.GetOrdinal(colName)))
                return returnNullAsEmptyString ? string.Empty : null;
            else
            {
             
                return dr[colName].ToString();
            }
        }


        public static string AsToString(this IDataReader dr, int columnIndex, bool returnNullAsEmptyString = false)
        {
            if (dr.IsDBNull(columnIndex))
                return returnNullAsEmptyString ? string.Empty : null;
            else
            {
                return dr[columnIndex].ToString();
            }

        }



        #endregion

        #region TimeSpan


        public static TimeSpan AsTimeSpan(this IDataReader dr, string colName)
        {
            var val = dr.AsNullTimeSpan(colName);
            return ThrowExceptionIfIsNull(val, colName, "TimeSpan");
        }


        public static TimeSpan AsTimeSpan(this IDataReader dr, int columnIndex)
        {
            var val = dr.AsNullTimeSpan(columnIndex);
            return ThrowExceptionIfIsNull(val, columnIndex, "TimeSpan");
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