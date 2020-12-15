using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Service
{
    public class ColorService : TEMPGenericService<Models.Color>, IColorService
    {
        public ColorService(IColorRepository colorRepository) : base(colorRepository)
        {

        }

        //public override void Delete(params object[] objs)
        //{
        //    int? id = (int?)objs[0];
        //    using (MyDbContext db = new MyDbContext())
        //    {
        //        IPersonService _PersonService = new PersonService(new PersonRepository(db));
        //        foreach (Person person in _PersonService.GetAllExcludes(1, int.MaxValue, null, p => p.FavoriteColor.Id == id))
        //        {
        //            _PersonService.UpdateOne(person, "FavoriteColor", null);
        //        }
        //        foreach (Person person in _PersonService.GetAllExcludes(1, int.MaxValue, null, p => p.LeastLikedColor.Id == id))
        //        {
        //            _PersonService.UpdateOne(person, "LeastLikedColor", null);
        //        }
        //    }
        //    _repository.Delete(objs);
        //}

        //public override void Delete(Color t)
        //{
        //    using (MyDbContext db = new MyDbContext())
        //    {
        //        IPersonService _PersonService = new PersonService(new PersonRepository(db));
        //        foreach (Person person in _PersonService.GetAllExcludes(1, int.MaxValue, null, p => p.FavoriteColor.Id == t.Id))
        //        {
        //            _PersonService.UpdateOne(person, "FavoriteColor", null);
        //        }
        //        foreach (Person person in _PersonService.GetAllExcludes(1, int.MaxValue, null, p => p.LeastLikedColor.Id == t.Id))
        //        {
        //            _PersonService.UpdateOne(person, "LeastLikedColor", null);
        //        }
        //    }
        //    _repository.Delete(t);
        //}

        public override Expression<Func<IQueryable<Color>, IOrderedQueryable<Color>>> OrderExpression()
        {
            return null;
        }

        //public override void Save(Color t)
        //{
        //    _repository.Save(t);
        //}

        public override Expression<Func<Color, bool>> SearchExpression(string searchField = "")
        {
            searchField = searchField.Trim().ToLower();
            return c => c.Name.Trim().ToLower().Contains(searchField);
        }

        //public override void Update(Color t)
        //{
        //    _repository.Update(t);
        //}

        //public override void UpdateOne(Color t, string propertyName, object newValue)
        //{
        //    _repository.UpdateOne(t, propertyName, newValue);
        //}
    }
}