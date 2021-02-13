using System;
using System.Data;
using System.Text;
using System.Threading;
using CA.Blocks.DataAccess.DI;
using CA.Blocks.MySQLDataAccess;
using CA.Blocks.MySQLDataAccessUnitTests.Base;
using NUnit.Framework;


namespace CA.Blocks.SQLServerDataAccessUnitTests.Samples.ReadingData
{
    [TestFixture]
    public class ReadDataTable
    {

        public class ReadDataTableDataAccess : MySqlDataAccess
        {
            public ReadDataTableDataAccess() : base(
                new DataAccessConfig("UnitTestDataAccess",
                    new DataAccessConfigOptions { ConnectionStringKey = "notused" },
                    new MySQLTestDataAccessKeyToConnectionStringResolver())
            )
            {
            }

            public DataTable GetInformationSchema()
            {
                var cmd = CreateTextCommand("select *  from information_schema.tables");
                return base.ExecuteDataTable(cmd);
            }

        }

       

        protected string DataTableToText(DataTable dt)
        {
            var maxLengths = new int[dt.Columns.Count];

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                maxLengths[i] = dt.Columns[i].ColumnName.Length;

                foreach (DataRow row in dt.Rows)
                {
                    if (!row.IsNull(i))
                    {
                        int length = row[i].ToString().Length;

                        if (length > maxLengths[i])
                        {
                            maxLengths[i] = length;
                        }
                    }
                }
            }

            var sb = new StringBuilder();
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sb.Append(dt.Columns[i].ColumnName.PadRight(maxLengths[i] + 2));
                }

                sb.AppendLine();

                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        sb.Append(!row.IsNull(i)
                            ? row[i].ToString().PadRight(maxLengths[i] + 2)
                            : new string(' ', maxLengths[i] + 2));
                    }

                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }

        [Test]
        public void GetGetInformationSchema()
        {
            var target = new ReadDataTableDataAccess();
            var executeResult = target.GetInformationSchema();
            TestContext.WriteLine(DataTableToText(executeResult));

        }

    }
}