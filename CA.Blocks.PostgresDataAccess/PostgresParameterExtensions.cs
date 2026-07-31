//===============================================================================
// Copyright (C) 2002-2022 Ravin Enterprises Ltd. 
// All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Data;
using CA.Blocks.DataAccess;
using Npgsql;
using NpgsqlTypes;

namespace CA.Blocks.PostgresDataAccess
{
    ///// <summary>
    ///// The SpecificSQLDecimalType to use this will default to Decimal if not specified
    ///// </summary>
    public enum SpecificSQLDecimalType
    {
        Decimal, // The default
        Money
    }

    public enum SpecificSQLStringType
    {
       
        Char, // fixed length string
        VarChar, // The Default variable-length with limit
        Text, // variable unlimited length
    }


    //public enum SpecificSQLCharType
    //{
    //    Char, // default 
    //    NChar 
    //}

    public static class PostgresParameterExtensions
    {
        public static NpgsqlCommand WithParameters(this NpgsqlCommand cmd, IEnumerable<NpgsqlParameter> parameters)
        {
            return cmd.WithParameters<NpgsqlCommand, NpgsqlParameter>(parameters);
        }

        public static NpgsqlCommand WithParameter(this NpgsqlCommand cmd, NpgsqlParameter parameter)
        {
            return cmd.WithParameter<NpgsqlCommand, NpgsqlParameter>(parameter);
        }

        //    public static SqlCommand WithReturnResult(this SqlCommand cmd)
        //    {
        //        var sqlParam = cmd.CreateParameter();
        //        sqlParam.ParameterName = "Return";
        //        sqlParam.SqlDbType = SqlDbType.Int;
        //        sqlParam.Direction = ParameterDirection.ReturnValue;
        //        cmd.Parameters.Add(sqlParam);
        //        return cmd;
        //    }

        //    public static int? GetReturnResult(this SqlCommand cmd)
        //    {
        //        int? result = null;
        //        var sqlParam = cmd.Parameters["Return"];
        //        if (sqlParam != null && sqlParam.SqlDbType == SqlDbType.Int && sqlParam.Direction == ParameterDirection.ReturnValue)
        //        {
        //            if (sqlParam.Value != null && sqlParam.Value != DBNull.Value)
        //                result = (int)sqlParam.Value;
        //        }
        //        return result;
        //    }


        //    public static T ToValue<T>(this NpgsqlParameter sqlParameter)
        //    {
        //        return sqlParameter.ToValue<SqlParameter, T>();
        //    }

        //    public static T ToValueWithConvert<T>(this SqlParameter sqlParameter)
        //    {
        //        return sqlParameter.ToValueWithConvert<SqlParameter, T>();
        //    }

        #region SqlDbType.BigInt ( long, Int64 )


        private static NpgsqlParameter ToSqlParameterBigIntBigInt(long? input, string strParameterName)
        {
            return new NpgsqlParameter(strParameterName, SqlDbType.BigInt)
            {
                Value = ParameterHelper.ToDbParameterValue(input)
            };
        }

        public static NpgsqlParameter ToPostgresParameter(this long input, string strParameterName)
        {
            return ToSqlParameterBigIntBigInt(input, strParameterName);
        }

        public static NpgsqlParameter ToPostgresParameter(this long? input, string strParameterName)
        {
            return ToSqlParameterBigIntBigInt(input, strParameterName);
        }
        #endregion


        //        #region SqlDbType.Binary / SqlDbType.VarBinary  ( We dont know the size so VarBinary and Binary are the same

        //        public static SqlParameter ToSqlParameter(this byte[] input, string strParameterName)
        //        {
        //            return new SqlParameter(strParameterName, SqlDbType.VarBinary)
        //            {
        //                Value = ParameterHelper.ToDbParameterValue(input)
        //            };
        //        }
        //        #endregion


