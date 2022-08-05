using System;
using System.Data;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.TestObjects
{
    public class TestSysobjects
    {
        public string name { get; set; }
        public int id { get; set; }
        public string xtype { get; set; }
        public DateTime crdate { get; set; }
    }

    public class CustomTestSysobjects : TestSysobjects
    {
        public DateTime CrazyNameForRefDate { get; set; }
    }


    public class TestSysobjectsTranslator : SimpleDbRow2ObjectTranslator<TestSysobjects>
    {
        public static TestSysobjectsTranslator CurrentInstance = new TestSysobjectsTranslator();

        protected override TestSysobjects CustomTranslate(DataRow dr)
        {
            var result = new TestSysobjects();
            result.id = dr.AsInt("id");
            result.name = dr.AsString("name");
            result.xtype = dr.AsString("xtype");
            result.crdate = dr.AsDateTime("crdate");
            return result;
        }
    }



    public class TestSysobjectsReaderTranslator : SimpleDbReader2ObjectTranslator<TestSysobjects>
    {
        public static TestSysobjectsReaderTranslator CurrentInstance = new TestSysobjectsReaderTranslator();

        protected override TestSysobjects CustomTranslate(IDataReader dr)
        {
            var result = new TestSysobjects();

            result.id = dr.AsInt("id");
            result.name = dr.AsString("name");
            result.xtype = dr.AsString("xtype");
            result.crdate = dr.AsDateTime("crdate");
            return result;
        }
    }

    public class TestSysobjectsOrginalReaderTranslator : SimpleDbReader2ObjectTranslator<TestSysobjects>
    {
        public static TestSysobjectsOrginalReaderTranslator CurrentInstance = new TestSysobjectsOrginalReaderTranslator();

        protected override TestSysobjects CustomTranslate(IDataReader dr)
        {
            var result = new TestSysobjects();

            result.id = dr.AsInt(0);
            result.name = dr.AsString(1);
            result.xtype = dr.AsString(2);
            result.crdate = dr.AsDateTime(3);
            return result;
        }
    }


#pragma warning disable CS0618 // Type or member is obsolete setill need to test code that is marked for obsolete
    public class CustomTestSysobjectsTranslator : BaseDb2ObjectTranslator<CustomTestSysobjects>
#pragma warning restore CS0618 // Type or member is obsolete
    {
        public CustomTestSysobjectsTranslator()
        {
            _mappings.RemoveByName("CrazyNameForRefDate");
            _mappings.Add(CreateDatabaseToObjectMapping(typeof(DateTime).FullName, "CrazyNameForRefDate" , "refdate", false));
        }
    }
}
