//===============================================================================
// Code Associate Data Access Block for .NET Core
// DataHelper.cs
//
//===============================================================================
// Copyright (C) 2002-2022 Ravin Enterprises Ltd. 
// All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================


using System;
using System.Data;
using System.Diagnostics;

namespace CA.Blocks.DataAccess
{
    /// <summary>
    /// This class is a helper class for dealing data values.  It is intended to be a static helper class only.
    /// </summary>
    public static class DataHelper
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

        /// <summary>
        /// Will get the data value from the row as an object retuning the .NET null value  in the event the data value is 
        /// null in the DataRow.
        /// </summary>
        /// <param name="dr"> A Valid <see cref="System.Data.DataRow"/> DataRow</param>
        /// <param name="sColumnName">The Name of the Column in the DataRow</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static object GetValueFromRow(DataRow dr, string sColumnName)
        {
            return dr.IsNull(sColumnName) ? null : dr[sColumnName];
        }

        /// <summary>
        /// Will get the data value from the row as an object retuning the .NET null value  in the event the data value is 
        /// null in the DataRow.
        /// </summary>
        /// <param name="dr"> A Valid <see cref="System.Data.DataRow"/> DataRow</param>
        /// <param name="column">The Column that belongs to the DataTable</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static object GetValueFromRow(DataRow dr, DataColumn column)
        {
            return dr.IsNull(column) ? null : dr[column];
        }

        /// <summary>
        /// Will get the data value from the row as an object retuning the .NET null value  in the event the data value is 
        /// null in the DataRow.
        /// </summary>
        /// <param name="dr"> A Valid <see cref="System.Data.DataRow"/> DataRow</param>
        /// <param name="columnIndex">The index of the Column that belongs to the DataTable</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static object GetValueFromRow(DataRow dr, int columnIndex)
        {
            return !dr.IsNull(columnIndex) ? dr[columnIndex] : null;
        }

        /// <summary>
        /// Will get the data value from the row as a string. The return value will be set to either null or and empty string depending 
        /// on the value of returnNullAsEmptyString, 
        /// </summary>
        /// <remarks>This method assumes the data comes from the data source as a string and will cast. This has the best performance but will not to conversion
        /// if you need conversion example selecting an int as a string then use <see cref="GetValueFromRowAsToString(DataRow, string, bool)"/></remarks>
        /// <param name="dr">A Valid <see cref="System.Data.DataRow"/> DataRow</param>
        /// <param name="sColumnName"> The Name of the Column in the DataRow </param>
        /// <param name="returnNullAsEmptyString">Sets the attribute on how an empty string will be treated, it true it will return string.empty else it will return null. </param>
        /// <returns></returns>
        public static string GetValueFromRowAsString(DataRow dr, string sColumnName, bool returnNullAsEmptyString = false)
        {
            var result = GetValueFromRow(dr, sColumnName);
            if (result == null && returnNullAsEmptyString)
                result = string.Empty;
            return (string)result;
        }

        /// <inheritdoc cref="GetValueFromRowAsString(DataRow, string, bool)" />
        public static string GetValueFromRowAsString(DataRow dr, int columnOrder, bool returnNullAsEmptyString = false)
        {
            object result = GetValueFromRow(dr, columnOrder);
            if (result == null && returnNullAsEmptyString)
                result = string.Empty;
            return (string)result;
        }

        /// <inheritdoc cref="GetValueFromRowAsString(DataRow, string, bool)" />
        public static string GetValueFromRowAsString(DataRow dr, DataColumn column, bool returnNullAsEmptyString = false)
        {
            object result = GetValueFromRow(dr, column);
            if (result == null && returnNullAsEmptyString)
                result = string.Empty;
            return (string)result;
        }

        // The AsToString is slower but will auto convert types to string like int 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="sColumnName"></param>
        /// <param name="returnNullAsEmptyString"></param>
        /// <returns></returns>
        public static string GetValueFromRowAsToString(DataRow dr, string sColumnName, bool returnNullAsEmptyString = false)
        {
            var result = GetValueFromRow(dr, sColumnName);
            if (result == null && returnNullAsEmptyString)
                result = string.Empty;
            return result.ToString();
        }

        public static string GetValueFromRowAsToString(DataRow dr, int columnOrder, bool returnNullAsEmptyString = false)
        {
            object result = GetValueFromRow(dr, columnOrder);
            if (result == null && returnNullAsEmptyString)
                result = string.Empty;
            return result.ToString();
        }

        public static string GetValueFromRowAsToString(DataRow dr, DataColumn column, bool returnNullAsEmptyString = false)
        {
            object result = GetValueFromRow(dr, column);
            if (result == null && returnNullAsEmptyString)
                result = string.Empty;
            return result.ToString();
        }

