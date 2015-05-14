using System.Linq;
using NUnit.Framework;
using SynchronicWorldService.Test;
using SynchronicWorldService.Utils;

namespace SynchronicWorldService.Tests
{
    [TestFixture(Category = "Contribution Tests")]
    public class ContributionTests : EffortBaseTest
    {
        #region setup
        private Service Service { get; set; }

        [SetUp]
        public void Setup()
        {
            Service = new Service();
        }
        #endregion

        #region other methods

        [TestCase(5, 3, Description = "Event with contributions")]
        [TestCase(3, 0, Description = "Event without contributions")]
        public void GetEventContributions(int eventId, int expectedContribs)
        {
            var svcResponse = Service.GetEventContributions(eventId);

            Assert.AreEqual(0, svcResponse.Report.GetNumberOfErrors());
            Assert.AreEqual(expectedContribs, svcResponse.Result.Count);
        }

        [Test]
        public void GetEventContributions_EventNotFound()
        {
            var svcResponse = Service.GetEventContributions(9999);

            Assert.AreEqual(1, svcResponse.Report.GetNumberOfErrors());
            Assert.AreEqual(SWResources.Event_Not_Found, svcResponse.Report.ErrorList.First());
        }

        [TestCase(1, 4, Description = "Person with contributions")]
        [TestCase(3, 0, Description = "Person without contributions")]
        public void GetPersonContributions(int userId, int expectedContribs)
        {
            var svcResponse = Service.GetPersonContributions(userId);

            Assert.AreEqual(0, svcResponse.Report.GetNumberOfErrors());
            Assert.AreEqual(expectedContribs, svcResponse.Result.Count);
        }

        [Test]
        public void GetPersonContributions_PersonNotFound()
        {
            var svcResponse = Service.GetPersonContributions(9999);

            Assert.AreEqual(1, svcResponse.Report.GetNumberOfErrors());
            Assert.AreEqual(SWResources.Person_Not_Found, svcResponse.Report.ErrorList.First());
        }

        [TestCase(1, 2, Description = "Person which have 3 contributions but only two for open events")]
        [TestCase(2, 2, Description = "Person which have 2 contributions only for open events")]
        public void DeleteAllPersonContributionsForOpenEvents(int userId, int expectedDeletedContribution)
        {
            var initialCountOfContribs = UoW.Context.Contributions.Count();

            var svcResponse = Service.DeleteAllPersonContributionsForOpenEvents(userId);

            Assert.AreEqual(0, svcResponse.Report.GetNumberOfErrors());
            Assert.IsTrue(svcResponse.Report.InfoList.First().Contains(expectedDeletedContribution.ToString()));
            Assert.AreEqual(initialCountOfContribs - expectedDeletedContribution, UoW.Context.Contributions.Count());
        }

        [Test]
        public void DeleteAllPersonContributionsForOpenEvents_PersonNotFound()
        {
            var initialCountOfContribs = UoW.Context.Contributions.Count();

            var svcResponse = Service.DeleteAllPersonContributionsForOpenEvents(9999);

            Assert.AreEqual(1, svcResponse.Report.GetNumberOfErrors());
            Assert.AreEqual(SWResources.Person_Not_Found, svcResponse.Report.ErrorList.First());
            Assert.AreEqual(initialCountOfContribs, UoW.Context.Contributions.Count());
        }

        #endregion
    }
}
