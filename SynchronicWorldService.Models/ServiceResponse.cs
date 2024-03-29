﻿using System.Runtime.Serialization;

namespace SynchronicWorldService.Models
{
    [DataContract]
    public class ServiceResponse<T>
    {
        [DataMember]
        public T Result { get; set; }

        [DataMember]
        public Report Report { get; set; }

        public ServiceResponse() {
            Report = new Report();
        }

        public ServiceResponse(T t){
            Result = t;
            Report = new Report();
        }

        public ServiceResponse(T t, Report report)
        {
            Result = t;
            Report = report;
        }

        public void SetResponseAndReport(T t, Report report)
        {
            Result = t;
            Report = report;
        }
    }
}