        /// <summary>
        /// Will get the data value from the row as a nullable int. The return value will be set to either null or the int value
        /// This procedure assumes that the data is an integer, if not a cast exception will be thrown.  
        /// </summary>
        /// <param name="dr">A Valid <see cref="System.Data.DataRow"/> DataRow</param>
        /// <param name="sColumnName">The Name of the Column in the DataRow</param>
        /// <returns></returns>
        public static int? GetValueFromRowAsNullInt(DataRow dr, string sColumnName)
        {
            var dbValue = GetValueFromRow(dr, sColumnName);
            return dbValue == null ? (int?)null : Convert.ToInt32(dbValue);
        }
        
        /// <summary>
        /// Will get the data value from the row as a nullable int. The return value will be set to either null or the int value
        /// This procedure assumes that the data is an integer, if not a cast exception will be thrown.  
        /// </summary>
        /// <param name="dr">A Valid <see cref="System.Data.DataRow"/> DataRow</param>
        /// <param name="column">The Column inside the datarow</param>
        /// <returns></returns>
        public static int? GetValueFromRowAsNullInt(DataRow dr, DataColumn column)
        {
            var dbValue = GetValueFromRow(dr, column);
            return dbValue == null ? (int?)null : Convert.ToInt32(dbValue);
        }

        public static int? GetValueFromRowAsNullInt(DataRow dr, int columnOrder)
        {
            var dbValue = GetValueFromRow(dr, columnOrder);
            return dbValue == null ? (int?)null : Convert.ToInt32(dbValue);
        }

        public static uint? GetValueFromRowAsNullUInt(DataRow dr, string sColumnName)
        {
            var dbValue = GetValueFromRow(dr, sColumnName);
            return dbValue == null ? (uint?)null : Convert.ToUInt32(dbValue);
        }

        public static uint? GetValueFromRowAsNullUInt(DataRow dr, DataColumn column)
        {
            var dbValue = GetValueFromRow(dr, column);
            return dbValue == null ? (uint?)null : Convert.ToUInt32(dbValue);
        }
        
        public static uint? GetValueFromRowAsNullUInt(DataRow dr, int columnOrder)
        {
            var dbValue = GetValueFromRow(dr, columnOrder);
            return dbValue == null ? (uint?)null : Convert.ToUInt32(dbValue);
        }

        /// <summary>
        /// Will get the data value from the row as an int. If the value is null an <see cref="ArgumentNullException"/> will be thrown
        /// This procedure assumes that the data is an integer, if not a cast exception will be thrown.  
        /// </summary>
        /// <param name="dr">A Valid <see cref="System.Data.DataRow"/> DataRow</param>
        /// <param name="sColumnName">The Name of the Column in the DataRow</param>
        /// <returns></returns>
        public static int GetValueFromRowAsInt(DataRow dr, string sColumnName)
        {
            var val = GetValueFromRowAsNullInt(dr, sColumnName);
            return ThrowExceptionIfIsNull(val, sColumnName, "int");
        }
        
        /// <summary>
        /// Will get the data value from the row as an int. If the value is null an <see cref="ArgumentNullException"/> will be thrown
        /// This procedure assumes that the data is an integer, if not a cast exception will be thrown.  
        /// </summary>
        /// <param name="dr">A Valid <see cref="System.Data.DataRow"/> DataRow</param>
        /// <param name="column">The Column which is part of the data row</param>
        /// <returns></returns>
        public static int GetValueFromRowAsInt(DataRow dr, DataColumn column)
        {
            int? val = GetValueFromRowAsNullInt(dr, column);
            return ThrowExceptionIfIsNull(val, column.ColumnName, "int");
        }
        
        public static int GetValueFromRowAsInt(DataRow dr, int columnIndex)
        {
            int? val = GetValueFromRowAsNullInt(dr, columnIndex);
            return ThrowExceptionIfIsNull(val, columnIndex, "int");
        }

        public static uint GetValueFromRowAsUInt(DataRow dr, string sColumnName)
        {
            uint? val = GetValueFromRowAsNullUInt(dr, sColumnName);
            return ThrowExceptionIfIsNull(val, sColumnName, "uint");
        }

        public static uint GetValueFromRowAsUInt(DataRow dr, DataColumn column)
        {
            uint? val = GetValueFromRowAsNullUInt(dr, column);
            return ThrowExceptionIfIsNull(val, column.ColumnName, "uint");
        }
        
        public static uint GetValueFromRowAsUInt(DataRow dr, int columnIndex)
        {
            uint? val = GetValueFromRowAsNullUInt(dr, columnIndex);
            return ThrowExceptionIfIsNull(val, columnIndex, "uint");
        }

        /// <summary>
        /// Will get the data value from the row as a nullable Decimal. The return value will be set to either null or the Decimal value
        /// This procedure assumes that the data is an Decimaleger, if not a cast exception will be thrown.  
        /// </summary>
        /// <param name="dr">A Valid <see cref="System.Data.DataRow"/> DataRow</param>
        /// <param name="sColumnName">The Name of the Column in the DataRow</param>
        /// <returns></returns>
        public static Decimal? GetValueFromRowAsNullDecimal(DataRow dr, string sColumnName)
        {
            return ((Decimal?)GetValueFromRow(dr, sColumnName));
        }
        
