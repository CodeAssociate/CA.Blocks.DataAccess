using CA.Blocks.DataAccess.Generators;

namespace CA.Blocks.SQLServerDataAccessBenchmarks.Benchmarks.ReadVrsDapper.Read
{
    // If you use aot optimization you need to tell it not to remove the ctor
    //[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | 
    //                            DynamicallyAccessedMemberTypes.PublicProperties)]
    public class ExampleSysObject
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? xtype { get; set; }
        public DateTime crdate { get; set; }
    }
    
    public class ExampleSysObject2
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? xtype { get; set; }
        public DateTime crdate { get; set; }
    }

    [GenerateTranslatorFor(typeof(ExampleSysObject2), "ExampleSysObject2DbRowTranslator")]
    public static class ExampleSysObjectMappingContext { }
}
