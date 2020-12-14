using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using WebApp.Exceptions;
using WebApp.Models;
using WebApp.Repositories;
using WebApp.Tools.Generic;

namespace WebApp.Tools
{
    public abstract class GenericTools
    {
        /// <summary>
        /// Test if a type implements <see cref="IList{}"/> of <typeparamref name="T"/>, and if so, determine <typeparamref name="T"/>.
        /// </summary>
        /// <remarks>
        /// Thanks internet for this one.
        /// </remarks>
        /// <param name="type">Type to test : check if it is <see cref="IList{T}"/></param>
        /// <param name="innerType">If <paramref name="type"/> is <see cref="IList{T}"/>, returns the inner type <typeparamref name="T"/></param>
        /// <returns>A boolean representing if <paramref name="type"/> is of type <see cref="IList{T}"/></returns>
        public static bool TryListOfWhat(Type type, out Type innerType)
        {
            Contract.Requires(type != null);

            var interfaceTest = new Func<Type, Type>(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IList<>) ? i.GetGenericArguments().Single() : null);

            innerType = interfaceTest(type);
            if (innerType != null)
            {
                return true;
            }

            foreach (var i in type.GetInterfaces())
            {
                innerType = interfaceTest(i);
                if (innerType != null)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Get the names of the properties of <typeparamref name="T"/> that have the annotation 
        /// <see cref="KeyAttribute"/>. The order will be the same as the order in the declaration of <typeparamref name="T"/>.
        /// </summary>
        /// <remarks>
        /// If <typeparamref name="T"/> derives from <see cref="BaseEntity"/>, returns <see langword="null"/>.
        /// </remarks>
        /// <typeparam name="T">The type invistigated.</typeparam>
        /// <returns>The properties names that are keys, in the same order as in the declaration of <typeparamref name="T"/>. If
        /// <typeparamref name="T"/> derives from <see cref="BaseEntity"/>, returns <see langword="null"/>.</returns>
        private static string[] KeyPropertiesNames<T>()
        {
            if (typeof(T).IsSubclassOf(typeof(BaseEntity)))
                return null;
            return typeof(T).GetProperties().Where(p => p.GetCustomAttribute(typeof(KeyAttribute), false) != null)
                                            .Select(p => p.Name)
                                            .ToArray();
        }

        /// <summary>
        /// Tells if <typeparamref name="T"/> is in a relationship with any other class in DB.
        /// </summary>
        /// <typeparam name="T">The type invistigated.</typeparam>
        /// <returns><see langword="true"/> if <typeparamref name="T"/> is in any relationship, <see langword="false"/> otherwise.</returns>
        public static bool HasDynamicDBTypeOrListType<T>()
        {
            return DynamicDBListTypes<T>().Count != 0 || DynamicDBTypes<T>().Count != 0;
        }

        /// <summary>
        /// An object <c>obj</c> of class <typeparamref name="T"/> has properties <c>obj.PropName</c> of
        /// class <see cref="IList"/>&lt;<c>ClassType</c>&gt; where <c>ClassType</c> is in a <see cref="DbSet"/> of the generic repository 
        /// <see cref="DataContext"/>. 
        /// <br/>
        /// This is every { PropName : ClassType }
        /// </summary>
        /// <typeparam name="T">The type invistigated.</typeparam>
        /// <returns>The dictionary</returns>
        public static Dictionary<string, Type> DynamicDBListTypes<T>()
        {
            return DynamicDBListTypesForType(typeof(T));
        }

        /// <summary>
        /// An object <c>obj</c> of class <typeparamref name="T"/> has properties <c>obj.PropName</c> of
        /// class <c>ClassType</c> which is in a <see cref="DbSet"/> of the generic repository 
        /// <see cref="DataContext"/>. 
        /// <br/>
        /// This is every { PropName : ClassType }
        /// </summary>
        /// <typeparam name="T">The type invistigated.</typeparam>
        /// <returns>The dictionary</returns>
        public static Dictionary<string, Type> DynamicDBTypes<T>()
        {
            return DynamicDBTypesForType(typeof(T));
        }

        /// <summary>
        /// An object <c>obj</c> of class <typeparamref name="T"/> has properties <c>obj.PropName</c> of
        /// class <see cref="IList"/>&lt;<c>ClassType</c>&gt; where <c>ClassType</c> is in a <see cref="DbSet"/> of the generic repository 
        /// <see cref="DataContext"/>. 
        /// <br/>
        /// This is every { PropName : ClassType }
        /// </summary>
        /// <param name="t">The type invistigated.</param>
        /// <returns>The dictionary</returns>
        private static Dictionary<string, Type> DynamicDBListTypesForType(Type t)
        {
            Dictionary<string, Type> res = new Dictionary<string, Type>();
            foreach (PropertyInfo property in t.GetProperties())
            {
                if (TryListOfWhat(property.PropertyType, out Type innerType))
                {
                    if (innerType.IsSubclassOf(typeof(BaseEntity)) ||
                        innerType.GetProperties().Where(p => p.GetCustomAttribute(typeof(KeyAttribute), false) != null)
                                                 .Count() > 0)
                    {
                        res.Add(property.Name, innerType);
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// An object <c>obj</c> of class <typeparamref name="T"/> has properties <c>obj.PropName</c> of
        /// class <c>ClassType</c> which is in a <see cref="DbSet"/> of the generic repository 
        /// <see cref="DataContext"/>. 
        /// <br/>
        /// This is every { PropName : ClassType }
        /// </summary>
        /// <param name="t">The type invistigated.</param>
        /// <returns>The dictionary</returns>
        private static Dictionary<string, Type> DynamicDBTypesForType(Type t)
        {
            Dictionary<string, Type> res = new Dictionary<string, Type>();
            foreach (PropertyInfo property in t.GetProperties())
            {
                if (property.PropertyType.IsSubclassOf(typeof(BaseEntity)) ||
                    property.PropertyType.GetProperties().Where(p => p.GetCustomAttribute(typeof(KeyAttribute), false) != null)
                                                         .Count() > 0)
                {
                    res.Add(property.Name, property.PropertyType);
                }
            }
            return res;
        }

        /// <summary>
        /// Setup all possible and necessary Include(propertyname) for <see cref="DbSet"/> queries.
        /// </summary>
        /// <param name="myDbContext">The context used for the query</param>
        /// <typeparam name="T">The type invistigated.</typeparam>
        /// <returns>The query. Essentially, context.DbSetName.AsNoTracking().Include(...).Include(...)....Include(...)</returns>
        public static IQueryable<T> QueryTInclude<T>(MyDbContext myDbContext) where T : class
        {
            IQueryable<T> req = myDbContext.Set<T>().AsNoTracking().AsQueryable();
            foreach (string name in DynamicDBListTypes<T>().Keys.ToList())
            {
                req = req.Include(name);
            }
            foreach (string name in DynamicDBTypes<T>().Keys.ToList())
            {
                req = req.Include(name);
            }
            return req;
        }

        /// <summary>
        /// Setup all possible and necessary Include(propertyname) for <see cref="DbSet"/> queries.
        /// </summary>
        /// <param name="myDbContext">The context used for the query</param>
        /// <typeparam name="T">The type invistigated.</typeparam>
        /// <returns>The query. Essentially, context.DbSetName.Include(...).Include(...)....Include(...)</returns>
        public static IQueryable<T> QueryTIncludeTracked<T>(MyDbContext myDbContext) where T : class
        {
            IQueryable<T> req = myDbContext.Set<T>().AsQueryable();
            foreach (string name in DynamicDBListTypes<T>().Keys.ToList())
            {
                req = req.Include(name);
            }
            foreach (string name in DynamicDBTypes<T>().Keys.ToList())
            {
                req = req.Include(name);
            }
            return req;
        }

        /// <summary>
        /// Checks if <paramref name="objs"/> is either
        /// <list type="bullet">
        /// <item>
        /// an integer, the Id, if <typeparamref name="T"/> derives from <see cref="BaseEntity"/>
        /// </item>
        /// <item>
        /// an array (of proper length) of objects (of proper types) corresponding to the keys of <typeparamref name="T"/>.
        /// </item>
        /// </list>
        /// </summary>
        /// <typeparam name="T">The type invistigated.</typeparam>
        /// <param name="objs">The array of object tested.</param>
        /// <exception cref="InvalidKeyForClassException"/>
        public static void CheckIfObjectIsKey<T>(object[] objs)
        {
            if (typeof(T).IsSubclassOf(typeof(BaseEntity)))
            {
                if (objs.Length != 1 || !(objs[0] is int))
                    throw new InvalidKeyForClassException(typeof(T));
            }
            else
            {
                if (objs.Length != KeyPropertiesNames<T>().Length)
                    throw new InvalidKeyForClassException(typeof(T));
                int len = KeyPropertiesNames<T>().Length;
                for (int i = 0; i < len; i++)
                {
                    object ototest = objs[i];
                    string propname = KeyPropertiesNames<T>()[i];
                    if (!typeof(T).GetProperty(propname).PropertyType.IsAssignableFrom(ototest.GetType()))
                        throw new InvalidKeyForClassException(typeof(T));
                }
            }
        }

        /// <summary>
        /// From an array of objects, supposed to be either Id or keys, get the Id
        /// </summary>
        /// <typeparam name="T">The type investifated</typeparam>
        /// <param name="objs">The objects to cast to int as an Id</param>
        /// <returns>The Id</returns>
        /// <exception cref="InvalidKeyForClassException"/>
        private static int? ObjectsToId<T>(object[] objs)
        {
            if (!typeof(T).IsSubclassOf(typeof(BaseEntity)))
                throw new InvalidKeyForClassException(typeof(T));
            if ((objs.Length != 1) || !(objs[0] is int))
                throw new InvalidKeyForClassException(typeof(T));
            return (int?)objs[0];
        }

        /// <summary>
        /// Construct the expression tree for a lambda expression (<c>o => o.prop == value</c>) dynamically
        /// <br/>
        /// Purpose : LINQ to SQL interpretation will be successfull. 
        /// Something using reflection such as 
        /// <br/>
        /// <c>o => o.GetType().GetProperty("prop").GetValue(o)==value</c>
        /// <br/>
        /// doesn't work : LINQ to ENTITY won't work.
        /// <br/>
        /// The object is of type <typeparamref name="TItem"/>, value is of type <typeparamref name="TValue"/>, 
        /// the property to check is <paramref name="property"/> and the value is <paramref name="value"/>.
        /// </summary>
        /// <remarks>
        /// Thanks internet for this one (had to modify it a little bit though)
        /// </remarks>
        /// <typeparam name="TItem">The type of the object the lambda expression will test whether or not the 
        /// property <paramref name="property"/> is equal to <paramref name="value"/></typeparam>
        /// <typeparam name="TValue">Tje type of <paramref name="value"/></typeparam>
        /// <param name="property">The property</param>
        /// <param name="value">The value</param>
        /// <returns>The lambda expression properly constructed so that LINQ to SQL interpretation will be
        /// successfull</returns>
        private static Expression<Func<TItem, bool>> PropertyEquals<TItem>(PropertyInfo property, object value)
        {
            var param = Expression.Parameter(typeof(TItem));
            var body = Expression.Equal(Expression.Property(param, property),
                                        Expression.Constant(value, property.PropertyType));
            return Expression.Lambda<Func<TItem, bool>>(body, param);
        }

        /// <summary>
        /// Construct the expression tree for a lambda expression (<c>o => o.prop</c>) dynamically
        /// <br/>
        /// Purpose : LINQ to SQL interpretation will be successfull. 
        /// Something using reflection such as 
        /// <br/>
        /// <c>o => o.GetType().GetProperty("prop").GetValue(o)</c>
        /// <br/>
        /// doesn't work : LINQ to ENTITY won't work.
        /// <br/>
        /// The object is of type <typeparamref name="TItem"/>, 
        /// the property to specify is <paramref name="property"/> and is of type <typeparamref name="TKey"/>.
        /// </summary>
        /// <typeparam name="TItem">The type of the object the lambda expression will specify the property
        /// <paramref name="property"/>.</typeparam>
        /// <typeparam name="TKey">The type of the property <paramref name="property"/></typeparam>
        /// <param name="property">The property to specify</param>
        /// <returns>The lambda expression properly constructed so that LINQ to SQL interpretation will be
        /// successfull</returns>
        private static Expression<Func<TItem, TKey>> GetKey<TItem, TKey>(PropertyInfo property)
        {
            var param = Expression.Parameter(typeof(TItem));
            var body = Expression.Property(param, property);
            var bodyconverted = Expression.Convert(body, typeof(TKey));
            return Expression.Lambda<Func<TItem, TKey>>(bodyconverted, param);
        }

        /// <summary>
        /// From a query <paramref name="req"/>, specify either :
        /// <list type="bullet">
        /// <item>
        /// <c>req.Where(o => o.Id == Id)</c> if <typeparamref name="T"/> derives from <see cref="BaseEntity"/>. In
        /// that case, <paramref name="objs"/> is the Id.
        /// </item>
        /// <item> 
        /// otherwise, <c>req.Where(o => o.Key1 = KeyValue1)....Where(o => o.Keyn = KeyValuen)</c>. In that case, 
        /// <paramref name="objs"/> is the array containing <c>{ KeyValue1, ..., KeyValuen }</c>
        /// </item>
        /// </list>
        /// </summary>
        /// <typeparam name="T">The type of the objects of the query</typeparam>
        /// <param name="req">The query</param>
        /// <param name="objs">Either the Id or the keys</param>
        /// <returns>The query specified</returns>
        /// <exception cref="InvalidKeyForClassException"/>
        public static IQueryable<T> QueryWhereKeysAre<T>(IQueryable<T> req, params object[] objs)
        {
            CheckIfObjectIsKey<T>(objs);
            if (typeof(T).IsSubclassOf(typeof(BaseEntity)))
            {
                int? id = ObjectsToId<T>(objs);
                req = req.Where(
                               PropertyEquals<T>(typeof(T).GetProperty("Id"), id)
                               );
            }
            else
            {
                int i = 0;
                foreach (object obj in objs)
                {
                    req = req.Where(
                                    PropertyEquals<T>(typeof(T).GetProperty(KeyPropertiesNames<T>()[i]), obj)
                                    );
                    i++;
                }
            }
            return req;
        }

        /// <summary>
        /// From a query <paramref name="req"/>, specify that elements must satisfy a predicate <paramref name="predicateWhere"/>.
        /// <br/>
        /// If <paramref name="predicateWhere"/> fails to be interpreted from LINQ to SQL, the predicate will be ignored.
        /// </summary>
        /// <remarks>TODO(?) : log the error ? Throw custom exception ?</remarks>
        /// <typeparam name="T">The type of the objects of the query</typeparam>
        /// <param name="req">The query</param>
        /// <param name="predicateWhere">The predicate</param>
        /// <returns>The query specified</returns>
        public static IQueryable<T> QueryTryPredicateWhere<T>(IQueryable<T> req, Expression<Func<T, bool>> predicateWhere = null)
        {
            if (predicateWhere == null)
                return req;
            IQueryable<T> req2;
            try
            {
                req2 = req.Where(predicateWhere);
                double test = req.Count();
            }
            catch
            {
                //Linq to entity cannot interpret the predicate predicateWhere
                //-> ignore the restriction "where"
                //-> TODO(?) : log the error ?
                req2 = req;
            }
            return req2;
        }

        /// <summary>
        /// Specify a query <paramref name="orderedreq"/> (ordered) to get elements following the condition <paramref name="predicateWhere"/> from element <paramref name="start"/> with at most <paramref name="maxByPage"/> elements
        /// </summary>
        /// <param name="orderedreq">The ordered query to specify</param>
        /// <param name="start">Starting index</param>
        /// <param name="maxByPage">Maximum number of elements</param>
        /// <param name="predicateWhere">Conidition</param>
        /// <returns>The query</returns>
        public static IQueryable<T> WhereSkipTake<T>(IQueryable<T> orderedreq, int start, int maxByPage, Expression<Func<T, bool>> predicateWhere)
        {
            if (predicateWhere != null)
                orderedreq = QueryTryPredicateWhere(orderedreq,predicateWhere);

            orderedreq = orderedreq.Skip(start).Take(maxByPage);
            return orderedreq;
        }

        /// <summary>
        /// Get the <see cref="MethodInfo"/> of <see cref="GetKey{TItem, TKey}(PropertyInfo)"/> where
        /// <c>TItem</c> is <typeparamref name="T"/> and <c>TKey</c> is the type of the property of <typeparamref name="T"/>
        /// having the name <paramref name="propertyName"/>.
        /// <br/>
        /// The purpose is to dynamically call the generic method <see cref="GetKey{TItem, TKey}(PropertyInfo)"/>.
        /// </summary>
        /// <typeparam name="T">The type investigated</typeparam>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The method.</returns>
        private static MethodInfo GetMethodGetKey<T>(string propertyName)
        {
            return typeof(GenericTools).GetMethod("GetKey", BindingFlags.NonPublic | BindingFlags.Static)
                                       .MakeGenericMethod(new[] { 
                                                                 typeof(T), 
                                                                 typeof(T).GetProperty(propertyName).PropertyType 
                                                                });
        }

        /// <summary>
        /// Get the <see cref="MethodInfo"/> of <see cref="Queryable.OrderBy{TSource, TKey}(IQueryable{TSource}, Expression{Func{TSource, TKey}})"/>
        /// where <c>TSource</c> is <typeparamref name="T"/> and <c>TKey</c> is the type of the property of <typeparamref name="T"/>
        /// having the name <paramref name="propertyName"/>.
        /// </summary>
        /// <typeparam name="T">The type investigated</typeparam>
        /// <param name="propertyName">The name of the property</param>
        /// <returns>The method.</returns>
        private static MethodInfo GetMethodOrderBy<T>(string propertyName)
        {
            return typeof(Queryable).GetMethods().Single(m => m.Name == "OrderBy" &&
                                                              m.GetParameters().Length == 2)
                                                  .MakeGenericMethod(new[] { 
                                                                            typeof(T), 
                                                                            typeof(T).GetProperty(propertyName).PropertyType 
                                                                           });
        }

        /// <summary>
        /// From the methods <paramref name="methodOrderBy"/> and <paramref name="methodGetKey"/>, representing
        /// respectively the <see cref="MethodInfo"/> given by <see cref="GetMethodOrderBy{T}(string)"/> and
        /// <see cref="GetMethodGetKey{T}(string)"/>, specify the query <paramref name="req"/> appropriately.
        /// <br/>
        /// Intent : to specify the query : <c>req.OrderBy(o => o.</c><paramref name="propertyName"/><c>)</c>
        /// dynamically, so that LINQ to SQL interpretation will be successfull.
        /// </summary>
        /// <typeparam name="T">The type invistigated</typeparam>
        /// <param name="req">The query to specify</param>
        /// <param name="methodOrderBy">The method OrderBy</param>
        /// <param name="methodGetKey">The method GetKey</param>
        /// <param name="propertyName">The name of the property</param>
        /// <returns>The specified query</returns>
        private static IQueryable<T> OrderByCustomKeyFromMethods<T>(IQueryable<T> req, MethodInfo methodOrderBy, MethodInfo methodGetKey, string propertyName)
        {
            return (IQueryable<T>)methodOrderBy.Invoke(typeof(IQueryable<T>), new object[] {
                                                                                            req,
                                                                                            methodGetKey.Invoke(typeof(GenericTools), new object[] { typeof(T).GetProperty(propertyName) })
                                                                                           });
        }

        /// <summary>
        /// Specify the query <paramref name="req"/> ordering elements by <paramref name="defaultPropertyName"/>, 
        /// essentially do <c>req.OrderBy(o => o.</c><paramref name="defaultPropertyName"/><c>)</c> dynamically.
        /// </summary>
        /// <typeparam name="T">The type invistigated</typeparam>
        /// <param name="req">The query to specify</param>
        /// <param name="defaultPropertyName">The name of the property</param>
        /// <returns>The specified query</returns>
        private static IQueryable<T> QueryOrderByKey<T>(IQueryable<T> req, string defaultPropertyName)
        {
            MethodInfo methodGetKey = GetMethodGetKey<T>(defaultPropertyName);
            MethodInfo methodOrderBy = GetMethodOrderBy<T>(defaultPropertyName);

            return OrderByCustomKeyFromMethods(req, methodOrderBy, methodGetKey, defaultPropertyName);
        }

        /// <summary>
        /// Specifies a query <paramref name="req"/> with a default ordering, ordering by either :
        /// <list type="bullet">
        /// <item>
        /// Id if <typeparamref name="T"/> derives from <see cref="BaseEntity"/>
        /// </item>
        /// <item>
        /// otherwise, the first key (the first declared in the class <typeparamref name="T"/>).
        /// </item>
        /// </list>
        /// </summary>
        /// <typeparam name="T">The type investigated</typeparam>
        /// <param name="req">The query to specify</param>
        /// <returns>The specified query.</returns>
        public static IQueryable<T> QueryDefaultOrderBy<T>(IQueryable<T> req)
        {
            string defaultPropertyName;

            if (typeof(BaseEntity).IsAssignableFrom(typeof(T)))
            {
                defaultPropertyName = "Id";
            }
            else
            {
                defaultPropertyName = KeyPropertiesNames<T>()[0];
            }

            return QueryOrderByKey(req, defaultPropertyName);
        }

        /// <summary>
        /// From an object <paramref name="t"/> of type <typeparamref name="T"/>, get either
        /// <list type="bullet">
        /// <item>
        /// the id of <paramref name="t"/> if <typeparamref name="T"/> derives from <see cref="BaseEntity"/>
        /// </item>
        /// <item>
        /// otherwise the array of keys of <paramref name="t"/>
        /// </item>
        /// </list>
        /// </summary>
        /// <typeparam name="T">The type invistigated</typeparam>
        /// <param name="t">The object</param>
        /// <returns>Either the Id of <paramref name="t"/> or its keys values</returns>
        public static object[] GetKeysValues<T>(T t)
        {
            if (typeof(BaseEntity).IsAssignableFrom(typeof(T)))
            {
                return new object[] { ((int?)typeof(T).GetProperty("Id").GetValue(t)).Value };
            }
            else
            {
                object[] res = new object[KeyPropertiesNames<T>().Length];
                int i = 0;
                foreach (string propname in KeyPropertiesNames<T>())
                {
                    object value = typeof(T).GetProperty(propname).GetValue(t);
                    res[i] = Convert.ChangeType(value, typeof(T).GetProperty(propname).PropertyType);
                    i++;
                }
                return res;
            }
        }

        /// <summary>
        /// Call the generic method <see cref="GetKeysValues{T}(T)"/> dynamically. 
        /// <br/>
        /// That is, get either the Id or the keys values
        /// of an object <paramref name="item"/> of type <paramref name="typeofElement"/>.
        /// </summary>
        /// <param name="item">The item</param>
        /// <param name="typeofElement">The type of the object</param>
        /// <returns>Either the Id or the keys values of <paramref name="item"/></returns>
        public static object[] GetKeysValuesForType(object item, Type typeofElement)
        {
            MethodInfo methodGetKeysValues = typeof(GenericTools).GetMethod("GetKeysValues", BindingFlags.Public | BindingFlags.Static)
                                                                 .MakeGenericMethod(new[] { typeofElement });
            return (object[])methodGetKeysValues.Invoke(typeof(GenericTools), new object[] { item });
        }

        /// <summary>
        /// Find an object of type <paramref name="typeofElement"/> with either Id or Keys <paramref name="objs"/>
        /// in context <paramref name="newContext"/>.
        /// <br/>
        /// Essentially, this is a dynamic call to the generic method <see cref="GenericRepository{T}.FindByIdIncludesTrackedInNewContext(MyDbContext, object[])"/>
        /// where <c>T</c> is <paramref name="typeofElement"/>.
        /// </summary>
        /// <remarks>
        /// Other properties will not be included, and element will be tracked.
        /// </remarks>
        /// <param name="typeofElement">The type of the the object searched</param>
        /// <param name="newContext">The context in which the search has to be done</param>
        /// <param name="objs">Either the Id or the keys values of the object searched</param>
        /// <returns>The object if found, <see langword="null"/> otherwise.</returns>
        /// <exception cref="InvalidKeyForClassException"/>
        public static object FindByKeysInNewContextForType(Type typeofElement, MyDbContext newContext, object[] objs)
        {
            MethodInfo methodCheckIfObjectIsKey = typeof(GenericTools).GetMethod("CheckIfObjectIsKey", BindingFlags.Public | BindingFlags.Static)
                                                                      .MakeGenericMethod(new[] { typeofElement });
            methodCheckIfObjectIsKey.Invoke(typeof(GenericTools), new object[] { objs });
            MethodInfo methodQueryWhereKeysAre = typeof(GenericTools).GetMethod("QueryWhereKeysAre", BindingFlags.Public | BindingFlags.Static)
                                                                     .MakeGenericMethod(new[] { typeofElement });
            object query = methodQueryWhereKeysAre.Invoke(typeof(GenericTools), new object[] { newContext.Set(typeofElement), objs });
            MethodInfo methodSingleOrDefault = typeof(Queryable).GetMethods().Single(m => m.Name == "SingleOrDefault" &&
                                                                                          m.GetParameters().Length == 1)
                                                                .MakeGenericMethod(new[] { typeofElement });
            return methodSingleOrDefault.Invoke(typeof(Queryable), new object[] { query });
        }

        /// <summary>
        /// Assuming <typeparamref name="T"/> does not derive from <see cref="BaseEntity"/>, check
        /// if <paramref name="objs"/> is an array of keys.
        /// </summary>
        /// <typeparam name="T">The type invistigated</typeparam>
        /// <param name="objs">The array that must be many keys values.</param>
        /// <exception cref="InvalidKeyForClassException"/>
        private static void CheckIfObjsIsManyKeys<T>(object[] objs)
        {
            int nbrkeys = KeyPropertiesNames<T>().Length;
            if (objs.Length == 0 || objs.Length % nbrkeys != 0)
                throw new InvalidKeyForClassException(typeof(T));
            int nbrobjs = objs.Length / nbrkeys;
            for (int i = 0; i < nbrobjs; i++)
            {
                object[] keystotest = new object[nbrkeys];
                for (int j = 0; j < nbrkeys; j++)
                {
                    keystotest[j] = objs[i * nbrkeys + j];
                }
                CheckIfObjectIsKey<T>(objs);
            }
        }

        /// <summary>
        /// From a one dimensionnal array of keys values <paramref name="objs"/>, get an array
        /// of array containing key values.
        /// <br/>
        /// <example>
        /// From <c>{ key1value1, key2value1, key1value2, key2value2 }</c> get
        /// <c>{ { key1value1, key2value1 } , { key1value2, key2value2 } }</c>
        /// </example>
        /// </summary>
        /// <typeparam name="T">The type invistigated</typeparam>
        /// <param name="objs">The array of keys values</param>
        /// <returns>The array of array containing key values.</returns>
        public static object[][] GetManyKeys<T>(object[] objs)
        {
            int nbrkeys = KeyPropertiesNames<T>().Length;
            int nbrobjs = objs.Length / nbrkeys;
            object[][] res = new object[nbrobjs][];
            for (int i = 0; i < nbrobjs; i++)
            {
                res[i] = new object[nbrkeys];
                for (int j = 0; j < nbrkeys; j++)
                {
                    res[i][j] = objs[i * nbrkeys + j];
                }
            }
            return res;
        }

        /// <summary>
        /// Checks if <paramref name="objs"/> is either
        /// <list type="bullet">
        /// <item>
        /// many Ids if <typeparamref name="T"/> derives from <see cref="BaseEntity"/>
        /// </item>
        /// <item>
        /// or many keys values.
        /// </item>
        /// </list>
        /// </summary>
        /// <typeparam name="T">The type invistigated</typeparam>
        /// <param name="objs">The array that must either be many Ids or manys keys values.</param>
        /// <exception cref="IdListEmptyForClassException"/>
        /// <exception cref="InvalidKeyForClassException" />
        public static void CheckIfObjsIsManyKeysOrIds<T>(params object[] objs)
        {
            if (typeof(BaseEntity).IsAssignableFrom(typeof(T)))
            {
                if (objs.Length == 0)
                    throw new IdListEmptyForClassException(typeof(T));
                for (int i = 0; i < objs.Length; i++)
                {
                    if (!typeof(int?).IsAssignableFrom(objs[i].GetType()) && !typeof(int?).IsAssignableFrom(objs[i].GetType()) && !int.TryParse(objs[i].ToString(), out _))
                        throw new InvalidKeyForClassException(typeof(T));
                }
            }
            else
            {
                CheckIfObjsIsManyKeys<T>(objs);
            }
        }

        /// <summary>
        /// From an array <paramref name="objs"/> of objects containing many Ids,
        /// get an array <see cref="int"/>?[] containing the Ids.
        /// </summary>
        /// <param name="objs">The array to convert.</param>
        /// <returns>The array of Ids.</returns>
        public static int?[] GetManyIds(params object[] objs)
        {
            int?[] ids = new int?[objs.Length];
            for (int i = 0; i < objs.Length; i++)
            {
                try
                {
                    ids[i] = (int?)objs[i];
                }
                catch
                {
                    int.TryParse((string)objs[i], out int j);
                    ids[i] = (int?)j;
                }
            }
            return ids;
        }













        private static Expression<Func<TItem, bool>> PropertyKeysEquals<TItem>(PropertyInfo prop1, PropertyInfo prop2, object value)
        {
            var param = Expression.Parameter(typeof(TItem));
            var exp = Expression.Property(param, prop1);
            var body = Expression.Equal(Expression.Property(exp, prop2),
                                        Expression.Constant(value, prop2.PropertyType));
            return Expression.Lambda<Func<TItem, bool>>(body, param);
        }

        private static Expression<Func<TItem, bool>> PropertyKeysNotNull<TItem>(PropertyInfo prop1)
        {
            var param = Expression.Parameter(typeof(TItem));
            var exp = Expression.Property(param, prop1);
            var body = Expression.IsFalse(Expression.Equal(exp, Expression.Constant(null)));
            return Expression.Lambda<Func<TItem, bool>>(body, param);
        }

        private static List<Q> ListWherePropNotNull<T, Q>(List<Q> req, Type type, string propname)
        {
            req = req.Where(
                            PropertyKeysNotNull<Q>(typeof(Q).GetProperty(propname)).Compile()
                            ).ToList();
            return req;
        }

        private static List<Q> ListWherePropKeysAre<T, Q>(List<Q> req, Type type, string propname, object[] objs)
        {
            CheckIfObjectIsKey<T>(objs);
            if (typeof(T).IsSubclassOf(typeof(BaseEntity)))
            {
                int? id = ObjectsToId<T>(objs);
                req = req.Where(
                               PropertyKeysEquals<Q>(typeof(Q).GetProperty(propname), typeof(T).GetProperty("Id"), id).Compile()
                               ).ToList();
            }
            else
            {
                int i = 0;
                foreach (object obj in objs)
                {
                    req = req.Where(
                                    PropertyKeysEquals<Q>(typeof(Q).GetProperty(propname), typeof(T).GetProperty(KeyPropertiesNames<T>()[i]), obj).Compile()
                                    ).ToList();
                    i++;
                }
            }
            return req;
        }

        private static Expression<Func<T,bool>> ExpressionWhereKeysAre<T>(params object[] objs)
        {
            var param = Expression.Parameter(typeof(T));
            Expression body;
            if (typeof(BaseEntity).IsAssignableFrom(typeof(T)))
            {
                int id = (int)objs[0];
                body = Expression.Equal(Expression.Property(param, typeof(T).GetProperty("Id")),
                                        Expression.Constant(id, typeof(T).GetProperty("Id").PropertyType));
            }
            else
            {
                body = Expression.Equal(Expression.Property(param, typeof(T).GetProperty(KeyPropertiesNames<T>()[0])),
                                        Expression.Constant(objs[0], typeof(T).GetProperty(KeyPropertiesNames<T>()[0]).PropertyType));
                for (int i = 1; i < objs.Length; i++)
                {
                    body = Expression.And(body,
                                          Expression.Equal(Expression.Property(param, typeof(T).GetProperty(KeyPropertiesNames<T>()[i])),
                                                           Expression.Constant(objs[i], typeof(T).GetProperty(KeyPropertiesNames<T>()[i]).PropertyType)));
                }
            }
            return Expression.Lambda<Func<T, bool>>(body, param);
        }

        private static bool ListWhereKeysAreCountSup1<T>(IList<T> req, params object[] objs)
        {
            Func<T, bool> func = ExpressionWhereKeysAre<T>(objs).Compile();
            return req.Where(func).Count() >= 1;
        }

        private static Expression<Func<Q, bool>> ExpressionListWherePropListCountainsElementWithGivenKeys<T, Q>(PropertyInfo prop, params object[] objs)
        {
            var param = Expression.Parameter(typeof(Q));
            var exp = Expression.Property(param, prop);
            // t => t.propname
            //var body = Expression.GetActionType;
            MethodInfo methodExpressionWhereKeysAre = typeof(GenericTools).GetMethod("ExpressionWhereKeysAre", BindingFlags.NonPublic | BindingFlags.Static)
                                                                             .MakeGenericMethod(typeof(T));
            //var body = Expression.Call(methodListWhereKeysAreCountSup1, exp, Expression.Constant(objs, typeof(object[])));
            Expression exp2 = (Expression)methodExpressionWhereKeysAre.Invoke(typeof(GenericTools), new object[] { objs });

            MethodInfo methodWhere = typeof(Enumerable).GetMethods().Where(m => m.Name == "Where").ToList()[0].MakeGenericMethod(typeof(T));
            Expression body = Expression.Call(methodWhere, exp, exp2);

            MethodInfo methodCount = typeof(Enumerable).GetMethods().Where(m => m.Name == "Count" && m.GetParameters().Length == 1).Single()
                                                       .MakeGenericMethod(typeof(T)); //
            body = Expression.Call(methodCount, body);

            body = Expression.GreaterThanOrEqual(body, Expression.Constant(1));
            // t => t.propname.Where(...).Count()>=1
            return Expression.Lambda<Func<Q, bool>>(body, param);
        }

        private static List<Q> ListWherePropListCountainsElementWithGivenKeys<T,Q>(List<Q> req, Type t, string propname, params object[] objs)
        {
            Func<Q, bool> func = ExpressionListWherePropListCountainsElementWithGivenKeys<T,Q>(t.GetProperty(propname),objs).Compile();
            return req.Where(func).ToList();
            //req.Where( t => t.propname.Where(...).Count()>=1)
        }

        private static Expression<Func<Q,bool>> ExpressionListRemoveElementWithGivenKeys<T,Q>(params object[] objs)
        {
            var param = Expression.Parameter(typeof(Q));
            Expression body = Expression.Not(Expression.Equal(Expression.Property(param, typeof(Q).GetProperty(KeyPropertiesNames<Q>()[0])),
                                                              Expression.Constant(objs[0], typeof(Q).GetProperty(KeyPropertiesNames<Q>()[0]).PropertyType)));
            for (int i = 1; i < objs.Length; i++)
            {
                body = Expression.Or(body,
                                    Expression.Not(Expression.Equal(Expression.Property(param, typeof(Q).GetProperty(KeyPropertiesNames<Q>()[i])),
                                                                    Expression.Constant(objs[i], typeof(Q).GetProperty(KeyPropertiesNames<Q>()[i]).PropertyType))));
            }
            // t => t.key1 != value1 || t.key2 != value2 ...
            return Expression.Lambda<Func<Q, bool>>(body, param);
        }

        private static Expression<Func<Q,bool>> ExpressionListRemoveElementWithGivenId<T,Q>(int? id)
        {
            var param = Expression.Parameter(typeof(Q));
            var body = Expression.Not(Expression.Equal(Expression.Property(param, typeof(Q).GetProperty("Id")),
                                                       Expression.Constant(id, typeof(int?))));
            return Expression.Lambda<Func<Q, bool>>(body, param);
        }

        private static List<Q> ListRemoveElementWithGivenKeys<T,Q>(List<Q> req, params object[] objs)
        {
            CheckIfObjectIsKey<T>(objs);
            Func<Q, bool> func;
            if (typeof(BaseEntity).IsAssignableFrom(typeof(T)))
            {
                int id = (int)objs[0];
               func = ExpressionListRemoveElementWithGivenId<T,Q>(id).Compile();
            }
            else
            {
                func = ExpressionListRemoveElementWithGivenKeys<T,Q>(objs).Compile();
            }
            return req.Where(func).ToList();
        }

        private static bool HasPropertyRelation(Type t1, Type t2)
        {
            return HasPropertyRelationNotList(t1, t2) || HasPropertyRelationList(t1, t2);
        }

        private static bool HasPropertyRelationNotList(Type t1, Type t2)
        {
            return DynamicDBTypesForType(t1).Values.Contains(t2);
        }

        private static bool HasPropertyRelationList(Type t1, Type t2)
        {
            return DynamicDBListTypesForType(t1).Values.Contains(t2);
        }

        private static IEnumerable<string> GetPropsNames(Type t1, Type t2)
        {
            return t1.GetProperties().Where(prop => prop.PropertyType == t2).Select(prop => prop.Name);
        }

        private static bool HasPropertyRelationRequired(Type t1, Type t2)
        {
            if (HasPropertyRelationNotList(t1, t2))
            {
                List<string> PropNames = DynamicDBTypesForType(t1).Where((propnametype) => propnametype.Value == t2).Select(propnametype => propnametype.Key).ToList();
                return t1.GetProperties().Where(p => PropNames.Contains(p.Name)).Where(p => p.GetCustomAttribute(typeof(RequiredAttribute), false) != null).Count() != 0;
            }
            if (HasPropertyRelationList(t1, t2))
            {
                List<string> PropNames = DynamicDBListTypesForType(t1).Where((propnametype) => propnametype.Value == t2).Select(propnametype => propnametype.Key).ToList();
                return t1.GetProperties().Where(p => PropNames.Contains(p.Name)).Where(p => p.GetCustomAttribute(typeof(RequiredAttribute), false) != null).Count() != 0;
            }
            return false;
        }

        private static IEnumerable<Type> GetTypesInRelationWithTHavingTPropertyTAndTNotHavingProperty<T>()
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

        private static IEnumerable<Type> GetTypesInRelationWithTHavingRequiredTProperty<T>()
        {
            List<Type> res = new List<Type>();
            foreach (Type type in Assembly.GetAssembly(typeof(T)).GetTypes()
                                                                 .Where(myType => myType.IsClass && !myType.IsAbstract
                                                                                                 && (myType.IsSubclassOf(typeof(BaseEntity)) || myType.IsSubclassOf(typeof(EntityWithKeys)))
                                                                       ))
            {
                if (HasPropertyRelationRequired(type, typeof(T)))
                    res.Add(type);
            }
            return res;
            // person -> finger
            // person -> action
        }

        private static bool HasManyProperties(Type t1, Type t2)
        {
            int countNotList = DynamicDBTypesForType(t1).Values.Where(t => t == t2).Count();
            int countList = DynamicDBListTypesForType(t1).Values.Where(t => t == t2).Count();
            return countNotList + countList > 1;
        }

        private static IEnumerable<Type> GetTypesForWhichTHasManyProperties<T>()
        {
            List<Type> res = new List<Type>();
            foreach (Type type in DynamicDBTypes<T>().Values.Concat(DynamicDBListTypes<T>().Values))
            {
                if (HasManyProperties(typeof(T), type))
                    res.Add(type);
            }
            return res;
            // person -> color
            // person -> thoughts
            // thoughts -> person
        }

        private static IEnumerable<Type> GetTypesForWhichTHasOneProperty<T>()
        {
            List<Type> res = new List<Type>();
            foreach (Type type in DynamicDBTypes<T>().Values.Concat(DynamicDBListTypes<T>().Values))
            {
                if (!HasManyProperties(typeof(T), type))
                    res.Add(type);
            }
            return res;
        }

        private static dynamic GetServiceFromContext(MyDbContext context, Type t)
        {
            Type typetRepository = Assembly.GetAssembly(t).GetTypes().Single(typ => typ.Name == t.Name + "Repository");
            Type typetService = Assembly.GetAssembly(t).GetTypes().Single(typ => typ.Name == t.Name + "Service");
            dynamic tRepository = Activator.CreateInstance(typetRepository, context);
            dynamic tService = Activator.CreateInstance(typetService, tRepository);
            return tService;
        }

        private static void SetForTypePropertyWithGivenKeysToNullInNewContext<T>(MyDbContext context, Type t, string propname, params object[] objs)
        {
            dynamic tService = GetServiceFromContext(context, t);

            MethodInfo methodListWherePropNotNull = typeof(GenericTools).GetMethod("ListWherePropNotNull", BindingFlags.NonPublic | BindingFlags.Static)
                                                                                    .MakeGenericMethod(new Type[] { typeof(T), t });
            dynamic req = methodListWherePropNotNull.Invoke(typeof(GenericTools), new object[] { tService.GetAllIncludes(1, int.MaxValue, null, null), t, propname });

            MethodInfo methodQueryWherePropKeysAre = typeof(GenericTools).GetMethod("ListWherePropKeysAre", BindingFlags.NonPublic | BindingFlags.Static)
                                                                         .MakeGenericMethod(new Type[] { typeof(T), t });
            req = methodQueryWherePropKeysAre.Invoke(typeof(GenericTools), new object[] { req, t, propname, objs });
            foreach (var tItem in req)
            {
                if (t.GetProperty(propname).GetCustomAttribute(typeof(RequiredAttribute), false) == null)
                    tService.UpdateOne(tItem, propname, null);
                else
                    tService.Delete(tItem);
            }
        }

        private static object[] ConcatArrays(object[] A1, object[] A2)
        {
            object[] res = new object[A1.Length + 1];
            for (int i = 0; i < A1.Length; i++)
            {
                res[i] = A1[i];
            }
            res[A1.Length] = A2;
            return res;
        }

        private static void RemoveForTypePropertyListElementWithGivenKeyInNewContext<T>(MyDbContext context, Type t, string propname, params object[] objs)
        {
            dynamic tService = GetServiceFromContext(context, t);

            MethodInfo methodExpressionListWherePropListCountainsElementWithGivenKeys = typeof(GenericTools).GetMethod("ExpressionListWherePropListCountainsElementWithGivenKeys")
                                                                                                            .MakeGenericMethod(new Type[] { typeof(T), t });
            dynamic func = methodExpressionListWherePropListCountainsElementWithGivenKeys.Invoke(typeof(GenericTools), ConcatArrays(new object[] { t.GetProperty(propname) }, objs));

            dynamic req = tService.GetAllIncludes(1, int.MaxValue, null, func);

            //MethodInfo methodListWherePropListCountainsElementWithGivenKeys = typeof(GenericTools).GetMethod("ListWherePropListCountainsElementWithGivenKeys", BindingFlags.Public | BindingFlags.Static)
            //                                                                                      .MakeGenericMethod(new Type[] { typeof(T), t });
            //req = methodListWherePropListCountainsElementWithGivenKeys.Invoke(typeof(GenericTools), new object[] { req, t, propname, objs });

            //get elements for which the property includes the element gith given keys
            //req = tService.GetAllIncludes(1, int.MaxValue, null, null).Where(t => t.propname.Where(...).Count()>=1)

            foreach (var tItem in req)
            {
                var oldValue = tItem.GetType().GetProperty(propname).GetValue(tItem);
                if (t.GetProperty(propname).GetCustomAttribute(typeof(RequiredAttribute), false) != null && ((oldValue as IList).Count == 1))
                {
                    tService.Delete(tItem);
                }
                else
                {
                    MethodInfo methodListRemoveElementWithGivenKeys = typeof(GenericTools).GetMethod("ListRemoveElementWithGivenKeys", BindingFlags.NonPublic | BindingFlags.Static)
                                                                                          .MakeGenericMethod(new Type[] { t, typeof(T) });
                    var newValue = methodListRemoveElementWithGivenKeys.Invoke(typeof(GenericTools), new object[] { oldValue, objs });
                    tService.UpdateOne(tItem, propname, newValue);
                }
            }

            //foreach (var tItem in req)
            //{
            //    // remove from the property of these items the element with given keys
            //    tItem.propname.Where(...);
            //    tService.UpdateOne(tItem, propname, tItem.propname);
            //    if (required and tItem.propname.count()=0)
            //    tService.Delete(tItem);
            //}
        }

        private static void DeleteOtherPropInRelationWithTHavingTPropertyTAndTNotHavingProperty<T>(Type t, params object[] objs)
        {
            // OK address -> person
            // OK color -> person
            // OK worldvision -> person
            if (HasPropertyRelationNotList(t, typeof(T)))
            {
                List<string> propnames = DynamicDBTypesForType(t).Where(kv => kv.Value == typeof(T))
                                                                              .Select(kv => kv.Key)
                                                                              .ToList();
                using (MyDbContext context = new MyDbContext())
                {
                    foreach (string propname in propnames)
                    {
                        SetForTypePropertyWithGivenKeysToNullInNewContext<T>(context, t, propname, objs);
                    }
                }
            }
            else
            {
                if (HasPropertyRelationList(t, typeof(T)))
                {
                    List<string> propnames = DynamicDBListTypesForType(t).Where(kv => kv.Value == typeof(T))
                                                                                      .Select(kv => kv.Key)
                                                                                      .ToList();
                    using (MyDbContext context = new MyDbContext())
                    {
                        foreach (string propname in propnames)
                        {
                            RemoveForTypePropertyListElementWithGivenKeyInNewContext<T>(context, t, propname, objs);
                        }
                    }
                }
                else
                {
                    throw new HasNoPropertyRelationException(t, typeof(T));
                }
            }
        }

        private static void DeleteItemOfTypeWithRequiredPropertyHavingGivenKeysInNewContext<T>(MyDbContext context, Type t, string propname, params object[] objs)
        {
            dynamic tService = GetServiceFromContext(context, t);

            dynamic req = tService.GetAllIncludes(1, int.MaxValue, null, null);
            MethodInfo methodQueryWherePropKeysAre = typeof(GenericTools).GetMethod("ListWherePropKeysAre", BindingFlags.NonPublic | BindingFlags.Static)
                                                                         .MakeGenericMethod(new Type[] { typeof(T), t });
            req = methodQueryWherePropKeysAre.Invoke(typeof(GenericTools), new object[] { req, t, propname, objs });
            foreach (var tItem in req)
            {
                tService.Delete(tItem);
            }
        }

        private static void DeleteOrUpdateItemOfTypeWithRequiredListPropertyHavingGivenKeysInNewContext<T>(MyDbContext context, Type t, string propname, params object[] objs)
        {
            dynamic tService = GetServiceFromContext(context, t);

            dynamic req = tService.GetAllIncludes(1, int.MaxValue, null, null);

            MethodInfo methodListWherePropListCountainsElementWithGivenKeys = typeof(GenericTools).GetMethod("ListWherePropListCountainsElementWithGivenKeys", BindingFlags.NonPublic | BindingFlags.Static)
                                                                                                  .MakeGenericMethod(new Type[] { typeof(T), t });
            req = methodListWherePropListCountainsElementWithGivenKeys.Invoke(typeof(GenericTools), new object[] { req, t, propname, objs });

            // get elements for which the element with given keys is t
            //req = tService.GetAllIncludes(1, int.MaxValue, null, null).Where(t => t.propname.Where(...).Count()>=1)

            foreach (var tItem in req)
            {
                var oldValue = tItem.GetType().GetProperty(propname).GetValue(tItem);
                if ((oldValue as IList).Count == 1)
                {
                    tService.Delete(tItem);
                }
                else
                {
                    MethodInfo methodListRemoveElementWithGivenKeys = typeof(GenericTools).GetMethod("ListRemoveElementWithGivenKeys", BindingFlags.NonPublic | BindingFlags.Static)
                                                                                          .MakeGenericMethod(new Type[] { typeof(T), t });
                    var newValue = methodListRemoveElementWithGivenKeys.Invoke(typeof(GenericTools), new object[] { oldValue, objs });
                    tService.UpdateOne(tItem, propname, newValue);
                }
            }

            //foreach (var tItem in req)
            //{
            //    if tItem.propname.Count() == 1
            //    tService.Delete(tItem)
            //    else
            //    tItem.propname.Where(...);
            //}

        }

        private static void DeleteOtherPropInRelationWithTHavingRequiredTProperty<T>(Type t, params object[] objs)
        {
            // person -> finger
            // person -> action
            if (HasPropertyRelationNotList(t, typeof(T)))
            {
                List<string> propnames = DynamicDBTypesForType(t).Where(kv => kv.Value == typeof(T)).Select(kv => kv.Key)
                                                                              .Where(propname => t.GetProperty(propname).GetCustomAttribute(typeof(RequiredAttribute), false) != null)
                                                                              .ToList();
                using (MyDbContext context = new MyDbContext())
                {
                    foreach (string propname in propnames)
                    {
                        DeleteItemOfTypeWithRequiredPropertyHavingGivenKeysInNewContext<T>(context, t, propname, objs);
                    }
                }
            }
            else
            {
                if (HasPropertyRelationList(t, typeof(T)))
                {
                    List<string> propnames = DynamicDBListTypesForType(t).Where(kv => kv.Value == typeof(T)).Select(kv => kv.Key)
                                                                                      .Where(propname => t.GetProperty(propname).GetCustomAttribute(typeof(RequiredAttribute), false) != null)
                                                                                      .ToList();
                    using (MyDbContext context = new MyDbContext())
                    {
                        foreach (string propname in propnames)
                        {
                            DeleteOrUpdateItemOfTypeWithRequiredListPropertyHavingGivenKeysInNewContext<T>(context, t, propname, objs);
                        }
                    }
                }
                else
                {
                    throw new HasNoPropertyRelationException(t, typeof(T));
                }
            }
        }

        private static void DeleteOtherPropInSeveralRelationshipsWithT<T>(Type t, params object[] objs)
        {
            // person -> thought
            // thought -> person
            // person -> color
            if (HasPropertyRelationNotList(t, typeof(T)))
            {
                List<string> propnames = DynamicDBTypesForType(t).Where(kv => kv.Value == typeof(T))
                                                                              .Select(kv => kv.Key)
                                                                              .ToList();
                using (MyDbContext context = new MyDbContext())
                {
                    foreach (string propname in propnames)
                    {
                        SetForTypePropertyWithGivenKeysToNullInNewContext<T>(context, t, propname, objs);
                    }
                }
            }
            else
            {
                if (HasPropertyRelationList(t, typeof(T)))
                {
                    // remove from list
                    List<string> propnames = DynamicDBListTypesForType(t).Where(kv => kv.Value == typeof(T))
                                                                                      .Select(kv => kv.Key)
                                                                                      .ToList();
                    using (MyDbContext context = new MyDbContext())
                    {
                        foreach (string propname in propnames)
                        {
                            RemoveForTypePropertyListElementWithGivenKeyInNewContext<T>(context, t, propname, objs);
                        }
                    }
                }
                else
                {
                    throw new HasNoPropertyRelationException(t, typeof(T));
                }
            }
        }

        public static void PrepareDelete<T>(params object[] objs)
        {
            foreach (Type type in GetTypesInRelationWithTHavingTPropertyTAndTNotHavingProperty<T>())
            {
                DeleteOtherPropInRelationWithTHavingTPropertyTAndTNotHavingProperty<T>(type, objs);
                // address -> person
                // color -> person
                // worldvision -> person
            }
            foreach (Type type in GetTypesInRelationWithTHavingRequiredTProperty<T>())
            {
                DeleteOtherPropInRelationWithTHavingRequiredTProperty<T>(type, objs);
                // person -> finger
                // person -> action
            }
            foreach (Type type in GetTypesForWhichTHasManyProperties<T>())
            {
                DeleteOtherPropInSeveralRelationshipsWithT<T>(type, objs);
                // person -> thought
                // thought -> person
                // person -> color
            }
        }

        public static object[] PrepareSave<T>(T t)
        {
            // person -> color
            // person -> thoughts
            // thoughts -> person
            IEnumerable<Type> TypesForWhichTHasManyProperties = GetTypesForWhichTHasManyProperties<T>();
            //IEnumerable<Type> TypesForWhichTHasOneProperty = GetTypesForWhichTHasOneProperty();
            Dictionary<string, Type> dynamicDBTypes = DynamicDBTypes<T>();
            Dictionary<string, Type> dynamicDBListTypes = DynamicDBListTypes<T>();
            object[] res = new object[dynamicDBTypes.Count + dynamicDBListTypes.Count];
            var i = 0;
            foreach (PropertyInfo prop in typeof(T).GetProperties())
            {
                if (DynamicDBTypes<T>().Keys.Contains(prop.Name))
                {
                    if (TypesForWhichTHasManyProperties.Contains(prop.PropertyType))
                    {
                        if (prop.GetValue(t) == null)
                        {
                            res[i] = new PropToNull(prop.Name);
                        }
                        else
                        {
                            res[i] = prop.GetValue(t);
                        }
                    }
                    else
                    {
                        res[i] = prop.GetValue(t);
                    }
                }
                else
                {
                    if (DynamicDBListTypes<T>().Keys.Contains(prop.Name))
                    {
                        if (TypesForWhichTHasManyProperties.Contains(DynamicDBListTypes<T>()[prop.Name]))
                        {
                            if (prop.GetValue(t) == null || (prop.GetValue(t) as IList).Count == 0)
                            {
                                res[i] = new PropToNull(prop.Name);
                            }
                            else
                            {
                                res[i] = prop.GetValue(t);
                            }
                        }
                        else
                        {
                            res[i] = prop.GetValue(t);
                        }
                    }
                    else
                    {
                        i--;
                    }
                }
                i++;
            }
            return res;
        }

        private static void UpdateOtherPropInRelationWithTHavingRequiredTProperty<T>(Type type, T t)
        {
            //get type elements where the required property may have changed
            //delete if necessary
            //throw new NotImplementedException();
        }

        private static void UpdateOneOtherPropInRelationWithTHavingRequiredTProperty<T>(string propertyName, object newValue)
        {
            Type typeChanged;
            if (DynamicDBTypes<T>().Keys.Contains(propertyName))
            {
                typeChanged = DynamicDBTypes<T>()[propertyName];

                using (MyDbContext context = new MyDbContext())
                {
                    dynamic tService = GetServiceFromContext(context, typeChanged);

                    dynamic req = tService.GetAllIncludes(1, int.MaxValue, null, null);

                    //req.Where(...)

                    foreach (var tItem in req)
                    {
                        tService.Delete(tItem);
                    }
                }
            }
            else
            {
                typeChanged = DynamicDBListTypes<T>()[propertyName];

                using (MyDbContext context = new MyDbContext())
                {
                    dynamic tService = GetServiceFromContext(context, typeChanged);

                    dynamic req = tService.GetAllIncludes(1, int.MaxValue, null, null);

                    //req.Where(...)

                    foreach (var tItem in req)
                    {
                        if (tItem.prop.Count = 1)
                            tService.Delete(tItem);
                    }
                }
            }
        }

        public static object[] PrepareUpdate<T>(T t)
        {
            foreach (Type type in GetTypesInRelationWithTHavingRequiredTProperty<T>())
            {
                UpdateOtherPropInRelationWithTHavingRequiredTProperty(type, t);
                // person -> finger
                // person -> action
            }

            IEnumerable<Type> TypesForWhichTHasManyProperties = GetTypesForWhichTHasManyProperties<T>();
            //IEnumerable<Type> TypesForWhichTHasOneProperty = GetTypesForWhichTHasOneProperty();
            Dictionary<string, Type> dynamicDBTypes = DynamicDBTypes<T>();
            Dictionary<string, Type> dynamicDBListTypes = DynamicDBListTypes<T>();
            object[] res = new object[dynamicDBTypes.Count + dynamicDBListTypes.Count];
            var i = 0;
            foreach (PropertyInfo prop in typeof(T).GetProperties())
            {
                if (DynamicDBTypes<T>().Keys.Contains(prop.Name))
                {
                    if (TypesForWhichTHasManyProperties.Contains(prop.PropertyType))
                    {
                        if (prop.GetValue(t) == null)
                        {
                            res[i] = new PropToNull(prop.Name);
                        }
                        else
                        {
                            res[i] = prop.GetValue(t);
                        }
                    }
                    else
                    {
                        res[i] = prop.GetValue(t);
                    }
                }
                else
                {
                    if (DynamicDBListTypes<T>().Keys.Contains(prop.Name))
                    {
                        if (TypesForWhichTHasManyProperties.Contains(DynamicDBListTypes<T>()[prop.Name]))
                        {
                            if (prop.GetValue(t) == null || (prop.GetValue(t) as IList).Count == 0)
                            {
                                res[i] = new PropToNull(prop.Name);
                            }
                            else
                            {
                                res[i] = prop.GetValue(t);
                            }
                        }
                        else
                        {
                            res[i] = prop.GetValue(t);
                        }
                    }
                    else
                    {
                        i--;
                    }
                }
                i++;
            }
            return res;
        }

        public static void PrepareUpdateOne<T>(T t, string propertyName, object newValue)
        {
            if (GetTypesInRelationWithTHavingRequiredTProperty<T>().Select(type => t.GetType().GetProperties().Where(prop => prop.PropertyType == type).Select(prop => prop.Name).SingleOrDefault()).Contains(propertyName))
            {
                UpdateOneOtherPropInRelationWithTHavingRequiredTProperty<T>(propertyName, newValue);
            }
        }
    }
}