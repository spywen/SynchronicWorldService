using NUnit.Framework;
using SynchronicWorldService.Business;
using System.Linq;

namespace SynchronicWorldService.Test
{
    [TestFixture(Category="Person Tests")]
    public class PersonTests : EffortBaseTest
    {
        #region setup
        private Service service { get; set; }

        [SetUp]
        public void SetUp()
        {
            service = new Service();
        }
        #endregion

        #region integration tests

        [Test]
        public void Get()
        {
            var response = service.GetPerson(1);

            Assert.AreEqual("Laurent", response.Result.Name);
        }

        [Test]
        public void Get_Not_Found()
        {
            var response = service.GetPerson(9999);

            Assert.IsNull(response.Result);
            Assert.AreEqual(response.Report.ErrorList.First(), SynchronicWorldServiceResources.Person_Not_Found);
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
            var response = service.CreatePerson(newPerson);

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

            var response = service.UpdatePerson(new Models.Person(personToUpdate));

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

            var response = service.DeletePerson(2);

            Assert.IsTrue(response.Result);
            Assert.AreEqual(0, response.Report.ErrorList.Count);
            var personRemoved = UoW.Context.People.Find(2);
            Assert.IsNull(personRemoved);
            Assert.AreEqual(0, UoW.Context.Events.First(x => x.Id == 1).People.Count);
        }

        [Test]
        public void Delete_Not_Exist()
        {
            var response = service.DeletePerson(9999);

            Assert.IsFalse(response.Result);
            Assert.AreEqual(1, response.Report.ErrorList.Count);
        }
         
        #endregion
    }
}
