using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SynchronicWorldService.Models
{
    [DataContract]
    public class EventStatus
    {
        public EventStatus() { }

        public EventStatus(DataAccess.EventStatus eventStatus)
        {
            this.Id = eventStatus.Id;
            this.Code = eventStatus.Code;
            this.Value = eventStatus.Value;
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
    public enum EventStatusCode
    {
        Open,
        Closed,
        Pending
    }
}
