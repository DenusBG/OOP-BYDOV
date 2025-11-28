// IRepository.cs
using System;
using System.Collections.Generic;

public interface IRepository<T> where T : class
{
    void Add(T entity);
    void Remove(Guid id);
    T Find(Guid id);
    IEnumerable<T> All();
    IEnumerable<T> Where(Func<T, bool> predicate);
}