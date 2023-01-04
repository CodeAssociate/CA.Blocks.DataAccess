//===============================================================================
// Copyright (C) 2002-2022 Ravin Enterprises Ltd. 
// All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.DataTableHelpers;

namespace CA.Blocks.SQLServerDataAccess
{
    /// <summary>
    /// When going from the .NET world info the database we need to tell the database the the precision on which to store the DateTime.  SQL server exposes four levels
    /// </summary>
    public enum SpecificSQLDateTimeType
    {
        /// <summary>
        /// Will only store the date value of the DateTime Input, is is accurate to the day and uses 3 bytes of storage.
        /// </summary>
        Date,
        /// <summary>
        /// Will only store the date and time value on the input, this is the default, Expect this default to change to DateTime2 as DateTime is the legacy
        /// </summary>
        [System.Obsolete("Consider DateTime2 much higher precision, you can use DateTime2 in you code and the server will be backwards compatible, you will loose precision using DateTime vrs DateTime2 ")]
        DateTime, 
        /// <summary>
        /// DateTime2 provides a much larger Date Range, in addition higher accuracy sorting down to the 100 nanoseconds level. Microsoft recommends datetime2 over datetime for new work.. but it usage is still catching up hence the default is still DateTime.
        /// </summary>
        DateTime2,// The default
        /// <summary>
        /// Will only store the date and time value on the input, this will be accurate up to the one minute. SmallDateTime is 4 bytes so has big storage advantages over big tables, however it is not SQL ANSI Compliant..
        /// </summary>
        SmallDateTime
    }

    /// <summary>
    /// The SpecificSQLDecimalType to use this will default to Decimal if not specified
    /// </summary>
    public enum SpecificSQLDecimalType
    {
        Decimal, // The default
        Money, 
        SmallMoney,
    }

    public enum SpecificSQLStringType
    {
        [System.Obsolete("Backwards compatibility only remove when they remove from SQL server http://msdn.microsoft.com/en-nz/library/ms187993.aspx")]
        NText,
        NVarChar, // The Default
        [System.Obsolete("Backwards compatibility only remove when they remove from SQL server http://msdn.microsoft.com/en-nz/library/ms187993.aspx")]
        Text,
        VarChar, 
    }


    public enum SpecificSQLCharType
    {
        Char, // default 
        NChar 
    }

    public static class SqlServerParameterExtensions
    {
        public static SqlCommand WithParameters(this SqlCommand cmd, IEnumerable<SqlParameter> parameters)
        {
            return cmd.WithParameters<SqlCommand, SqlParameter>(parameters);
        }

        public static SqlCommand WithParameter(this SqlCommand cmd, SqlParameter parameter)
        {
            return cmd.WithParameter<SqlCommand, SqlParameter>(parameter);
        }

        public static SqlCommand WithReturnResult(this SqlCommand cmd)
        {
            var sqlParam = cmd.CreateParameter();
            sqlParam.ParameterName = "Return";
            sqlParam.SqlDbType = SqlDbType.Int;
            sqlParam.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(sqlParam);
            return cmd;
        }
        
        public static int? GetReturnResult(this SqlCommand cmd)
        {
            int? result = null;
            var sqlParam = cmd.Parameters["Return"];
            if (sqlParam != null && sqlParam.SqlDbType == SqlDbType.Int && sqlParam.Direction == ParameterDirection.ReturnValue)
            {
                if (sqlParam.Value != null && sqlParam.Value != DBNull.Value)
                    result = (int)sqlParam.Value;
            }
            return result;
        }


        public static T ToValue<T>(this SqlParameter sqlParameter)
        {
            return sqlParameter.ToValue<SqlParameter, T>();
        }

        public static T ToValueWithConvert<T>(this SqlParameter sqlParameter)
        {
            return sqlParameter.ToValueWithConvert<SqlParameter, T>();
        }

        #region SqlDbType.BigInt ( long, Int64 ) 
        private static SqlParameter ToSqlParameterBigInt(long? input, string strParameterName)
        {
            return new SqlParameter(strParameterName, SqlDbType.BigInt)
            {
                Value = ParameterHelper.ToDbParameterValue(input)
            };
        }

