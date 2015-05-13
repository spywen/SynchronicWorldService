namespace SynchronicWorldService.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISWContext _context;

        public UnitOfWork()
        {
            _context = DataAccessFactory.Resolve<ISWContext>();
        }
        ISWContext IUnitOfWork.Context
        {
            get { return _context; }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
