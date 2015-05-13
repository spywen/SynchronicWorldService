using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;

namespace SynchronicWorldService.DataAccess
{
    public partial class SWContext : DbContext, ISWContext
    {
        public SWContext(EntityConnection connection) : base(connection, true){}
    }
}
