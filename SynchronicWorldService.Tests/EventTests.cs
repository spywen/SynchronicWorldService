using NUnit.Framework;
using SynchronicWorldService.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using SynchronicWorldService.Utils;

namespace SynchronicWorldService.Test
{
    [TestFixture(Category = "Event Tests")]
    public class EventTests : EffortBaseTest
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
        public void GetAll()
        {
            var eventsCount = UoW.Context.Events.ToList().Count;

            Assert.AreEqual(Service.GetAllEvents().Result.Count, eventsCount);
            Assert.AreEqual(Service.GetAllEvents().Result.First(x => x.Id == 1).Status.Code, "Open");
        }

        [Test]
        public void Get()
        {
            var response = Service.GetEvent(1);

            Assert.AreEqual("First Event", response.Result.Name);
        }

        [Test]
        public void Get_Not_Found()
        {
            var response = Service.GetEvent(9999);

            Assert.IsNull(response.Result);
            Assert.AreEqual(response.Report.ErrorList.First(), SWResources.Event_Not_Found);
        }

        [Test]
        public void Create()
        {
            var eventsCountBefore = UoW.Context.Events.ToList().Count;

            var newEvent = new Models.Event{
                Name = "My Event",
                Description = "quick",
                Address = "address",
                Date = DateTime.Now,
                Status = new Models.EventStatus{Id = 1, Code = "", Value = ""},
                Type = new Models.EventType { Id = 1, Code = "", Value = "" }
            };
            var response = Service.CreateEvent(newEvent);

            Assert.IsNotNull(response.Result);
            Assert.AreNotEqual(0, response.Result.Id);
            Assert.AreEqual(0, response.Report.ErrorList.Count);
            Assert.AreEqual(eventsCountBefore + 1, UoW.Context.Events.ToList().Count);
        }

        [Test]
        public void Update()
        {
            var eventToUpdate = UoW.Context.Events.Find(1);

            eventToUpdate.Name = "New Name";

            var response = Service.UpdateEvent(new Models.Event(eventToUpdate));

            Assert.IsNotNull(response.Result);
            Assert.AreEqual(0, response.Report.ErrorList.Count);
            Assert.AreEqual("New Name", UoW.Context.Events.Find(1).Name);
        }

        [Test]
        public void Update_Status()
        {
            var eventToUpdate = UoW.Context.Events.Find(1);
            Assert.AreEqual(1, eventToUpdate.Fk_Status);
            Assert.AreEqual("Open", eventToUpdate.EventStatus.Code);

            eventToUpdate.Fk_Status = 2;
            eventToUpdate.EventStatus = UoW.Context.EventStatuses.Find(2);

            var response = Service.UpdateEvent(new Models.Event(eventToUpdate));

            Assert.IsNotNull(response.Result);
            Assert.AreEqual(0, response.Report.ErrorList.Count);
            Assert.AreEqual(2, UoW.Context.Events.Find(1).Fk_Status);
            Assert.AreEqual("Closed", UoW.Context.Events.Find(1).EventStatus.Code);
        }

        [Test]
        public void Delete()
        {
            var eventToRemove = UoW.Context.Events.Find(1);
            Assert.IsNotNull(eventToRemove);

            var response = Service.DeleteEvent(1);

            Assert.IsTrue(response.Result);
            Assert.AreEqual(0, response.Report.ErrorList.Count);
            var eventRemoved = UoW.Context.Events.Find(1);
            Assert.IsNull(eventRemoved);
            Assert.AreEqual(0, UoW.Context.People.First(x => x.Id == 2).Events.Count);
        }

        [Test]
        public void Delete_Not_Exist()
        {
            var response = Service.DeleteEvent(9999);

            Assert.IsFalse(response.Result);
            Assert.AreEqual(1, response.Report.ErrorList.Count);
        }
        #endregion

        #region Search for events

        [TestCaseSource("SearchForEventsCases")]
        public void SearchForEvents(Models.Facades.EventSearchFacade searchObject, int idOfTheFirstEventFound, int countOfEventsFound)
        {
            var response = Service.SearchForEvents(searchObject);

            Assert.AreEqual(0, response.Report.GetNumberOfErrors());
            Assert.AreEqual(idOfTheFirstEventFound, response.Result.First().Id);
            Assert.AreEqual(countOfEventsFound, response.Result.Count);
        }
        #endregion

        #region other methods
        [Test]
        public void DeleteClosedEvents()
        {
            var response = Service.DeleteClosedEvents();

            Assert.IsTrue(response.Result);
            Assert.AreEqual(0, response.Report.GetNumberOfErrors());
            Assert.AreEqual(String.Format(SWResources.Closed_Events_Removed, 2), response.Report.InfoList.First());
        }

        [Test]
        public void UpgradePendingEventsAsOpen()
        {
            var response = Service.UpgradePendingEventsAsOpen();

            Assert.IsTrue(response.Result);
            Assert.AreEqual(0, response.Report.GetNumberOfErrors());
            Assert.AreEqual(String.Format(SWResources.Upgrade_Events_Status_From_Pending_To_Open_Done, 1), response.Report.InfoList.First());
        }
        #endregion

        #region cases

        public IEnumerable<object[]> SearchForEventsCases()
        {
            yield return new object[] { new Models.Facades.EventSearchFacade(), 5, 5};
            yield return new object[] { new Models.Facades.EventSearchFacade { Name = "First Event" }, 1, 1 };
            yield return new object[] { new Models.Facades.EventSearchFacade { Date = new DateTime(2016,12,06)}, 3, 1 };
            yield return new object[] { new Models.Facades.EventSearchFacade { StartDate = new DateTime(2000, 12, 06), EndDate = new DateTime(2016, 12, 20) }, 3, 3 };
            yield return new object[] { new Models.Facades.EventSearchFacade { EventStatusCode = Models.EventStatusCode.Pending.ToString()}, 4, 1 };
            yield return new object[] { new Models.Facades.EventSearchFacade { EventTypeCode = Models.EventTypeCode.Party.ToString() }, 4, 2 };
        }

        #endregion
    }
}
