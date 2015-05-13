using System;

namespace SynchronicWorldService.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
       ISWContext Context { get; }
    }
}
