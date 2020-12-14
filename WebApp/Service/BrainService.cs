using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Service
{
    public class BrainService : TEMPGenericService<Brain>, IBrainService
    {
        public BrainService(IBrainRepository brainRepository) : base(brainRepository)
        {

        }

        //public override void Delete(params object[] objs)
        //{
        //    _repository.Delete(objs);
        //}

        //public override void Delete(Brain t)
        //{
        //    _repository.Delete(t);
        //}

        public override Expression<Func<IQueryable<Brain>, IOrderedQueryable<Brain>>> OrderExpression()
        {
            return null;
        }

        //public override void Save(Brain t)
        //{
        //    _repository.Save(t, t.Owner);
        //}

        public override Expression<Func<Brain, bool>> SearchExpression(string searchField = "")
        {
            searchField = searchField.Trim().ToLower();
            return b => b.Name.Contains(searchField);
        }

        //public override void Update(Brain t)
        //{
        //    _repository.Update(t, t.Owner);
        //}

        //public override void UpdateOne(Brain t, string propertyName, object newValue)
        //{
        //    _repository.UpdateOne(t, propertyName, newValue);
        //}
    }
}