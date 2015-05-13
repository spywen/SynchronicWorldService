using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SynchronicWorldService.Models
{
    [DataContract]
    public class EventType
    {
        public EventType() { }

        public EventType(DataAccess.EventType eventType)
        {
            this.Id = eventType.Id;
            this.Code = eventType.Code;
            this.Value = eventType.Value;
        }

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
    public enum EventTypeCode
    {
        Party,
        Lunch
    }
}
