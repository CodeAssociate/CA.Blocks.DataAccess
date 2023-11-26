using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CA.Blocks.DataAccess;
using Microsoft.Data.Sqlite;

namespace CA.Blocks.SqliteDataAccess
{
	// SQlLite is unique in the fact that the DB is Typeless -  This means that you can store any kind of data you want in any column of any table, regardless of the declared datatype of that column
	// see https://www.sqlite.org/datatypes.html
	// AS such these extensions are more helpers of intent
	// The SQL will have to have knowledge of type operations example if you working with date as col you need to wrap the syntax with a date function ie DatCol > date(value)
	// the blocks tries to standardise on the ISO-8601 format for a little more structure   
	public static class SqliteParameterExtensions
    {
        public static SqliteCommand WithParameters(this SqliteCommand cmd, IList<SqliteParameter> parameters)
        {
            cmd.Parameters.AddRange(parameters.ToArray());
            return cmd;
        }


        public static SqliteParameter NullSqliteParameter(string strParameterName)
        {
            return new SqliteParameter(strParameterName, DBNull.Value);
        }


        #region SqlDbType.BigInt ( long, Int64 ) 
        private static SqliteParameter ToSqlParameterBigInt(long? input, string strParameterName)
        {
            return  new SqliteParameter(strParameterName, SqliteType.Integer)
            {
                Direction = ParameterDirection.Input,
                Size = 8,
                Value = ParameterHelper.ToDbParameterValue(input)
            };
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
            return new SqliteParameter(strParameterName, SqliteType.Blob)
            {
                Direction = ParameterDirection.Input,
                DbType = DbType.Binary,
                Value = ParameterHelper.ToDbParameterValue(input)
            };
        }
        #endregion


