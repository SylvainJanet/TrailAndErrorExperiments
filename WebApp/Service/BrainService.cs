using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Service
{
    public class BrainService : GenericService<Brain>, IBrainService
    {
        public BrainService(IBrainRepository brainRepository) : base(brainRepository)
        {

        }

        public override Expression<Func<IQueryable<Brain>, IOrderedQueryable<Brain>>> OrderExpression()
        {
            return null;
        }

        public override Expression<Func<Brain, bool>> SearchExpression(string searchField = "")
        {
            searchField = searchField.Trim().ToLower();
            return b => b.Name.Contains(searchField);
        }
    }
}