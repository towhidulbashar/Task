using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task.Api.Core.Domain;
using Task.Api.Core.Repositories;

namespace Task.Api.Persistance
{
    public class WorkItemRepository : Repository<WorkItem>, IWorkItemRepository
    {
        private readonly TaskDbContext taskDbContext;
        public WorkItemRepository(TaskDbContext context) : base(context)
        {
            taskDbContext = context;
        }
        public IEnumerable<WorkItem> GetCurrentUserTasks(string userId)
        {
            return taskDbContext.WorkItems
                .Where(x => x.ApplicationUserId == userId)
                .Include(x => x.AssignedTo);            
        }
    }
}
