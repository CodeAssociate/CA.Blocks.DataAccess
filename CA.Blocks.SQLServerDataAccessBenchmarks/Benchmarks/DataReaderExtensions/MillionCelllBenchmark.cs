#pragma warning disable CS8618

using System.Data;
using System.Data.Common;
using BenchmarkDotNet.Attributes;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.Extensions;

namespace CA.Blocks.SQLServerDataAccessBenchmarks.Benchmarks.DataReaderExtensions;

/// <summary>
/// This tests ony the Translators, use using an in memory data table, So there zero blocking IO. 
/// </summary>
[MemoryDiagnoser]
public class MillionCellBenchmark
{

    /*
     BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
Intel Core i7-8550U CPU 1.80GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.303
  [Host]     : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT
  Job-UXQIKO : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT

InvocationCount=1  UnrollFactor=1

|                            Method |      Mean |    Error |   StdDev | Ratio |     Gen 0 | Allocated |
|---------------------------------- |----------:|---------:|---------:|------:|----------:|----------:|
|                      ToListOfSync | 291.64 ms | 3.336 ms | 2.957 ms |  1.00 | 1000.0000 |      9 MB |
| ToListOfSyncCustomTranslateByName |  94.78 ms | 1.894 ms | 3.318 ms |  0.33 | 1000.0000 |      8 MB |
|   ToListOfSyncCustomTranslateById |  56.02 ms | 0.901 ms | 0.752 ms |  0.19 | 1000.0000 |      8 MB |


    with 1 millsion cells using auto <ToListOfSync> takes +-291ms
    custom translator by name +-94 ms
    custom translator by index  +-56 ms
*/

    protected class TestDataObject
    {
        public int Id { get; set; }
        public string StringCol1 { get; set; }
        public string StringCol2 { get; set; }
        public string StringCol3 { get; set; }
        public string StringCol4 { get; set; }
        public string StringCol5 { get; set; }
        public string StringCol6 { get; set; }
        public string StringCol7 { get; set; }
        public string StringCol8 { get; set; }
        public string StringCol9 { get; set; }
        public string StringCol10 { get; set; }
        public string StringCol11 { get; set; }
        public string StringCol12 { get; set; }
        public string StringCol13 { get; set; }
        public string StringCol14 { get; set; }
        public string StringCol15 { get; set; }
        public string StringCol16 { get; set; }
        public string StringCol17 { get; set; }
        public string StringCol18 { get; set; }
        public string StringCol19 { get; set; }
        public string StringCol20 { get; set; }
        public string StringCol21 { get; set; }
        public string StringCol22 { get; set; }
        public string StringCol23 { get; set; }
        public string StringCol24 { get; set; }
        public string StringCol25 { get; set; }
        public string StringCol26 { get; set; }
        public string StringCol27 { get; set; }
        public string StringCol28 { get; set; }
        public string StringCol29 { get; set; }
        public string StringCol30 { get; set; }
        public string StringCol31 { get; set; }
        public string StringCol32 { get; set; }
        public string StringCol33 { get; set; }
        public string StringCol34 { get; set; }
        public string StringCol35 { get; set; }
        public string StringCol36 { get; set; }
        public string StringCol37 { get; set; }
        public string StringCol38 { get; set; }
        public string StringCol39 { get; set; }
        public string StringCol40 { get; set; }
        public string StringCol41 { get; set; }
        public string StringCol42 { get; set; }
        public string StringCol43 { get; set; }
        public string StringCol44 { get; set; }
        public string StringCol45 { get; set; }
        public string StringCol46 { get; set; }
        public string StringCol47 { get; set; }
        public string StringCol48 { get; set; }
        public string StringCol49 { get; set; }
        public string StringCol50 { get; set; }
        public string StringCol51 { get; set; }
        public string StringCol52 { get; set; }
        public string StringCol53 { get; set; }
        public string StringCol54 { get; set; }
        public string StringCol55 { get; set; }
        public string StringCol56 { get; set; }
        public string StringCol57 { get; set; }
        public string StringCol58 { get; set; }
        public string StringCol59 { get; set; }
        public string StringCol60 { get; set; }
        public string StringCol61 { get; set; }
        public string StringCol62 { get; set; }
        public string StringCol63 { get; set; }
        public string StringCol64 { get; set; }
        public string StringCol65 { get; set; }
        public string StringCol66 { get; set; }
        public string StringCol67 { get; set; }
        public string StringCol68 { get; set; }
        public string StringCol69 { get; set; }
        public string StringCol70 { get; set; }
        public string StringCol71 { get; set; }
        public string StringCol72 { get; set; }
        public string StringCol73 { get; set; }
        public string StringCol74 { get; set; }
        public string StringCol75 { get; set; }
        public string StringCol76 { get; set; }
        public string StringCol77 { get; set; }
        public string StringCol78 { get; set; }
        public string StringCol79 { get; set; }
        public string StringCol80 { get; set; }
        public string StringCol81 { get; set; }
        public string StringCol82 { get; set; }
        public string StringCol83 { get; set; }
        public string StringCol84 { get; set; }
        public string StringCol85 { get; set; }
        public string StringCol86 { get; set; }
        public string StringCol87 { get; set; }
        public string StringCol88 { get; set; }
        public string StringCol89 { get; set; }
        public string StringCol90 { get; set; }
        public string StringCol91 { get; set; }
        public string StringCol92 { get; set; }
        public string StringCol93 { get; set; }
        public string StringCol94 { get; set; }
        public string StringCol95 { get; set; }
        public string StringCol96 { get; set; }
        public string StringCol97 { get; set; }
        public string StringCol98 { get; set; }
        public string StringCol99 { get; set; }
    }

