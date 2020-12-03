using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Service
{
    public class FingerService : GenericService<Finger>, IFingerService
    {
        public FingerService(IFingerRepository fingerRepository) : base(fingerRepository)
        {

        }

        public override Expression<Func<IQueryable<Finger>, IOrderedQueryable<Finger>>> OrderExpression()
        {
            return null;
        }

        public override Expression<Func<Finger, bool>> SearchExpression(string searchField = "")
        {
            searchField = searchField.Trim().ToLower();
            return b => b.Name.Contains(searchField);
        }
    }
}