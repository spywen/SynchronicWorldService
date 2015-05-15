namespace SynchronicWorldService.DataAccess
{
    /// <summary>
    /// Unit of work which contains SW context
    /// This class enable us to contact the database by open and closing connection each time that we will use service methods
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Context
        /// </summary>
        private readonly ISWContext _context;

        /// <summary>
        /// Context
        /// </summary>
        ISWContext IUnitOfWork.Context
        {
            get { return _context; }
        }

        /// <summary>
        /// Unit of work which instantiate the context
        /// </summary>
        public UnitOfWork()
        {
            _context = DataAccessFactory.Resolve<ISWContext>();
        }

        /// <summary>
        /// Dispose the context
        /// If we use 'using' c# feature to call the UoW the context will be automaticaly disposed at the end
        /// </summary>
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
