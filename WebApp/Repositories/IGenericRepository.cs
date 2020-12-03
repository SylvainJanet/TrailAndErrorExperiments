using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WebApp.Models;

namespace WebApp.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        void Add(T t);
        IQueryable<T> CollectionExcludes();
        IQueryable<T> CollectionExcludesTracked();
        IQueryable<T> CollectionIncludes();
        IQueryable<T> CollectionIncludesTracked();
        void Commit();
        long Count(Expression<Func<T, bool>> predicateWhere = null);
        void Delete(int id);
        void Delete(T t);
        T FindByIdExcludes(int id);
        T FindByIdExcludesTracked(int id);
        T FindByIdIncludes(int id);
        T FindByIdIncludesTracked(int id);
        List<T> GetAllExcludes(int start = 0, int maxByPage = int.MaxValue, Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderreq = null, Expression<Func<T, bool>> predicateWhere = null);
        List<T> GetAllExcludesTracked(int start = 0, int maxByPage = int.MaxValue, Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderreq = null, Expression<Func<T, bool>> predicateWhere = null);
        List<T> GetAllIncludes(int start = 0, int maxByPage = int.MaxValue, Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderreq = null, Expression<Func<T, bool>> predicateWhere = null);
        List<T> GetAllIncludesTracked(int start = 0, int maxByPage = int.MaxValue, Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderreq = null, Expression<Func<T, bool>> predicateWhere = null);
        List<T> ListExcludes();
        List<T> ListExcludesTracked();
        List<T> ListIncludes();
        List<T> ListIncludesTracked();
        void Modify(T t);
        void Remove(int id);
        void Remove(T t);
        void Save(T t, params object[] objs);
        void Update(T t, params object[] objs);
        void UpdateOne(T t, string propertyName, object newValue);
    }
}