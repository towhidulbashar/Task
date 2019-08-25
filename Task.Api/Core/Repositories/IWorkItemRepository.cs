using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task.Api.Core.Domain;

namespace Task.Api.Core.Repositories
{
    public interface IWorkItemRepository : IRepository<WorkItem>
    {
        IEnumerable<WorkItem> GetCurrentUserTasks(string userId);
    }
}
