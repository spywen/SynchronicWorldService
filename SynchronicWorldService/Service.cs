using SynchronicWorldService.Business;
using SynchronicWorldService.DataAccess;
using System;
using System.Collections.Generic;

namespace SynchronicWorldService
{
    /// <summary>
    /// Event service
    /// </summary>
    public partial class Service : IEventService
    {
        /// <summary>
        /// See interface
        /// </summary>
        /// <param name="eventt"></param>
        /// <returns></returns>
        public Models.ServiceResponse<Models.Event> CreateEvent(Models.Event eventt)
        {
            var response = new Models.ServiceResponse<Models.Event>();
            try
            {
                using (var uow = DataAccessFactory.Resolve<IUnitOfWork>())
                {
                    var eventManager = ManagerFactory.Resolve<IEventManager>();
                    eventManager.UoW = uow;
                    eventt.Id = 0;
                    var mgrResponse = eventManager.Save(eventManager.ConvertWcfEventToEvent(eventt));


                    if (mgrResponse.Report.GetNumberOfErrors() == 0)
                    {
                        uow.Context.SaveChanges();
                        uow.Context.GetId(mgrResponse.Result);
                    }

                    response.SetResponseAndReport(eventManager.ConvertEventToWcfEvent(mgrResponse.Result), mgrResponse.Report);
                }
            }
            catch (Exception e)
            {
                response.Report.ErrorList.Add(SynchronicWorldServiceResources.ServiceError);
                response.Report.LogException(e);
            }
            return response;
        }

        /// <summary>
        /// See interface
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Models.ServiceResponse<Models.Event> GetEvent(int id)
        {
            var response = new Models.ServiceResponse<Models.Event>();
            try
            {
                using (var uow = DataAccessFactory.Resolve<IUnitOfWork>())
                {
                    var eventManager = ManagerFactory.Resolve<IEventManager>();
                    eventManager.UoW = uow;

                    var mgrResponse = eventManager.Get(id);
                    response.SetResponseAndReport(eventManager.ConvertEventToWcfEvent(mgrResponse.Result), mgrResponse.Report);
                }
            }
            catch (Exception e)
            {
                response.Report.ErrorList.Add(SynchronicWorldServiceResources.ServiceError);
                response.Report.LogException(e);
            }
            return response;
        }

        /// <summary>
        /// See interface
        /// </summary>
        /// <returns></returns>
        public Models.ServiceResponse<List<Models.Event>> GetAllEvents()
        {
            var response = new Models.ServiceResponse<List<Models.Event>>();
            try
            {
                using (var uow = DataAccessFactory.Resolve<IUnitOfWork>())
                {
                    var eventManager = ManagerFactory.Resolve<IEventManager>();
                    eventManager.UoW = uow;
                    var mgrResponse = eventManager.GetAll();

                    var events = new List<Models.Event>();
                    mgrResponse.Result.ForEach(x => events.Add(eventManager.ConvertEventToWcfEvent(x)));
                    response.SetResponseAndReport(events, mgrResponse.Report);
                }
            }
            catch (Exception e)
            {
                response.Report.ErrorList.Add(SynchronicWorldServiceResources.ServiceError);
                response.Report.LogException(e);
            }
            return response;
        }

        /// <summary>
        /// See interface
        /// </summary>
        /// <param name="eventt"></param>
        /// <returns></returns>
        public Models.ServiceResponse<Models.Event> UpdateEvent(Models.Event eventt)
        {
            var response = new Models.ServiceResponse<Models.Event>();
            try
            {
                using (var uow = DataAccessFactory.Resolve<IUnitOfWork>())
                {
                    var eventManager = ManagerFactory.Resolve<IEventManager>();
                    eventManager.UoW = uow;
                    var mgrResponse = eventManager.Save(eventManager.ConvertWcfEventToEvent(eventt));

                    if(mgrResponse.Report.GetNumberOfErrors() == 0)
                        uow.Context.SaveChanges();

                    response.SetResponseAndReport(eventManager.ConvertEventToWcfEvent(mgrResponse.Result), mgrResponse.Report);
                }
            }
            catch (Exception e)
            {
                response.Report.ErrorList.Add(SynchronicWorldServiceResources.ServiceError);
                response.Report.LogException(e);
            }
            return response;
        }

