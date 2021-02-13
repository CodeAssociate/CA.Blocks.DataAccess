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
using System.ComponentModel;
using System.Data;
using System.Linq;
using MySqlConnector;

namespace CA.Blocks.MySQLDataAccess
{

    public enum SpecificMySQLDateTimeType
    {
        /// <summary>
        /// Will only store the date value of the DateTime Input, is is accurate to the day and uses 3 bytes of storage.
        /// </summary>
        Date,
        /// <summary>
        /// Will only store the date and time value on the input, this is the default
        /// </summary>
        DateTime, // The default

    }

    public enum SpecificMySQLStringType
    {
        VarChar, // The Default up to 65,535 
        Text, // to 65,535 
        MediumText, // to 16 MB
        LongText, // To 4 GB
    }



    public static class MySqlParameterExtensions
    {
        public static MySqlCommand WithParameters(this MySqlCommand cmd, IList<MySqlParameter> parameters)
        {
            cmd.Parameters.AddRange(parameters.ToArray());
            return cmd;
        }

        public static MySqlCommand WithParameter(this MySqlCommand cmd, MySqlParameter parameter)
        {
            cmd.Parameters.Add(parameter);
            return cmd;
        }

        //public static MySqlCommand WithReturnResult(this MySqlCommand cmd)
        //{
        //    MySqlParameter sqlparam = cmd.CreateParameter();
        //    sqlparam.ParameterName = "Return";
        //    sqlparam.DbType = DbType.Int32;
        //    sqlparam.Direction = ParameterDirection.ReturnValue;
        //    cmd.Parameters.Add(sqlparam);
        //    return cmd;
        //}


        //public static int? GetReturnResult(this MySqlCommand cmd)
        //{
        //    int? result = null;
        //    var sqlParam = cmd.Parameters["Return"];
        //    if (sqlParam != null && sqlParam.MySqlDbType == MySqlDbType.Int && sqlParam.Direction == ParameterDirection.ReturnValue)
        //    {
        //        if (sqlParam.Value != null && sqlParam.Value != DBNull.Value)
        //            result = (int)sqlParam.Value;
        //    }
        //    return result;
        //}


        public static MySqlParameter AsOutput(this MySqlParameter sqlParameter)
        {
            sqlParameter.Direction = ParameterDirection.Output;
            return sqlParameter;
        }

        public static MySqlParameter AsInputOutput(this MySqlParameter sqlParameter)
        {
            sqlParameter.Direction = ParameterDirection.InputOutput;
            return sqlParameter;
        }

        public static T ToValue<T>(this MySqlParameter sqlParameter)
        {
            T result = default(T);
            if (sqlParameter != null && (sqlParameter.Direction == ParameterDirection.Output || sqlParameter.Direction == ParameterDirection.InputOutput))
            {
                if (sqlParameter.Value != null && sqlParameter.Value != DBNull.Value)
                    result = (T)sqlParameter.Value;
            }
            return result;
        }

        public static T ToValueWithConvert<T>(this MySqlParameter sqlParameter)
        {
            T result = default(T);
            if (sqlParameter != null && (sqlParameter.Direction == ParameterDirection.Output || sqlParameter.Direction == ParameterDirection.InputOutput))
            {
                if (sqlParameter.Value != null && sqlParameter.Value != DBNull.Value)
                    result = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(sqlParameter.Value.ToString());
            }
            return result;
        }

        #region MySqlDbType.BigInt ( long, Int64 ) 
        private static MySqlParameter ToSqlParameterBigInt(long? input, string strParameterName)
        {
            var sqlparam = new MySqlParameter(strParameterName, MySqlDbType.Int64)
            {
                Direction = ParameterDirection.Input,
                Size = 8,
            };
            if (input.HasValue)
                sqlparam.Value = input;
            else
            {
                sqlparam.Value = DBNull.Value;
            }
            return (sqlparam);
        }

        public static MySqlParameter ToSqlParameter(this long input, string strParameterName)
        {
            return ToSqlParameterBigInt(input, strParameterName);
        }

        public static MySqlParameter ToSqlParameter(this long? input, string strParameterName)
        {
            return ToSqlParameterBigInt(input, strParameterName);
        }
        #endregion


