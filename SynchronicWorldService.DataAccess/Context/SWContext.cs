using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;

namespace SynchronicWorldService.DataAccess
{
    /// <summary>
    /// Synchronic world context
    /// </summary>
    public partial class SWContext : DbContext, ISWContext
    {
        public SWContext(EntityConnection connection) : base(connection, true){}
    }
}
