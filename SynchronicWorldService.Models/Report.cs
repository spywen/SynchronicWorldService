using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SynchronicWorldService.Utils;

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

        /// <summary>
        /// Get number of errors contain inside ErrorList
        /// </summary>
        /// <returns></returns>
        public int GetNumberOfErrors()
        {
            return ErrorList.Count;
        }

        /// <summary>
        /// Log error
        /// </summary>
        /// <param name="message"></param>
        public void LogError(string message)
        {
            ErrorList.Add(message);
        }

        /// <summary>
        /// Log info
        /// </summary>
        /// <param name="message"></param>
        public void LogInfo(string message)
        {
            InfoList.Add(message);
        }

        /// <summary>
        /// Log exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="e"></param>
        public void LogException(String message, Exception e)
        {
            //1) Send message to the customer
            LogError(SWResources.UnexpectedError);

            //2) Send as well logs of the exception through the WCF service
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

            //3) Log the exception
            LogManager.Logger.Error(message, e);
        }
    }
}
