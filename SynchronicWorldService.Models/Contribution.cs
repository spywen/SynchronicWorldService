using System.Runtime.Serialization;

namespace SynchronicWorldService.Models
{
    [DataContract]
    public class Contribution
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public int FkPerson { get; set; }
        [DataMember]
        public ContributionType Type { get; set; }
        [DataMember]
        public int FkEvent { get; set; }
    }
}