        public static Decimal? GetValueFromRowAsNullDecimal(DataRow dr, int columnIndex)
        {
            return ((Decimal?)GetValueFromRow(dr, columnIndex));
        }
        
        public static Decimal? GetValueFromRowAsNullDecimal(DataRow dr, DataColumn dc)
        {
            return ((Decimal?)GetValueFromRow(dr, dc));
        }

        /// <summary>
        /// Will get the data value from the row as an Decimal. If the value is null an <see cref="ArgumentNullException"/> will be thrown
        /// This procedure assumes that the data is an Decimaleger, if not a cast exception will be thrown.  
        /// </summary>
        /// <param name="dr">A Valid <see cref="System.Data.DataRow"/> DataRow</param>
        /// <param name="sColumnName">The Name of the Column in the DataRow</param>
        /// <returns></returns>
        public static Decimal GetValueFromRowAsDecimal(DataRow dr, string sColumnName)
        {
            Decimal? val = GetValueFromRowAsNullDecimal(dr, sColumnName);
            return ThrowExceptionIfIsNull(val, sColumnName, "Decimal");
        }

        public static Decimal GetValueFromRowAsDecimal(DataRow dr, int columnIndex)
        {
            Decimal? val = GetValueFromRowAsNullDecimal(dr, columnIndex);
            return ThrowExceptionIfIsNull(val, columnIndex, "Decimal");
        }
        
        public static Decimal GetValueFromRowAsDecimal(DataRow dr, DataColumn dc)
        {
            Decimal? val = GetValueFromRowAsNullDecimal(dr, dc);
            return ThrowExceptionIfIsNull(val, dc.ColumnName, "Decimal");
        }

        /// <summary>
        /// Will get the data value from the row as a nullable Double. The return value will be set to either null or the Double value
        /// This procedure assumes that the data is an Double, if not a cast exception will be thrown.  
        /// </summary>
        /// <param name="dr">A Valid <see cref="System.Data.DataRow"/> DataRow</param>
        /// <param name="sColumnName">The Name of the Column in the DataRow</param>
        /// <returns></returns>
        public static Double? GetValueFromRowAsNullDouble(DataRow dr, string sColumnName)
        {
            return ((Double?)GetValueFromRow(dr, sColumnName));
        }

        public static Double? GetValueFromRowAsNullDouble(DataRow dr, int columnIndex)
        {
            return ((Double?)GetValueFromRow(dr, columnIndex));
        }
        
        public static Double? GetValueFromRowAsNullDouble(DataRow dr, DataColumn dc)
        {
            return ((Double?)GetValueFromRow(dr, dc));
        }

        /// <summary>
        /// Will get the data value from the row as an Double. If the value is null an <see cref="ArgumentNullException"/> will be thrown
        /// This procedure assumes that the data is an Double, if not a cast exception will be thrown.  
        /// </summary>
        /// <param name="dr">A Valid <see cref="System.Data.DataRow"/> DataRow</param>
        /// <param name="sColumnName">The Name of the Column in the DataRow</param>
        /// <returns></returns>
        public static Double GetValueFromRowAsDouble(DataRow dr, string sColumnName)
        {
            Double? val = GetValueFromRowAsNullDouble(dr, sColumnName);
            return ThrowExceptionIfIsNull(val, sColumnName, "Double");
        }

        public static Double GetValueFromRowAsDouble(DataRow dr, int columnIndex)
        {
            Double? val = GetValueFromRowAsNullDouble(dr, columnIndex);
            return ThrowExceptionIfIsNull(val, columnIndex, "Double");
        }
        
        public static Double GetValueFromRowAsDouble(DataRow dr, DataColumn dc)
        {
            Double? val = GetValueFromRowAsNullDouble(dr, dc);
            return ThrowExceptionIfIsNull(val, dc.ColumnName, "Double");
        }
        
        public static float? GetValueFromRowAsNullFloat(DataRow dr, string sColumnName)
        {
            return ((float?)GetValueFromRow(dr, sColumnName));
        }

        public static float? GetValueFromRowAsNullFloat(DataRow dr, int columnIndex)
        {
            return ((float?)GetValueFromRow(dr, columnIndex));
        }
        
        public static float? GetValueFromRowAsNullFloat(DataRow dr, DataColumn dc)
        {
            return ((float?)GetValueFromRow(dr, dc));
        }
        
        public static float GetValueFromRowAsFloat(DataRow dr, string sColumnName)
        {
            float? val = GetValueFromRowAsNullFloat(dr, sColumnName);
            return ThrowExceptionIfIsNull(val, sColumnName, "float");
        }
        
        public static float GetValueFromRowAsFloat(DataRow dr, int columnIndex)
        {
            float? val = GetValueFromRowAsNullFloat(dr, columnIndex);
            return ThrowExceptionIfIsNull(val, columnIndex, "float");
        }

