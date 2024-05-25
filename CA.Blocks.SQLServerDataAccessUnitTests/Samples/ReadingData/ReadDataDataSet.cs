using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.WebSockets;
using CA.Blocks.DataAccess.DI;
using NUnit.Framework;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Translator;
using NUnit.Framework.Legacy;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Samples.ReadingData
{
    [TestFixture]
    public class ReadDataDataSet
    {

        public class ExampleReturnMultiCollection
        {
            public IList<ExampleSysObject> Sysobjects { get; set; }

            public IList<ExamplesSysIndex> SysIndexes { get; set; }
        }

        public class ExampleSysObject
        {
            public int id { get; set; }
            public string name { get; set; }
            public DateTime refdate { get; set; }
        }

        public class ExamplesSysIndex
        {
            public int object_id { get; set; }
    
            public string name { get; set; }

            public int index_id { get; set; }

            public string type_desc { get; set; }
        }


        public class ExampleReadDataDataSet : SqlServerDataAccess
        {
            public ExampleReadDataDataSet() : base(
                new DataAccessConfig(new DataAccessConfigOptions { ConnectionStringKey = "notused" },
                    new HardCodedConnectionStringsResolver(TestConnectionStrings.LOCAL_TEMP_DB))
            )
            {

            }


            public DataSet GetRawDateSet()
            {
                var cmd = CreateTextCommand(@"
Select top 10 * from Sysobjects
Select top 5 * from sys.indexes");
                return ExecuteDataSet(cmd);
            }

            public DataSet GetRawDateSetWithNamedTables()
            {
                var cmd = CreateTextCommand(@"
Select top 10 * from Sysobjects
Select top 5 * from sys.indexes");
                DataSet ds = new DataSet();
                return ExecuteDataSet(cmd, ds, "Sysobjects , SysIndexes");
            }


            public ExampleReturnMultiCollection GetExampleReturnMultiCollection()
            {

                var cmd = CreateTextCommand(@"
Select top 10 * from Sysobjects
Select top 5 * from sys.indexes");
                var dataset =  ExecuteDataSet(cmd);


                var result = new ExampleReturnMultiCollection();
                result.Sysobjects = TranslateToListOf<ExampleSysObject>(dataset.Tables[0]);
                result.SysIndexes = TranslateToListOf<ExamplesSysIndex>(dataset.Tables[1]);
                return result;
            }
        }



        [Test]
        public void ExecuteDataSetDefault()
        {
            var target = new ReadDataDataSet.ExampleReadDataDataSet();
            var executeResult = target.GetRawDateSet();
            ClassicAssert.AreEqual("Results", executeResult.Tables[0].TableName);
            ClassicAssert.AreEqual("Results1", executeResult.Tables[1].TableName);

        }


        [Test]
        public void ExecuteDataSetWithNamedTables()
        {
            var target = new ReadDataDataSet.ExampleReadDataDataSet();
            var executeResult = target.GetRawDateSetWithNamedTables();
            ClassicAssert.AreEqual("Sysobjects", executeResult.Tables[0].TableName);
            ClassicAssert.AreEqual("SysIndexes", executeResult.Tables[1].TableName);
        }


        [Test]
        public void ExecuteDataSetIntoObject()
        {
            var target = new ReadDataDataSet.ExampleReadDataDataSet();
            var executeResult = target.GetExampleReturnMultiCollection();
            ClassicAssert.IsNotNull(executeResult);
            ClassicAssert.IsNotNull(executeResult.Sysobjects);
            ClassicAssert.AreEqual(10, executeResult.Sysobjects.Count);
            ClassicAssert.IsNotNull(executeResult.SysIndexes);
            ClassicAssert.AreEqual(5, executeResult.SysIndexes.Count);
        }

    }
}