        #region SqlDbType.Bit ( boolean ) 
        private static SqliteParameter ToSqlParameterBool(bool? input, string strParameterName)
        {
            return new SqliteParameter(strParameterName, SqliteType.Integer )
            {
                Direction = ParameterDirection.Input,
                Value = ParameterHelper.ToDbParameterValue(input)
            };
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


        private static SqliteParameter ToSqlParameterChar(Char? input, string strParameterName)
        {
            return new SqliteParameter(strParameterName, SqliteType.Text)
            {
                Direction = ParameterDirection.Input,
                Value = ParameterHelper.ToDbParameterValue(input)
            };
        }

        public static SqliteParameter ToSqlParameter(this Char input, string strParameterName)
        {
            return ToSqlParameterChar(input, strParameterName);
        }

        public static SqliteParameter ToSqlParameter(this Char? input, string strParameterName)
        {
            return ToSqlParameterChar(input, strParameterName);
        }
        #endregion


        #region SqlDbType.DateTime ( System.DateTime )

        public static SqliteParameter ToSqlParameter(this DateTime input, string strParameterName)
        {
            // sqlite does not have a storage class set aside for storing dates and/or times.
            // the blocks wil use string in ISO-8601 format the use sqllite functions on those values
            // Instead, the built-in Date And Time Functions of SQLite are capable of storing dates and times as TEXT, REAL, or INTEGER values: see https://www.sqlite.org/lang_datefunc.html,
            // Use the datetime('{datetime:o}') C# roundtrip function to get the string.
            // Not the o format complies  with ISO 8601. and will output timezone pending DateTime.Kind
            return $"{input:o}".ToSqlParameter(strParameterName);
        }

        // Default to DateTime


        public static SqliteParameter ToSqlParameter(this DateTime? input, string strParameterName)
        {
            return input.HasValue
                ? $"{input:o}".ToSqlParameter(strParameterName)
                : NullSqliteParameter(strParameterName);
        }

#if NET6_0_OR_GREATER
        public static SqliteParameter ToSqlParameter(this DateOnly input, string strParameterName)
        {
            return $"{input:o}".ToSqlParameter(strParameterName);
        }
        // Default to DateTime


        public static SqliteParameter ToSqlParameter(this DateOnly? input, string strParameterName)
        {
            return input.HasValue
                ? $"{input:o}".ToSqlParameter(strParameterName)
                : NullSqliteParameter(strParameterName);
        }

        public static SqliteParameter ToSqlParameter(this TimeOnly input, string strParameterName)
        {
            return $"{input:o}".ToSqlParameter(strParameterName);
        }
        // Default to DateTime


        public static SqliteParameter ToSqlParameter(this TimeOnly? input, string strParameterName)
        {
            return input.HasValue
                ? $"{input:o}".ToSqlParameter(strParameterName)
                : NullSqliteParameter(strParameterName);
        }

#endif
        #endregion



        #region SqlDbType.DateTimeOffset;

        public static SqliteParameter ToSqlParameter(this DateTimeOffset? input, string strParameterName)
        {
            return input.HasValue
                ? $"{input:o}".ToSqlParameter(strParameterName)
                : NullSqliteParameter(strParameterName);
        }
        #endregion

        #region SqlDbType.Decimal  (Decimal, Money, SmallMoney)


        private static SqliteParameter ToSqlParameterDecimal(Decimal? input, string strParameterName)
        {
            return new SqliteParameter(strParameterName, SqliteType.Text)
            {
                Direction = ParameterDirection.Input,
                Value = ParameterHelper.ToDbParameterValue(input)
            };
        }


        public static SqliteParameter ToSqlParameter(this Decimal input, string strParameterName)
        {
            return ToSqlParameterDecimal(input, strParameterName);
        }

        // Default is SpecificSQLDecimalType.Decimal
        public static SqliteParameter ToSqlParameter(this Decimal? input, string strParameterName)
        {
            return ToSqlParameterDecimal(input, strParameterName);
        }

        #endregion

        #region SqlDbType.Float  (System.Double)

        private static SqliteParameter ToSqlParameterDouble(Double? input, string strParameterName)
        {
            return new SqliteParameter(strParameterName, SqliteType.Real)
            {
                Direction = ParameterDirection.Input,
                Value = ParameterHelper.ToDbParameterValue(input)
            };
        }

        public static SqliteParameter ToSqlParameter(this Double input, string strParameterName)
        {
            return ToSqlParameterDouble(input, strParameterName);
        }

        public static SqliteParameter ToSqlParameter(this Double? input, string strParameterName)
        {
            return ToSqlParameterDouble(input, strParameterName);
        }

        private static SqliteParameter ToSqlParameterDouble(Single? input, string strParameterName)
        {
            return new SqliteParameter(strParameterName, SqliteType.Real)
			{
                Direction = ParameterDirection.Input,
                Value = ParameterHelper.ToDbParameterValue(input)
            };
        }

        public static SqliteParameter ToSqlParameter(this Single input, string strParameterName)
        {
            return ToSqlParameterDouble(input, strParameterName);
        }

        public static SqliteParameter ToSqlParameter(this Single? input, string strParameterName)
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
            return new SqliteParameter(strParameterName, SqliteType.Integer, 4)
            {
                Value = ParameterHelper.ToDbParameterValue(input)
            };
        }

        public static SqliteParameter ToSqlParameter(this int input, string strParameterName)
        {
            return ToSqlParameterInt(input, strParameterName);
        }

        public static SqliteParameter ToSqlParameter(this int? input, string strParameterName)
        {
            return ToSqlParameterInt(input, strParameterName);
        }

        private static SqliteParameter ToSqlParameterUInt(uint? input, string strParameterName)
        {
            return new SqliteParameter(strParameterName, SqliteType.Integer)
            {
                Value = ParameterHelper.ToDbParameterValue(input)
            };
        }

        public static SqliteParameter ToSqlParameter(this uint input, string strParameterName)
        {
            return ToSqlParameterUInt(input, strParameterName);
        }

        public static SqliteParameter ToSqlParameter(this uint? input, string strParameterName)
        {
            return ToSqlParameterUInt(input, strParameterName);
        }
        #endregion 