    protected DataTable GenerateTestData()
    {
        var testData = new DataTable();
        testData.Columns.Add("Id", typeof(int));
        testData.Columns.Add("StringCol1", typeof(string));
        testData.Columns.Add("StringCol2", typeof(string));
        testData.Columns.Add("StringCol3", typeof(string));
        testData.Columns.Add("StringCol4", typeof(string));
        testData.Columns.Add("StringCol5", typeof(string));
        testData.Columns.Add("StringCol6", typeof(string));
        testData.Columns.Add("StringCol7", typeof(string));
        testData.Columns.Add("StringCol8", typeof(string));
        testData.Columns.Add("StringCol9", typeof(string));
        testData.Columns.Add("StringCol10", typeof(string));
        testData.Columns.Add("StringCol11", typeof(string));
        testData.Columns.Add("StringCol12", typeof(string));
        testData.Columns.Add("StringCol13", typeof(string));
        testData.Columns.Add("StringCol14", typeof(string));
        testData.Columns.Add("StringCol15", typeof(string));
        testData.Columns.Add("StringCol16", typeof(string));
        testData.Columns.Add("StringCol17", typeof(string));
        testData.Columns.Add("StringCol18", typeof(string));
        testData.Columns.Add("StringCol19", typeof(string));
        testData.Columns.Add("StringCol20", typeof(string));
        testData.Columns.Add("StringCol21", typeof(string));
        testData.Columns.Add("StringCol22", typeof(string));
        testData.Columns.Add("StringCol23", typeof(string));
        testData.Columns.Add("StringCol24", typeof(string));
        testData.Columns.Add("StringCol25", typeof(string));
        testData.Columns.Add("StringCol26", typeof(string));
        testData.Columns.Add("StringCol27", typeof(string));
        testData.Columns.Add("StringCol28", typeof(string));
        testData.Columns.Add("StringCol29", typeof(string));
        testData.Columns.Add("StringCol30", typeof(string));
        testData.Columns.Add("StringCol31", typeof(string));
        testData.Columns.Add("StringCol32", typeof(string));
        testData.Columns.Add("StringCol33", typeof(string));
        testData.Columns.Add("StringCol34", typeof(string));
        testData.Columns.Add("StringCol35", typeof(string));
        testData.Columns.Add("StringCol36", typeof(string));
        testData.Columns.Add("StringCol37", typeof(string));
        testData.Columns.Add("StringCol38", typeof(string));
        testData.Columns.Add("StringCol39", typeof(string));
        testData.Columns.Add("StringCol40", typeof(string));
        testData.Columns.Add("StringCol41", typeof(string));
        testData.Columns.Add("StringCol42", typeof(string));
        testData.Columns.Add("StringCol43", typeof(string));
        testData.Columns.Add("StringCol44", typeof(string));
        testData.Columns.Add("StringCol45", typeof(string));
        testData.Columns.Add("StringCol46", typeof(string));
        testData.Columns.Add("StringCol47", typeof(string));
        testData.Columns.Add("StringCol48", typeof(string));
        testData.Columns.Add("StringCol49", typeof(string));
        testData.Columns.Add("StringCol50", typeof(string));
        testData.Columns.Add("StringCol51", typeof(string));
        testData.Columns.Add("StringCol52", typeof(string));
        testData.Columns.Add("StringCol53", typeof(string));
        testData.Columns.Add("StringCol54", typeof(string));
        testData.Columns.Add("StringCol55", typeof(string));
        testData.Columns.Add("StringCol56", typeof(string));
        testData.Columns.Add("StringCol57", typeof(string));
        testData.Columns.Add("StringCol58", typeof(string));
        testData.Columns.Add("StringCol59", typeof(string));
        testData.Columns.Add("StringCol60", typeof(string));
        testData.Columns.Add("StringCol61", typeof(string));
        testData.Columns.Add("StringCol62", typeof(string));
        testData.Columns.Add("StringCol63", typeof(string));
        testData.Columns.Add("StringCol64", typeof(string));
        testData.Columns.Add("StringCol65", typeof(string));
        testData.Columns.Add("StringCol66", typeof(string));
        testData.Columns.Add("StringCol67", typeof(string));
        testData.Columns.Add("StringCol68", typeof(string));
        testData.Columns.Add("StringCol69", typeof(string));
        testData.Columns.Add("StringCol70", typeof(string));
        testData.Columns.Add("StringCol71", typeof(string));
        testData.Columns.Add("StringCol72", typeof(string));
        testData.Columns.Add("StringCol73", typeof(string));
        testData.Columns.Add("StringCol74", typeof(string));
        testData.Columns.Add("StringCol75", typeof(string));
        testData.Columns.Add("StringCol76", typeof(string));
        testData.Columns.Add("StringCol77", typeof(string));
        testData.Columns.Add("StringCol78", typeof(string));
        testData.Columns.Add("StringCol79", typeof(string));
        testData.Columns.Add("StringCol80", typeof(string));
        testData.Columns.Add("StringCol81", typeof(string));
        testData.Columns.Add("StringCol82", typeof(string));
        testData.Columns.Add("StringCol83", typeof(string));
        testData.Columns.Add("StringCol84", typeof(string));
        testData.Columns.Add("StringCol85", typeof(string));
        testData.Columns.Add("StringCol86", typeof(string));
        testData.Columns.Add("StringCol87", typeof(string));
        testData.Columns.Add("StringCol88", typeof(string));
        testData.Columns.Add("StringCol89", typeof(string));
        testData.Columns.Add("StringCol90", typeof(string));
        testData.Columns.Add("StringCol91", typeof(string));
        testData.Columns.Add("StringCol92", typeof(string));
        testData.Columns.Add("StringCol93", typeof(string));
        testData.Columns.Add("StringCol94", typeof(string));
        testData.Columns.Add("StringCol95", typeof(string));
        testData.Columns.Add("StringCol96", typeof(string));
        testData.Columns.Add("StringCol97", typeof(string));
        testData.Columns.Add("StringCol98", typeof(string));
        testData.Columns.Add("StringCol99", typeof(string));
        testData.AcceptChanges();
        for (var i = 1; i <= 10000; i++)
        {
            testData.Rows.Add(i,
                $"row#{i}_cell1",
                $"row#{i}_cell2",
                $"row#{i}_cell3",
                $"row#{i}_cell4",
                $"row#{i}_cell5",
                $"row#{i}_cell6",
                $"row#{i}_cell7",
                $"row#{i}_cell8",
                $"row#{i}_cell9",
                $"row#{i}_cell10",
                $"row#{i}_cell11",
                $"row#{i}_cell12",
                $"row#{i}_cell13",
                $"row#{i}_cell14",
                $"row#{i}_cell15",
                $"row#{i}_cell16",
                $"row#{i}_cell17",
                $"row#{i}_cell18",
                $"row#{i}_cell19",
                $"row#{i}_cell20",
                $"row#{i}_cell21",
                $"row#{i}_cell22",
                $"row#{i}_cell23",
                $"row#{i}_cell24",
                $"row#{i}_cell25",
                $"row#{i}_cell26",
                $"row#{i}_cell27",
                $"row#{i}_cell28",
                $"row#{i}_cell29",
                $"row#{i}_cell30",
                $"row#{i}_cell31",
                $"row#{i}_cell32",
                $"row#{i}_cell33",
                $"row#{i}_cell34",
                $"row#{i}_cell35",
                $"row#{i}_cell36",
                $"row#{i}_cell37",
                $"row#{i}_cell38",
                $"row#{i}_cell39",
                $"row#{i}_cell40",
                $"row#{i}_cell41",
                $"row#{i}_cell42",
                $"row#{i}_cell43",
                $"row#{i}_cell44",
                $"row#{i}_cell45",
                $"row#{i}_cell46",
                $"row#{i}_cell47",
                $"row#{i}_cell48",
                $"row#{i}_cell49",
                $"row#{i}_cell50",
                $"row#{i}_cell51",
                $"row#{i}_cell52",
                $"row#{i}_cell53",
                $"row#{i}_cell54",
                $"row#{i}_cell55",
                $"row#{i}_cell56",
                $"row#{i}_cell57",
                $"row#{i}_cell58",
                $"row#{i}_cell59",
                $"row#{i}_cell60",
                $"row#{i}_cell61",
                $"row#{i}_cell62",
                $"row#{i}_cell63",
                $"row#{i}_cell64",
                $"row#{i}_cell65",
                $"row#{i}_cell66",
                $"row#{i}_cell67",
                $"row#{i}_cell68",
                $"row#{i}_cell69",
                $"row#{i}_cell70",
                $"row#{i}_cell71",
                $"row#{i}_cell72",
                $"row#{i}_cell73",
                $"row#{i}_cell74",
                $"row#{i}_cell75",
                $"row#{i}_cell76",
                $"row#{i}_cell77",
                $"row#{i}_cell78",
                $"row#{i}_cell79",
                $"row#{i}_cell80",
                $"row#{i}_cell81",
                $"row#{i}_cell82",
                $"row#{i}_cell83",
                $"row#{i}_cell84",
                $"row#{i}_cell85",
                $"row#{i}_cell86",
                $"row#{i}_cell87",
                $"row#{i}_cell88",
                $"row#{i}_cell89",
                $"row#{i}_cell90",
                $"row#{i}_cell91",
                $"row#{i}_cell92",
                $"row#{i}_cell93",
                $"row#{i}_cell94",
                $"row#{i}_cell95",
                $"row#{i}_cell96",
                $"row#{i}_cell97",
                $"row#{i}_cell98",
                $"row#{i}_cell99");
        }
        testData.AcceptChanges();
        return testData;
    }
    protected IDataReader GenerateTestSet()
    {
        return GenerateTestData().CreateDataReader();
    }

