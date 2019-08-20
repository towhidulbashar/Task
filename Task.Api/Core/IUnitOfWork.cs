using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task.Api.Core.Repositories;

namespace Task.Api.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IWorkItemRepository WorkItemRepository { get; }

        int Complete();
    }
}