        #region SqlDbType.Bit ( boolean ) 
        private static NpgsqlParameter ToPostgresParameterBool(bool? input, string strParameterName)
        {
            return new NpgsqlParameter(strParameterName, NpgsqlDbType.Boolean)
            {
                Value = ParameterHelper.ToDbParameterValue(input)
            };
        }

        public static NpgsqlParameter ToPostgresParameter(this bool input, string strParameterName)
        {
            return ToPostgresParameterBool(input, strParameterName);
        }

        public static NpgsqlParameter ToPostgresParameter(this bool? input, string strParameterName)
        {
            return ToPostgresParameterBool(input, strParameterName);
        }
        #endregion



        #region SqlDbType.Char
        private static NpgsqlParameter ToPostgresParameterChar(Char? input, string strParameterName)
        {
            return new NpgsqlParameter(strParameterName, NpgsqlDbType.Char)
            {
                Value = ParameterHelper.ToDbParameterValue(input)
            };
        }

        public static NpgsqlParameter ToPostgresParameter(this Char input, string strParameterName)
        {
            return ToPostgresParameterChar(input, strParameterName);
        }

        public static NpgsqlParameter ToSqlParToPostgresParameterameter(this Char? input, string strParameterName)
        {
            return ToPostgresParameterChar(input, strParameterName);
        }
        #endregion


        #region SqlDbType.DateTime ( System.DateTime )

        private static NpgsqlParameter ToPostgresParameterDateTime(DateTime? input, string strParameterName)
        {
            return new NpgsqlParameter(strParameterName, NpgsqlDbType.Timestamp)
            {
                Value = ParameterHelper.ToDbParameterValue(input)
            };
        }

        public static NpgsqlParameter ToPostgresParameter(this DateTime input, string strParameterName)
        {
            return ToPostgresParameterDateTime(input, strParameterName);
        }

        // Default to DateTime

        public static NpgsqlParameter ToPostgresParameter(this DateTime? input, string strParameterName)
        {
            return ToPostgresParameterDateTime(input, strParameterName);
        }
        #endregion

        #region  DateTimeOffset
        private static NpgsqlParameter ToPostgresParameterDateTimeOffset(DateTimeOffset? input, string strParameterName)
        {
            return new NpgsqlParameter(strParameterName, NpgsqlDbType.Timestamp)
            {
                Value = ParameterHelper.ToDbParameterValue(input)
            };
        }

        public static NpgsqlParameter ToPostgresParameter(this DateTimeOffset input, string strParameterName)
        {
            return ToPostgresParameterDateTimeOffset(input, strParameterName);
        }

        public static NpgsqlParameter ToPostgresParameter(this DateTimeOffset? input, string strParameterName)
        {
            return ToPostgresParameterDateTimeOffset(input, strParameterName);
        }
        #endregion

        #region SqlDbType.Decimal  (Decimal, Money, SmallMoney)

        private static NpgsqlDbType ToSqlDbType(SpecificSQLDecimalType dbType)
        {
            switch (dbType)
            {
                case SpecificSQLDecimalType.Decimal:
                    {
                        return NpgsqlDbType.Numeric;
                    }
                case SpecificSQLDecimalType.Money:
                    {
                        return NpgsqlDbType.Money;
                    }
                default:
                    return NpgsqlDbType.Numeric;
            }
        }

        private static NpgsqlParameter ToPostgresParameterDecimal(Decimal? input, string strParameterName, SpecificSQLDecimalType dbType)
        {
            return new NpgsqlParameter(strParameterName, ToSqlDbType(dbType))
            {
                Value = ParameterHelper.ToDbParameterValue(input)
            };
        }


        public static NpgsqlParameter ToPostgresParameter(this Decimal input, string strParameterName, SpecificSQLDecimalType dbType = SpecificSQLDecimalType.Decimal)
        {
            return ToPostgresParameterDecimal(input, strParameterName, dbType);
        }

        // Default is SpecificSQLDecimalType.Decimal
        public static NpgsqlParameter ToPostgresParameter(this Decimal? input, string strParameterName, SpecificSQLDecimalType dbType = SpecificSQLDecimalType.Decimal)
        {
            return ToPostgresParameterDecimal(input, strParameterName, dbType);
        }

