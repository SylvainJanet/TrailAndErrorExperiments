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

        public override void Delete(params object[] objs)
        {
            int? id = (int?)objs[0];
            using (MyDbContext db = new MyDbContext())
            {
                IPersonService _PersonService = new PersonService(new PersonRepository(db));
                foreach (Person person in _PersonService.GetAllExcludes(1, int.MaxValue, null, p => p.Vision.Id == id))
                {
                    _PersonService.UpdateOne(person, "Vision", null);
                }
            }
            _repository.Delete(objs);
        }

        public override void Delete(WorldVision t)
        {
            using (MyDbContext db = new MyDbContext())
            {
                IPersonService _PersonService = new PersonService(new PersonRepository(db));
                foreach (Person person in _PersonService.GetAllExcludes(1, int.MaxValue, null, p => p.Vision.Id == t.Id))
                {
                    _PersonService.UpdateOne(person, "Vision", null);
                }
            }
            _repository.Delete(t);
        }

        public override Expression<Func<IQueryable<WorldVision>, IOrderedQueryable<WorldVision>>> OrderExpression()
        {
            return null;
        }

        public override void Save(WorldVision t)
        {
            _repository.Save(t);
        }

        public override Expression<Func<WorldVision, bool>> SearchExpression(string searchField = "")
        {
            searchField = searchField.Trim().ToLower();
            return b => b.Name.Contains(searchField);
        }

        public override void Update(WorldVision t)
        {
            _repository.Update(t);
        }

        public override void UpdateOne(WorldVision t, string propertyName, object newValue)
        {
            _repository.UpdateOne(t, propertyName, newValue);
        }
    }
}