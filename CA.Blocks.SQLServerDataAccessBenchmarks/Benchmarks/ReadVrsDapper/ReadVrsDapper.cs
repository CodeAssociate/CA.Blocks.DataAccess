using BenchmarkDotNet.Attributes;
using CA.Blocks.SQLServerDataAccessBenchmarks.Benchmarks.ReadVrsDapper.Read;


namespace CA.Blocks.SQLServerDataAccessBenchmarks.Benchmarks.ReadVrsDapper
{
    [MemoryDiagnoser]
    public  class ReadVrsDapper
    {

        private BlocksReadTest? _blocksTarget;
        private DapperReadTest? _dapperReadTarget;
        [GlobalSetup]
        public void GlobalSetup()
        {
            _blocksTarget = new BlocksReadTest();
            _dapperReadTarget = new DapperReadTest();
        }

        [Benchmark(Baseline = true)]
        public void BlocksReadobjects()
        {
            var result = _blocksTarget!.ReadSysobjects();
        }


        [Benchmark]
        public void BlocksReadobjectsDispose()
        {
	        var result = _blocksTarget!.ReadSysobjectsDispose();
        }

		[Benchmark]
        public async Task BlocksReadobjectsAsync()
        {
	        var result = await _blocksTarget!.ReadSysobjectsAsync();
        }


		[Benchmark()]
        public void BlocksReadobjectsCustom()
        {
            var result = _blocksTarget!.ReadSysobjects2();

        }

        [Benchmark()]
        public void DapperReadobjects()
        {
            var result = _dapperReadTarget!.ReadSysobjects();

        }

        [Benchmark()]
        public async Task DapperReadobjectAsync()
        {
	        var result = await _dapperReadTarget!.ReadSysobjectsAsync();

        }


		[GlobalCleanup]
        public void GlobalCleanup()
        {
            // Disposing logic
        }
    }
}
