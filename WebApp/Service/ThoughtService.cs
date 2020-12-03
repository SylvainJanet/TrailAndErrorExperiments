using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Service
{
    public class ThoughtService : GenericService<Thought>, IThoughtService
    {
        public ThoughtService(IThoughtRepository thoughtRepository) : base(thoughtRepository)
        {

        }

        public override Expression<Func<IQueryable<Thought>, IOrderedQueryable<Thought>>> OrderExpression()
        {
            return null;
        }

        public override Expression<Func<Thought, bool>> SearchExpression(string searchField = "")
        {
            searchField = searchField.Trim().ToLower();
            return t => t.Name.Trim().ToLower().Contains(searchField);
        }
    }
}