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
using System.Linq;

namespace CA.Blocks.DataAccess
{
    /// <summary>
    /// This is a helper class to add some semantic sugar to the DataHelper class using  extension methods
    /// </summary>
    /// <example>
    ///  <code>
    ///   var varName = DataHelper.GetValueFromRowAsInt(dr, "ColName");
    ///   //can become
    ///   var varName = dr.AsInt("ColName");
    /// </code>
    /// </example>
    public static class DataRowExtensions
    {
        #region Binary
        public static byte[] AsBinary(this DataRow dr, string colName)
        {
            return DataHelper.GetValueFromRowAsBinary(dr, colName);
        }

        public static byte[] AsBinary(this DataRow dr, int columnIndex)
        {
            return DataHelper.GetValueFromRowAsBinary(dr, columnIndex);
        }

        public static byte[] AsBinary(this DataRow dr, DataColumn column)
        {
            return DataHelper.GetValueFromRowAsBinary(dr, column);
        }

        #endregion

        #region bool
        public static bool AsBool(this DataRow dr, string colName)
        {
            return DataHelper.GetValueFromRowAsBool(dr, colName);
        }

        public static bool AsBool(this DataRow dr, int columnindex)
        {
            return DataHelper.GetValueFromRowAsBool(dr, columnindex);
        }

        public static bool AsBool(this DataRow dr, DataColumn column)
        {
            return DataHelper.GetValueFromRowAsBool(dr, column);
        }
        // Nulls
        public static bool? AsNullBool(this DataRow dr, string colName)
        {
            return DataHelper.GetValueFromRowAsNullBool(dr, colName);
        }

        public static bool? AsNullBool(this DataRow dr, int columnindex)
        {
            return DataHelper.GetValueFromRowAsNullBool(dr, columnindex);
        }

        public static bool? AsNullBool(this DataRow dr, DataColumn column)
        {
            return DataHelper.GetValueFromRowAsNullBool(dr, column);
        }
        #endregion

        #region Byte

        public static byte AsByte(this DataRow dr, string colName)
        {
            return DataHelper.GetValueFromRowAsByte(dr, colName);
        }

        public static byte AsByte(this DataRow dr, int columnIndex)
        {
            return DataHelper.GetValueFromRowAsByte(dr, columnIndex);
        }

        public static byte AsByte(this DataRow dr, DataColumn column)
        {
            return DataHelper.GetValueFromRowAsByte(dr, column);
        }
        // Nulls
        public static byte? AsNullByte(this DataRow dr, string colName)
        {
            return DataHelper.GetValueFromRowAsNullByte(dr, colName);
        }

        public static byte? AsNullByte(this DataRow dr, int columnIndex)
        {
            return DataHelper.GetValueFromRowAsNullByte(dr, columnIndex);
        }

        public static byte? AsNullByte(this DataRow dr, DataColumn column)
        {
            return DataHelper.GetValueFromRowAsNullByte(dr, column);
        }

        #endregion

        #region Char

        public static char AsChar(this DataRow dr, string colName)
        {
            return DataHelper.GetValueFromRowAsChar(dr, colName);
        }

        public static char AsChar(this DataRow dr, int columnIndex)
        {
            return DataHelper.GetValueFromRowAsChar(dr, columnIndex);
        }

        public static char AsChar(this DataRow dr, DataColumn column)
        {
            return DataHelper.GetValueFromRowAsChar(dr, column);
        }
        // Nulls
        public static char? AsNullChar(this DataRow dr, string colName)
        {
            return DataHelper.GetValueFromRowAsNullChar(dr, colName);
        }

        public static char? AsNullChar(this DataRow dr, int columnIndex)
        {
            return DataHelper.GetValueFromRowAsNullChar(dr, columnIndex);
        }

        public static char? AsNullChar(this DataRow dr, DataColumn column)
        {
            return DataHelper.GetValueFromRowAsNullChar(dr, column);
        }

        #endregion

        #region DateTime
        public static DateTime AsDateTime(this DataRow dr, string colName)
        {
            return DataHelper.GetValueFromRowAsDateTime(dr, colName);
        }