        /// <summary>
        /// See interface
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Models.ServiceResponse<bool> DeleteEvent(int id)
        {
            var response = new Models.ServiceResponse<bool>();
            try
            {
                using (var uow = DataAccessFactory.Resolve<IUnitOfWork>())
                {
                    var eventManager = ManagerFactory.Resolve<IEventManager>();
                    eventManager.UoW = uow;
                    var mgrResponse = eventManager.Delete(id);

                    if(mgrResponse.Report.GetNumberOfErrors() == 0)
                        uow.Context.SaveChanges();

                    response.SetResponseAndReport(mgrResponse.Result, mgrResponse.Report);
                }
            }
            catch (Exception e)
            {
                response.Report.ErrorList.Add(SynchronicWorldServiceResources.ServiceError);
                response.Report.LogException(e);
                response.Result = false;
            }
            return response;
        }

        /// <summary>
        /// See interface
        /// </summary>
        /// <param name="searchObject"></param>
        /// <returns></returns>
        public Models.ServiceResponse<List<Models.Event>> SearchForEvents(Models.Facades.EventSearchFacade searchObject)
        {
            var response = new Models.ServiceResponse<List<Models.Event>>();
            try
            {
                using (var uow = DataAccessFactory.Resolve<IUnitOfWork>())
                {
                    var eventManager = ManagerFactory.Resolve<IEventManager>();
                    eventManager.UoW = uow;
                    var mgrResponse = eventManager.SearchForEvents(searchObject);

                    var events = new List<Models.Event>();
                    mgrResponse.Result.ForEach(x => events.Add(eventManager.ConvertEventToWcfEvent(x)));
                    response.SetResponseAndReport(events, mgrResponse.Report);
                }
            }
            catch (Exception e)
            {
                response.Report.ErrorList.Add(SynchronicWorldServiceResources.ServiceError);
                response.Report.LogException(e);
            }
            return response;
        }

        /// <summary>
        /// See interface
        /// </summary>
        /// <returns></returns>
        public Models.ServiceResponse<bool> DeleteClosedEvents()
        {
            var response = new Models.ServiceResponse<bool>();
            try
            {
                using (var uow = DataAccessFactory.Resolve<IUnitOfWork>())
                {
                    var eventManager = ManagerFactory.Resolve<IEventManager>();
                    eventManager.UoW = uow;
                    var mgrResponse = eventManager.DeleteClosedEvents();

                    if (mgrResponse.Report.GetNumberOfErrors() == 0)
                        uow.Context.SaveChanges();

                    response.SetResponseAndReport(mgrResponse.Result, mgrResponse.Report);
                }
            }
            catch (Exception e)
            {
                response.Report.ErrorList.Add(SynchronicWorldServiceResources.ServiceError);
                response.Report.LogException(e);
                response.Result = false;
            }
            return response;
        }

        /// <summary>
        /// See interface
        /// </summary>
        /// <returns></returns>
        public Models.ServiceResponse<bool> UpgradePendingEventsAsOpen()
        {
            var response = new Models.ServiceResponse<bool>();
            try
            {
                using (var uow = DataAccessFactory.Resolve<IUnitOfWork>())
                {
                    var eventManager = ManagerFactory.Resolve<IEventManager>();
                    eventManager.UoW = uow;
                    var mgrResponse = eventManager.UpgradePendingEventsAsOpen();

                    if (mgrResponse.Report.GetNumberOfErrors() == 0)
                        uow.Context.SaveChanges();

                    response.SetResponseAndReport(mgrResponse.Result, mgrResponse.Report);
                }
            }
            catch (Exception e)
            {
                response.Report.ErrorList.Add(SynchronicWorldServiceResources.ServiceError);
                response.Report.LogException(e);
                response.Result = false;
            }
            return response;
        }
    }

