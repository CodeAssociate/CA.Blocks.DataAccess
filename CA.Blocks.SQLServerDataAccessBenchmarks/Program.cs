// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using CA.Blocks.SQLServerDataAccessBenchmarks.Benchmarks.DataReaderExtensions;
using CA.Blocks.SQLServerDataAccessBenchmarks.Benchmarks.ReadVrsDapper;
using CA.Blocks.SQLServerDataAccessBenchmarks.Benchmarks.ReadVrsDapper.Read;

// TODO Add choice for choosing benchmark run.

//BenchmarkRunner.Run<AsStringBenchmarks>();
//BenchmarkRunner.Run<AsyncVrsSyncWithAutoNameAndIdBenchmarks>();
//BenchmarkRunner.Run<MillionCellBenchmark>();
//BenchmarkRunner.Run<ReadVrsDapper>();
BenchmarkRunner.Run<ReadVrsDapper>();
Console.ReadLine();