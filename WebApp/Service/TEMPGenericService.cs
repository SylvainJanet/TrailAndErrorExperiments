using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using WebApp.Exceptions;
using WebApp.Models;
using WebApp.Repositories;
using WebApp.Tools;

namespace WebApp.Service
{
    public abstract class TEMPGenericService<T> : IGenericService<T> where T : class
    {
        protected IGenericRepository<T> _repository;

        public TEMPGenericService(IGenericRepository<T> repository)
        {
            _repository = repository;
        }

        public IQueryable<T> Collection(bool isIncludes, bool isTracked)
        {
            return _repository.Collection(isIncludes, isTracked);
        }

        public IQueryable<T> CollectionExcludes()
        {
            return Collection(false, false);
        }

        public IQueryable<T> CollectionExcludesTracked()
        {
            return Collection(false, true);
        }

        public IQueryable<T> CollectionIncludes()
        {
            return Collection(true, false);
        }

        public IQueryable<T> CollectionIncludesTracked()
        {
            return Collection(true, true);
        }

        public long Count(Expression<Func<T, bool>> pedicateWhere = null)
        {
            return _repository.Count(pedicateWhere);
        }

        public List<T> FindAll(bool isIncludes, bool isTracked, int page = 1, int maxByPage = int.MaxValue, string searchField = "")
        {
            return GetAll(isIncludes, isTracked, page, maxByPage, OrderExpression(), SearchExpression(searchField));
        }

        public List<T> FindAllExcludes(int page = 1, int maxByPage = int.MaxValue, string searchField = "")
        {
            return FindAll(false, false, page, maxByPage, searchField);
        }

        public List<T> FindAllExcludesTracked(int page = 1, int maxByPage = int.MaxValue, string searchField = "")
        {
            return FindAll(false, true, page, maxByPage, searchField);
        }

        public List<T> FindAllIncludes(int page = 1, int maxByPage = int.MaxValue, string searchField = "")
        {
            return FindAll(true, false, page, maxByPage, searchField);
        }

        public List<T> FindAllIncludesTracked(int page = 1, int maxByPage = int.MaxValue, string searchField = "")
        {
            return FindAll(true, true, page, maxByPage, searchField);
        }

        public T FindById(bool isIncludes, bool isTracked, params object[] objs)
        {
            return _repository.FindById(isIncludes, isTracked, objs);
        }

        public T FindByIdExcludes(params object[] objs)
        {
            return FindById(false, false, objs);
        }

        public T FindByIdExcludesTracked(params object[] objs)
        {
            return FindById(false, true, objs);
        }

        public T FindByIdIncludes(params object[] objs)
        {
            return FindById(true, false, objs);
        }

        public T FindByIdIncludesTracked(params object[] objs)
        {
            return FindById(true, true, objs);
        }

        public List<T> FindByManyId(bool isInclude, bool isTracked, params object[] objs)
        {
            GenericTools.CheckIfObjsIsManyKeysOrIds<T>(objs);
            List<T> lst = new List<T>();
            if (typeof(BaseEntity).IsAssignableFrom(typeof(T)))
            {
                int?[] ids = GenericTools.GetManyIds(objs);
                foreach (int? id in ids)
                {
                    if (!id.HasValue)
                        throw new IdNullExceptionForClass(typeof(T));
                    lst.Add(_repository.FindById(isInclude, isTracked, id.Value));
                }
                return lst;
            }
            else
            {
                object[][] objectskeys = GenericTools.GetManyKeys<T>(objs);
                foreach (object[] keys in objectskeys)
                {
                    lst.Add(_repository.FindById(isInclude, isTracked, keys));
                }
                return lst;
            }
        }

        public List<T> FindManyByIdExcludes(params object[] objs)
        {
            return FindByManyId(false, false, objs);
        }

        public List<T> FindManyByIdExcludesTracked(params object[] objs)
        {
            return FindByManyId(false, true, objs);
        }

        public List<T> FindManyByIdIncludes(params object[] objs)
        {
            return FindByManyId(true, false, objs);
        }

        public List<T> FindManyByIdIncludesTracked(params object[] objs)
        {
            return FindByManyId(true, true, objs);
        }

        public List<T> GetAllBy(bool isIncludes, bool isTracked, Expression<Func<T, bool>> predicateWhere)
        {
            return _repository.GetAllBy(isIncludes, isTracked, predicateWhere);
        }

        public List<T> GetAllByExcludes(Expression<Func<T, bool>> predicateWhere)
        {
            return GetAllBy(false, false, predicateWhere);
        }

        public List<T> GetAllByExcludesTracked(Expression<Func<T, bool>> predicateWhere)
        {
            return GetAllBy(false, true, predicateWhere);
        }

