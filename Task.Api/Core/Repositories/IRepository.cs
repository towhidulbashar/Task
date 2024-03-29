﻿using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Task.Api.Core.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(long id);

        Task<IEnumerable<T>> GetAllAsync(IList<string> includes = null);

        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        void AddAsync(T entity);

        void AddRangeAsync(IEnumerable<T> entities);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);
        void Update(T entity);
    }
}
