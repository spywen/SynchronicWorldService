using System.Collections.Generic;
using System.ServiceModel;

namespace SynchronicWorldService
{
    [ServiceContract]
    public interface IEventService
    {
        /// <summary>
        /// Create an event
        /// </summary>
        /// <param name="eventt"></param>
        /// <returns></returns>
        [OperationContract]
        Models.ServiceResponse<Models.Event> CreateEvent(Models.Event eventt);

        /// <summary>
        /// Get an event by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        Models.ServiceResponse<Models.Event> GetEvent(int id);

        /// <summary>
        /// Get all events
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        Models.ServiceResponse<List<Models.Event>> GetAllEvents();

        /// <summary>
        /// Update an event
        /// </summary>
        /// <param name="eventt"></param>
        /// <returns></returns>
        [OperationContract]
        Models.ServiceResponse<Models.Event> UpdateEvent(Models.Event eventt);

        /// <summary>
        /// Delete an event
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        Models.ServiceResponse<bool> DeleteEvent(int id);

        /// <summary>
        /// Search for events according to many parameters
        /// </summary>
        /// <param name="searchObject"></param>
        /// <returns></returns>
        Models.ServiceResponse<List<Models.Event>> SearchForEvents(Models.Facades.EventSearchFacade searchObject);

        /// <summary>
        /// Delete all events closed
        /// </summary>
        /// <returns></returns>
        Models.ServiceResponse<bool> DeleteClosedEvents();

        /// <summary>
        /// Upgrade pending events as open
        /// </summary>
        /// <returns></returns>
        Models.ServiceResponse<bool> UpgradePendingEventsAsOpen();

        /// <summary>
        /// Suscribe a user to an event
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="eventId"></param>
        /// <returns></returns>
        Models.ServiceResponse<bool> SuscribeUserToAnOpenEvent(int userId, int eventId);
    }


    
}
