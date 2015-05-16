using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SynchronicWorldService.Models.Facades
{
    [DataContract]
    public class EventSearchFacade
    {
        /// <summary>
        /// Search by Name
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Search by Date
        /// </summary>
        [DataMember]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Search by start date
        /// </summary>
        [DataMember]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Search by end date
        /// </summary>
        [DataMember]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Search by event status
        /// </summary>
        [DataMember]
        public EventStatusCode? EventStatusCode { get; set; }

        /// <summary>
        /// Search by event type
        /// </summary>
        [DataMember]
        public EventTypeCode? EventTypeCode { get; set; }
    }
}