        #region MySqlDbType.Binary / MySqlDbType.VarBinary  ( We dont know the size so VarBinary and Binary are the same

        public static MySqlParameter ToSqlParameter(this byte[] input, string strParameterName)
        {
            var sqlparam = new MySqlParameter(strParameterName, MySqlDbType.VarBinary)
            {
                Direction = ParameterDirection.Input,
            };
            if (input != null)
                sqlparam.Value = input;
            else
            {
                sqlparam.Value = DBNull.Value;
            }
            return (sqlparam);
        }
        #endregion


        #region MySqlDbType.Bit ( boolean ) 
        private static MySqlParameter ToSqlParameterBool(bool? input, string strParameterName)
        {
            var sqlparam = new MySqlParameter(strParameterName, MySqlDbType.Bit)
            {
                Direction = ParameterDirection.Input,
            };
            if (input.HasValue)
                sqlparam.Value = input;
            else
            {
                sqlparam.Value = DBNull.Value;
            }
            return (sqlparam);
        }

        public static MySqlParameter ToSqlParameter(this bool input, string strParameterName)
        {
            return ToSqlParameterBool(input, strParameterName);
        }

        public static MySqlParameter ToSqlParameter(this bool? input, string strParameterName)
        {
            return ToSqlParameterBool(input, strParameterName);
        }
        #endregion


        #region MySqlDbType.Char   (Char)



        private static MySqlParameter ToSqlParameterChar(Char? input, string strParameterName)
        {
            var sqlparam = new MySqlParameter(strParameterName, MySqlDbType.VarChar)
            {
                Direction = ParameterDirection.Input,
            };
            if (input.HasValue)
                sqlparam.Value = input;
            else
            {
                sqlparam.Value = DBNull.Value;
            }
            return (sqlparam);
        }

        public static MySqlParameter ToSqlParameter(this Char input, string strParameterName)
        {
            return ToSqlParameterChar(input, strParameterName);
        }

        public static MySqlParameter ToSqlParameter(this Char? input, string strParameterName)
        {
            return ToSqlParameterChar(input, strParameterName);
        }
        #endregion


        #region MySqlDbType.DateTime ( System.DateTime )
        private static MySqlDbType ToSqlDbType(SpecificMySQLDateTimeType dbType)
        {
            switch (dbType)
            {
                case SpecificMySQLDateTimeType.DateTime:
                    {
                        return MySqlDbType.DateTime;
                    }
                case SpecificMySQLDateTimeType.Date:
                    {
                        return MySqlDbType.Date;
                    }
                default:
                    return MySqlDbType.DateTime;
            }
        }

        private static MySqlParameter ToSqlParameterDateTime(DateTime? input, string strParameterName, SpecificMySQLDateTimeType dbType)
        {
            var sqlparam = new MySqlParameter(strParameterName, ToSqlDbType(dbType))
            {
                Direction = ParameterDirection.Input
            };
            if (input.HasValue)
                sqlparam.Value = input;
            else
            {
                sqlparam.Value = DBNull.Value;
            }
            return (sqlparam);
        }

        public static MySqlParameter ToSqlParameter(this DateTime input, string strParameterName, SpecificMySQLDateTimeType dbType = SpecificMySQLDateTimeType.DateTime)
        {
            return ToSqlParameterDateTime(input, strParameterName, dbType);
        }

        // Default to DateTime

        public static MySqlParameter ToSqlParameter(this DateTime? input, string strParameterName, SpecificMySQLDateTimeType dbType = SpecificMySQLDateTimeType.DateTime)
        {
            return ToSqlParameterDateTime(input, strParameterName, dbType);
        }
        #endregion


        #region MySqlDbType.Decimal  (Decimal, Money, SmallMoney)

        //private static MySqlDbType ToSqlDbType(SpecificSQLDecimalType dbType)
        //{
        //    switch (dbType)
        //    {
        //        case SpecificSQLDecimalType.Decimal:
        //            {
        //                return MySqlDbType.Decimal;
        //            }
        //        case SpecificSQLDecimalType.Money:
        //            {
        //                return MySqlDbType.NewDecimal;
        //            }
        //        case SpecificSQLDecimalType.SmallMoney:
        //            {
        //                return MySqlDbType.;
        //            }
        //        default:
        //            return MySqlDbType.Decimal;
        //    }
        //}

