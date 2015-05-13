using Microsoft.Practices.Unity;

namespace SynchronicWorldService.DataAccess
{
    public class DataAccessFactory
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
        static DataAccessFactory()
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
            //var test = new SWContext();
            //test.Entry(new Event()).State = System.Data.Entity.EntityState.
            Container.RegisterType<IUnitOfWork, UnitOfWork>(new TransientLifetimeManager());
            Container.RegisterType<ISWContext, SWContext>(new TransientLifetimeManager());
            Container.RegisterType<SWContext>(new InjectionConstructor());
        } 

        #endregion
    }
}
