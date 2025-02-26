using System;
using System.Collections.Generic;

namespace DAL.Abstractions
{
    public interface IRepository<T>
    {
        void Add(T entity);
        T Get(Func<T, bool> predicate);
        IEnumerable<T> GetAll();
        void Remove(T entity);
        void SaveChanges();
    }
}
