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

        public override void Delete(params object[] objs)
        {
            _repository.Delete(objs);
        }

        public override void Delete(Idea t)
        {
            _repository.Delete(t);
        }

        public override Expression<Func<IQueryable<Idea>, IOrderedQueryable<Idea>>> OrderExpression()
        {
            return null;
        }

        public override void Save(Idea t)
        {
            _repository.Save(t, t.Owners);
        }

        public override Expression<Func<Idea, bool>> SearchExpression(string searchField = "")
        {
            searchField = searchField.Trim().ToLower();
            return b => b.Name.Contains(searchField);
        }

        public override void Update(Idea t)
        {
            _repository.Update(t, t.Owners);
        }

        public override void UpdateOne(Idea t, string propertyName, object newValue)
        {
            _repository.UpdateOne(t, propertyName, newValue);
        }
    }
}