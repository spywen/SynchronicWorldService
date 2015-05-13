using System;
using System.Data.Entity;

namespace SynchronicWorldService.DataAccess
{
    public interface ISWContext : IDisposable
    {
        IDbSet<Person> People { get; set; }
        IDbSet<Contribution> Contributions { get; set; }
        IDbSet<ContributionType> ContributionTypes { get; set; }
        IDbSet<Event> Events { get; set; }
        IDbSet<EventStatus> EventStatuses { get; set; }
        IDbSet<EventType> EventTypes { get; set; }

        int SaveChanges();
        void SetModified(object entity);
        void SetAdded(object entity);
        void SetDeleted(object entity);
        void GetId(object entity);
    }


}
