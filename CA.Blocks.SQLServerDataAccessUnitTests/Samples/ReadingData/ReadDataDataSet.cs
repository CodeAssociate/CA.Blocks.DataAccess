using System;
using System.Collections.Generic;
using System.Data;
using CA.Blocks.DataAccess.DI;
using CA.Blocks.SQLServerDataAccess;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Samples.ReadingData
{
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



        [Fact]
        public void ExecuteDataSetDefault()
        {
            var target = new ReadDataDataSet.ExampleReadDataDataSet();
            var executeResult = target.GetRawDateSet();
            Assert.Equal("Results", executeResult.Tables[0].TableName);
            Assert.Equal("Results1", executeResult.Tables[1].TableName);

        }


        [Fact]
        public void ExecuteDataSetWithNamedTables()
        {
            var target = new ReadDataDataSet.ExampleReadDataDataSet();
            var executeResult = target.GetRawDateSetWithNamedTables();
            Assert.Equal("Sysobjects", executeResult.Tables[0].TableName);
            Assert.Equal("SysIndexes", executeResult.Tables[1].TableName);
        }


        [Fact]
        public void ExecuteDataSetIntoObject()
        {
            var target = new ReadDataDataSet.ExampleReadDataDataSet();
            var executeResult = target.GetExampleReturnMultiCollection();
            Assert.NotNull(executeResult);
            Assert.NotNull(executeResult.Sysobjects);
            Assert.Equal(10, executeResult.Sysobjects.Count);
            Assert.NotNull(executeResult.SysIndexes);
            Assert.Equal(5, executeResult.SysIndexes.Count);
        }

    }
}