    protected async Task<DbDataReader> GenerateTestDataReaderAsync(int count)
    {
        await Task.Delay(1);
        return GenerateTestData().CreateDataReader();
    }


    private TestDataObject CustomTranslateByName(IDataReader dr)
    {
        var result = new TestDataObject
        {
            Id = dr.AsInt("Id"),

            StringCol1 = dr.AsString("StringCol1"),
            StringCol2 = dr.AsString("StringCol2"),
            StringCol3 = dr.AsString("StringCol3"),
            StringCol4 = dr.AsString("StringCol4"),
            StringCol5 = dr.AsString("StringCol5"),
            StringCol6 = dr.AsString("StringCol6"),
            StringCol7 = dr.AsString("StringCol7"),
            StringCol8 = dr.AsString("StringCol8"),
            StringCol9 = dr.AsString("StringCol9"),
            StringCol10 = dr.AsString("StringCol10"),
            StringCol11 = dr.AsString("StringCol11"),
            StringCol12 = dr.AsString("StringCol12"),
            StringCol13 = dr.AsString("StringCol13"),
            StringCol14 = dr.AsString("StringCol14"),
            StringCol15 = dr.AsString("StringCol15"),
            StringCol16 = dr.AsString("StringCol16"),
            StringCol17 = dr.AsString("StringCol17"),
            StringCol18 = dr.AsString("StringCol18"),
            StringCol19 = dr.AsString("StringCol19"),
            StringCol20 = dr.AsString("StringCol20"),
            StringCol21 = dr.AsString("StringCol21"),
            StringCol22 = dr.AsString("StringCol22"),
            StringCol23 = dr.AsString("StringCol23"),
            StringCol24 = dr.AsString("StringCol24"),
            StringCol25 = dr.AsString("StringCol25"),
            StringCol26 = dr.AsString("StringCol26"),
            StringCol27 = dr.AsString("StringCol27"),
            StringCol28 = dr.AsString("StringCol28"),
            StringCol29 = dr.AsString("StringCol29"),
            StringCol30 = dr.AsString("StringCol30"),
            StringCol31 = dr.AsString("StringCol31"),
            StringCol32 = dr.AsString("StringCol32"),
            StringCol33 = dr.AsString("StringCol33"),
            StringCol34 = dr.AsString("StringCol34"),
            StringCol35 = dr.AsString("StringCol35"),
            StringCol36 = dr.AsString("StringCol36"),
            StringCol37 = dr.AsString("StringCol37"),
            StringCol38 = dr.AsString("StringCol38"),
            StringCol39 = dr.AsString("StringCol39"),
            StringCol40 = dr.AsString("StringCol40"),
            StringCol41 = dr.AsString("StringCol41"),
            StringCol42 = dr.AsString("StringCol42"),
            StringCol43 = dr.AsString("StringCol43"),
            StringCol44 = dr.AsString("StringCol44"),
            StringCol45 = dr.AsString("StringCol45"),
            StringCol46 = dr.AsString("StringCol46"),
            StringCol47 = dr.AsString("StringCol47"),
            StringCol48 = dr.AsString("StringCol48"),
            StringCol49 = dr.AsString("StringCol49"),
            StringCol50 = dr.AsString("StringCol50"),
            StringCol51 = dr.AsString("StringCol51"),
            StringCol52 = dr.AsString("StringCol52"),
            StringCol53 = dr.AsString("StringCol53"),
            StringCol54 = dr.AsString("StringCol54"),
            StringCol55 = dr.AsString("StringCol55"),
            StringCol56 = dr.AsString("StringCol56"),
            StringCol57 = dr.AsString("StringCol57"),
            StringCol58 = dr.AsString("StringCol58"),
            StringCol59 = dr.AsString("StringCol59"),
            StringCol60 = dr.AsString("StringCol60"),
            StringCol61 = dr.AsString("StringCol61"),
            StringCol62 = dr.AsString("StringCol62"),
            StringCol63 = dr.AsString("StringCol63"),
            StringCol64 = dr.AsString("StringCol64"),
            StringCol65 = dr.AsString("StringCol65"),
            StringCol66 = dr.AsString("StringCol66"),
            StringCol67 = dr.AsString("StringCol67"),
            StringCol68 = dr.AsString("StringCol68"),
            StringCol69 = dr.AsString("StringCol69"),
            StringCol70 = dr.AsString("StringCol70"),
            StringCol71 = dr.AsString("StringCol71"),
            StringCol72 = dr.AsString("StringCol72"),
            StringCol73 = dr.AsString("StringCol73"),
            StringCol74 = dr.AsString("StringCol74"),
            StringCol75 = dr.AsString("StringCol75"),
            StringCol76 = dr.AsString("StringCol76"),
            StringCol77 = dr.AsString("StringCol77"),
            StringCol78 = dr.AsString("StringCol78"),
            StringCol79 = dr.AsString("StringCol79"),
            StringCol80 = dr.AsString("StringCol80"),
            StringCol81 = dr.AsString("StringCol81"),
            StringCol82 = dr.AsString("StringCol82"),
            StringCol83 = dr.AsString("StringCol83"),
            StringCol84 = dr.AsString("StringCol84"),
            StringCol85 = dr.AsString("StringCol85"),
            StringCol86 = dr.AsString("StringCol86"),
            StringCol87 = dr.AsString("StringCol87"),
            StringCol88 = dr.AsString("StringCol88"),
            StringCol89 = dr.AsString("StringCol89"),
            StringCol90 = dr.AsString("StringCol90"),
            StringCol91 = dr.AsString("StringCol91"),
            StringCol92 = dr.AsString("StringCol92"),
            StringCol93 = dr.AsString("StringCol93"),
            StringCol94 = dr.AsString("StringCol94"),
            StringCol95 = dr.AsString("StringCol95"),
            StringCol96 = dr.AsString("StringCol96"),
            StringCol97 = dr.AsString("StringCol97"),
            StringCol98 = dr.AsString("StringCol98"),
            StringCol99 = dr.AsString("StringCol99")
        };
        return result;
    }