        #endregion

        #region SqlDbType.Float  (Single)

        private static NpgsqlParameter ToSqlParameterSingle(float? input, string strParameterName)
        {
            return new NpgsqlParameter(strParameterName, NpgsqlDbType.Real)
            {
                Value = ParameterHelper.ToDbParameterValue(input)
            };
        }

        public static NpgsqlParameter ToPostgresParameter(this float input, string strParameterName)
        {
            return ToSqlParameterSingle(input, strParameterName);
        }

        public static NpgsqlParameter ToPostgresParameter(this float? input, string strParameterName)
        {
            return ToSqlParameterSingle(input, strParameterName);
        }
        #endregion

        #region  (System.Double)

        private static NpgsqlParameter ToPostgresParameterDouble(Double? input, string strParameterName)
        {
            return new NpgsqlParameter(strParameterName, NpgsqlDbType.Double)
            {
                Value = ParameterHelper.ToDbParameterValue(input)
            };
        }

        public static NpgsqlParameter ToPostgresParameter(this Double input, string strParameterName)
        {
            return ToPostgresParameterDouble(input, strParameterName);
        }

        public static NpgsqlParameter ToPostgresParameter(this Double? input, string strParameterName)
        {
            return ToPostgresParameterDouble(input, strParameterName);
        }
        #endregion

        //        /*
        //        SqlDbType.Image; // use VarBinary
        //        */
        //        #region SqlDbType.Int  ( int, Int32, uint)

        private static NpgsqlParameter ToPostgresParameterInt(int? input, string strParameterName)
        {
            return new NpgsqlParameter(strParameterName, NpgsqlDbType.Integer, 4)
            {
                Value = ParameterHelper.ToDbParameterValue(input),
            };
        }

        public static NpgsqlParameter ToPostgresParameter(this int input, string strParameterName)
        {
            return ToPostgresParameterInt(input, strParameterName);
        }

        public static NpgsqlParameter ToPostgresParameter(this int? input, string strParameterName)
        {
            return ToPostgresParameterInt(input, strParameterName);
        }

        //        private static SqlParameter ToSqlParameterInt(uint? input, string strParameterName)
        //        {
        //            // There is no native support for UNSIGNED int in sql sever do you have to use a BigInt to cater for values between 2147483648 and 4294967296  
        //            return new SqlParameter(strParameterName, SqlDbType.BigInt, 8)
        //            {
        //                Value = ParameterHelper.ToDbParameterValue(input)
        //            };
        //        }

        //        public static SqlParameter ToSqlParameter(this uint input, string strParameterName)
        //        {
        //            return ToSqlParameterInt(input, strParameterName);
        //        }

        //        public static SqlParameter ToSqlParameter(this uint? input, string strParameterName)
        //        {
        //            return ToSqlParameterInt(input, strParameterName);
        //        }


        //        #endregion
        //        /*
        //        SqlDbType.Money;
        //        SqlDbType.NChar;
        //        SqlDbType.Real;
        //        SqlDbType.SmallDateTime;*/

        //        #region mapping type for sbyte  There is no Sbtye in sql so assue we use SmallInt
        //        private static SqlParameter ToSqlParameterSbtye(sbyte? input, string strParameterName)
        //        {
        //            // this is the smallet sql server type for the ranges -128-127 ie sbyte
        //            return new SqlParameter(strParameterName, SqlDbType.SmallInt)
        //            {
        //                Value = ParameterHelper.ToDbParameterValue(input)
        //            };
        //        }

        //        public static SqlParameter ToSqlParameter(this sbyte input, string strParameterName)
        //        {
        //            return ToSqlParameterSbtye(input, strParameterName);
        //        }

        //        public static SqlParameter ToSqlParameter(this sbyte? input, string strParameterName)
        //        {
        //            return ToSqlParameterSbtye(input, strParameterName);
        //        }
        //#endregion

