using System.Runtime.Serialization;

namespace SynchronicWorldService.Models
{
    [DataContract]
    public class EventStatus
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Value { get; set; }
    }

    /// <summary>
    /// Event Status code
    /// </summary>
    public enum EventStatusCode
    {
        Open,
        Closed,
        Pending
    }
}
