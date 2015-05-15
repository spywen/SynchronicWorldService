using System.Runtime.Serialization;

namespace SynchronicWorldService.Models
{
    [DataContract]
    public class Person
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Nickname { get; set; }
    }
}
