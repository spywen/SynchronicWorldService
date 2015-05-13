using SynchronicWorldService.DataAccess;

namespace SynchronicWorldService.Test
{
    public class EffortUnitOfWork : IUnitOfWork
    {
        private ISWContext _context;

        public ISWContext Context { get { return _context; } set { _context = value; } }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            //Empty in the testing case to allowed the multiple requests
        }
    }
}
