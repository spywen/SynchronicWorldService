using System.Runtime.Serialization;

namespace SynchronicWorldService.Models
{
    [DataContract]
    public class Event
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public System.DateTime Date { get; set; }
        [DataMember]
        public EventType Type { get; set; }
        [DataMember]
        public EventStatus Status { get; set; }
    }
}
