using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.Sqlite;

namespace CA.Blocks.SQLLiteDataAccess
{
    public enum SpecificSQLDateTimeType
    {
        Date,
        DateTime, // The default
    }

    public enum SpecificSQLDecimalType
    {
        Decimal,
        Money, // The default
        SmallMoney,
    }

    public enum SpecificSQLStringType
    {
        //[System.Obsolete("Backwards compatibility only remove when they remove from SQL server http://msdn.microsoft.com/en-nz/library/ms187993.aspx")]
        //NText,
        //NVarChar,
        //[System.Obsolete("Backwards compatibility only remove when they remove from SQL server http://msdn.microsoft.com/en-nz/library/ms187993.aspx")]
        //Text,
        VarChar, // The Default
    }


    public enum SpecificSQLCharType
    {
        Char, // default 
        NChar
    }

    public static class SqlLiteParameterExtensions
    {
        public static SqliteCommand WithParameters(this SqliteCommand cmd, IList<SqliteParameter> parameters)
        {
            cmd.Parameters.AddRange(parameters.ToArray());
            return cmd;
        }

        #region SqlDbType.BigInt ( long, Int64 ) 
        private static SqliteParameter ToSqlParameterBigInt(long? input, string strParameterName)
        {
            var sqlparam = new SqliteParameter(strParameterName, SqlDbType.BigInt)
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

        public static SqliteParameter ToSqlParameter(this long input, string strParameterName)
        {
            return ToSqlParameterBigInt(input, strParameterName);
        }

        public static SqliteParameter ToSqlParameter(this long? input, string strParameterName)
        {
            return ToSqlParameterBigInt(input, strParameterName);
        }
        #endregion


        #region SqlDbType.Binary / SqlDbType.VarBinary  ( We dont know the size so VarBinary and Binary are the same

        public static SqliteParameter ToSqlParameter(this byte[] input, string strParameterName)
        {
            throw new NotSupportedException("SQL Lite does not support  byte[] convert to unicode string first");
        }
        #endregion


        #region SqlDbType.Bit ( boolean ) 
        private static SqliteParameter ToSqlParameterBool(bool? input, string strParameterName)
        {
            var sqlparam = new SqliteParameter(strParameterName, SqlDbType.Bit)
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

        public static SqliteParameter ToSqlParameter(this bool input, string strParameterName)
        {
            return ToSqlParameterBool(input, strParameterName);
        }

        public static SqliteParameter ToSqlParameter(this bool? input, string strParameterName)
        {
            return ToSqlParameterBool(input, strParameterName);
        }
        #endregion


        #region SqlDbType.Char   (Char)


        private static SqlDbType ToSqlDbType(SpecificSQLCharType dbType)
        {
            return dbType == SpecificSQLCharType.Char ? SqlDbType.Char : SqlDbType.NChar;
        }


        private static SqliteParameter ToSqlParameterChar(Char? input, string strParameterName, SpecificSQLCharType dbType)
        {
            var sqlparam = new SqliteParameter(strParameterName, ToSqlDbType(dbType))
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

        public static SqliteParameter ToSqlParameter(this Char input, string strParameterName, SpecificSQLCharType dbType = SpecificSQLCharType.Char)
        {
            return ToSqlParameterChar(input, strParameterName, dbType);
        }

        public static SqliteParameter ToSqlParameter(this Char? input, string strParameterName, SpecificSQLCharType dbType = SpecificSQLCharType.Char)
        {
            return ToSqlParameterChar(input, strParameterName, dbType);
        }
        #endregion


        #region SqlDbType.DateTime ( System.DateTime )
        private static SqlDbType ToSqlDbType(SpecificSQLDateTimeType dbType)
        {
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
                default:
                    return SqlDbType.DateTime;
            }
        }

        //private static SqliteParameter ToSqlParameterDateTime(DateTime? input, string strParameterName, SpecificSQLDateTimeType dbType)
        //{
        //    var sqlparam = new SqliteParameter(strParameterName, ToSqlDbType(dbType))
        //    {
        //        Direction = ParameterDirection.Input
        //    };
        //    if (input.HasValue)
        //    {
        //        //TEXT as ISO8601 strings("YYYY-MM-DD HH:MM:SS.SSS").
        //        //string search = input.Value.ToString("yyyy-MM-dd HH:mm:ss.fff");
        //        string search = input.Value.ToString("yyyy-MM-DD HH:mm:ss.fff");
        //        sqlparam.Value = search;
        //    }
        //    else
        //    {
        //        sqlparam.Value = DBNull.Value;
        //    }
        //    return (sqlparam);
        //}

        public static SqliteParameter ToSqlParameter(this DateTime input, string strParameterName, SpecificSQLDateTimeType dbType = SpecificSQLDateTimeType.DateTime)
        {
            throw new NotSupportedException("SQLite does not have a storage class set aside for storing dates and/or times. Instead, the built-in Date And Time Functions of SQLite are capable of storing dates and times as TEXT, REAL, or INTEGER values: see https://www.sqlite.org/lang_datefunc.html, Use the datetime('{data:o}') C# roundtrip function to get the string");
            //return ToSqlParameterDateTime(input, strParameterName, dbType);
        }

        // Default to DateTime

        public static SqliteParameter ToSqlParameter(this DateTime? input, string strParameterName, SpecificSQLDateTimeType dbType = SpecificSQLDateTimeType.DateTime)
        {
            throw new NotSupportedException("SQLite does not have a storage class set aside for storing dates and/or times. Instead, the built-in Date And Time Functions of SQLite are capable of storing dates and times as TEXT, REAL, or INTEGER values: see https://www.sqlite.org/lang_datefunc.html, Use the datetime('{data:o}') C# roundtrip function to get the string");
            //return ToSqlParameterDateTime(input, strParameterName, dbType);
        }
        #endregion

        /*TODO
        SqlDbType.DateTimeOffset;
        */
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

        private static SqliteParameter ToSqlParameterDecimal(Decimal? input, string strParameterName, SpecificSQLDecimalType dbType)
        {
            var sqlparam = new SqliteParameter(strParameterName, ToSqlDbType(dbType))
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


        public static SqliteParameter ToSqlParameter(this Decimal input, string strParameterName, SpecificSQLDecimalType dbType = SpecificSQLDecimalType.Decimal)
        {
            return ToSqlParameterDecimal(input, strParameterName, dbType);
        }

        // Default is SpecificSQLDecimalType.Decimal
        public static SqliteParameter ToSqlParameter(this Decimal? input, string strParameterName, SpecificSQLDecimalType dbType = SpecificSQLDecimalType.Decimal)
        {
            return ToSqlParameterDecimal(input, strParameterName, dbType);
        }

        #endregion

        #region SqlDbType.Float  (System.Double)

        private static SqliteParameter ToSqlParameterDouble(Double? input, string strParameterName)
        {
            var sqlparam = new SqliteParameter(strParameterName, SqlDbType.Float)
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

        public static SqliteParameter ToSqlParameter(this Double input, string strParameterName)
        {
            return ToSqlParameterDouble(input, strParameterName);
        }

        public static SqliteParameter ToSqlParameter(this Double? input, string strParameterName)
        {
            return ToSqlParameterDouble(input, strParameterName);
        }
        #endregion

        /*
        SqlDbType.Image;
        */
        #region SqlDbType.Int  ( int, Int32 )

        private static SqliteParameter ToSqlParameterInt(int? input, string strParameterName)
        {
            var sqlparam = new SqliteParameter(strParameterName, SqlDbType.Int)
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

        public static SqliteParameter ToSqlParameter(this int input, string strParameterName)
        {
            return ToSqlParameterInt(input, strParameterName);
        }

        public static SqliteParameter ToSqlParameter(this int? input, string strParameterName)
        {
            return ToSqlParameterInt(input, strParameterName);
        }
        #endregion 
        /*
        SqlDbType.Money;
        SqlDbType.NChar;
        SqlDbType.Real;
        SqlDbType.SmallDateTime;*/


        #region SqlDbType.SmallInt  -> ( short, Int16)
        private static SqliteParameter ToSqlParameterInt16(Int16? input, string strParameterName)
        {
            var sqlparam = new SqliteParameter(strParameterName, SqlDbType.SmallInt)
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

        public static SqliteParameter ToSqlParameter(this Int16 input, string strParameterName)
        {
            return ToSqlParameterInt16(input, strParameterName);
        }

        public static SqliteParameter ToSqlParameter(this Int16? input, string strParameterName)
        {
            return ToSqlParameterInt16(input, strParameterName);
        }
        #endregion 

        /*
        SqlDbType.SmallMoney;
        SqlDbType.Structured;
         */
        #region SqlDbType.Time ( System.TimeSpan )

        private static SqliteParameter ToSqlParameterTimeSpan(TimeSpan? input, string strParameterName)
        {
            var sqlparam = new SqliteParameter(strParameterName, SqlDbType.Time)
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

        public static SqliteParameter ToSqlParameter(this TimeSpan input, string strParameterName)
        {
            return ToSqlParameterTimeSpan(input, strParameterName);
        }

        public static SqliteParameter ToSqlParameter(this TimeSpan? input, string strParameterName)
        {
            return ToSqlParameterTimeSpan(input, strParameterName);
        }

        #endregion 

        /*
        SqlDbType.Timestamp;*/

        #region SqlDbType.TinyInt ( Byte ) 
        private static SqliteParameter ToSqlParameterByte(byte? input, string strParameterName)
        {
            var sqlparam = new SqliteParameter(strParameterName, SqlDbType.TinyInt)
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

        public static SqliteParameter ToSqlParameter(this byte input, string strParameterName)
        {
            return ToSqlParameterByte(input, strParameterName);
        }

        public static SqliteParameter ToSqlParameter(this byte? input, string strParameterName)
        {
            return ToSqlParameterByte(input, strParameterName);
        }
        #endregion 

        /*
        SqlDbType.Udt;
         */
        #region SqlDbType.UniqueIdentifier ( System.Guid)
        private static SqliteParameter ToSqlParameterGuid(Guid? input, string strParameterName)
        {
            var sqlparam = new SqliteParameter(strParameterName, SqlDbType.UniqueIdentifier)
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

        public static SqliteParameter ToSqlParameter(this Guid input, string strParameterName)
        {
            return ToSqlParameterGuid(input, strParameterName);
        }

        public static SqliteParameter ToSqlParameter(this Guid? input, string strParameterName)
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
                //case SpecificSQLStringType.NVarChar:
                //    {
                //        return SqlDbType.NVarChar;
                //    }
                //case SpecificSQLStringType.Text:
                //    {
                //        return SqlDbType.Text;
                //    }
                //case SpecificSQLStringType.NText:
                //    {
                //        return SqlDbType.NText;
                //    }

                default:
                    return SqlDbType.VarChar;
            }
        }

        private static SqliteParameter ToSqlParameterString(string input, string strParameterName, SpecificSQLStringType dbType, bool useEmptyStringForNull, int trimInputTo)
        {
            var sqlparam = new SqliteParameter(strParameterName, ToSqlDbType(dbType))
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

        public static SqliteParameter ToSqlParameter(this string input, string strParameterName, SpecificSQLStringType dbType = SpecificSQLStringType.VarChar, bool useEmptyStringForNull = false, int trimInputTo = -1)
        {
            return ToSqlParameterString(input, strParameterName, dbType, useEmptyStringForNull, trimInputTo);
        }

        #endregion


        //public static SqliteParameter ToSqlParameter(object input, string strParameterName)
        //{
        //    return NotImplementedException("TODO");
        //}
        /*
       SqlDbType.Variant; // ?lets find a usage ? 
       SqlDbType.Xml;
       */

    }
}