    /// <summary>
    /// Person service
    /// </summary>
    public partial class Service : IPersonService
    {
        /// <summary>
        /// See interface
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public Models.ServiceResponse<Models.Person> CreatePerson(Models.Person person)
        {
            var response = new Models.ServiceResponse<Models.Person>();
            try
            {
                using (var uow = DataAccessFactory.Resolve<IUnitOfWork>())
                {
                    var personManager = ManagerFactory.Resolve<IPersonManager>();
                    personManager.UoW = uow;
                    person.Id = 0;
                    var mgrResponse = personManager.Save(personManager.ConvertWcfPersonToPerson(person));

                    if (mgrResponse.Report.GetNumberOfErrors() == 0)
                        uow.Context.SaveChanges();

                    response.SetResponseAndReport(personManager.ConvertPersonToWcfPerson(mgrResponse.Result), mgrResponse.Report);
                }
            }
            catch (Exception e)
            {
                response.Report.ErrorList.Add(SynchronicWorldServiceResources.ServiceError);
                response.Report.LogException(e);
            }
            return response;
        }

        /// <summary>
        /// See interface
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Models.ServiceResponse<Models.Person> GetPerson(int id)
        {
            var response = new Models.ServiceResponse<Models.Person>();
            try
            {
                using (var uow = DataAccessFactory.Resolve<IUnitOfWork>())
                {
                    var personManager = ManagerFactory.Resolve<IPersonManager>();
                    personManager.UoW = uow;
                    var mgrReponse = personManager.Get(id);

                    response.SetResponseAndReport(personManager.ConvertPersonToWcfPerson(mgrReponse.Result), mgrReponse.Report);
                }
            }
            catch (Exception e)
            {
                response.Report.ErrorList.Add(SynchronicWorldServiceResources.ServiceError);
                response.Report.LogException(e);
            }
            return response;
        }

        /// <summary>
        /// See interface
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public Models.ServiceResponse<Models.Person> UpdatePerson(Models.Person person)
        {
            var response = new Models.ServiceResponse<Models.Person>();
            try
            {
                using (var uow = DataAccessFactory.Resolve<IUnitOfWork>())
                {
                    var personManager = ManagerFactory.Resolve<IPersonManager>();
                    personManager.UoW = uow;
                    var mgrResponse = personManager.Save(personManager.ConvertWcfPersonToPerson(person));

                    if(mgrResponse.Report.GetNumberOfErrors() == 0)
                        uow.Context.SaveChanges();

                    response.SetResponseAndReport(personManager.ConvertPersonToWcfPerson(mgrResponse.Result), mgrResponse.Report);
                }
            }
            catch (Exception e)
            {
                response.Report.ErrorList.Add(SynchronicWorldServiceResources.ServiceError);
                response.Report.LogException(e);
            }
            return response;
        }

        /// <summary>
        /// See interface
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Models.ServiceResponse<bool> DeletePerson(int id)
        {
            var response = new Models.ServiceResponse<bool>();
            try
            {
                using (var uow = DataAccessFactory.Resolve<IUnitOfWork>())
                {
                    var personManager = ManagerFactory.Resolve<IPersonManager>();
                    personManager.UoW = uow;
                    var mgrResponse = personManager.Delete(id);

                    if(mgrResponse.Report.GetNumberOfErrors() == 0)
                        uow.Context.SaveChanges();

                    response.SetResponseAndReport(mgrResponse.Result, mgrResponse.Report);
                }
            }
            catch (Exception e)
            {
                response.Report.ErrorList.Add(SynchronicWorldServiceResources.ServiceError);
                response.Report.LogException(e);
                response.Result = false;
            }
            return response;
        }
    }

