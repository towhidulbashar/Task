using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
        private readonly TaskDbContext identityDbContext;
        public WorkItemRepository(TaskDbContext context) : base(context)
        {
            identityDbContext = context;
        }
        public IEnumerable<WorkItem> GetDoneWorkItem()
        {
            throw new NotImplementedException();
        }
    }
}
