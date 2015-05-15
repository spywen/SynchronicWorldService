using Microsoft.Practices.Unity;

namespace SynchronicWorldService.Business
{
    /// <summary>
    /// Manager factory for dependencies injections of the business classes
    /// </summary>
    public class ManagerFactory
    {
        #region Variables

        private static UnityContainer _container; 

        #endregion

        #region Properties

        /// <summary>
        /// Public reference to the unity container which will
        /// allow the ability to register instrances or take
        /// other actions on the container.
        /// </summary>
        public static UnityContainer Container
        {
            get { return _container ?? (_container = new UnityContainer()); }
        } 

        #endregion

        #region Constructor

        /// <summary>
        /// Static constructor for DependencyFactory which will
        /// initialize the unity container.
        /// </summary>
        static ManagerFactory()
        {
            SetDefaultDependencies();
        } 

        #endregion

        #region Public Methods

        /// <summary>
        /// Resolves the type parameter T to an instance of the appropriate type.
        /// </summary>
        /// <typeparam name="T">Type of object to return</typeparam>
        public static T Resolve<T>()
        {
            T ret = default(T);

            if (Container.IsRegistered(typeof(T)))
            {
                ret = Container.Resolve<T>();
            }

            return ret;
        }

        /// <summary>
        /// Set default dependencies of manager factory
        /// </summary>
        public static void SetDefaultDependencies()
        {
            Container.RegisterType<IEventManager, EventManager>(new TransientLifetimeManager());
            Container.RegisterType<IPersonManager, PersonManager>(new TransientLifetimeManager());
            Container.RegisterType<IEventStatusManager, EventStatusManager>(new TransientLifetimeManager());
            Container.RegisterType<IEventTypeManager, EventTypeManager>(new TransientLifetimeManager());
            Container.RegisterType<IContributionManager, ContributionManager>(new TransientLifetimeManager());
        } 

        #endregion
    }
}
