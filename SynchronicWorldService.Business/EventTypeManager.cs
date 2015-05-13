using System.Collections.Generic;
using System.Linq;
using SynchronicWorldService.DataAccess;
using SynchronicWorldService.Utils;

namespace SynchronicWorldService.Business
{
    public class EventTypeManager : IEventTypeManager
    {

        /// <summary>
        /// See interface
        /// </summary>
        public IUnitOfWork UoW { get; set; }

        /// <summary>
        /// See interface
        /// </summary>
        /// <returns></returns>
        public Models.ServiceResponse<List<EventType>> GetAll()
        {
            return new Models.ServiceResponse<List<EventType>>
            {
                Result = UoW.Context.EventTypes.ToList()
            };
        }

        /// <summary>
        /// See interface
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Models.ServiceResponse<EventType> FindByCode(string code)
        {
            var response = new Models.ServiceResponse<EventType>
            {
                Result = UoW.Context.EventTypes.FirstOrDefault(x => x.Code == code)
            };
            if (response.Result == null)
            {
                response.Report.ErrorList.Add(SWResources.Event_Type_Not_Found);
            }
            return response;
        }

        /// <summary>
        /// See interface
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns></returns>
        public Models.EventType ConvertEventTypeToWcfEventType(DataAccess.EventType eventType)
        {
            if (eventType == null)
                return null;
            return new Models.EventType
            {
                Id = eventType.Id,
                Code = eventType.Code,
                Value = eventType.Value
            };
        }

        /// <summary>
        /// See interface
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns></returns>
        public DataAccess.EventType ConvertWcfEventTypeToEventtype(Models.EventType eventType)
        {
            if (eventType == null)
                return null;
            return new EventType
            {
                Id = eventType.Id,
                Code = eventType.Code,
                Value = eventType.Value
            };
        }

    }

    public interface IEventTypeManager
    {
        /// <summary>
        /// Unit of work
        /// </summary>
        IUnitOfWork UoW { get; set; }

        /// <summary>
        /// Get all event type
        /// </summary>
        /// <returns></returns>
        Models.ServiceResponse<List<EventType>> GetAll();

        /// <summary>
        /// Find status by code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Models.ServiceResponse<EventType> FindByCode(string code);

        /// <summary>
        /// Convert event type to WCF event type facade
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns></returns>
        Models.EventType ConvertEventTypeToWcfEventType(DataAccess.EventType eventType);

        /// <summary>
        /// Convert WCF event type facade to event type
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns></returns>
        DataAccess.EventType ConvertWcfEventTypeToEventtype(Models.EventType eventType);
    }
}