    private TestDataObject CustomTranslateById(IDataReader dr)
    {
        var result = new TestDataObject
        {
            Id = dr.AsInt("Id"),
            StringCol1 = dr.AsString(1),
            StringCol2 = dr.AsString(2),
            StringCol3 = dr.AsString(3),
            StringCol4 = dr.AsString(4),
            StringCol5 = dr.AsString(5),
            StringCol6 = dr.AsString(6),
            StringCol7 = dr.AsString(7),
            StringCol8 = dr.AsString(8),
            StringCol9 = dr.AsString(9),
            StringCol10 = dr.AsString(10),
            StringCol11 = dr.AsString(11),
            StringCol12 = dr.AsString(12),
            StringCol13 = dr.AsString(13),
            StringCol14 = dr.AsString(14),
            StringCol15 = dr.AsString(15),
            StringCol16 = dr.AsString(16),
            StringCol17 = dr.AsString(17),
            StringCol18 = dr.AsString(18),
            StringCol19 = dr.AsString(19),
            StringCol20 = dr.AsString(20),
            StringCol21 = dr.AsString(21),
            StringCol22 = dr.AsString(22),
            StringCol23 = dr.AsString(23),
            StringCol24 = dr.AsString(24),
            StringCol25 = dr.AsString(25),
            StringCol26 = dr.AsString(26),
            StringCol27 = dr.AsString(27),
            StringCol28 = dr.AsString(28),
            StringCol29 = dr.AsString(29),
            StringCol30 = dr.AsString(30),
            StringCol31 = dr.AsString(31),
            StringCol32 = dr.AsString(32),
            StringCol33 = dr.AsString(33),
            StringCol34 = dr.AsString(34),
            StringCol35 = dr.AsString(35),
            StringCol36 = dr.AsString(36),
            StringCol37 = dr.AsString(37),
            StringCol38 = dr.AsString(38),
            StringCol39 = dr.AsString(39),
            StringCol40 = dr.AsString(40),
            StringCol41 = dr.AsString(41),
            StringCol42 = dr.AsString(42),
            StringCol43 = dr.AsString(43),
            StringCol44 = dr.AsString(44),
            StringCol45 = dr.AsString(45),
            StringCol46 = dr.AsString(46),
            StringCol47 = dr.AsString(47),
            StringCol48 = dr.AsString(48),
            StringCol49 = dr.AsString(49),
            StringCol50 = dr.AsString(50),
            StringCol51 = dr.AsString(51),
            StringCol52 = dr.AsString(52),
            StringCol53 = dr.AsString(53),
            StringCol54 = dr.AsString(54),
            StringCol55 = dr.AsString(55),
            StringCol56 = dr.AsString(56),
            StringCol57 = dr.AsString(57),
            StringCol58 = dr.AsString(58),
            StringCol59 = dr.AsString(59),
            StringCol60 = dr.AsString(60),
            StringCol61 = dr.AsString(61),
            StringCol62 = dr.AsString(62),
            StringCol63 = dr.AsString(63),
            StringCol64 = dr.AsString(64),
            StringCol65 = dr.AsString(65),
            StringCol66 = dr.AsString(66),
            StringCol67 = dr.AsString(67),
            StringCol68 = dr.AsString(68),
            StringCol69 = dr.AsString(69),
            StringCol70 = dr.AsString(70),
            StringCol71 = dr.AsString(71),
            StringCol72 = dr.AsString(72),
            StringCol73 = dr.AsString(73),
            StringCol74 = dr.AsString(74),
            StringCol75 = dr.AsString(75),
            StringCol76 = dr.AsString(76),
            StringCol77 = dr.AsString(77),
            StringCol78 = dr.AsString(78),
            StringCol79 = dr.AsString(79),
            StringCol80 = dr.AsString(80),
            StringCol81 = dr.AsString(81),
            StringCol82 = dr.AsString(82),
            StringCol83 = dr.AsString(83),
            StringCol84 = dr.AsString(84),
            StringCol85 = dr.AsString(85),
            StringCol86 = dr.AsString(86),
            StringCol87 = dr.AsString(87),
            StringCol88 = dr.AsString(88),
            StringCol89 = dr.AsString(89),
            StringCol90 = dr.AsString(90),
            StringCol91 = dr.AsString(91),
            StringCol92 = dr.AsString(92),
            StringCol93 = dr.AsString(93),
            StringCol94 = dr.AsString(94),
            StringCol95 = dr.AsString(95),
            StringCol96 = dr.AsString(96),
            StringCol97 = dr.AsString(97),
            StringCol98 = dr.AsString(98),
            StringCol99 = dr.AsString(99)
        };
        return result;
    }

