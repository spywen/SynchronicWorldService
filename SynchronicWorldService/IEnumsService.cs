using System.Collections.Generic;
using System.ServiceModel;

namespace SynchronicWorldService
{
    [ServiceContract]
    public interface IEnumsService
    {
        /// <summary>
        /// Get all event status
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        Models.ServiceResponse<List<Models.EventStatus>> GetAllEventsStatus();

        /// <summary>
        /// Find event status by code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [OperationContract]
        Models.ServiceResponse<Models.EventStatus> FindEventStatusByCode(string code);

        /// <summary>
        /// Get all event status
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        Models.ServiceResponse<List<Models.EventType>> GetAllEventsType();

        /// <summary>
        /// Find event status by code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [OperationContract]
        Models.ServiceResponse<Models.EventType> FindEventTypeByCode(string code);
    }
}
