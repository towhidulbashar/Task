using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task.Api.Core;
using Task.Api.Core.Repositories;

namespace Task.Api.Persistance
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TaskDbContext _context;

        public UnitOfWork(TaskDbContext context)
        {
            _context = context;
            this.WorkItemRepository = new WorkItemRepository(_context);
        }

        public IWorkItemRepository WorkItemRepository { get; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
