using System;

namespace SynchronicWorldService.DataAccess
{
    /// <summary>
    /// Unit of work which contains SW context
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
       ISWContext Context { get; }
    }
}