        public List<T> GetAllByIncludes(Expression<Func<T, bool>> predicateWhere)
        {
            return GetAllBy(true, false, predicateWhere);
        }

        public List<T> GetAllByIncludesTracked(Expression<Func<T, bool>> predicateWhere)
        {
            return GetAllBy(true, true, predicateWhere);
        }

        public List<T> GetAll(bool isIncludes, bool isTracked, int page = 1, int maxByPage = int.MaxValue, Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderreq = null, Expression<Func<T, bool>> predicateWhere = null)
        {
            int start = (page - 1) * maxByPage;
            return _repository.GetAll(isIncludes, isTracked, start, maxByPage, orderreq, predicateWhere);
        }

        public List<T> GetAllExcludes(int page = 1, int maxByPage = int.MaxValue, Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderreq = null, Expression<Func<T, bool>> predicateWhere = null)
        {
            return GetAll(false, false, page, maxByPage, orderreq, predicateWhere);
        }

        public List<T> GetAllExcludesTracked(int page = 1, int maxByPage = int.MaxValue, Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderreq = null, Expression<Func<T, bool>> predicateWhere = null)
        {
            return GetAll(false, true, page, maxByPage, orderreq, predicateWhere);
        }

        public List<T> GetAllIncludes(int page = 1, int maxByPage = int.MaxValue, Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderreq = null, Expression<Func<T, bool>> predicateWhere = null)
        {
            return GetAll(true, false, page, maxByPage, orderreq, predicateWhere);
        }

        public List<T> GetAllIncludesTracked(int page = 1, int maxByPage = int.MaxValue, Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderreq = null, Expression<Func<T, bool>> predicateWhere = null)
        {
            return GetAll(true, true, page, maxByPage, orderreq, predicateWhere);
        }

        public List<T> List(bool isIncludes, bool isTracked)
        {
            return _repository.List(isIncludes, isTracked);
        }

        public List<T> ListExcludes()
        {
            return List(false, false);
        }

        public List<T> ListExcludesTracked()
        {
            return List(false, true);
        }

        public List<T> ListIncludes()
        {
            return List(true, false);
        }

        public List<T> ListIncludesTracked()
        {
            return List(true, true);
        }

        public bool NextExist(int page = 1, int maxByPage = int.MaxValue, string searchField = "")
        {
            return (page * maxByPage) < _repository.Count(SearchExpression(searchField));
        }

        public abstract Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> OrderExpression();

        public abstract Expression<Func<T, bool>> SearchExpression(string searchField = "");

        public void Delete(params object[] objs)
        {
            PrepareDelete(objs);
            _repository.Delete(objs);
        }

        public void Delete(T t)
        {
            PrepareDelete(GenericTools.GetKeysValues(t));
            _repository.Delete(t);
        }

        public void Save(T t)
        {
            object[] objs = PrepareSave(t);
            _repository.Save(t, objs);
        }

        public void Update(T t)
        {
            object[] objs = PrepareUpdate(t);
            _repository.Update(t, objs);
        }

        public void UpdateOne(T t, string propertyName, object newValue)
        {
            PrepareUpdateOne(t, propertyName, newValue);
            _repository.UpdateOne(t, propertyName, newValue);
        }

        private bool HasPropertyRelation(Type t1, Type t2)
        {
            return HasPropertyRelationNotList(t1, t2) || HasPropertyRelationList(t1, t2);
        }

        private bool HasPropertyRelationNotList(Type t1, Type t2)
        {
            return GenericTools.DynamicDBTypesForType(t1).Values.Contains(t2);
        }

        private bool HasPropertyRelationList(Type t1, Type t2)
        {
            return GenericTools.DynamicDBListTypesForType(t1).Values.Contains(t2);
        }

        private IEnumerable<string> GetPropsNames(Type t1, Type t2)
        {
            return t1.GetProperties().Where(prop => prop.PropertyType == t2).Select(prop => prop.Name);
        }

        private IEnumerable<Type> GetTypesInRelationWithTHavingTPropertyTAndTNotHavingProperty()
        {
            List<Type> res = new List<Type>();
            foreach (Type type in Assembly.GetAssembly(typeof(T)).GetTypes()
                                                                 .Where(myType => myType.IsClass && !myType.IsAbstract
                                                                                                 && (myType.IsSubclassOf(typeof(BaseEntity)) || myType.IsSubclassOf(typeof(EntityWithKeys)))
                                                                       ))
            {
                if (HasPropertyRelation(type, typeof(T)))
                    if (!HasPropertyRelation(typeof(T), type))
                        res.Add(type);
            }
            return res;
        }