        private static MySqlParameter ToSqlParameterDecimal(Decimal? input, string strParameterName)
        {
            var sqlparam = new MySqlParameter(strParameterName, MySqlDbType.Decimal)
            {
                Direction = ParameterDirection.Input
            };
            if (input.HasValue)
                sqlparam.Value = input;
            else
            {
                sqlparam.Value = DBNull.Value;
            }
            return (sqlparam);
        }


        public static MySqlParameter ToSqlParameter(this Decimal input, string strParameterName)
        {
            return ToSqlParameterDecimal(input, strParameterName);
        }

        // Default is SpecificSQLDecimalType.Decimal
        public static MySqlParameter ToSqlParameter(this Decimal? input, string strParameterName)
        {
            return ToSqlParameterDecimal(input, strParameterName);
        }

        #endregion

        #region MySqlDbType.Float  (System.Double)

        private static MySqlParameter ToSqlParameterDouble(Double? input, string strParameterName)
        {
            var sqlparam = new MySqlParameter(strParameterName, MySqlDbType.Double)
            {
                Direction = ParameterDirection.Input
            };
            if (input.HasValue)
                sqlparam.Value = input;
            else
            {
                sqlparam.Value = DBNull.Value;
            }
            return (sqlparam);
        }

        public static MySqlParameter ToSqlParameter(this Double input, string strParameterName)
        {
            return ToSqlParameterDouble(input, strParameterName);
        }

        public static MySqlParameter ToSqlParameter(this Double? input, string strParameterName)
        {
            return ToSqlParameterDouble(input, strParameterName);
        }
        #endregion

        /*
        MySqlDbType.Image;
        */
        #region MySqlDbType.Int  ( int, Int32 )

        private static MySqlParameter ToSqlParameterInt(int? input, string strParameterName)
        {
            var sqlparam = new MySqlParameter(strParameterName, MySqlDbType.Int32)
            {
                Direction = ParameterDirection.Input,
                Size = 4,
            };
            if (input.HasValue)
                sqlparam.Value = input;
            else
            {
                sqlparam.Value = DBNull.Value;
            }
            return (sqlparam);
        }

        public static MySqlParameter ToSqlParameter(this int input, string strParameterName)
        {
            return ToSqlParameterInt(input, strParameterName);
        }

        public static MySqlParameter ToSqlParameter(this int? input, string strParameterName)
        {
            return ToSqlParameterInt(input, strParameterName);
        }
        #endregion 
        /*
        MySqlDbType.Money;
        MySqlDbType.NChar;
        MySqlDbType.Real;
        MySqlDbType.SmallDateTime;*/


        #region MySqlDbType.SmallInt  -> ( short, Int16)
        private static MySqlParameter ToSqlParameterInt16(Int16? input, string strParameterName)
        {
            var sqlparam = new MySqlParameter(strParameterName, MySqlDbType.Int16)
            {
                Direction = ParameterDirection.Input,
                Size = 2,
            };
            if (input.HasValue)
                sqlparam.Value = input;
            else
            {
                sqlparam.Value = DBNull.Value;
            }
            return (sqlparam);
        }

        public static MySqlParameter ToSqlParameter(this Int16 input, string strParameterName)
        {
            return ToSqlParameterInt16(input, strParameterName);
        }

        public static MySqlParameter ToSqlParameter(this Int16? input, string strParameterName)
        {
            return ToSqlParameterInt16(input, strParameterName);
        }
        #endregion 

        #region MySqlDbType.Time ( System.TimeSpan )

        private static MySqlParameter ToSqlParameterTimeSpan(TimeSpan? input, string strParameterName)
        {
            var sqlparam = new MySqlParameter(strParameterName, MySqlDbType.Time)
            {
                Direction = ParameterDirection.Input,
            };
            if (input.HasValue)
                sqlparam.Value = input;
            else
            {
                sqlparam.Value = DBNull.Value;
            }
            return (sqlparam);
        }

