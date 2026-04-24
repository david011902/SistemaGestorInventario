using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Abstractions
{
    public interface IUnitOfWork :IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task ExecuteTransactionAsync(Func<Task> action);

    }
}
