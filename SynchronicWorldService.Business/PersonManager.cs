using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;
using SynchronicWorldService.DataAccess;
using SynchronicWorldService.Models;
using SynchronicWorldService.Utils;
using Person = SynchronicWorldService.DataAccess.Person;

namespace SynchronicWorldService.Business
{
    public class PersonManager : IPersonManager
    {
        /// <summary>
        /// Unit of work
        /// </summary>
        public IUnitOfWork UoW { get; set; }

        /// <summary>
        /// See interface
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Models.ServiceResponse<Person> Get(int id)
        {
            var svcResponse = new Models.ServiceResponse<Person>();
            var personFound = UoW.Context.People.Find(id);
            if (personFound == null)
            {
                svcResponse.Report.ErrorList.Add(SWResources.Person_Not_Found);
            }
            else
            {
                svcResponse.Result = personFound;
            }
            return svcResponse;
        }

        /// <summary>
        /// See interface
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public Models.ServiceResponse<Person> Save(Person person)
        {
            var svcResponse = new Models.ServiceResponse<Person>();
            if (person.Id == 0)
            {
                //Create
                var newPerson = new Person
                {
                    Name = person.Name,
                    Nickname = person.Nickname
                };
                UoW.Context.People.Add(newPerson);
                svcResponse.Result = newPerson;
            }
            else
            {
                //Update
                var personFound = UoW.Context.People.Find(person.Id);
                if (personFound == null)
                {
                    svcResponse.Report.ErrorList.Add(SWResources.Person_Not_Found);
                }
                else
                {
                    personFound.Name = person.Name;
                    personFound.Nickname = person.Nickname;
                }
                svcResponse.Result = personFound;
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
            var personFound = UoW.Context.People.Include(x => x.Events).FirstOrDefault(x => x.Id == id);
            if (personFound == null)
            {
                svcResponse.Report.ErrorList.Add(SWResources.Person_Not_Found);
                svcResponse.Result = false;
            }
            else
            {
                //Remove his contributions
                var contribsToRemove = UoW.Context.Contributions.Where(x => x.Fk_Person == id);
                contribsToRemove.ForEach(x => UoW.Context.Contributions.Remove(x));

                UoW.Context.People.Remove(personFound);
                svcResponse.Result = true;
            }
            return svcResponse;
        }

        /// <summary>
        /// See interface
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public Models.ServiceResponse<bool> SuscribeUserToAnOpenEvent(int userId, int eventId)
        {
            var response = new Models.ServiceResponse<bool> { Result = true };

            //Get event
            var eventMgr = ManagerFactory.Resolve<IEventManager>();
            eventMgr.UoW = UoW;
            var eventtResponse = eventMgr.Get(eventId);
            if (eventtResponse.Report.GetNumberOfErrors() != 0)
            {
                response.Result = false;
                response.SetResponseAndReport(false, eventtResponse.Report);
                return response;
            }
            if (eventtResponse.Result.EventStatus.Code != EventStatusCode.Open.ToString())
            {
                response.Result = false;
                response.Report.ErrorList.Add(SWResources.SuscribeUserToAnEvent_EventNotOpen);
                return response;
            }

            //Get user
            var personResponse = Get(userId);
            if (personResponse.Report.GetNumberOfErrors() != 0)
            {
                response.Result = false;
                response.SetResponseAndReport(false, personResponse.Report);
                return response;
            }

            //Check if user already suscribed to the event
            if (eventtResponse.Result.People.Any(x => x.Id == userId))
            {
                response.Result = false;
                response.Report.ErrorList.Add(SWResources.SuscribeUserToAnEvent_UserAlreadySuscribed);
                return response;
            }

            eventtResponse.Result.People.Add(personResponse.Result);

            return response;
        }

        /// <summary>
        /// See interface
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public Models.ServiceResponse<List<Person>> FindPeopleLinkToOpenEvent(int eventId)
        {
            var response = new Models.ServiceResponse<List<Person>>();

            //Get event
            var eventMgr = ManagerFactory.Resolve<IEventManager>();
            eventMgr.UoW = UoW;
            var eventtResponse = eventMgr.Get(eventId);
            if (eventtResponse.Report.GetNumberOfErrors() != 0)
            {
                response.SetResponseAndReport(null, eventtResponse.Report);
                return response;
            }
            if (eventtResponse.Result.EventStatus.Code != EventStatusCode.Open.ToString())
            {
                response.Report.ErrorList.Add(SWResources.SuscribeUserToAnEvent_EventNotOpen);
                return response;
            }

            response.Result = eventtResponse.Result.People.ToList();

            return response;
        }

        #region converters
        /// <summary>
        /// See interface
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public Models.Person ConvertPersonToWcfPerson(Person person)
        {
            if (person == null)
                return null;
            return new Models.Person
            {
                Id = person.Id,
                Name = person.Name,
                Nickname = person.Nickname
            };
        }

        /// <summary>
        /// See interface
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public Person ConvertWcfPersonToPerson(Models.Person person)
        {
            if (person == null)
                return null;
            return new Person
            {
                Id = person.Id,
                Name = person.Name,
                Nickname = person.Nickname
            };
        }
        #endregion

    }

    public interface IPersonManager
    {
        /// <summary>
        /// Unit of work
        /// </summary>
        IUnitOfWork UoW { get; set; }

        /// <summary>
        /// Get a person by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Models.ServiceResponse<Person> Get(int id);

        /// <summary>
        /// Create or update person
        /// If contains an id -> update
        /// If doesn't contain an id -> create
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        Models.ServiceResponse<Person> Save(Person person);

        /// <summary>
        /// Delete a person by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Models.ServiceResponse<bool> Delete(int id);

        /// <summary>
        /// Suscribe a user to an event
        /// </summary>
        /// <returns></returns>
        Models.ServiceResponse<bool> SuscribeUserToAnOpenEvent(int userId, int eventId);

        /// <summary>
        /// Find people link to an open event
        /// </summary>
        /// <returns></returns>
        Models.ServiceResponse<List<Person>> FindPeopleLinkToOpenEvent(int eventId);

        /// <summary>
        /// Convert event to WCF event facade
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        Models.Person ConvertPersonToWcfPerson(DataAccess.Person person);

        /// <summary>
        /// Convert WCF event facade to event
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        DataAccess.Person ConvertWcfPersonToPerson(Models.Person person);
    }
}