        //#region SqlDbType.SmallInt  -> ( short, Int16)
        private static NpgsqlParameter ToPostgresParameterInt16(short? input, string strParameterName)
        {
            return new NpgsqlParameter(strParameterName, NpgsqlDbType.Smallint)
            {
                Value = ParameterHelper.ToDbParameterValue(input)
            };
        }

        public static NpgsqlParameter ToPostgresParameter(this short input, string strParameterName)
        {
            return ToPostgresParameterInt16(input, strParameterName);
        }

        public static NpgsqlParameter ToPostgresParameter(this short? input, string strParameterName)
        {
            return ToPostgresParameterInt16(input, strParameterName);
        }

        //        private static SqlParameter ToSqlParameterUInt16(ushort? input, string strParameterName)
        //        {
        //            return new SqlParameter(strParameterName, SqlDbType.Int)
        //            {
        //                Value = ParameterHelper.ToDbParameterValue(input)
        //            };
        //        }

        //        public static SqlParameter ToSqlParameter(this ushort input, string strParameterName)
        //        {
        //            return ToSqlParameterUInt16(input, strParameterName);
        //        }

        //        public static SqlParameter ToSqlParameter(this ushort? input, string strParameterName)
        //        {
        //            return ToSqlParameterUInt16(input, strParameterName);
        //        }
        //        #endregion



        //        /*
        //        SqlDbType.SmallMoney;
        //        SqlDbType.Structured;
        //         */
        //        #region SqlDbType.Time ( System.TimeSpan )

        private static NpgsqlParameter ToSqlParameterTimeSpan(TimeSpan? input, string strParameterName)
        {
            return new NpgsqlParameter(strParameterName, NpgsqlDbType.Time)
            {
                Value = ParameterHelper.ToDbParameterValue(input)
            };
        }

        public static NpgsqlParameter ToSqlParameter(this TimeSpan input, string strParameterName)
        {
            return ToSqlParameterTimeSpan(input, strParameterName);
        }

        public static NpgsqlParameter ToSqlParameter(this TimeSpan? input, string strParameterName)
        {
            return ToSqlParameterTimeSpan(input, strParameterName);
        }

        //#endregion

        //        /*
        //        SqlDbType.Timestamp; no timestamp*/

        #region SqlDbType.TinyInt ( Byte ) 
        private static NpgsqlParameter ToPostgresParameterByte(byte? input, string strParameterName)
        {
            return new NpgsqlParameter(strParameterName, SqlDbType.TinyInt)
            {
                Value = ParameterHelper.ToDbParameterValue(input)
            };
        }

        public static NpgsqlParameter ToPostgresParameter(this byte input, string strParameterName)
        {
            return ToPostgresParameterByte(input, strParameterName);
        }

        public static NpgsqlParameter ToPostgresParameter(this byte? input, string strParameterName)
        {
            return ToPostgresParameterByte(input, strParameterName);
        }
        #endregion

        //        /*
        //        SqlDbType.Udt;
        //         */
        #region SqlDbType.UniqueIdentifier ( System.Guid)
        private static NpgsqlParameter ToPostgresParameterGuid(Guid? input, string strParameterName)
        {
            return new NpgsqlParameter(strParameterName, NpgsqlDbType.Uuid)
            {
                Value = ParameterHelper.ToDbParameterValue(input)
            };
        }

        public static NpgsqlParameter ToPostgresParameter(this Guid input, string strParameterName)
        {
            return ToPostgresParameterGuid(input, strParameterName);
        }

        public static NpgsqlParameter ToPostgresParameter(this Guid? input, string strParameterName)
        {
            return ToPostgresParameterGuid(input, strParameterName);
        }
        #endregion

        //        /*
        //        SqlDbType.VarBinary; // use Binary
        //        */

