using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Repositories;

namespace WebApp.Service
{
    public class ActionService : TEMPGenericService<Models.Action>, IActionService
    {
        public ActionService(IActionRepository actionRepository) : base(actionRepository)
        {

        }

        //public override void Delete(Models.Action t)
        //{
        //    _repository.Delete(t);
        //}

        //public override void Delete(params object[] objs)
        //{
        //    _repository.Delete(objs);
        //}

        public override Expression<Func<IQueryable<Models.Action>, IOrderedQueryable<Models.Action>>> OrderExpression()
        {
            return null;
        }

        //public override void Save(Models.Action t)
        //{
        //    _repository.Save(t, t.People);
        //}

        public override Expression<Func<Models.Action, bool>> SearchExpression(string searchField = "")
        {
            searchField = searchField.Trim().ToLower();
            return a => a.Name.Contains(searchField);
        }

        //public override void Update(Models.Action t)
        //{
        //    _repository.Update(t, t.People);
        //}

        //public override void UpdateOne(Models.Action t, string propertyName, object newValue)
        //{
        //    _repository.Update(t, propertyName, newValue);
        //}
    }
}