        public static SqlParameter ToSqlParameter(this long input, string strParameterName)
        {
            return ToSqlParameterBigInt(input, strParameterName);
        }

        public static SqlParameter ToSqlParameter(this long? input, string strParameterName)
        {
            return ToSqlParameterBigInt(input, strParameterName);
        }
        #endregion


        #region SqlDbType.Binary / SqlDbType.VarBinary  ( We dont know the size so VarBinary and Binary are the same

        public static SqlParameter ToSqlParameter(this byte[] input, string strParameterName)
        {
            return new SqlParameter(strParameterName, SqlDbType.VarBinary)
            {
                Value = ParameterHelper.ToDbParameterValue(input)
            };
        }
        #endregion


        #region SqlDbType.Bit ( boolean ) 
        private static SqlParameter ToSqlParameterBool(bool? input, string strParameterName)
        {
            return new SqlParameter(strParameterName, SqlDbType.Bit)
            {
                Value = ParameterHelper.ToDbParameterValue(input)
            };
        }

        public static SqlParameter ToSqlParameter(this bool input, string strParameterName)
        {
            return ToSqlParameterBool(input, strParameterName);
        }

        public static SqlParameter ToSqlParameter(this bool? input, string strParameterName)
        {
            return ToSqlParameterBool(input, strParameterName);
        }
        #endregion


        #region SqlDbType.Char   (Char)
        private static SqlDbType ToSqlDbType(SpecificSQLCharType dbType)
        {
            return dbType == SpecificSQLCharType.Char ? SqlDbType.Char : SqlDbType.NChar;
        }


        private static SqlParameter ToSqlParameterChar(Char? input, string strParameterName, SpecificSQLCharType dbType)
        {
            var sqlparam = new SqlParameter(strParameterName, ToSqlDbType(dbType));
            sqlparam.Value = ParameterHelper.ToDbParameterValue(input);
            return (sqlparam);
        }

        public static SqlParameter ToSqlParameter(this Char input, string strParameterName, SpecificSQLCharType dbType = SpecificSQLCharType.Char)
        {
            return ToSqlParameterChar(input, strParameterName, dbType);
        }

        public static SqlParameter ToSqlParameter(this Char? input, string strParameterName, SpecificSQLCharType dbType = SpecificSQLCharType.Char)
        {
            return ToSqlParameterChar(input, strParameterName, dbType);
        }
        #endregion


        #region SqlDbType.DateTime ( System.DateTime )
        private static SqlDbType ToSqlDbType(SpecificSQLDateTimeType dbType)
        {
#pragma warning disable CS0618 // Type or member is obsolete we know that SpecificSQLDateTimeType.DateTime is obsolete but we need to support it. 
            switch (dbType)
            {
                case SpecificSQLDateTimeType.DateTime:
                    {
                        return SqlDbType.DateTime;
                    }
                case SpecificSQLDateTimeType.Date:
                    {
                        return SqlDbType.Date;
                    }
                case SpecificSQLDateTimeType.DateTime2:
                    {
                        return SqlDbType.DateTime2;
                    }
                case SpecificSQLDateTimeType.SmallDateTime:
                    {
                        return SqlDbType.SmallDateTime;
                    }
                default:
                    return SqlDbType.DateTime2;
            }
#pragma warning restore CS0618 // Type or member is obsolete
        }

        private static SqlParameter ToSqlParameterDateTime(DateTime? input, string strParameterName, SpecificSQLDateTimeType dbType)
        {
            var sqlparam = new SqlParameter(strParameterName, ToSqlDbType(dbType));
            sqlparam.Value = ParameterHelper.ToDbParameterValue(input);
            return (sqlparam);
        }

        public static SqlParameter ToSqlParameter(this DateTime input, string strParameterName, SpecificSQLDateTimeType dbType = SpecificSQLDateTimeType.DateTime2)
        {
            return ToSqlParameterDateTime(input, strParameterName, dbType);
        }

        // Default to DateTime

        public static SqlParameter ToSqlParameter(this DateTime? input, string strParameterName, SpecificSQLDateTimeType dbType = SpecificSQLDateTimeType.DateTime2)
        {
            return ToSqlParameterDateTime(input, strParameterName, dbType);
        }
        #endregion

