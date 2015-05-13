using System;
using SynchronicWorldService.DataAccess;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SynchronicWorldService.Models;
using SynchronicWorldService.Utils;
using Event = SynchronicWorldService.DataAccess.Event;

namespace SynchronicWorldService.Business
{
    public class EventManager : IEventManager
    {
        /// <summary>
        /// Unit of work
        /// </summary>
        public IUnitOfWork UoW { get; set; }

        /// <summary>
        /// See interface
        /// </summary>
        /// <returns></returns>
        public Models.ServiceResponse<List<Event>> GetAll()
        {
            var svcResponse = new Models.ServiceResponse<List<Event>> { 
                Result = new List<Event>()
            };
            UoW.Context.Events.ToList().ForEach(x => svcResponse.Result.Add(x));
            return svcResponse;
        }

        /// <summary>
        /// See interface
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Models.ServiceResponse<Event> Get(int id)
        {
            var svcResponse = new Models.ServiceResponse<Event>();
            var eventFound = UoW.Context.Events.Include(x => x.EventStatus).Include(x => x.EventType).Include(x => x.People).FirstOrDefault(x => x.Id == id);
            if (eventFound == null)
            {
                svcResponse.Report.ErrorList.Add(SWResources.Event_Not_Found);
            }
            else
            {
                svcResponse.Result = eventFound;
            }
            return svcResponse;
        }

        /// <summary>
        /// See interface
        /// </summary>
        /// <param name="eventt"></param>
        /// <returns></returns>
        public Models.ServiceResponse<Event> Save(Event eventt)
        {
            var svcResponse = new Models.ServiceResponse<Event>();
            if (eventt.Id == 0)
            {
                //Create
                eventt.EventStatus = null;
                eventt.EventType = null;
                UoW.Context.Events.Add(eventt);
                svcResponse.Result = eventt;
            }
            else
            {
                //Update
                var eventFound = UoW.Context.Events.Find(eventt.Id);
                if (eventFound == null)
                {
                    svcResponse.Report.ErrorList.Add(SWResources.Event_Not_Found);
                }
                else
                {
                    eventFound.Name = eventt.Name;
                    eventFound.Description = eventt.Description;
                    eventFound.Date = eventt.Date;
                    eventFound.Address = eventt.Address;
                    if (eventFound.Fk_Status != eventt.Fk_Status)
                    {
                        eventFound.Fk_Status = eventt.Fk_Status;
                        eventFound.EventStatus = UoW.Context.EventStatuses.Find(eventt.Fk_Status);
                    }

                    if (eventFound.Fk_Type != eventt.Fk_Type)
                    {
                        eventFound.Fk_Type = eventt.Fk_Type;
                        eventFound.EventType = UoW.Context.EventTypes.Find(eventt.Fk_Type);
                    }
                }
                svcResponse.Result = eventFound;
            }

            return svcResponse;
        }

        /// <summary>
        /// See interface
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Models.ServiceResponse<bool> Delete(int id)
        {
            var svcResponse = new Models.ServiceResponse<bool>();
            var eventFound = UoW.Context.Events.Include(x => x.People).FirstOrDefault(x => x.Id == id);
            if (eventFound == null)
            {
                svcResponse.Report.ErrorList.Add(SWResources.Event_Not_Found);
                svcResponse.Result = false;
            }
            else
            {
                UoW.Context.Events.Remove(eventFound);
                svcResponse.Result = true;
            }
            return svcResponse;
        }

        /// <summary>
        /// See interface
        /// </summary>
        /// <param name="searchObject"></param>
        /// <returns></returns>
        public Models.ServiceResponse<List<Event>> SearchForEvents(Models.Facades.EventSearchFacade searchObject)
        {
            var response = new Models.ServiceResponse<List<Event>>();

            //WHERE
            var query = UoW.Context.Events.Where( x =>
                (searchObject.Name == null || x.Name.Equals(searchObject.Name)) &&
                (searchObject.Date == null || (x.Date.Year == searchObject.Date.Value.Year && x.Date.Month == searchObject.Date.Value.Month && x.Date.Day == searchObject.Date.Value.Day)) &&
                (searchObject.StartDate == null || (x.Date.Year >= searchObject.StartDate.Value.Year && x.Date.Month >= searchObject.StartDate.Value.Month && x.Date.Day >= searchObject.StartDate.Value.Day)) &&
                (searchObject.EndDate == null || (x.Date.Year <= searchObject.EndDate.Value.Year && x.Date.Month <= searchObject.EndDate.Value.Month && x.Date.Day <= searchObject.EndDate.Value.Day)) &&
                (searchObject.EventStatusCode == null || x.EventStatus.Code.Equals(searchObject.EventStatusCode)) &&
                (searchObject.EventTypeCode == null || x.EventType.Code.Equals(searchObject.EventTypeCode))
            );

            //ORDER BY
            query = query.OrderByDescending(x => x.Date);

            response.Result = query.ToList();
            return response;
        }

        /// <summary>
        /// See interface
        /// </summary>
        /// <returns></returns>
        public Models.ServiceResponse<bool> DeleteClosedEvents()
        {
            var response = new Models.ServiceResponse<bool>{ Result = true };

            var eventsClosed =
                UoW.Context.Events.Where(x => x.EventStatus.Code == Models.EventStatusCode.Closed.ToString())
                    .ToList();


            eventsClosed.ForEach(x => UoW.Context.Events.Remove(x));
            response.Report.InfoList.Add(String.Format(SWResources.Closed_Events_Removed, eventsClosed.Count));

            return response;
        }

