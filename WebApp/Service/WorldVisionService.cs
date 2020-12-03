using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Service
{
    public class WorldVisionService : GenericService<WorldVision> , IWorldVisionService
    {
        public WorldVisionService(IWorldVisionRepository worldVisionRepository) : base(worldVisionRepository)
        {

        }

        public override Expression<Func<IQueryable<WorldVision>, IOrderedQueryable<WorldVision>>> OrderExpression()
        {
            return null;
        }

        public override Expression<Func<WorldVision, bool>> SearchExpression(string searchField = "")
        {
            searchField = searchField.Trim().ToLower();
            return b => b.Name.Contains(searchField);
        }
    }
}