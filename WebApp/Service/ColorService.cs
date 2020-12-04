using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Service
{
    public class ColorService : GenericService<Models.Color>, IColorService
    {
        public ColorService(IColorRepository colorRepository) : base(colorRepository)
        {

        }

        public override Expression<Func<IQueryable<Color>, IOrderedQueryable<Color>>> OrderExpression()
        {
            return null;
        }

        public override Expression<Func<Color, bool>> SearchExpression(string searchField = "")
        {
            searchField = searchField.Trim().ToLower();
            return c => c.Name.Trim().ToLower().Contains(searchField);
        }
    }
}