using System.Runtime.Serialization;

namespace SynchronicWorldService.Models
{
    [DataContract]
    public class Event
    {
        public Event() { }
        public Event(DataAccess.Event eventt){
            this.Id = eventt.Id;
            this.Name = eventt.Name;
            this.Address = eventt.Address;
            this.Description = eventt.Description;
            this.Date = eventt.Date;
            this.Type = new EventType(eventt.EventType);
            this.Status = new EventStatus(eventt.EventStatus);
        }

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
