using System;
using System.Data;

namespace CA.Blocks.SQLLiteDataAccessUnitTests.Base
{
    public class MockDataReader : IDataReader
    {
        private bool open = true;
        private DataTable resultSet;
        private int currentPosition = 0;
        
        internal MockDataReader(DataTable tbl)
        {
            resultSet = tbl;
        }

        #region IDataReader Members

        public int RecordsAffected => -1;

        public bool IsClosed => !open;

        public bool NextResult()
        {
            // TODO:  if we need
            return false;
        }

        public void Close()
        {
            open = false;
        }

        public bool Read()
        {
            return ++currentPosition < resultSet.Rows.Count;
        }

        public int Depth => 0;

        public DataTable GetSchemaTable()
        {
            throw new NotSupportedException();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {

        }

        #endregion

        #region IDataRecord Members

        public int GetInt32(int i)
        {
            return (int)resultSet.Rows[currentPosition][i];
        }

        public object this[string name] => resultSet.Rows[currentPosition][name];

        object System.Data.IDataRecord.this[int i] => resultSet.Rows[currentPosition][i];

        public object GetValue(int i)
        {
            return resultSet.Rows[currentPosition][i];
        }

        public bool IsDBNull(int i)
        {
            return ((resultSet.Rows[currentPosition][i] == DBNull.Value) ||
              (resultSet.Rows[currentPosition][i] == null));
        }

        public long
          GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotSupportedException();
        }

        public byte GetByte(int i)
        {
            return (byte)resultSet.Rows[currentPosition][i];
        }

        public Type GetFieldType(int i)
        {
            return resultSet.Rows[currentPosition][i].GetType();
        }

        public decimal GetDecimal(int i)
        {
            return (decimal)resultSet.Rows[currentPosition][i];
        }

        public int GetValues(object[] values)
        {
            throw new NotSupportedException();
        }

        public string GetName(int i)
        {
            return resultSet.Columns[i].ColumnName;
        }

        public int FieldCount
        {
            get
            {
                return resultSet.Columns.Count;
            }
        }

        public long GetInt64(int i)
        {
            return (long)resultSet.Rows[currentPosition][i];
        }

        public double GetDouble(int i)
        {
            return (double)resultSet.Rows[currentPosition][i];
        }

        public bool GetBoolean(int i)
        {
            return (bool)resultSet.Rows[currentPosition][i];
        }

        public Guid GetGuid(int i)
        {
            return (Guid)resultSet.Rows[currentPosition][i];
        }

        public DateTime GetDateTime(int i)
        {
            return (DateTime)resultSet.Rows[currentPosition][i];
        }

        public int GetOrdinal(string name)
        {
            return resultSet.Columns.IndexOf(name);
        }

        public string GetDataTypeName(int i)
        {
            return resultSet.Rows[currentPosition][i].GetType().ToString();
        }

        public float GetFloat(int i)
        {
            return (float)resultSet.Rows[currentPosition][i];
        }

        public IDataReader GetData(int i)
        {
            throw new NotSupportedException();
        }

        public long
          GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotSupportedException();
        }

        public string GetString(int i)
        {
            return (string)resultSet.Rows[currentPosition][i];
        }

        public char GetChar(int i)
        {
            return (char)resultSet.Rows[currentPosition][i];
        }

        public short GetInt16(int i)
        {
            return (short)resultSet.Rows[currentPosition][i];
        }

        #endregion
    }

}