        public static DateTime AsDateTime(this DataRow dr, int columnIndex)
        {
            return DataHelper.GetValueFromRowAsDateTime(dr, columnIndex);
        }

        public static DateTime AsDateTime(this DataRow dr, DataColumn column)
        {
            return DataHelper.GetValueFromRowAsDateTime(dr, column);
        }
        // Nulls
        public static DateTime? AsNullDateTime(this DataRow dr, string colName)
        {
            return DataHelper.GetValueFromRowAsNullDateTime(dr, colName);
        }

        public static DateTime? AsNullDateTime(this DataRow dr, int columnIndex)
        {
            return DataHelper.GetValueFromRowAsNullDateTime(dr, columnIndex);
        }

        public static DateTime? AsNullDateTime(this DataRow dr, DataColumn column)
        {
            return DataHelper.GetValueFromRowAsNullDateTime(dr, column);
        }

        #endregion

        #region Double
        public static double AsDouble(this DataRow dr, string colName)
        {
            return DataHelper.GetValueFromRowAsDouble(dr, colName);
        }

        public static double AsDouble(this DataRow dr, int columnIndex)
        {
            return DataHelper.GetValueFromRowAsDouble(dr, columnIndex);
        }

        public static double AsDouble(this DataRow dr, DataColumn column)
        {
            return DataHelper.GetValueFromRowAsDouble(dr, column);
        }
        // Nulls
        public static double? AsNullDouble(this DataRow dr, string colName)
        {
            return DataHelper.GetValueFromRowAsNullDouble(dr, colName);
        }

        public static double? AsNullDouble(this DataRow dr, int columnIndex)
        {
            return DataHelper.GetValueFromRowAsNullDouble(dr, columnIndex);
        }

        public static double? AsNullDouble(this DataRow dr, DataColumn column)
        {
            return DataHelper.GetValueFromRowAsNullDouble(dr, column);
        }
        #endregion

        #region Single / float 
        public static float AsFloat(this DataRow dr, string colName)
        {
            return DataHelper.GetValueFromRowAsFloat(dr, colName);
        }

        public static float AsFloat(this DataRow dr, int columnIndex)
        {
            return DataHelper.GetValueFromRowAsFloat(dr, columnIndex);
        }

        public static float AsFloat(this DataRow dr, DataColumn column)
        {
            return DataHelper.GetValueFromRowAsFloat(dr, column);
        }
        // Nulls
        public static float? AsNullFloat(this DataRow dr, string colName)
        {
            return DataHelper.GetValueFromRowAsNullFloat(dr, colName);
        }

        public static float? AsNullFloat(this DataRow dr, int columnIndex)
        {
            return DataHelper.GetValueFromRowAsNullFloat(dr, columnIndex);
        }

        public static float? AsNullFloat(this DataRow dr, DataColumn column)
        {
            return DataHelper.GetValueFromRowAsNullFloat(dr, column);
        }
        // Single 
        public static Single AsSingle(this DataRow dr, string colName)
        {
            return DataHelper.GetValueFromRowAsFloat(dr, colName);
        }

        public static Single AsSingle(this DataRow dr, int columnIndex)
        {
            return DataHelper.GetValueFromRowAsFloat(dr, columnIndex);
        }

        public static Single AsSingle(this DataRow dr, DataColumn column)
        {
            return DataHelper.GetValueFromRowAsFloat(dr, column);
        }
        // Nulls
        public static Single? AsNullSingle(this DataRow dr, string colName)
        {
            return DataHelper.GetValueFromRowAsNullFloat(dr, colName);
        }

        public static Single? AsNullSingle(this DataRow dr, int columnIndex)
        {
            return DataHelper.GetValueFromRowAsNullFloat(dr, columnIndex);
        }

        public static Single? AsNullSingle(this DataRow dr, DataColumn column)
        {
            return DataHelper.GetValueFromRowAsNullFloat(dr, column);
        }

        #endregion

        #region Decimal

        public static decimal AsDecimal(this DataRow dr, string colName)
        {
            return DataHelper.GetValueFromRowAsDecimal(dr, colName);
        }

