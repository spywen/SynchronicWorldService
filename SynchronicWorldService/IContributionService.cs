using System.Collections.Generic;
using System.ServiceModel;

namespace SynchronicWorldService
{
    [ServiceContract]
    public interface IContributionService
    {
        /// <summary>
        /// Retrieve all the contributions for an event
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        Models.ServiceResponse<List<Models.Contribution>> GetEventContributions(int eventId);

        /// <summary>
        /// Retrieve all the contributions of a person (for all events)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Models.ServiceResponse<List<Models.Contribution>> GetPersonContributions(int userId);

        /// <summary>
        /// Delete all the contributions of a person for all open events
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Models.ServiceResponse<bool> DeleteAllPersonContributionsForOpenEvents(int userId);
    }
}
