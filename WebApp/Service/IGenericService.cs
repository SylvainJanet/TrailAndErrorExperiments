using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WebApp.Models;

namespace WebApp.Service
{
    public interface IGenericService<T> where T : BaseEntity
    {
        IQueryable<T> CollectionExcludes();
        IQueryable<T> CollectionExcludesTracked();
        IQueryable<T> CollectionIncludes();
        IQueryable<T> CollectionIncludesTracked();
        long Count(Expression<Func<T, bool>> pedicateWhere = null);
        void Delete(int? id);
        void Delete(T t);
        List<T> FindAllExcludes(int page = 1, int maxByPage = int.MaxValue, string searchField = "");
        List<T> FindAllExcludesTracked(int page = 1, int maxByPage = int.MaxValue, string searchField = "");
        List<T> FindAllIncludes(int page = 1, int maxByPage = int.MaxValue, string searchField = "");
        List<T> FindAllIncludesTracked(int page = 1, int maxByPage = int.MaxValue, string searchField = "");
        T FindByIdExcludes(int? id);
        T FindByIdExcludesTracked(int? id);
        T FindByIdIncludes(int? id);
        T FindByIdIncludesTracked(int? id);
        List<T> FindManyByIdExcludesTracked(int?[] ids);
        List<T> FindManyByIdIncludes(int?[] ids);
        List<T> FindManyByIdIncludesTracked(int?[] ids);
        List<T> FindManyByIdExcludes(int?[] ids);
        List<T> GetAllExcludes(int page = 1, int maxByPage = int.MaxValue, Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderreq = null, Expression<Func<T, bool>> predicateWhere = null);
        List<T> GetAllExcludesTracked(int page = 1, int maxByPage = int.MaxValue, Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderreq = null, Expression<Func<T, bool>> predicateWhere = null);
        List<T> GetAllIncludes(int page = 1, int maxByPage = int.MaxValue, Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderreq = null, Expression<Func<T, bool>> predicateWhere = null);
        List<T> GetAllIncludesTracked(int page = 1, int maxByPage = int.MaxValue, Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderreq = null, Expression<Func<T, bool>> predicateWhere = null);
        List<T> ListExcludes();
        List<T> ListExcludesTracked();
        List<T> ListIncludes();
        List<T> ListIncludesTracked();
        bool NextExist(int page = 1, int maxByPage = int.MaxValue, string searchField = "");
        Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> OrderExpression();
        void Save(T t, params object[] objs);
        Expression<Func<T, bool>> SearchExpression(string searchField = "");
        void Update(T t, params object[] objs);
        void UpdateOne(T t, string properyname, object newValue);
    }
}