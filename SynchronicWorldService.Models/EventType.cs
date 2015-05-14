﻿using System.Runtime.Serialization;

namespace SynchronicWorldService.Models
{
    [DataContract]
    public class EventType
    {
        public EventType() { }

        public EventType(DataAccess.EventType eventType)
        {
            Id = eventType.Id;
            Code = eventType.Code;
            Value = eventType.Value;
        }

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Value { get; set; }

    }

    /// <summary>
    /// Event Type code
    /// </summary>
    public enum EventTypeCode
    {
        Party,
        Lunch
    }
}
