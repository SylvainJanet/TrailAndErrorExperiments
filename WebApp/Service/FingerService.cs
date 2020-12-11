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

        public override void Delete(params object[] objs)
        {
            _repository.Delete(objs);
        }

        public override void Delete(Finger t)
        {
            _repository.Delete(t);
        }

        public override Expression<Func<IQueryable<Finger>, IOrderedQueryable<Finger>>> OrderExpression()
        {
            return null;
        }

        public override void Save(Finger t)
        {
            _repository.Save(t, t.Owner);
        }

        public override Expression<Func<Finger, bool>> SearchExpression(string searchField = "")
        {
            searchField = searchField.Trim().ToLower();
            return b => b.Name.Contains(searchField);
        }

        public override void Update(Finger t)
        {
            _repository.Update(t, t.Owner);
        }

        public override void UpdateOne(Finger t, string propertyName, object newValue)
        {
            _repository.UpdateOne(t, propertyName, newValue);
        }
    }
}