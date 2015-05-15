using System;
using System.Linq;
using SynchronicWorldService.DataAccess;
using SynchronicWorldService.Models;

namespace SynchronicWorldService.Business
{
    public class InfoManager : IInfoManager
    {
        public const string DatabaseStatus =
            "Events : {0}, People : {1}, Contributions : {2}, ContributionTypes : {3}, EventStatuses : {4}, EventTypes : {5}";
        
        /// <summary>
        /// See interface
        /// </summary>
        public IUnitOfWork UoW { get; set; }

        /// <summary>
        /// See interface
        /// </summary>
        /// <returns></returns>
        public ServiceResponse<string> GetDatabaseStatus()
        {
            return new ServiceResponse<string>
            {
                Result = String.Format(DatabaseStatus,  
                    UoW.Context.Events.Count(),
                    UoW.Context.People.Count(),
                    UoW.Context.Contributions.Count(),
                    UoW.Context.ContributionTypes.Count(),
                    UoW.Context.EventStatuses.Count(),
                    UoW.Context.EventTypes.Count())
            };
        }
    }

    public interface IInfoManager
    {
        /// <summary>
        /// Unit of work
        /// </summary>
        IUnitOfWork UoW { get; set; }

        /// <summary>
        /// Get database status with the number of entities inside each tables
        /// </summary>
        /// <returns></returns>
        ServiceResponse<String> GetDatabaseStatus();
    }
}
