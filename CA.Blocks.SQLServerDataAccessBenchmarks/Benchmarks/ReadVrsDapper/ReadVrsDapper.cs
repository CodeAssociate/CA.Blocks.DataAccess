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
        public void ReadSysObjectsSync()
        {
            var result = _blocksTarget!.ReadSysObjectsSync();
        }


        [Benchmark]
        public void ReadSysObjectsSyncDispose()
        {
	        var result = _blocksTarget!.ReadSysObjectsSyncDispose();
        }

        [Benchmark]
        public void ReadSysObjectsSyncWithCustom()
        {
            var result = _blocksTarget!.ReadSysObjectsSyncWithCustom();
        }

        [Benchmark]
        public void ReadSysObjectsSyncWithIndexedCustom()
        {
            var result = _blocksTarget!.ReadSysObjectsSyncWithIndexedCustom();
        }

        [Benchmark()]
        public async Task ReadSysObjectsASync()
        {
            var result = await _blocksTarget!.ReadSysObjectsASync();
        }

        [Benchmark()]
        public async Task ReadSysObjectsASyncWithReaderAsync()
        {
            var result = await _blocksTarget!.ReadSysObjectsASyncWithReaderAsync();
        }

        [Benchmark()]
        public async Task ReadSysObjectsASyncWithCustom()
        {
            var result = await _blocksTarget!.ReadSysObjectsASyncWithCustom();
        }

        [Benchmark()]
        public async Task ReadSysObjectsASyncWithDispose()
        {
            var result = await _blocksTarget!.ReadSysObjectsASyncWithDispose();
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
