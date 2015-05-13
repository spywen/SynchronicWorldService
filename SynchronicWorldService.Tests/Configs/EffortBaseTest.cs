using Effort.DataLoaders;
using NUnit.Framework;
using SynchronicWorldService.DataAccess;
using System;
using System.Data.Entity.Core.EntityClient;
using System.Text;
using Microsoft.Practices.Unity;

namespace SynchronicWorldService.Test
{
    public class EffortBaseTest
    {
        [TestFixtureSetUp]
        public virtual void RegisterEffort()
        {
            Effort.Provider.EffortProviderConfiguration.RegisterProvider();
        }

        /// <summary>
        /// Context for Ngppdb
        /// </summary>
        public ISWContext Context { get; set; }

        /// <summary>
        /// UoW
        /// </summary>
        public IUnitOfWork UoW { get; set; }

        /// <summary>
        /// CSVs folder path
        /// </summary>
        private const string CsvPath = "\\Database";

        /// <summary>
        /// Context names (present in the App.config)
        /// </summary>
        private const string SynchronicWorldContextContextName = "SWContext";

        /// <summary>
        /// Initialization of the context by a set of csv files
        /// </summary>
        [SetUp]
        public virtual void SetUp()
        {
            //CSV files are stored in a specific folder : Data_For_EffortUnitTests
            string dir = Environment.CurrentDirectory + CsvPath;

            //Load DS related data
            IDataLoader ngLoader = new CsvDataLoader(dir);
            try
            {
                EntityConnection synchronicWorldContextConnection = Effort.EntityConnectionFactory.CreateTransient(
                    new StringBuilder().Append("name=").Append(SynchronicWorldContextContextName).ToString(),
                    ngLoader);
                //Context creation
                Context = new SWContext(synchronicWorldContextConnection);
            }
            catch (Exception e)
            {
                var error = new StringBuilder()
                    .Append("An error occured when effort try to import CSVs files and/or load the context... Please check the next logs :")
                    .Append(" MESSAGE : ")
                    .Append(e.Message)
                    .Append(" INNEREXCEPTION : ")
                    .Append(e.InnerException)
                    .ToString();
                throw new Exception(error);
            }

            UoW = new EffortUnitOfWork { Context = Context };
            DataAccessFactory.Container.RegisterInstance(typeof(IUnitOfWork), UoW);
        }

        [TearDown]
        public virtual void TearDown()
        {
            DataAccessFactory.SetDefaultDependencies();
        }
    }
}
