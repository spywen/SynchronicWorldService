using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;
using SynchronicWorldService.Business;
using SynchronicWorldService.Utils;

namespace SynchronicWorldService.Test
{
    [TestFixture(Category="Person Tests")]
    public class PersonTests : EffortBaseTest
    {
        #region setup
        private Service Service { get; set; }

        [SetUp]
        public void Setup()
        {
            Service = new Service();
        }
        #endregion

        #region CRUD

        [Test]
        public void Get()
        {
            var response = Service.GetPerson(1);

            Assert.AreEqual("Laurent", response.Result.Name);
        }

        [Test]
        public void Get_Not_Found()
        {
            var response = Service.GetPerson(9999);

            Assert.IsNull(response.Result);
            Assert.AreEqual(response.Report.ErrorList.First(), SWResources.Person_Not_Found);
        }

        [Test]
        public void Create()
        {
            var personCountBefore = UoW.Context.People.ToList().Count;

            var newPerson = new Models.Person
            {
                Name = "Bernard",
                Nickname = "Bernardo"
            };
            var response = Service.CreatePerson(newPerson);

            Assert.IsNotNull(response.Result);
            Assert.AreNotEqual(0, response.Result.Id);
            Assert.AreEqual(0, response.Report.ErrorList.Count);
            Assert.AreEqual(personCountBefore + 1, UoW.Context.People.ToList().Count);
        }

        [Test]
        public void Update()
        {
            var personToUpdate = UoW.Context.People.Find(1);

            personToUpdate.Name = "Thomas";
            personToUpdate.Nickname = "Tom";

            var personMgr = ManagerFactory.Resolve<IPersonManager>();
            personMgr.UoW = UoW;

            var response = Service.UpdatePerson(personMgr.ConvertPersonToWcfPerson(personToUpdate));

            Assert.IsNotNull(response.Result);
            Assert.AreEqual(0, response.Report.ErrorList.Count);
            Assert.AreEqual("Thomas", UoW.Context.People.Find(1).Name);
            Assert.AreEqual("Tom", UoW.Context.People.Find(1).Nickname);
        }

        [Test]
        public void Delete()
        {
            var personToRemove = UoW.Context.People.Find(1);
            Assert.IsNotNull(personToRemove);

            var response = Service.DeletePerson(2);

            Assert.IsTrue(response.Result);
            Assert.AreEqual(0, response.Report.ErrorList.Count);
            var personRemoved = UoW.Context.People.Find(2);
            Assert.IsNull(personRemoved);
            Assert.AreEqual(0, UoW.Context.Events.First(x => x.Id == 1).People.Count);
            Assert.AreEqual(0, UoW.Context.Contributions.Count(x => x.Fk_Person == 2));
        }

        [Test]
        public void Delete_Not_Exist()
        {
            var response = Service.DeletePerson(9999);

            Assert.IsFalse(response.Result);
            Assert.AreEqual(1, response.Report.ErrorList.Count);
        }
         
        #endregion

        #region other methods
        [TestCaseSource("SuscribeUserToAnOpenEventCases")]
        public void SuscribeUserToAnOpenEvent(int userId, int eventId, bool expectedResult, string errorMessage)
        {
            var response = Service.SuscribeUserToAnOpenEvent(userId, eventId);

            Assert.AreEqual(expectedResult, response.Result);
            if (expectedResult)
            {
                Assert.AreEqual(0, response.Report.GetNumberOfErrors());
                Assert.IsTrue(UoW.Context.Events.Where(x => x.Id == eventId).FirstOrDefault().People.Any(x => x.Id == userId));
            }
            else
            {
                Assert.AreEqual(errorMessage, response.Report.ErrorList.First());
            }
        }

        [TestCaseSource("FindPeopleLinkToOpenEventCases")]
        public void FindPeopleLinkToOpenEvent(int eventId, bool shouldNotFailed, int expectedPeople, string errorMessage)
        {
            var response = Service.FindPeopleLinkToOpenEvent(eventId);

            if (shouldNotFailed)
            {
                Assert.AreEqual(0, response.Report.GetNumberOfErrors());
                Assert.AreEqual(expectedPeople, response.Result.Count);
            }
            else
            {
                Assert.AreEqual(errorMessage, response.Report.ErrorList.First());
            }
        }
        #endregion

        #region cases
        public IEnumerable<object[]> SuscribeUserToAnOpenEventCases()
        {
            yield return new object[] { 1, 1, true, "" };//Success case
            yield return new object[] { 2, 1, false, SWResources.SuscribeUserToAnEvent_UserAlreadySuscribed };//User already suscribed
            yield return new object[] { 1, 9999, false, SWResources.Event_Not_Found };//Event not found
            yield return new object[] { 2, 2, false, SWResources.SuscribeUserToAnEvent_EventNotOpen };//Event not open
            yield return new object[] { 9999, 1, false, SWResources.Person_Not_Found };//Person not found
        }

        public IEnumerable<object[]> FindPeopleLinkToOpenEventCases()
        {
            yield return new object[] { 1, true, 1, "" };//Success case with one user
            yield return new object[] { 5, true, 0, "" };//Success case with 0 user
            yield return new object[] { 9999, false, 0, SWResources.Event_Not_Found };//Event not found
            yield return new object[] { 2, false, 0, SWResources.SuscribeUserToAnEvent_EventNotOpen };//Event not open
        }
        #endregion
    }
}