    /// <summary>
    /// Enums service
    /// </summary>
    public partial class Service : IEnumsService
    {
        /// <summary>
        /// See interface
        /// </summary>
        /// <returns></returns>
        public Models.ServiceResponse<List<Models.EventStatus>> GetAllEventsStatus()
        {
            var response = new Models.ServiceResponse<List<Models.EventStatus>>();
            try
            {
                using (var uow = DataAccessFactory.Resolve<IUnitOfWork>())
                {
                    var eventStatusManager = ManagerFactory.Resolve<IEventStatusManager>();
                    eventStatusManager.UoW = uow;
                    var mgrResponse = eventStatusManager.GetAll();

                    var eventStatus = new List<Models.EventStatus>();
                    mgrResponse.Result.ForEach(x => eventStatus.Add(eventStatusManager.ConvertEventStatusToWcfEventStatus(x)));
                    response.SetResponseAndReport(eventStatus, mgrResponse.Report);
                }
            }
            catch (Exception e)
            {
                response.Report.ErrorList.Add(SynchronicWorldServiceResources.ServiceError);
                response.Report.LogException(e);
            }
            return response;
        }

        /// <summary>
        /// See interface
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Models.ServiceResponse<Models.EventStatus> FindEventStatusByCode(string code)
        {
            var response = new Models.ServiceResponse<Models.EventStatus>();
            try
            {
                using (var uow = DataAccessFactory.Resolve<IUnitOfWork>())
                {
                    var eventStatusManager = ManagerFactory.Resolve<IEventStatusManager>();
                    eventStatusManager.UoW = uow;
                    var mgrResponse = eventStatusManager.FindByCode(code);

                    response.SetResponseAndReport(eventStatusManager.ConvertEventStatusToWcfEventStatus(mgrResponse.Result), mgrResponse.Report);
                }
            }
            catch (Exception e)
            {
                response.Report.ErrorList.Add(SynchronicWorldServiceResources.ServiceError);
                response.Report.LogException(e);
            }
            return response;
        }

        /// <summary>
        /// See interface
        /// </summary>
        /// <returns></returns>
        public Models.ServiceResponse<List<Models.EventType>> GetAllEventsType()
        {
            var response = new Models.ServiceResponse<List<Models.EventType>>();
            try
            {
                using (var uow = DataAccessFactory.Resolve<IUnitOfWork>())
                {
                    var eventTypeManager = ManagerFactory.Resolve<IEventTypeManager>();
                    eventTypeManager.UoW = uow;
                    var mgrResponse = eventTypeManager.GetAll();

                    var eventType = new List<Models.EventType>();
                    mgrResponse.Result.ForEach(x => eventType.Add(eventTypeManager.ConvertEventTypeToWcfEventType(x)));
                    response.SetResponseAndReport(eventType, mgrResponse.Report);
                }
            }
            catch (Exception e)
            {
                response.Report.ErrorList.Add(SynchronicWorldServiceResources.ServiceError);
                response.Report.LogException(e);
            }
            return response;
        }

        /// <summary>
        /// See interface
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Models.ServiceResponse<Models.EventType> FindEventTypeByCode(string code)
        {
            var response = new Models.ServiceResponse<Models.EventType>();
            try
            {
                using (var uow = DataAccessFactory.Resolve<IUnitOfWork>())
                {
                    var eventTypeManager = ManagerFactory.Resolve<IEventTypeManager>();
                    eventTypeManager.UoW = uow;
                    var mgrResponse = eventTypeManager.FindByCode(code);

                    response.SetResponseAndReport(eventTypeManager.ConvertEventTypeToWcfEventType(mgrResponse.Result), mgrResponse.Report);
                }
            }
            catch (Exception e)
            {
                response.Report.ErrorList.Add(SynchronicWorldServiceResources.ServiceError);
                response.Report.LogException(e);
            }
            return response;
        }
    }
}