        #region  DateTimeOffset
        private static SqlParameter ToSqlParameterDateTimeOffset(DateTimeOffset? input, string strParameterName)
        {
            var sqlparam = new SqlParameter(strParameterName, SqlDbType.DateTimeOffset);
            sqlparam.Value = ParameterHelper.ToDbParameterValue(input);
            return (sqlparam);
        }

        public static SqlParameter ToSqlParameter(this DateTimeOffset input, string strParameterName)
        {
            return ToSqlParameterDateTimeOffset(input, strParameterName);
        }

        public static SqlParameter ToSqlParameter(this DateTimeOffset? input, string strParameterName)
        {
            return ToSqlParameterDateTimeOffset(input, strParameterName);
        }
        #endregion
        
        #region SqlDbType.Decimal  (Decimal, Money, SmallMoney)

        private static SqlDbType ToSqlDbType(SpecificSQLDecimalType dbType)
        {
            switch (dbType)
            {
                case SpecificSQLDecimalType.Decimal:
                    {
                        return SqlDbType.Decimal;
                    }
                case SpecificSQLDecimalType.Money:
                    {
                        return SqlDbType.Money;
                    }
                case SpecificSQLDecimalType.SmallMoney:
                    {
                        return SqlDbType.SmallMoney;
                    }
                default:
                    return SqlDbType.Decimal;
            }
        }

        private static SqlParameter ToSqlParameterDecimal(Decimal? input, string strParameterName, SpecificSQLDecimalType dbType)
        {
            var sqlparam = new SqlParameter(strParameterName, ToSqlDbType(dbType));
            sqlparam.Value = ParameterHelper.ToDbParameterValue(input);
            return (sqlparam);
        }


        public static SqlParameter ToSqlParameter(this Decimal input, string strParameterName, SpecificSQLDecimalType dbType = SpecificSQLDecimalType.Decimal)
        {
            return ToSqlParameterDecimal(input, strParameterName, dbType);
        }

        // Default is SpecificSQLDecimalType.Decimal
        public static SqlParameter ToSqlParameter(this Decimal? input, string strParameterName, SpecificSQLDecimalType dbType = SpecificSQLDecimalType.Decimal)
        {
            return ToSqlParameterDecimal(input, strParameterName, dbType);
        }

        #endregion

        #region SqlDbType.Float  (Single)

        private static SqlParameter ToSqlParameterDouble(float? input, string strParameterName)
        {
            var sqlparam = new SqlParameter(strParameterName, SqlDbType.Real);
            sqlparam.Value = ParameterHelper.ToDbParameterValue(input);
            return (sqlparam);
        }

        public static SqlParameter ToSqlParameter(this float input, string strParameterName)
        {
            return ToSqlParameterDouble(input, strParameterName);
        }

        public static SqlParameter ToSqlParameter(this float? input, string strParameterName)
        {
            return ToSqlParameterDouble(input, strParameterName);
        }
        #endregion

        #region  (System.Double)

        private static SqlParameter ToSqlParameterDouble(Double? input, string strParameterName)
        {
            var sqlparam = new SqlParameter(strParameterName, SqlDbType.Float);
            sqlparam.Value = ParameterHelper.ToDbParameterValue(input);
            return (sqlparam);
        }

        public static SqlParameter ToSqlParameter(this Double input, string strParameterName)
        {
            return ToSqlParameterDouble(input, strParameterName);
        }

        public static SqlParameter ToSqlParameter(this Double? input, string strParameterName)
        {
            return ToSqlParameterDouble(input, strParameterName);
        }
        #endregion

        /*
        SqlDbType.Image; // use VarBinary
        */
        #region SqlDbType.Int  ( int, Int32, uint)

        private static SqlParameter ToSqlParameterInt(int? input, string strParameterName)
        {
            return new SqlParameter(strParameterName, SqlDbType.Int, 4)
            {
                Value = ParameterHelper.ToDbParameterValue(input),
            };
        }

        public static SqlParameter ToSqlParameter(this int input, string strParameterName)
        {
            return ToSqlParameterInt(input, strParameterName);
        }