        public static decimal AsDecimal(this DataRow dr, int columnIndex)
        {
            return DataHelper.GetValueFromRowAsDecimal(dr, columnIndex);
        }

        public static decimal AsDecimal(this DataRow dr, DataColumn column)
        {
            return DataHelper.GetValueFromRowAsDecimal(dr, column);
        }
        // Nulls
        public static decimal? AsNullDecimal(this DataRow dr, string colName)
        {
            return DataHelper.GetValueFromRowAsNullDecimal(dr, colName);
        }

        public static decimal? AsNullDecimal(this DataRow dr, int columnIndex)
        {
            return DataHelper.GetValueFromRowAsNullDecimal(dr, columnIndex);
        }

        public static decimal? AsNullDecimal(this DataRow dr, DataColumn column)
        {
            return DataHelper.GetValueFromRowAsNullDecimal(dr, column);
        }

        #endregion

        #region Guid

        public static Guid AsGuid(this DataRow dr, string colName)
        {
            return DataHelper.GetValueFromRowAsGuid(dr, colName);
        }

        public static Guid AsGuid(this DataRow dr, int columnIndex)
        {
            return DataHelper.GetValueFromRowAsGuid(dr, columnIndex);
        }

        public static Guid AsGuid(this DataRow dr, DataColumn column)
        {
            return DataHelper.GetValueFromRowAsGuid(dr, column);
        }
        // Nulls
        public static Guid? AsNullGuid(this DataRow dr, string colName)
        {
            return DataHelper.GetValueFromRowAsNullGuid(dr, colName);
        }

        public static Guid? AsNullGuid(this DataRow dr, int columnIndex)
        {
            return DataHelper.GetValueFromRowAsNullGuid(dr, columnIndex);
        }

        public static Guid? AsNullGuid(this DataRow dr, DataColumn column)
        {
            return DataHelper.GetValueFromRowAsNullGuid(dr, column);
        }

        #endregion

        #region Int
        public static int AsInt(this DataRow dr, string colName)
        {
            return DataHelper.GetValueFromRowAsInt(dr, colName);
        }

        public static int AsInt(this DataRow dr, int columnindex)
        {
            return DataHelper.GetValueFromRowAsInt(dr, columnindex);
        }

        public static int AsInt(this DataRow dr, DataColumn column)
        {
            return DataHelper.GetValueFromRowAsInt(dr, column);
        }
        // Nulls
        public static int? AsNullInt(this DataRow dr, string colName)
        {
            return DataHelper.GetValueFromRowAsNullInt(dr, colName);
        }

        public static int? AsNullInt(this DataRow dr, int columnindex)
        {
            return DataHelper.GetValueFromRowAsNullInt(dr, columnindex);
        }

        public static int? AsNullInt(this DataRow dr, DataColumn column)
        {
            return DataHelper.GetValueFromRowAsNullInt(dr, column);
        }

        //// As delimited List eg "1,2,3" will be a IList {1,2,3}
        //[System.Obsolete("This logic does not belong here it should be a string extension", true)]
        //public static IList<int> AsIntList(this DataRow dr, string columnName, char delimiter = ',')
        //{
        //    var result = new List<int>();
        //    var s = dr.AsString(columnName);
        //    if (!string.IsNullOrWhiteSpace(s))
        //    {
        //        var sarray = s.Split(delimiter);
        //        result.AddRange(sarray.Select(int.Parse));
        //    }
        //    return result;
        //}

        #endregion

        #region uInt

        public static uint AsUInt(this DataRow dr, string colName)
        {
            return DataHelper.GetValueFromRowAsUInt(dr, colName);
        }

        public static uint AsUInt(this DataRow dr, int columnIndex)
        {
            return DataHelper.GetValueFromRowAsUInt(dr, columnIndex);
        }

        public static uint AsUInt(this DataRow dr, DataColumn column)
        {
            return DataHelper.GetValueFromRowAsUInt(dr, column);
        }

        // Nulls
        public static uint? AsNullUInt(this DataRow dr, string colName)
        {
            return DataHelper.GetValueFromRowAsNullUInt(dr, colName);
        }

