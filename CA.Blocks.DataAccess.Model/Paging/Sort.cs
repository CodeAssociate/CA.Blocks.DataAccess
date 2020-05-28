using System.Runtime.Serialization;

namespace CA.Blocks.DataAccess.Model.Paging
{
    [DataContract]
    public class Sort
    {
        [DataMember]
        public string Field { get; set; }

        [DataMember]
        public string Dir { get; set; }
    }
}
