using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;
using WebApp.Repositories;
using WebApp.Tools.Generic;

namespace WebApp.Service
{
    public class ThoughtService : GenericService<Thought>, IThoughtService
    {
        public ThoughtService(IThoughtRepository thoughtRepository) : base(thoughtRepository)
        {

        }

        public override void Delete(params object[] objs)
        {
            int? id = (int?)objs[0];
            using (MyDbContext db = new MyDbContext())
            {
                IPersonService _PersonService = new PersonService(new PersonRepository(db));
                foreach (Person person in _PersonService.GetAllExcludes(1, int.MaxValue, null, t => t.ComfortableThoughts.Where(p => p.Id == id).Count() >= 1))
                {
                    _PersonService.UpdateOne(person, "ComfortableThoughts", person.ComfortableThoughts.Where(p => p.Id != id).ToList());
                }
                foreach (Person person in _PersonService.GetAllExcludes(1, int.MaxValue, null, t => t.SecretThoughts.Where(p => p.Id == id).Count() >= 1))
                {
                    _PersonService.UpdateOne(person, "SecretThoughts", person.SecretThoughts.Where(p => p.Id != id).ToList());
                }
            }
            _repository.Delete(objs);
        }

        public override void Delete(Thought t)
        {
            using (MyDbContext db = new MyDbContext())
            {
                IPersonService _PersonService = new PersonService(new PersonRepository(db));
                foreach (Person person in _PersonService.GetAllExcludes(1, int.MaxValue, null, th => th.ComfortableThoughts.Where(p => p.Id == t.Id).Count() >= 1))
                {
                    _PersonService.UpdateOne(person, "ComfortableThoughts", person.ComfortableThoughts.Where(p => p.Id != t.Id).ToList());
                }
                foreach (Person person in _PersonService.GetAllExcludes(1, int.MaxValue, null, th => th.SecretThoughts.Where(p => p.Id == t.Id).Count() >= 1))
                {
                    _PersonService.UpdateOne(person, "SecretThoughts", person.SecretThoughts.Where(p => p.Id != t.Id).ToList());
                }
            }
            _repository.Delete(t);
        }

        public override Expression<Func<IQueryable<Thought>, IOrderedQueryable<Thought>>> OrderExpression()
        {
            return null;
        }

        public override void Save(Thought t)
        {
            object owners;
            if (t.PeopleAtEase == null)
                owners = new PropToNull("PeopleAtEase");
            else
                owners = t.PeopleAtEase;
            object owners2;
            if (t.PeopleTimid == null)
                owners2 = new PropToNull("PeopleTimid");
            else
                owners2 = t.PeopleTimid;
            _repository.Save(t, owners, owners2);
        }

        public override Expression<Func<Thought, bool>> SearchExpression(string searchField = "")
        {
            searchField = searchField.Trim().ToLower();
            return t => t.Name.Trim().ToLower().Contains(searchField);
        }

        public override void Update(Thought t)
        {
            object owners;
            if (t.PeopleAtEase == null)
                owners = new PropToNull("PeopleAtEase");
            else
                owners = t.PeopleAtEase;
            object owners2;
            if (t.PeopleTimid == null)
                owners2 = new PropToNull("PeopleTimid");
            else
                owners2 = t.PeopleTimid;
            _repository.Update(t, owners, owners2);
        }

        public override void UpdateOne(Thought t, string propertyName, object newValue)
        {
            _repository.UpdateOne(t, propertyName, newValue);
        }
    }
}