        public static SqlParameter ToSqlParameter(this int? input, string strParameterName)
        {
            return ToSqlParameterInt(input, strParameterName);
        }

        private static SqlParameter ToSqlParameterInt(uint? input, string strParameterName)
        {
            // There is no native support for UNSIGNED int in sql sever do you have to use a BigInt to cater for values between 2147483648 and 4294967296  
            return new SqlParameter(strParameterName, SqlDbType.BigInt, 8)
            {
                Value = ParameterHelper.ToDbParameterValue(input)
            };
        }

        public static SqlParameter ToSqlParameter(this uint input, string strParameterName)
        {
            return ToSqlParameterInt(input, strParameterName);
        }

        public static SqlParameter ToSqlParameter(this uint? input, string strParameterName)
        {
            return ToSqlParameterInt(input, strParameterName);
        }


        #endregion
        /*
        SqlDbType.Money;
        SqlDbType.NChar;
        SqlDbType.Real;
        SqlDbType.SmallDateTime;*/

        #region mapping type for sbyte  There is no Sbtye in sql so assue we use SmallInt
        private static SqlParameter ToSqlParameterSbtye(sbyte? input, string strParameterName)
        {
            // this is the smallet sql server type for the ranges -128-127 ie sbyte
            return new SqlParameter(strParameterName, SqlDbType.SmallInt)
            {
                Value = ParameterHelper.ToDbParameterValue(input)
            };
        }

        public static SqlParameter ToSqlParameter(this sbyte input, string strParameterName)
        {
            return ToSqlParameterSbtye(input, strParameterName);
        }

        public static SqlParameter ToSqlParameter(this sbyte? input, string strParameterName)
        {
            return ToSqlParameterSbtye(input, strParameterName);
        }
#endregion

#region SqlDbType.SmallInt  -> ( short, Int16)
        private static SqlParameter ToSqlParameterInt16(short? input, string strParameterName)
        {
            return new SqlParameter(strParameterName, SqlDbType.SmallInt)
            {
                Value = ParameterHelper.ToDbParameterValue(input)
            };
        }

        public static SqlParameter ToSqlParameter(this short input, string strParameterName)
        {
            return ToSqlParameterInt16(input, strParameterName);
        }

        public static SqlParameter ToSqlParameter(this short? input, string strParameterName)
        {
            return ToSqlParameterInt16(input, strParameterName);
        }

        private static SqlParameter ToSqlParameterUInt16(ushort? input, string strParameterName)
        {
            return new SqlParameter(strParameterName, SqlDbType.Int)
            {
                Value = ParameterHelper.ToDbParameterValue(input)
            };
        }

        public static SqlParameter ToSqlParameter(this ushort input, string strParameterName)
        {
            return ToSqlParameterUInt16(input, strParameterName);
        }

        public static SqlParameter ToSqlParameter(this ushort? input, string strParameterName)
        {
            return ToSqlParameterUInt16(input, strParameterName);
        }
        #endregion



        /*
        SqlDbType.SmallMoney;
        SqlDbType.Structured;
         */
        #region SqlDbType.Time ( System.TimeSpan )

        private static SqlParameter ToSqlParameterTimeSpan(TimeSpan? input, string strParameterName)
        {
            return new SqlParameter(strParameterName, SqlDbType.Time)
            {
                Value = ParameterHelper.ToDbParameterValue(input)
            };
        }

        public static SqlParameter ToSqlParameter(this TimeSpan input, string strParameterName)
        {
            return ToSqlParameterTimeSpan(input, strParameterName);
        }

        public static SqlParameter ToSqlParameter(this TimeSpan? input, string strParameterName)
        {
            return ToSqlParameterTimeSpan(input, strParameterName);
        }

#endregion

        /*
        SqlDbType.Timestamp; no timestamp*/

#region SqlDbType.TinyInt ( Byte ) 
        private static SqlParameter ToSqlParameterByte(byte? input, string strParameterName)
        {
            return new SqlParameter(strParameterName, SqlDbType.TinyInt)
            {
                Value = ParameterHelper.ToDbParameterValue(input)
            };
        }

        public static SqlParameter ToSqlParameter(this byte input, string strParameterName)
        {
            return ToSqlParameterByte(input, strParameterName);
        }