        public static float GetValueFromRowAsFloat(DataRow dr, DataColumn dc)
        {
            float? val = GetValueFromRowAsNullFloat(dr, dc);
            return ThrowExceptionIfIsNull(val, dc.ColumnName, "float");
        }

        /// <summary>
        /// Will get the data value from the row as a nullable long. The return value will be set to either null or the long value
        /// This procedure assumes that the data is an long, if not a cast exception will be thrown.  
        /// </summary>
        /// <param name="dr">A Valid <see cref="System.Data.DataRow"/> DataRow</param>
        /// <param name="sColumnName">The Name of the Column in the DataRow</param>
        /// <returns></returns>
        
        public static long? GetValueFromRowAsNullLong(DataRow dr, string sColumnName)
        {
            return ((long?)GetValueFromRow(dr, sColumnName));
        }

        
        public static long? GetValueFromRowAsNullLong(DataRow dr, int columnIndex)
        {
            return ((long?)GetValueFromRow(dr, columnIndex));
        }

        /// <summary>
        /// Will get the data value from the row as a nullable long. The return value will be set to either null or the long value
        /// This procedure assumes that the data is an long, if not a cast exception will be thrown.  
        /// </summary>
        /// <param name="dr">A Valid <see cref="System.Data.DataRow"/> DataRow</param>
        /// <param name="column">The Name of the Column in the DataRow</param>
        /// <returns></returns>
        
        public static long? GetValueFromRowAsNullLong(DataRow dr, DataColumn column)
        {
            return ((long?)GetValueFromRow(dr, column));
        }

        /// <summary>
        /// Will get the data value from the row as an long. If the value is null an <see cref="ArgumentNullException"/> will be thrown
        /// This procedure assumes that the data is an long, if not a cast exception will be thrown.  
        /// </summary>
        /// <param name="dr">A Valid <see cref="System.Data.DataRow"/> DataRow</param>
        /// <param name="sColumnName">The Name of the Column in the DataRow</param>
        /// <returns></returns>
        
        public static long GetValueFromRowAsLong(DataRow dr, string sColumnName)
        {
            long? val = GetValueFromRowAsNullLong(dr, sColumnName);
            return ThrowExceptionIfIsNull(val, sColumnName, "long");
        }
        
        public static long GetValueFromRowAsLong(DataRow dr, int columnIndex)
        {
            long? val = GetValueFromRowAsNullLong(dr, columnIndex);
            return ThrowExceptionIfIsNull(val, columnIndex, "long");
        }

        /// <summary>
        /// Will get the data value from the row as an long. If the value is null an <see cref="ArgumentNullException"/> will be thrown
        /// This procedure assumes that the data is an long, if not a cast exception will be thrown.  
        /// </summary>
        /// <param name="dr">A Valid <see cref="System.Data.DataRow"/> DataRow</param>
        /// <param name="column">The Column in the DataRow</param>
        /// <returns></returns>
        
        public static long GetValueFromRowAsLong(DataRow dr, DataColumn column)
        {
            long? val = GetValueFromRowAsNullLong(dr, column);
            return ThrowExceptionIfIsNull(val, column.ColumnName, "long");
        }
        
        public static ulong? GetValueFromRowAsNullULong(DataRow dr, string sColumnName)
        {
            return ((ulong?)GetValueFromRow(dr, sColumnName));
        }

        public static ulong? GetValueFromRowAsNullULong(DataRow dr, int columnIndex)
        {
            return ((ulong?)GetValueFromRow(dr, columnIndex));
        }

        public static ulong? GetValueFromRowAsNullULong(DataRow dr, DataColumn column)
        {
            return ((ulong?)GetValueFromRow(dr, column));
        }

        public static ulong GetValueFromRowAsULong(DataRow dr, string sColumnName)
        {
            ulong? val = GetValueFromRowAsNullULong(dr, sColumnName);
            return ThrowExceptionIfIsNull(val, sColumnName, "ulong");
        }

        public static ulong GetValueFromRowAsULong(DataRow dr, int columnIndex)
        {
            ulong? val = GetValueFromRowAsNullULong(dr, columnIndex);
            return ThrowExceptionIfIsNull(val, columnIndex, "ulong");
        }

        public static ulong GetValueFromRowAsULong(DataRow dr, DataColumn column)
        {
            ulong? val = GetValueFromRowAsNullULong(dr, column);
            return ThrowExceptionIfIsNull(val, column.ColumnName, "ulong");
        }
        
        /// <summary>
        /// Will get a the data value from the data row as a bool. If a DB Null is found then throw a NullException
        /// </summary>
        /// <param name="dr">A Valid <see cref="System.Data.DataRow"/> DataRow</param>
        /// <param name="sColumnName">The Name of the Column in the DataRow</param>
        /// <returns></returns>
        public static bool GetValueFromRowAsBool(DataRow dr, string sColumnName)
        {
            bool? val = GetValueFromRowAsNullBool(dr, sColumnName);
            return ThrowExceptionIfIsNull(val, sColumnName, "bool");
        }

