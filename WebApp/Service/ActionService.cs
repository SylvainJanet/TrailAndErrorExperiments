using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Repositories;

namespace WebApp.Service
{
    public class ActionService : GenericService<Models.Action>, IActionService
    {
        public ActionService(IActionRepository actionRepository) : base(actionRepository)
        {

        }

        public override Expression<Func<IQueryable<Models.Action>, IOrderedQueryable<Models.Action>>> OrderExpression()
        {
            return null;
        }

        public override Expression<Func<Models.Action, bool>> SearchExpression(string searchField = "")
        {
            searchField = searchField.Trim().ToLower();
            return a => a.Name.Contains(searchField);
        }
    }
}