        public static uint? AsNullUInt(this DataRow dr, int columnIndex)
        {
            return DataHelper.GetValueFromRowAsNullUInt(dr, columnIndex);
        }

        public static uint? AsNullUInt(this DataRow dr, DataColumn column)
        {
            return DataHelper.GetValueFromRowAsNullUInt(dr, column);
        }

        #endregion

        #region Long

        public static long AsLong(this DataRow dr, string colName)
        {
            return DataHelper.GetValueFromRowAsLong(dr, colName);
        }

        public static long AsLong(this DataRow dr, int columnIndex)
        {
            return DataHelper.GetValueFromRowAsLong(dr, columnIndex);
        }

        public static long AsLong(this DataRow dr, DataColumn column)
        {
            return DataHelper.GetValueFromRowAsLong(dr, column);
        }
        // Nulls
        public static long? AsNullLong(this DataRow dr, string colName)
        {
            return DataHelper.GetValueFromRowAsNullLong(dr, colName);
        }

        public static long? AsNullLong(this DataRow dr, int columnIndex)
        {
            return DataHelper.GetValueFromRowAsNullLong(dr, columnIndex);
        }

        public static long? AsNullLong(this DataRow dr, DataColumn column)
        {
            return DataHelper.GetValueFromRowAsNullLong(dr, column);
        }

        // AS List
        //[System.Obsolete("This logic does not belong here it should be a string extension", true)]
        //public static IList<long> AsLongList(this DataRow dr, string columnName, char delimiter = ',')
        //{
        //    var result = new List<long>();
        //    var s = dr.AsString(columnName);
        //    if (!string.IsNullOrWhiteSpace(s))
        //    {
        //        var sarray = s.Split(delimiter);
        //        result.AddRange(sarray.Select(long.Parse));
        //    }
        //    return result;
        //}
        #endregion

        #region ulong

        
        public static ulong AsULong(this DataRow dr, string colName)
        {
            return DataHelper.GetValueFromRowAsULong(dr, colName);
        }

        public static ulong AsULong(this DataRow dr, int columnIndex)
        {
            return DataHelper.GetValueFromRowAsULong(dr, columnIndex);
        }

        public static ulong AsULong(this DataRow dr, DataColumn column)
        {
            return DataHelper.GetValueFromRowAsULong(dr, column);
        }

        // Nulls
        public static ulong? AsNullULong(this DataRow dr, string colName)
        {
            return DataHelper.GetValueFromRowAsNullULong(dr, colName);
        }

        public static ulong? AsNullULong(this DataRow dr, int columnIndex)
        {
            return DataHelper.GetValueFromRowAsNullULong(dr, columnIndex);
        }

        public static ulong? AsNullULong(this DataRow dr, DataColumn column)
        {
            return DataHelper.GetValueFromRowAsNullULong(dr, column);
        }

        #endregion

        #region Sbyte


        public static sbyte AsSbyte(this DataRow dr, string colName)
        {
            return DataHelper.GetValueFromRowAsSbyte(dr, colName);
        }

        public static sbyte AsSbyte(this DataRow dr, int columnIndex)
        {
            return DataHelper.GetValueFromRowAsSbyte(dr, columnIndex);
        }

        public static sbyte AsSbyte(this DataRow dr, DataColumn column)
        {
            return DataHelper.GetValueFromRowAsSbyte(dr, column);
        }

        // Nulls
        public static sbyte? AsNullSbyte(this DataRow dr, string colName)
        {
            return DataHelper.GetValueFromRowAsNullSbyte(dr, colName);
        }

        public static sbyte? AsNullSbyte(this DataRow dr, int columnIndex)
        {
            return DataHelper.GetValueFromRowAsNullSbyte(dr, columnIndex);
        }

        public static sbyte? AsNullSbyte(this DataRow dr, DataColumn column)
        {
            return DataHelper.GetValueFromRowAsNullSbyte(dr, column);
        }

        #endregion

        #region Short

        public static short AsShort(this DataRow dr, string colName)
        {
            return DataHelper.GetValueFromRowAsShort(dr, colName);
        }