        private IEnumerable<Type> GetTypesInRelationWithTHavingRequiredTProperty()
        {
            // person -> finger
            // person -> action
            throw new NotImplementedException();
        }

        private IEnumerable<Type> GetTypesForWhichTHasManyProperties()
        {
            // person -> color
            // person -> thoughts
            // thoughts -> person
            throw new NotImplementedException();
        }

        private IEnumerable<Type> GetTypesForWhichTHasOneProperty()
        {
            throw new NotImplementedException();
        }

        private void DeleteOtherPropInRelationWithTHavingTPropertyTAndTNotHavingProperty(Type t)
        {
            // address -> person
            // color -> person
            // worldvision -> person
            if (HasPropertyRelationNotList(t, typeof(T)))
            {
                // set to null
                throw new NotImplementedException();
            }
            else
            {
                if (HasPropertyRelationList(t, typeof(T)))
                {
                    // remove from list
                    throw new NotImplementedException();
                }
                else
                {
                    // EXCEPTION
                    throw new NotImplementedException();
                }
            }
        }

        private void DeleteOtherPropInRelationWithTHavingRequiredTProperty(Type t)
        {
            // person -> finger
            // person -> action
            throw new NotImplementedException();
        }

        private void DeleteOtherPropInSeveralManyToManyRelationshipsWithT(Type t)
        {
            // person -> thought
            // thought -> person
            // person -> color
            throw new NotImplementedException();
        }

        private void PrepareDelete(params object[] objs)
        {
            foreach (Type type in GetTypesInRelationWithTHavingTPropertyTAndTNotHavingProperty())
            {
                DeleteOtherPropInRelationWithTHavingTPropertyTAndTNotHavingProperty(type);
                // address -> person
                // color -> person
                // worldvision -> person
            }
            foreach (Type type in GetTypesInRelationWithTHavingRequiredTProperty())
            {
                DeleteOtherPropInRelationWithTHavingRequiredTProperty(type);
                // person -> finger
                // person -> action
            }
            foreach (Type type in GetTypesForWhichTHasManyProperties())
            {
                DeleteOtherPropInSeveralManyToManyRelationshipsWithT(type);
                // person -> thought
                // thought -> person
                // person -> color
            }
        }

        private object[] SetPropToNull(T t,Type type)
        {
            // person -> color
            // person -> thoughts
            // thoughts -> person
            throw new NotImplementedException();
        }

        private object[] GetProp(T t, Type type)
        {
            throw new NotImplementedException();
        }

        private object[] PrepareSave(T t)
        {
            object[] res = new object[0];
            foreach (Type type in GetTypesForWhichTHasManyProperties())
            {
                object[] temp = SetPropToNull(t, type);
                res = (object[])res.Concat(temp);
                // person -> color
                // person -> thoughts
                // thoughts -> person
            }
            foreach (Type type in GetTypesForWhichTHasOneProperty())
            {
                object[] temp = GetProp(t, type);
                res = (object[])res.Concat(temp);
            }
            return res;
        }

        private void UpdateOtherPropInRelationWithTHavingRequiredTProperty(Type t)
        {
            throw new NotImplementedException();
        }

        private void UpdateOneOtherPropInRelationWithTHavingRequiredTProperty(string propertyName, object newValue)
        {
            throw new NotImplementedException();
        }

        private object[] UpdateOtherPropForWhichTHasManyProperties(Type t)
        {
            // person -> thought
            // thought -> person
            // person -> color
            throw new NotImplementedException();
        }

        private object[] PrepareUpdate(T t)
        {
            object[] res = new object[0];
            foreach (Type type in GetTypesInRelationWithTHavingRequiredTProperty())
            {
                UpdateOtherPropInRelationWithTHavingRequiredTProperty(type);
                // person -> finger
                // person -> action
            }
            foreach (Type type in GetTypesForWhichTHasManyProperties())
            {
                object[] temp = UpdateOtherPropForWhichTHasManyProperties(type);
                res = (object[])res.Concat(temp);
                // person -> thought
                // thought -> person
                // person -> color
            }
            foreach (Type type in GetTypesForWhichTHasOneProperty())
            {
                object[] temp = GetProp(t, type);
                res = (object[])res.Concat(temp);
            }
            return res;
        }

        private void PrepareUpdateOne(T t, string propertyName, object newValue)
        {
            if (GetTypesInRelationWithTHavingRequiredTProperty().Select(type => t.GetType().GetProperties().Where(prop => prop.PropertyType == type).Select(prop => prop.Name).Single()).Contains(propertyName))
            {
                UpdateOneOtherPropInRelationWithTHavingRequiredTProperty(propertyName, newValue);
            }
        }
    }
}