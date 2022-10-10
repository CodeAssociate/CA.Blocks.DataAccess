using System.Data;
using BenchmarkDotNet.Attributes;
using CA.Blocks.DataAccess;

namespace CA.Blocks.SQLServerDataAccessBenchmarks.Benchmarks.DataReaderExtensions
{
    [MemoryDiagnoser]
    public class AsStringBenchmarks
    {
        private DataTable GenerateTestData(int count)
        {
            return GenerateTestData(1, count);
        }

        protected DataTable GenerateTestData(int start, int count)
        {
            var testData = new DataTable();
            testData.Columns.Add("IntCol", typeof(int));
            testData.Columns.Add("StringCol", typeof(string));
            testData.Columns.Add("GuidCol", typeof(Guid));
            testData.Columns.Add("DateCol", typeof(DateTime));
            testData.AcceptChanges();
            for (var i = start; i <= (start + count - 1); i++)
            {
                testData.Rows.Add(i, $"row#{i}", Guid.NewGuid(), DateTime.Now.AddMinutes(i));
            }
            testData.AcceptChanges();
            return testData;
        }
        protected IDataReader GenerateTestSet(int count)
        {
            return GenerateTestData(1).CreateDataReader();
        }



        private IDataReader? _target;
        [GlobalSetup]
        public void GlobalSetup()
        {
            _target = GenerateTestSet(1);
            _target.Read();
        }

      
        //[IterationSetup]
        //public void IterationSetup()
        //{
            
        //}


        [Benchmark(Baseline = true)]
        public string AsStringBaseline()
        {
            return _target.AsString("StringCol");
        }

        [Benchmark]
        public string AsToStringBaseline()
        {
            return _target.AsToString("StringCol");
        }

        [Benchmark]
        public string AsToStringIndex()
        {
            return _target.AsToString(1);
        }

        //[IterationCleanup]
        //public void IterationCleanup()
        //{

        //    // Disposing logic
        //}

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            _target!.Dispose();
            // Disposing logic
        }


    }
}
