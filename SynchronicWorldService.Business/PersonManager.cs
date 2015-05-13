using System.Data.Entity;
using System.Linq;
using SynchronicWorldService.DataAccess;
using SynchronicWorldService.Utils;

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
                UoW.Context.People.Remove(personFound);
                svcResponse.Result = true;
            }
            return svcResponse;
        }

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