        public static MySqlParameter ToSqlParameter(this TimeSpan input, string strParameterName)
        {
            return ToSqlParameterTimeSpan(input, strParameterName);
        }

        public static MySqlParameter ToSqlParameter(this TimeSpan? input, string strParameterName)
        {
            return ToSqlParameterTimeSpan(input, strParameterName);
        }

        #endregion 

        /*
        MySqlDbType.Timestamp;*/

        #region MySqlDbType.TinyInt ( Byte ) 
        private static MySqlParameter ToSqlParameterByte(byte? input, string strParameterName)
        {
            var sqlparam = new MySqlParameter(strParameterName, MySqlDbType.Byte)
            {
                Direction = ParameterDirection.Input,
                Size = 1,
            };
            if (input.HasValue)
                sqlparam.Value = input;
            else
            {
                sqlparam.Value = DBNull.Value;
            }
            return (sqlparam);
        }

        public static MySqlParameter ToSqlParameter(this byte input, string strParameterName)
        {
            return ToSqlParameterByte(input, strParameterName);
        }

        public static MySqlParameter ToSqlParameter(this byte? input, string strParameterName)
        {
            return ToSqlParameterByte(input, strParameterName);
        }
        #endregion 

        /*
        MySqlDbType.Udt;
         */
        #region MySqlDbType.UniqueIdentifier ( System.Guid)
        private static MySqlParameter ToSqlParameterGuid(Guid? input, string strParameterName)
        {
            var sqlparam = new MySqlParameter(strParameterName, MySqlDbType.Guid)
            {
                Direction = ParameterDirection.Input,
                Size = 16,
            };
            if (input.HasValue)
                sqlparam.Value = input;
            else
            {
                sqlparam.Value = DBNull.Value;
            }
            return (sqlparam);
        }

        public static MySqlParameter ToSqlParameter(this Guid input, string strParameterName)
        {
            return ToSqlParameterGuid(input, strParameterName);
        }

        public static MySqlParameter ToSqlParameter(this Guid? input, string strParameterName)
        {
            return ToSqlParameterGuid(input, strParameterName);
        }
        #endregion

        /*
        MySqlDbType.VarBinary; // use Binary
        */

        #region MySqlDbType.VarChar;
        private static MySqlDbType ToSqlDbType(SpecificMySQLStringType input)
        {
            switch (input)
            {
                case SpecificMySQLStringType.VarChar:
                    {
                        return MySqlDbType.VarChar;
                    }
                case SpecificMySQLStringType.Text:
                    {
                        return MySqlDbType.Text;
                    }
                case SpecificMySQLStringType.MediumText:
                    {
                        return MySqlDbType.MediumText;
                    }
                case SpecificMySQLStringType.LongText:
                    {
                        return MySqlDbType.LongText;
                    }
                default:
                    return MySqlDbType.VarChar;
            }
        }

        private static MySqlParameter ToSqlParameterString(string input, string strParameterName, SpecificMySQLStringType dbType, bool useEmptyStringForNull, int trimInputTo)
        {
            var sqlparam = new MySqlParameter(strParameterName, ToSqlDbType(dbType))
            {
                Direction = ParameterDirection.Input
            };
            if (input != null)
            {
                if (input.Length > trimInputTo)
                {
                    sqlparam.Value = trimInputTo > 0 ? input.Substring(0, trimInputTo) : input;
                }
                else
                {
                    sqlparam.Value = input;
                }
            }
            else
            {
                if (useEmptyStringForNull)
                    sqlparam.Value = string.Empty;
                else
                    sqlparam.Value = DBNull.Value;
            }
            return (sqlparam);
        }

        public static MySqlParameter ToSqlParameter(this string input, string strParameterName, SpecificMySQLStringType dbType = SpecificMySQLStringType.VarChar, bool useEmptyStringForNull = false, int trimInputTo = -1)
        {
            return ToSqlParameterString(input, strParameterName, dbType, useEmptyStringForNull, trimInputTo);
        }

        #endregion

        /*
       MySqlDbType.Variant; // ?lets find a usage ? 
       MySqlDbType.Xml;
       */



    }
}
