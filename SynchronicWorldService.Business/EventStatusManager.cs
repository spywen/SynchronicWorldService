using System.Collections.Generic;
using System.Linq;
using SynchronicWorldService.DataAccess;

namespace SynchronicWorldService.Business
{
    public class EventStatusManager : IEventStatusManager
    {

        /// <summary>
        /// See interface
        /// </summary>
        public IUnitOfWork UoW { get; set; }

        /// <summary>
        /// See interface
        /// </summary>
        /// <returns></returns>
        public Models.ServiceResponse<List<EventStatus>> GetAll()
        {
            var response = new Models.ServiceResponse<List<EventStatus>>
            {
                Result = UoW.Context.EventStatuses.ToList()
            };
            return response;
        }

        /// <summary>
        /// See interface
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Models.ServiceResponse<EventStatus> FindByCode(string code)
        {
            var response = new Models.ServiceResponse<EventStatus>
            {
                Result = UoW.Context.EventStatuses.FirstOrDefault(x => x.Code == code)
            };
            if (response.Result == null)
            {
                response.Report.ErrorList.Add(SynchronicWorldServiceResources.Event_Status_Not_Found);
            }
            return response;
        }

        /// <summary>
        /// See interface
        /// </summary>
        /// <param name="eventStatus"></param>
        /// <returns></returns>
        public Models.EventStatus ConvertEventStatusToWcfEventStatus(EventStatus eventStatus)
        {
            if (eventStatus == null)
                return null;
            return new Models.EventStatus
            {
                Id = eventStatus.Id,
                Code = eventStatus.Code,
                Value = eventStatus.Value
            };
        }

        /// <summary>
        /// See interface
        /// </summary>
        /// <param name="eventStatus"></param>
        /// <returns></returns>
        public EventStatus ConvertWcfEventStatusToEventStatus(Models.EventStatus eventStatus)
        {
            if (eventStatus == null)
                return null;
            return new EventStatus
            {
                Id = eventStatus.Id,
                Code = eventStatus.Code,
                Value = eventStatus.Value
            };
        }

    }

    public interface IEventStatusManager
    {
        /// <summary>
        /// Unit of work
        /// </summary>
        IUnitOfWork UoW { get; set; }

        /// <summary>
        /// Get all event status
        /// </summary>
        /// <returns></returns>
        Models.ServiceResponse<List<EventStatus>> GetAll();

        /// <summary>
        /// Find status by code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Models.ServiceResponse<EventStatus> FindByCode(string code);

        /// <summary>
        /// Convert event status to WCF event status facade
        /// </summary>
        /// <param name="eventStatus"></param>
        /// <returns></returns>
        Models.EventStatus ConvertEventStatusToWcfEventStatus(DataAccess.EventStatus eventStatus);

        /// <summary>
        /// Convert WCF event status facade to event status
        /// </summary>
        /// <param name="eventStatus"></param>
        /// <returns></returns>
        DataAccess.EventStatus ConvertWcfEventStatusToEventStatus(Models.EventStatus eventStatus);
    }
}