        public static short AsShort(this DataRow dr, int columnIndex)
        {
            return DataHelper.GetValueFromRowAsShort(dr, columnIndex);
        }

        public static short AsShort(this DataRow dr, DataColumn column)
        {
            return DataHelper.GetValueFromRowAsShort(dr, column);
        }
        // Nulls
        public static short? AsNullShort(this DataRow dr, string colName)
        {
            return DataHelper.GetValueFromRowAsNullShort(dr, colName);
        }

        public static short? AsNullShort(this DataRow dr, int columnIndex)
        {
            return DataHelper.GetValueFromRowAsNullShort(dr, columnIndex);
        }

        public static short? AsNullShort(this DataRow dr, DataColumn column)
        {
            return DataHelper.GetValueFromRowAsNullShort(dr, column);
        }
        


        //public static IList<short> AsShortList(this DataRow dr, string columnName, char delimiter = ',')
        //{
        //    var result = new List<short>();
        //    var s = dr.AsString(columnName);
        //    if (!string.IsNullOrWhiteSpace(s))
        //    {
        //        var sarray = s.Split(delimiter);
        //        result.AddRange(sarray.Select(short.Parse));
        //    }
        //    return result;
        //}

        #endregion

        #region uInt


        public static ushort AsUShort(this DataRow dr, string colName)
        {
            return DataHelper.GetValueFromRowAsUShort(dr, colName);
        }

        public static ushort AsUShort(this DataRow dr, int columnIndex)
        {
            return DataHelper.GetValueFromRowAsUShort(dr, columnIndex);
        }

        public static ushort AsUShort(this DataRow dr, DataColumn column)
        {
            return DataHelper.GetValueFromRowAsUShort(dr, column);
        }

        // Nulls
        public static ushort? AsNullUShort(this DataRow dr, string colName)
        {
            return DataHelper.GetValueFromRowAsNullUShort(dr, colName);
        }

        public static ushort? AsNullUShort(this DataRow dr, int columnIndex)
        {
            return DataHelper.GetValueFromRowAsNullUShort(dr, columnIndex);
        }

        public static ushort? AsNullUShort(this DataRow dr, DataColumn column)
        {
            return DataHelper.GetValueFromRowAsNullUShort(dr, column);
        }

        #endregion

        #region String
        public static string AsString(this DataRow dr, string colName, bool returnNullAsEmptyString = false)
        {
            return DataHelper.GetValueFromRowAsString(dr, colName, returnNullAsEmptyString);
        }
        public static string AsString(this DataRow dr, int columnOrder, bool returnNullAsEmptyString = false)
        {
            return DataHelper.GetValueFromRowAsString(dr, columnOrder, returnNullAsEmptyString);
        }
        public static string AsString(this DataRow dr, DataColumn column, bool returnNullAsEmptyString = false)
        {
            return DataHelper.GetValueFromRowAsString(dr, column, returnNullAsEmptyString);
        }

        #endregion 

        #region TimeSpan

        public static TimeSpan AsTimeSpan(this DataRow dr, string colName)
        {
            return DataHelper.GetValueFromRowAsTimeSpan(dr, colName);
        }

        public static TimeSpan AsTimeSpan(this DataRow dr, int columnIndex)
        {
            return DataHelper.GetValueFromRowAsTimeSpan(dr, columnIndex);
        }

        public static TimeSpan AsTimeSpan(this DataRow dr, DataColumn column)
        {
            return DataHelper.GetValueFromRowAsTimeSpan(dr, column);
        }
        // Nulls
        public static TimeSpan? AsNullTimeSpan(this DataRow dr, string colName)
        {
            return DataHelper.GetValueFromRowAsNullTimeSpan(dr, colName);
        }

        public static TimeSpan? AsNullTimeSpan(this DataRow dr, int columnIndex)
        {
            return DataHelper.GetValueFromRowAsNullTimeSpan(dr, columnIndex);
        }

        public static TimeSpan? AsNullTimeSpan(this DataRow dr, DataColumn column)
        {
            return DataHelper.GetValueFromRowAsNullTimeSpan(dr, column);
        }
        #endregion
    }
}
