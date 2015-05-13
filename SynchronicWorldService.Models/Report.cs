using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SynchronicWorldService.Models
{
    [DataContract]
    public class Report
    {
        public Report()
        {
            ErrorList = new List<string>();
            WarningList = new List<string>();
            InfoList = new List<string>();
            ExceptionList = new List<string>();
        }

        [DataMember]
        public List<string> ErrorList { get; set; }
        [DataMember]
        public List<string> WarningList { get; set; }
        [DataMember]
        public List<string> InfoList { get; set; }
        [DataMember]
        public List<string> ExceptionList { get; set; }

        public int GetNumberOfErrors()
        {
            return ErrorList.Count;
        }

        public void LogException(Exception e)
        {
            ExceptionList.Add(e.Source);
            ExceptionList.Add(e.Message);
            ExceptionList.Add(e.StackTrace);
            while (e.InnerException != null)
            {
                e = e.InnerException;
                ExceptionList.Add(e.Source);
                ExceptionList.Add(e.Message);
                ExceptionList.Add(e.StackTrace);
            }
        }
    }
}