        #region SqlDbType.SmallInt  -> ( short, Int16)
        private static SqliteParameter ToSqlParameterInt16(Int16? input, string strParameterName)
        {
            return new SqliteParameter(strParameterName, SqliteType.Integer)
            {
                Direction = ParameterDirection.Input,
                Size = 2,
                Value = ParameterHelper.ToDbParameterValue(input)
            };
        }

        public static SqliteParameter ToSqlParameter(this Int16 input, string strParameterName)
        {
            return ToSqlParameterInt16(input, strParameterName);
        }

        public static SqliteParameter ToSqlParameter(this Int16? input, string strParameterName)
        {
            return ToSqlParameterInt16(input, strParameterName);
        }

        private static SqliteParameter ToSqlParameterUInt16(UInt16? input, string strParameterName)
        {
            return new SqliteParameter(strParameterName, SqliteType.Integer)
            {
                Direction = ParameterDirection.Input,
                Size = 2,
                Value = ParameterHelper.ToDbParameterValue(input)
            };
        }

        public static SqliteParameter ToSqlParameter(this UInt16 input, string strParameterName)
        {
            return ToSqlParameterUInt16(input, strParameterName);
        }

        public static SqliteParameter ToSqlParameter(this UInt16? input, string strParameterName)
        {
            return ToSqlParameterUInt16(input, strParameterName);
        }
        #endregion 

        #region SqlDbType.Time ( System.TimeSpan )

        private static SqliteParameter ToSqlParameterTimeSpan(TimeSpan? input, string strParameterName)
        {
            return  new SqliteParameter(strParameterName, SqliteType.Text)
            {
                Direction = ParameterDirection.Input,
                Value = ParameterHelper.ToDbParameterValue(input)
            };
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


        #region SqlDbType.TinyInt ( Byte ) 
        private static SqliteParameter ToSqlParameterByte(byte? input, string strParameterName)
        {
            return new SqliteParameter(strParameterName, SqliteType.Integer)
            {
                Direction = ParameterDirection.Input,
                Size = 1,
                Value = ParameterHelper.ToDbParameterValue(input)
            };
        }

        public static SqliteParameter ToSqlParameter(this byte input, string strParameterName)
        {
            return ToSqlParameterByte(input, strParameterName);
        }

        public static SqliteParameter ToSqlParameter(this byte? input, string strParameterName)
        {
            return ToSqlParameterByte(input, strParameterName);
        }

        private static SqliteParameter ToSqlParameterSByte(sbyte? input, string strParameterName)
        {
            return new SqliteParameter(strParameterName, SqliteType.Integer)
            {
                Direction = ParameterDirection.Input,
                Size = 1,
                Value = ParameterHelper.ToDbParameterValue(input)
            };
        }

        public static SqliteParameter ToSqlParameter(this sbyte input, string strParameterName)
        {
            return ToSqlParameterSByte(input, strParameterName);
        }

        public static SqliteParameter ToSqlParameter(this sbyte? input, string strParameterName)
        {
            return ToSqlParameterSByte(input, strParameterName);
        }

        #endregion 

        /*
        SqlDbType.Udt;
         */
        #region SqlDbType.UniqueIdentifier ( System.Guid)
        private static SqliteParameter ToSqlParameterGuid(Guid? input, string strParameterName)
        {
            return new SqliteParameter(strParameterName, SqliteType.Text)
            {
                Direction = ParameterDirection.Input,
                Value = ParameterHelper.ToDbParameterValue(input)
            };
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


        private static SqliteParameter ToSqlParameterString(string input, string strParameterName, bool useEmptyStringForNull, int trimInputTo)
        {
            var inputString = ParameterHelper.PrepStringInput(input, useEmptyStringForNull, trimInputTo);
            return new SqliteParameter(strParameterName, SqliteType.Text)
            {
                Value = ParameterHelper.ToDbParameterValue(inputString)
            };
        }

        public static SqliteParameter ToSqlParameter(this string input, string strParameterName, bool useEmptyStringForNull = false, int trimInputTo = -1)
        {
            return ToSqlParameterString(input, strParameterName, useEmptyStringForNull, trimInputTo);
        }
    }
}