        /// <summary>
        /// See interface
        /// </summary>
        /// <returns></returns>
        public Models.ServiceResponse<bool> UpgradePendingEventsAsOpen()
        {
            var response = new Models.ServiceResponse<bool> { Result = true };

            //Get pending status object
            var eventStatusMgr = ManagerFactory.Resolve<IEventStatusManager>();
            eventStatusMgr.UoW = UoW;

            var pendingStatusResponse = eventStatusMgr.FindByCode(EventStatusCode.Pending.ToString());
            if (pendingStatusResponse.Report.GetNumberOfErrors() != 0)
            {
                response.Report.ErrorList.Add(
                    SWResources.Upgrade_Events_Status_From_Pending_To_Open_Error);
                response.Result = false;
            }
            else
            {
                var eventsPending =
                UoW.Context.Events.Where(x => x.EventStatus.Code == Models.EventStatusCode.Pending.ToString())
                    .ToList();

                foreach (var eventPending in eventsPending)
                {
                    eventPending.EventStatus = pendingStatusResponse.Result;
                    eventPending.Fk_Status = pendingStatusResponse.Result.Id;
                }

                response.Report.InfoList.Add(String.Format(SWResources.Upgrade_Events_Status_From_Pending_To_Open_Done, eventsPending.Count));
            }
                
            return response;
        }

        #region converters
        /// <summary>
        /// See interface
        /// </summary>
        /// <param name="eventt"></param>
        /// <returns></returns>
        public Models.Event ConvertEventToWcfEvent(Event eventt)
        {
            if (eventt == null)
                return null;
            if (eventt.EventStatus == null)
                eventt.EventStatus = UoW.Context.EventStatuses.First(x => x.Id == eventt.Fk_Status);
            if (eventt.EventType == null)
                eventt.EventType = UoW.Context.EventTypes.First(x => x.Id == eventt.Fk_Type);
            return new Models.Event
            {
                Id = eventt.Id,
                Name = eventt.Name,
                Description = eventt.Description,
                Address = eventt.Address,
                Date = eventt.Date,
                Status = new Models.EventStatus
                {
                    Id = eventt.EventStatus.Id,
                    Code = eventt.EventStatus.Code,
                    Value = eventt.EventStatus.Value
                },
                Type = new Models.EventType
                {
                    Id = eventt.EventType.Id,
                    Code = eventt.EventType.Code,
                    Value = eventt.EventType.Value
                }
            };
        }

        /// <summary>
        /// See interface
        /// </summary>
        /// <param name="eventt"></param>
        /// <returns></returns>
        public Event ConvertWcfEventToEvent(Models.Event eventt)
        {
            if (eventt == null)
                return null;
            return new Event
            {
                Id = eventt.Id,
                Name = eventt.Name,
                Description = eventt.Description,
                Address = eventt.Address,
                Date = eventt.Date,
                Fk_Status = eventt.Status.Id,
                EventStatus = new DataAccess.EventStatus()
                {
                    Id = eventt.Status.Id,
                    Code = eventt.Status.Code,
                    Value = eventt.Status.Value
                },
                Fk_Type = eventt.Type.Id,
                EventType = new DataAccess.EventType
                {
                    Id = eventt.Type.Id,
                    Code = eventt.Type.Code,
                    Value = eventt.Type.Value
                }
            };
        }
        #endregion
    }

    public interface IEventManager
    {
        /// <summary>
        /// Unit of work
        /// </summary>
        IUnitOfWork UoW { get; set; }

        /// <summary>
        /// Get all events
        /// </summary>
        /// <returns></returns>
        Models.ServiceResponse<List<Event>> GetAll();

        /// <summary>
        /// Get an event by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Models.ServiceResponse<Event> Get(int id);

        /// <summary>
        /// Create or update event
        /// If contains an id -> update
        /// If doesn't contain an id -> create
        /// </summary>
        /// <param name="eventt"></param>
        /// <returns></returns>
        Models.ServiceResponse<Event> Save(Event eventt);

        /// <summary>
        /// Delete an event by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Models.ServiceResponse<bool> Delete(int id);

        /// <summary>
        /// Search for events according to many parameters
        /// With this method we are able to find all event(s) :
        /// - by name and Date
        /// - between two specific dates (included)
        /// - by status or type
        /// </summary>
        /// <param name="searchObject"></param>
        /// <returns></returns>
        Models.ServiceResponse<List<Event>> SearchForEvents(Models.Facades.EventSearchFacade searchObject);

        /// <summary>
        /// Delete all closed events
        /// </summary>
        /// <returns></returns>
        Models.ServiceResponse<bool> DeleteClosedEvents();

        /// <summary>
        /// Upgrade pending events as open
        /// </summary>
        /// <returns></returns>
        Models.ServiceResponse<bool> UpgradePendingEventsAsOpen();

        /// <summary>
        /// Convert event to WCF event facade
        /// </summary>
        /// <param name="eventt"></param>
        /// <returns></returns>
        Models.Event ConvertEventToWcfEvent(Event eventt);

        /// <summary>
        /// Convert WCF event facade to event
        /// </summary>
        /// <param name="eventt"></param>
        /// <returns></returns>
        Event ConvertWcfEventToEvent(Models.Event eventt);
    }
}