        public static bool GetValueFromRowAsBool(DataRow dr, int columnIndex)
        {
            bool? val = GetValueFromRowAsNullBool(dr, columnIndex);
            return ThrowExceptionIfIsNull(val, columnIndex, "bool");
        }
        
        public static bool GetValueFromRowAsBool(DataRow dr, DataColumn dc)
        {
            bool? val = GetValueFromRowAsNullBool(dr, dc);
            return ThrowExceptionIfIsNull(val, dc.ColumnName, "bool");
        }
        
        // This is old code if a return value and be null we need to use the nullable Type. 
        /*/// <summary>
        /// Will get a the data value from the data row as a bool. If a DB Null is found then it will return the default bool value defined in DefaultReturnValueIfNull
        /// </summary>
        /// <param name="dr">A Valid <see cref="System.Data.DataRow"/> DataRow</param>
        /// <param name="sColumnName">The Name of the Column in the DataRow</param>
        /// <param name="defaultReturnValueIfNull"> The default value in the event the DB col is null</param>
        /// <returns></returns>
        public static bool GetValueFromRowAsBool(DataRow dr, string sColumnName, bool defaultReturnValueIfNull)
        {
            bool? val = GetValueFromRowAsNullBool(dr, sColumnName);
            return val == null ? defaultReturnValueIfNull : (bool)val;
        }*/
        public static bool? GetValueFromRowAsNullBool(DataRow dr, string sColumnName)
        {
            object value = GetValueFromRow(dr, sColumnName);
            if (value != null)
            {
                if (value is bool b)
                {
                    return b;
                }
                else // not all database have bit.. ie in MySQL it will come back as a ulong or ushort... in this case we need to convert  the bool value. 
                {
                    return Convert.ToBoolean(value);
                }
            }
            return null;
        }
        
        public static bool? GetValueFromRowAsNullBool(DataRow dr, int columnIndex)
        {
            return (bool?)GetValueFromRow(dr, columnIndex);
        }
        public static bool? GetValueFromRowAsNullBool(DataRow dr, DataColumn dc)
        {
            return (bool?)GetValueFromRow(dr, dc);
        }
        
        /// <summary>
        /// Will get the data value from the row as a nullable short. The return value will be set to either null or the short value
        /// This procedure assumes that the data is a short, if not a cast exception will be thrown.  
        /// </summary>
        /// <param name="dr">A Valid <see cref="System.Data.DataRow"/> DataRow</param>
        /// <param name="sColumnName">The Name of the Column in the DataRow</param>
        /// <returns></returns>
        public static short? GetValueFromRowAsNullShort(DataRow dr, string sColumnName)
        {
            var dbValue = GetValueFromRow(dr, sColumnName);
            return dbValue == null ? (short?)null : Convert.ToInt16(dbValue);
        }
        
        public static short? GetValueFromRowAsNullShort(DataRow dr, int columnIndex)
        {
            var dbValue = GetValueFromRow(dr, columnIndex);
            return dbValue == null ? (short?)null : Convert.ToInt16(dbValue);
        }
        
        public static short? GetValueFromRowAsNullShort(DataRow dr, DataColumn dc)
        {
            var dbValue = GetValueFromRow(dr, dc);
            return dbValue == null ? (short?)null : Convert.ToInt16(dbValue);
        }
        
        public static ushort GetValueFromRowAsUShort(DataRow dr, string sColumnName)
        {
            ushort? val = GetValueFromRowAsNullUShort(dr, sColumnName);
            return ThrowExceptionIfIsNull(val, sColumnName, "ushort");
        }
        
        public static ushort GetValueFromRowAsUShort(DataRow dr, int columnIndex)
        {
            ushort? val = GetValueFromRowAsNullUShort(dr, columnIndex);
            return ThrowExceptionIfIsNull(val, columnIndex, "ushort");
        }

        public static ushort GetValueFromRowAsUShort(DataRow dr, DataColumn dc)
        {
            ushort? val = GetValueFromRowAsNullUShort(dr, dc);
            return ThrowExceptionIfIsNull(val, dc.ColumnName, "ushort");
        }
        
        public static ushort? GetValueFromRowAsNullUShort(DataRow dr, string sColumnName)
        {
            var dbValue = GetValueFromRow(dr, sColumnName);
            return dbValue == null ? (ushort?)null : Convert.ToUInt16(dbValue);
        }
        
        public static ushort? GetValueFromRowAsNullUShort(DataRow dr, int columnIndex)
        {
            var dbValue = GetValueFromRow(dr, columnIndex);
            return dbValue == null ? (ushort?)null : Convert.ToUInt16(dbValue);
        }

        public static ushort? GetValueFromRowAsNullUShort(DataRow dr, DataColumn dc)
        {
            var dbValue = GetValueFromRow(dr, dc);
            return dbValue == null ? (ushort?)null : Convert.ToUInt16(dbValue);
        }
        
