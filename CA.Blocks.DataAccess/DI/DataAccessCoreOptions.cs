namespace CA.Blocks.DataAccess.DI
{
    public interface IDataAccessConfigOptions
    {
        string ConnectionStringKey {get; set;}
        bool DebugTrace { get; set; }
    }

    public class DataAccessConfigOptions : IDataAccessConfigOptions
    {
        public string ConnectionStringKey {get; set;}
        public bool DebugTrace { get; set; }
    }


}
