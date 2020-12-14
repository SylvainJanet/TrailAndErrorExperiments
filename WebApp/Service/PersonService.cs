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
    public class PersonService : GenericService<Person>, IPersonService
    {
        public PersonService(IPersonRepository personRepository) : base(personRepository)
        {

        }

        public override void Delete(params object[] objs)
        {
            using (MyDbContext db = new MyDbContext())
            {
                IFingerService _FingerService = new FingerService(new FingerRepository(db));
                IActionService _ActionService = new ActionService(new ActionRepository(db));
                IThoughtService _ThoughtService = new ThoughtService(new ThoughtRepository(db));
                int? id = (int?)objs[0];
                foreach (Finger finger in _FingerService.GetAllExcludes(1, int.MaxValue, null, f => f.OwnerId == id))
                {
                    _FingerService.Delete(finger);
                }
                foreach (Models.Action el in _ActionService.GetAllExcludes(1, int.MaxValue, null, a => a.People.Count() == 1 && a.People.Where(p => p.Id == id).Count() == 1))
                {
                    _ActionService.Delete(el);
                }
                foreach (Thought thought in _ThoughtService.GetAllExcludes(1, int.MaxValue, null, th => th.PeopleAtEase.Where(p => p.Id == id).Count() >= 1))
                {
                    _ThoughtService.UpdateOne(thought, "PeopleAtEase", thought.PeopleAtEase.Where(p => p.Id != id).ToList());
                }
                foreach (Thought thought in _ThoughtService.GetAllExcludes(1, int.MaxValue, null, th => th.PeopleTimid.Where(p => p.Id == id).Count() >= 1))
                {
                    _ThoughtService.UpdateOne(thought, "PeopleTimid", thought.PeopleTimid.Where(p => p.Id != id).ToList());
                }
            }
            _repository.Delete(objs);
        }

        public override void Delete(Person t)
        {
            using (MyDbContext db = new MyDbContext())
            {
                IFingerService _FingerService = new FingerService(new FingerRepository(db));
                IActionService _ActionService = new ActionService(new ActionRepository(db));
                IThoughtService _ThoughtService = new ThoughtService(new ThoughtRepository(db));
                foreach (Finger finger in _FingerService.GetAllExcludes(1, int.MaxValue, null, f => f.OwnerId == t.Id))
                {
                    _FingerService.Delete(finger);
                }
                foreach (Models.Action el in _ActionService.GetAllExcludes(1, int.MaxValue, null, a => a.People.Count() == 1 && a.People.Where(p => p.Id == t.Id).Count() == 1))
                {
                    _ActionService.Delete(el);
                }
                foreach (Thought thought in _ThoughtService.GetAllExcludes(1, int.MaxValue, null, th => th.PeopleAtEase.Where(p => p.Id == t.Id).Count() >= 1))
                {
                    _ThoughtService.UpdateOne(thought, "PeopleAtEase", thought.PeopleAtEase.Where(p => p.Id != t.Id).ToList());
                }
                foreach (Thought thought in _ThoughtService.GetAllExcludes(1, int.MaxValue, null, th => th.PeopleTimid.Where(p => p.Id == t.Id).Count() >= 1))
                {
                    _ThoughtService.UpdateOne(thought, "PeopleTimid", thought.PeopleTimid.Where(p => p.Id != t.Id).ToList());
                }
            }
            _repository.Delete(t);
        }

        public override Expression<Func<IQueryable<Person>, IOrderedQueryable<Person>>> OrderExpression()
        {
            return null;
        }

        public override void Save(Person t)
        {
            object owners, owners2;
            if (t.ComfortableThoughts == null)
                owners = new PropToNull("ComfortableThoughts");
            else
                owners = t.ComfortableThoughts;
            if (t.SecretThoughts == null)
                owners2 = new PropToNull("SecretThoughts");
            else
                owners2 = t.SecretThoughts;

            object FavoriteColor, LeastLikedColor;
            if (t.FavoriteColor == null)
                FavoriteColor = new PropToNull("FavoriteColor");
            else
                FavoriteColor = t.FavoriteColor;
            if (t.LeastLikedColor == null)
                LeastLikedColor = new PropToNull("LeastLikedColor");
            else
                LeastLikedColor = t.LeastLikedColor;

            _repository.Save(t, t.Address, t.Ideas, t.Actions, t.Vision, owners, owners2, FavoriteColor, LeastLikedColor);
        }

        public override Expression<Func<Person, bool>> SearchExpression(string searchField = "")
        {
            searchField = searchField.Trim().ToLower();
            return b => b.Name.Contains(searchField);
        }

        public override void Update(Person t)
        {
            using (MyDbContext db = new MyDbContext())
            {
                IFingerService _FingerService = new FingerService(new FingerRepository(db));
                IActionService _ActionService = new ActionService(new ActionRepository(db));
                foreach (Finger finger in _FingerService.GetAllExcludes(1, int.MaxValue, null, f => f.OwnerId == t.Id).Where(f => t.Fingers.Where(ff => ff.Id == f.Id).Count() == 0))
                {
                    _FingerService.Delete(finger);
                }
                foreach (Models.Action el in _ActionService.GetAllExcludes(1, int.MaxValue, null, a => a.People.Count() == 1 && a.People.Where(s => s.Id == t.Id).Count() == 1).Where(a => !t.Actions.Select(aa => aa.Id).Contains(a.Id)))
                {
                    _ActionService.Delete(el);
                }
            }
            object owners, owners2;
            if (t.ComfortableThoughts == null)
                owners = new PropToNull("ComfortableThoughts");
            else
                owners = t.ComfortableThoughts;
            if (t.SecretThoughts == null)
                owners2 = new PropToNull("SecretThoughts");
            else
                owners2 = t.SecretThoughts;

            object FavoriteColor, LeastLikedColor;
            if (t.FavoriteColor == null)
                FavoriteColor = new PropToNull("FavoriteColor");
            else
                FavoriteColor = t.FavoriteColor;
            if (t.LeastLikedColor == null)
                LeastLikedColor = new PropToNull("LeastLikedColor");
            else
                LeastLikedColor = t.LeastLikedColor;

            _repository.Update(t, t.Address, t.Ideas, t.Actions, t.Vision, t.Brain, t.Fingers, owners, owners2, FavoriteColor, LeastLikedColor);
        }

        public override void UpdateOne(Person t, string propertyName, object newValue)
        {
            _repository.UpdateOne(t, propertyName, newValue);
        }
    }
}