        public static SqlParameter ToSqlParameter(this byte? input, string strParameterName)
        {
            return ToSqlParameterByte(input, strParameterName);
        }
#endregion

        /*
        SqlDbType.Udt;
         */
#region SqlDbType.UniqueIdentifier ( System.Guid)
        private static SqlParameter ToSqlParameterGuid(Guid? input, string strParameterName)
        {
            var sqlparam = new SqlParameter(strParameterName, SqlDbType.UniqueIdentifier);
            sqlparam.Value = ParameterHelper.ToDbParameterValue(input);
            return (sqlparam);
        }

        public static SqlParameter ToSqlParameter(this Guid input, string strParameterName)
        {
            return ToSqlParameterGuid(input, strParameterName);
        }

        public static SqlParameter ToSqlParameter(this Guid? input, string strParameterName)
        {
            return ToSqlParameterGuid(input, strParameterName);
        }
#endregion

        /*
        SqlDbType.VarBinary; // use Binary
        */

#region SqlDbType.VarChar;
        private static SqlDbType ToSqlDbType(SpecificSQLStringType input)
        {
            switch (input)
            {
                case SpecificSQLStringType.VarChar:
                    {
                        return SqlDbType.VarChar;
                    }
                case SpecificSQLStringType.NVarChar:
                    {
                        return SqlDbType.NVarChar;
                    }

                default:
                    return SqlDbType.NVarChar;
            }
        }

        private static SqlParameter ToSqlParameterString(string input, string strParameterName, SpecificSQLStringType dbType, bool useEmptyStringForNull, int trimInputTo)
        {
            var inputString = ParameterHelper.PrepStringInput(input, useEmptyStringForNull, trimInputTo);
            return  new SqlParameter(strParameterName, ToSqlDbType(dbType))
            {
                Value = ParameterHelper.ToDbParameterValue(inputString)
            };
        }

        public static SqlParameter ToSqlParameter(this string input, string strParameterName, 
                SpecificSQLStringType dbType = SpecificSQLStringType.NVarChar, 
                bool useEmptyStringForNull = false, 
                int trimInputTo = -1)
        {
            return ToSqlParameterString(input, strParameterName, dbType, useEmptyStringForNull, trimInputTo);
        }
        #endregion

        /*
        SqlDbType.Variant; // ?lets find a usage ? 
        SqlDbType.Xml; // no direct xml support. 
        */


#if NET6_0_OR_GREATER
        private static SqlParameter ToSqlParameterDateOnly(DateOnly? input, string strParameterName)
        {
            var sqlparam = new SqlParameter(strParameterName, SqlDbType.Date);
            sqlparam.Value = input.HasValue ? (object)(input.Value.ToDateTime(new TimeOnly(0))) : (object)DBNull.Value;
            return (sqlparam);
        }

        public static SqlParameter ToSqlParameter(this DateOnly input, string strParameterName)
        {
            return ToSqlParameterDateOnly(input, strParameterName);
        }

        public static SqlParameter ToSqlParameter(this DateOnly? input, string strParameterName)
        {
            return ToSqlParameterDateOnly(input, strParameterName);
        }

    
        private static SqlParameter ToSqlParameterTimeOnly(TimeOnly? input, string strParameterName)
        {
            var sqlparam = new SqlParameter(strParameterName, SqlDbType.Time);
            sqlparam.Value = input.HasValue ? (object)(input.Value.ToTimeSpan()) : (object)DBNull.Value;
            return (sqlparam);
        }

        public static SqlParameter ToSqlParameter(this TimeOnly input, string strParameterName)
        {
            return ToSqlParameterTimeOnly(input, strParameterName);
        }

        public static SqlParameter ToSqlParameter(this TimeOnly? input, string strParameterName)
        {
            return ToSqlParameterTimeOnly(input, strParameterName);
        }
#endif


        public static SqlParameter ToValueDataTableSqlParameter<T>(this IEnumerable<T> input, string strParameterName, string tableTypeName)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }
            return new SqlParameter(strParameterName, SqlDbType.Structured)
            {
                Value = input.ToValueDataTable(),
                TypeName = tableTypeName
            };
        }

    }
}