        #region SqlDbType.VarChar;
        private static NpgsqlDbType ToSqlDbType(SpecificSQLStringType input)
        {
            switch (input)
            {
                case SpecificSQLStringType.VarChar:
                    {
                        return NpgsqlDbType.Varchar;
                    }
                case SpecificSQLStringType.Char:
                    {
                        return NpgsqlDbType.Char;
                    }
                case SpecificSQLStringType.Text:
                    {
                        return NpgsqlDbType.Text;
                    }

                default:
                    return NpgsqlDbType.Varchar;
            }
        }

        private static NpgsqlParameter ToPostgresParameterString(string input, string strParameterName, 
            SpecificSQLStringType dbType, bool useEmptyStringForNull, int trimInputTo)
        {
            var inputString = ParameterHelper.PrepStringInput(input, useEmptyStringForNull, trimInputTo);
            return new NpgsqlParameter(strParameterName, ToSqlDbType(dbType))
            {
                Value = ParameterHelper.ToDbParameterValue(inputString)
            };
        }

        public static NpgsqlParameter ToPostgresParameter(this string input, string strParameterName,
                SpecificSQLStringType dbType = SpecificSQLStringType.VarChar,
                bool useEmptyStringForNull = false,
                int trimInputTo = -1)
        {
            return ToPostgresParameterString(input, strParameterName, dbType, useEmptyStringForNull, trimInputTo);
        }


        public static NpgsqlParameter ToPostgresParameter(this Version input, string strParameterName)
        {
            return input == null ?
                string.Empty.ToPostgresParameter(strParameterName) :
                input.ToString().ToPostgresParameter(strParameterName);
        }
        #endregion

        //        /*
        //        SqlDbType.Variant; // ?lets find a usage ? 
        //        SqlDbType.Xml; // no direct xml support. 
        //        */


#if NET6_0_OR_GREATER
        private static NpgsqlParameter ToPostgresParameterDateOnly(DateOnly? input, string strParameterName)
        {
            return new NpgsqlParameter(strParameterName, NpgsqlDbType.Date)
            {
                Value = input.HasValue ? input.Value : (object)DBNull.Value
            };
        }

        public static NpgsqlParameter ToPostgresParameter(this DateOnly input, string strParameterName)
        {
            return ToPostgresParameterDateOnly(input, strParameterName);
        }

        public static NpgsqlParameter ToPostgresParameter(this DateOnly? input, string strParameterName)
        {
            return ToPostgresParameterDateOnly(input, strParameterName);
        }




        private static NpgsqlParameter ToPostgresParameterTimeOnly(TimeOnly? input, string strParameterName)
        {
            return new NpgsqlParameter(strParameterName, SqlDbType.Time)
            {
                Value = input.HasValue ? (object)(input.Value.ToTimeSpan()) : (object)DBNull.Value
            };
        }

        public static NpgsqlParameter ToPostgresParameter(this TimeOnly input, string strParameterName)
        {
            return ToPostgresParameterTimeOnly(input, strParameterName);
        }

        public static NpgsqlParameter ToPostgresParameter(this TimeOnly? input, string strParameterName)
        {
            return ToPostgresParameterTimeOnly(input, strParameterName);
        }
#endif


        //        public static SqlParameter ToValueDataTableSqlParameter<T>(this IEnumerable<T> input, string strParameterName, string tableTypeName)
        //        {
        //            if (input == null)
        //            {
        //                throw new ArgumentNullException(nameof(input));
        //            }
        //            return new SqlParameter(strParameterName, SqlDbType.Structured)
        //            {
        //                Value = input.ToValueDataTable(),
        //                TypeName = tableTypeName
        //            };
        //        }

        //        public static SqlParameter ToDataTableSqlParameter(this DataTable input, string strParameterName,
        //            string tableTypeName)
        //        {
        //            if (input == null)
        //            {
        //                throw new ArgumentNullException(nameof(input));
        //            }
        //            return new SqlParameter(strParameterName, SqlDbType.Structured)
        //            {
        //                Value = input,
        //                TypeName = tableTypeName
        //            };
        //        }

        //}

    }
}