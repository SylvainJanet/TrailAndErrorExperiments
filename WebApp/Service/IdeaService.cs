using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Service
{
    public class IdeaService : GenericService<Idea>, IIdeaService
    {
        public IdeaService(IIdeaRepository ideaRepository) : base(ideaRepository)
        {

        }

        public override Expression<Func<IQueryable<Idea>, IOrderedQueryable<Idea>>> OrderExpression()
        {
            return null;
        }

        public override Expression<Func<Idea, bool>> SearchExpression(string searchField = "")
        {
            searchField = searchField.Trim().ToLower();
            return b => b.Name.Contains(searchField);
        }
    }
}