        /// <summary>
        /// Will get the data value from the row as an short. If the value is null an <see cref="ArgumentNullException"/> will be thrown
        /// This procedure assumes that the data is an short, if not a cast exception will be thrown.  
        /// </summary>
        /// <param name="dr">A Valid <see cref="System.Data.DataRow"/> DataRow</param>
        /// <param name="sColumnName">The Name of the Column in the DataRow</param>
        /// <returns></returns>

        public static short GetValueFromRowAsShort(DataRow dr, string sColumnName)
        {
            short? val = GetValueFromRowAsNullShort(dr, sColumnName);
            return ThrowExceptionIfIsNull(val, sColumnName, "short");
        }

        public static short GetValueFromRowAsShort(DataRow dr, int columnIndex)
        {
            short? val = GetValueFromRowAsNullShort(dr, columnIndex);
            return ThrowExceptionIfIsNull(val, columnIndex, "short");
        }
        
        public static short GetValueFromRowAsShort(DataRow dr, DataColumn dc)
        {
            short? val = GetValueFromRowAsNullShort(dr, dc);
            return ThrowExceptionIfIsNull(val, dc.ColumnName, "short");
        }
        
        /// <summary>
        /// Will get the data value from the row as a nullable sbyte. The return value will be set to either null or the sbyte value
        /// This procedure assumes that the data is a sbyte, if not a cast exception will be thrown.  
        /// </summary>
        /// <param name="dr">A Valid <see cref="System.Data.DataRow"/> DataRow</param>
        /// <param name="sColumnName">The Name of the Column in the DataRow</param>
        /// <returns></returns>
        
        public static sbyte? GetValueFromRowAsNullSbyte(DataRow dr, string sColumnName)
        {
            var dbValue = GetValueFromRow(dr, sColumnName);
            return dbValue == null ? (sbyte?)null : Convert.ToSByte(dbValue);
        }
        
        public static sbyte? GetValueFromRowAsNullSbyte(DataRow dr, int columnIndex)
        {
            var dbValue = GetValueFromRow(dr, columnIndex);
            return dbValue == null ? (sbyte?)null : Convert.ToSByte(dbValue);
        }
        
        public static sbyte? GetValueFromRowAsNullSbyte(DataRow dr, DataColumn dc)
        {
            var dbValue = GetValueFromRow(dr, dc);
            return dbValue == null ? (sbyte?)null : Convert.ToSByte(dbValue);
        }

        /// <summary>
        /// Will get the data value from the row as an sbyte. If the value is null an <see cref="ArgumentNullException"/> will be thrown
        /// This procedure assumes that the data is an sbyte, if not a cast exception will be thrown.  
        /// </summary>
        /// <param name="dr">A Valid <see cref="System.Data.DataRow"/> DataRow</param>
        /// <param name="sColumnName">The Name of the Column in the DataRow</param>
        /// <returns></returns>
        
        public static sbyte GetValueFromRowAsSbyte(DataRow dr, string sColumnName)
        {
            sbyte? val = GetValueFromRowAsNullSbyte(dr, sColumnName);
            return ThrowExceptionIfIsNull(val, sColumnName, "sbyte");
        }
        
        public static sbyte GetValueFromRowAsSbyte(DataRow dr, int columnIndex)
        {
            sbyte? val = GetValueFromRowAsNullSbyte(dr, columnIndex);
            return ThrowExceptionIfIsNull(val, columnIndex, "sbyte");
        }

        public static sbyte GetValueFromRowAsSbyte(DataRow dr, DataColumn dc)
        {
            sbyte? val = GetValueFromRowAsNullSbyte(dr, dc);
            return ThrowExceptionIfIsNull(val, dc.ColumnName, "sbyte");
        }

        /// <summary>
        /// Will get the data value from the row as a nullable short. The return value will be set to either null or the short value
        /// This procedure assumes that the data is a short, if not a cast exception will be thrown.  
        /// </summary>
        /// <param name="dr">A Valid <see cref="System.Data.DataRow"/> DataRow</param>
        /// <param name="sColumnName">The Name of the Column in the DataRow</param>
        /// <returns></returns>
     
        public static byte? GetValueFromRowAsNullByte(DataRow dr, string sColumnName)
        {
            var dbValue = GetValueFromRow(dr, sColumnName);
            return dbValue == null ? (byte?) null : Convert.ToByte(dbValue);
        }
        
        public static byte? GetValueFromRowAsNullByte(DataRow dr, int columnIndex)
        {
            var dbValue = GetValueFromRow(dr, columnIndex);
            return dbValue == null ? (byte?)null : Convert.ToByte(dbValue);
        }

        public static byte? GetValueFromRowAsNullByte(DataRow dr, DataColumn dc)
        {
            return (byte?)(GetValueFromRow(dr, dc));
        }