    private IDataReader _target;

    [IterationSetup(Targets = new[]
        { nameof(ToListOfSync), nameof(ToListOfSyncCustomTranslateByName), nameof(ToListOfSyncCustomTranslateById) })]
    public void IterationSetup()
    {
        _target = GenerateTestSet();
    }

    
    [Benchmark(Baseline = true)]
    public int ToListOfSync()
    {
        //var syncTarget = GenerateTestSet(10);
        var result = _target.ToListOf<TestDataObject>();
        if (result[50].StringCol21 != "row#51_cell21")
            throw new ApplicationException($"Error Expected 'row#51_cell21' But was {result[50].StringCol21}");
        return result.Count;
    }

    [Benchmark]
    public int ToListOfSyncCustomTranslateByName()
    {
        //var syncTarget = GenerateTestSet(10);
        var result = _target.ToListOf<TestDataObject>(CustomTranslateByName);
        if (result[50].StringCol21 != "row#51_cell21")
            throw new ApplicationException($"Error Expected 'row#51_cell21' But was {result[50].StringCol21}");
        return result.Count;
    }

    [Benchmark]
    public int ToListOfSyncCustomTranslateById()
    {
        //var syncTarget = GenerateTestSet(10);
        var result = _target.ToListOf<TestDataObject>(CustomTranslateById);
        if (result[50].StringCol21 != "row#51_cell21")
            throw new ApplicationException($"Error Expected 'row#51_cell21' But was {result[50].StringCol21}");
        return result.Count;
    }

    
    public void IterationCleanup()
    {
        _target = null;
    }
}