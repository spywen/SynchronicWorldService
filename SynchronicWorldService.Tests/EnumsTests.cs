using NUnit.Framework;
using SynchronicWorldService.Models;

namespace SynchronicWorldService.Test
{
    [TestFixture(Category = "Event Tests")]
    public class EnumsTests : EffortBaseTest
    {
        #region setup
        private Service Service { get; set; }

        [SetUp]
        public void SetUp()
        {
            Service = new Service();
        }
        #endregion

        #region Event type

        [Test]
        public void GetAllEventType()
        {
            var enumsFound = Service.GetAllEventsType();

            Assert.AreEqual(0, enumsFound.Report.GetNumberOfErrors());
            Assert.AreEqual(2, enumsFound.Result.Count);
        }

        [Test]
        public void FindByCodeEventType()
        {
            var enumsFound = Service.FindEventTypeByCode(EventTypeCode.Party.ToString());

            Assert.AreEqual(0, enumsFound.Report.GetNumberOfErrors());
            Assert.AreEqual(1, enumsFound.Result.Id);
            Assert.AreEqual(EventTypeCode.Party.ToString(), enumsFound.Result.Code);
            Assert.AreEqual("Party", enumsFound.Result.Value);
        }

        #endregion

        #region Event type

        [Test]
        public void GetAllEventStatus()
        {
            var enumsFound = Service.GetAllEventsStatus();

            Assert.AreEqual(0, enumsFound.Report.GetNumberOfErrors());
            Assert.AreEqual(3, enumsFound.Result.Count);
        }

        [Test]
        public void FindByCodeEventStatus()
        {
            var enumsFound = Service.FindEventStatusByCode(EventStatusCode.Open.ToString());

            Assert.AreEqual(0, enumsFound.Report.GetNumberOfErrors());
            Assert.AreEqual(1, enumsFound.Result.Id);
            Assert.AreEqual(EventStatusCode.Open.ToString(), enumsFound.Result.Code);
            Assert.AreEqual("Open", enumsFound.Result.Value);
        }

        #endregion
    }
}