        /// <summary>
        /// Will get the data value from the row as an byte. If the value is null an <see cref="ArgumentNullException"/> will be thrown
        /// This procedure assumes that the data is an byte, if not a cast exception will be thrown.  
        /// </summary>
        /// <param name="dr">A Valid <see cref="System.Data.DataRow"/> DataRow</param>
        /// <param name="sColumnName">The Name of the Column in the DataRow</param>
        /// <returns></returns>
        public static byte GetValueFromRowAsByte(DataRow dr, string sColumnName)
        {
            byte? val = GetValueFromRowAsNullByte(dr, sColumnName);
            return ThrowExceptionIfIsNull(val, sColumnName, "byte");
        }
        
        public static byte GetValueFromRowAsByte(DataRow dr, int columnIndex)
        {
            byte? val = GetValueFromRowAsNullByte(dr, columnIndex);
            return ThrowExceptionIfIsNull(val, columnIndex, "byte");
        }
        
        public static byte GetValueFromRowAsByte(DataRow dr, DataColumn dc)
        {
            byte? val = GetValueFromRowAsNullByte(dr, dc);
            return ThrowExceptionIfIsNull(val, dc.ColumnName, "byte");
        }

        /// <summary>
        /// Will get the data value from the row as a nullable short. The return value will be set to either null or the short value
        /// This procedure assumes that the data is a short, if not a cast exception will be thrown.  
        /// </summary>
        /// <param name="dr">A Valid <see cref="System.Data.DataRow"/> DataRow</param>
        /// <param name="sColumnName">The Name of the Column in the DataRow</param>
        /// <returns></returns>
        
        public static DateTime? GetValueFromRowAsNullDateTime(DataRow dr, string sColumnName)
        {
            var dbValue = GetValueFromRow(dr, sColumnName);
            return dbValue == null ? (DateTime?)null : Convert.ToDateTime(dbValue);
        }
        
        public static DateTime? GetValueFromRowAsNullDateTime(DataRow dr, int columnIndex)
        {
            var dbValue = GetValueFromRow(dr, columnIndex);
            return dbValue == null ? (DateTime?)null : Convert.ToDateTime(dbValue);
        }

        public static DateTime? GetValueFromRowAsNullDateTime(DataRow dr, DataColumn dc)
        {
            var dbValue = GetValueFromRow(dr, dc);
            return dbValue == null ? (DateTime?)null : Convert.ToDateTime(dbValue);
        }

        /// <summary>
        /// Will get the data value from the row as an DateTime. If the value is null an <see cref="ArgumentNullException"/> will be thrown
        /// This procedure assumes that the data is an DateTime, if not a cast exception will be thrown.  
        /// </summary>
        /// <param name="dr">A Valid <see cref="System.Data.DataRow"/> DataRow</param>
        /// <param name="sColumnName">The Name of the Column in the DataRow</param>
        /// <returns></returns>
        
        public static DateTime GetValueFromRowAsDateTime(DataRow dr, string sColumnName)
        {
            DateTime? val = GetValueFromRowAsNullDateTime(dr, sColumnName);
            return ThrowExceptionIfIsNull(val, sColumnName, "DateTime");
        }

        public static DateTime GetValueFromRowAsDateTime(DataRow dr, int columnIndex)
        {
            DateTime? val = GetValueFromRowAsNullDateTime(dr, columnIndex);
            return ThrowExceptionIfIsNull(val, columnIndex, "DateTime");
        }

        public static DateTime GetValueFromRowAsDateTime(DataRow dr, DataColumn dc)
        {
            DateTime? val = GetValueFromRowAsNullDateTime(dr, dc);
            return ThrowExceptionIfIsNull(val, dc.ColumnName, "DateTime");
        }
        
        public static TimeSpan? GetValueFromRowAsNullTimeSpan(DataRow dr, string sColumnName)
        {
            return (TimeSpan?)(GetValueFromRow(dr, sColumnName));
        }

        public static TimeSpan? GetValueFromRowAsNullTimeSpan(DataRow dr, int columnIndex)
        {
            return (TimeSpan?)(GetValueFromRow(dr, columnIndex));
        }

        public static TimeSpan? GetValueFromRowAsNullTimeSpan(DataRow dr, DataColumn dc)
        {
            return (TimeSpan?)(GetValueFromRow(dr, dc));
        }

        public static TimeSpan GetValueFromRowAsTimeSpan(DataRow dr, string sColumnName)
        {
            TimeSpan? val = GetValueFromRowAsNullTimeSpan(dr, sColumnName);
            return ThrowExceptionIfIsNull(val, sColumnName, "TimeSpan");
        }

        public static TimeSpan GetValueFromRowAsTimeSpan(DataRow dr, int columnIndex)
        {
            TimeSpan? val = GetValueFromRowAsNullTimeSpan(dr, columnIndex);
            return ThrowExceptionIfIsNull(val, columnIndex, "TimeSpan");
        }

