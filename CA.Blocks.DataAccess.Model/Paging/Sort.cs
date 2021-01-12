using System.Runtime.Serialization;

namespace CA.Blocks.DataAccess.Model.Paging
{

    /// <summary>
    /// Sort Def
    /// </summary>
    [DataContract]
    public class Sort
    {
        /// <summary>
        /// The Name of the Field to Sort
        /// </summary>
        [DataMember]
        public string Field { get; set; }

        /// <summary>
        /// The Direction of the Sort
        /// </summary>
        [DataMember]
        public string Dir { get; set; }
    }
}
