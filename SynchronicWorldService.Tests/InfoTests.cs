using System;
using SynchronicWorldService.Test;
using NUnit.Framework;
using SynchronicWorldService.Business;

namespace SynchronicWorldService.Tests
{
    [TestFixture(Category = "Event Tests")]
    public class InfoTests : EffortBaseTest
    {
        #region setup
        private Service Service { get; set; }

        [SetUp]
        public void Setup()
        {
            Service = new Service();
        }
        #endregion

        #region GetDatabaseStatus

        [Test]
        public void GetDatabaseStatus()
        {
            var response = Service.GetDatabaseStatus();

            Assert.AreEqual(0, response.Report.GetNumberOfErrors());
            Assert.AreEqual(String.Format(InfoManager.DatabaseStatus, 5, 3, 6, 3, 3, 2), response.Result);
        }
        #endregion

    }
}