        public static TimeSpan GetValueFromRowAsTimeSpan(DataRow dr, DataColumn dc)
        {
            TimeSpan? val = GetValueFromRowAsNullTimeSpan(dr, dc);
            return ThrowExceptionIfIsNull(val, dc.ColumnName, "TimeSpan");
        }
        /// <summary>
        /// Will get the data value from the row as a Guid. If the value is null an <see cref="ArgumentNullException"/> will be thrown
        /// This procedure assumes that the data is a Guid, if not a cast exception will be thrown.  
        /// </summary>
        /// <param name="dr">A Valid <see cref="System.Data.DataRow"/> DataRow</param>
        /// <param name="sColumnName">The Name of the Column in the DataRow</param>
        /// <returns></returns>
        
        public static Guid GetValueFromRowAsGuid(DataRow dr, string sColumnName)
        {
            Guid? val = GetValueFromRowAsNullGuid(dr, sColumnName);
            return ThrowExceptionIfIsNull(val, sColumnName, "Guid");
        }
        
        public static Guid GetValueFromRowAsGuid(DataRow dr, int columnIndex)
        {
            Guid? val = GetValueFromRowAsNullGuid(dr, columnIndex);
            return ThrowExceptionIfIsNull(val, columnIndex, "Guid");
        }

        public static Guid GetValueFromRowAsGuid(DataRow dr, DataColumn dc)
        {
            Guid? val = GetValueFromRowAsNullGuid(dr, dc);
            return ThrowExceptionIfIsNull(val, dc.ColumnName, "Guid");
        }

        /// <summary>
        /// Will get the data value from the row as a nullable Guid. The return value will be set to either null or the Guid value
        /// This procedure assumes that the data is a Guid, if not a cast exception will be thrown.  
        /// </summary>
        /// <param name="dr">A Valid <see cref="System.Data.DataRow"/> DataRow</param>
        /// <param name="sColumnName">The Name of the Column in the DataRow</param>
        /// <returns></returns>
        
        public static Guid? GetValueFromRowAsNullGuid(DataRow dr, string sColumnName)
        {
            return ((Guid?)GetValueFromRow(dr, sColumnName));
        }
        
        public static Guid? GetValueFromRowAsNullGuid(DataRow dr, int columnIndex)
        {
            return ((Guid?)GetValueFromRow(dr, columnIndex));
        }

        public static Guid? GetValueFromRowAsNullGuid(DataRow dr, DataColumn dc)
        {
            return ((Guid?)GetValueFromRow(dr, dc));
        }

        public static char GetValueFromRowAsChar(DataRow dr, string sColumnName)
        {
            object dbValue = GetValueFromRow(dr, sColumnName);
            return dbValue == null ? '\0' : Convert.ToChar(dbValue);
        }

        public static char GetValueFromRowAsChar(DataRow dr, int columnIndex)
        {
            object dbValue = GetValueFromRow(dr, columnIndex);
            return dbValue == null ? '\0' : Convert.ToChar(dbValue);
        }

        public static char GetValueFromRowAsChar(DataRow dr, DataColumn dc)
        {
            object dbValue = GetValueFromRow(dr, dc);
            return dbValue == null ? '\0' : Convert.ToChar(dbValue);
        }

        public static char? GetValueFromRowAsNullChar(DataRow dr, string sColumnName)
        {
            var dbValue = GetValueFromRow(dr, sColumnName);
            return dbValue == null ? (char?)null : Convert.ToChar(dbValue);
        }

        public static char? GetValueFromRowAsNullChar(DataRow dr, int columnIndex)
        {
            var dbValue = GetValueFromRow(dr, columnIndex);
            return dbValue == null ? (char?)null : Convert.ToChar(dbValue);
        }

        public static char? GetValueFromRowAsNullChar(DataRow dr, DataColumn dc)
        {
            var dbValue = GetValueFromRow(dr, dc);
            return dbValue == null ? (char?)null : Convert.ToChar(dbValue);
        }
        
        public static ulong GetValueFromRowAsRowVersion(DataRow dr, string sColumnName)
        {
            byte[] result = (byte[])DataHelper.GetValueFromRow(dr, sColumnName);
            return BitConverter.ToUInt64(result, 0);
        }

        public static ulong GetValueFromRowAsRowVersion(DataRow dr, int columnIndex)
        {
            byte[] result = (byte[])DataHelper.GetValueFromRow(dr, columnIndex);
            return BitConverter.ToUInt64(result, 0);
        }
        
        public static ulong GetValueFromRowAsRowVersion(DataRow dr, DataColumn dc)
        {
            byte[] result = (byte[])DataHelper.GetValueFromRow(dr, dc);
            return BitConverter.ToUInt64(result, 0);
        }
        
        public static byte[] GetValueFromRowAsBinary(DataRow dr, string sColumnName)
        {
            return (byte[])GetValueFromRow(dr, sColumnName);
        }
        
        public static byte[] GetValueFromRowAsBinary(DataRow dr, int columnIndex)
        {
            return (byte[])GetValueFromRow(dr, columnIndex);
        }
        
        public static byte[] GetValueFromRowAsBinary(DataRow dr, DataColumn dc)
        {
            return (byte[])GetValueFromRow(dr, dc);
        }
    }
}