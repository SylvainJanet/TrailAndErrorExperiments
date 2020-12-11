using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.Repositories;
using WebApp.Service;
using WebApp.Tools.Generic;

namespace WebApp.Controllers
{
    [RoutePrefix("thoughts")]
    [Route("{action=index}")]
    public class ThoughtsController : Controller
    {
        private readonly MyDbContext db = new MyDbContext();
        #region Services
        private readonly IThoughtService _ThoughtService;
        private readonly IPersonService _PersonService;
        #endregion

        public ThoughtsController()
        {
            #region Services Init
            _PersonService = new PersonService(new PersonRepository(db));
            _ThoughtService = new ThoughtService(new ThoughtRepository(db));
            #endregion
        }

        [HttpGet]
        [Route("{page?}/{maxByPage?}/{searchField?}")]
        public ActionResult Index(int page = 1, int maxByPage = MyConstants.MAX_BY_PAGE, string SearchField = "")
        {
            List<Thought> elements = _ThoughtService.FindAllIncludes(page, maxByPage, SearchField);
            ViewBag.NextExist = _ThoughtService.NextExist(page, maxByPage, SearchField);
            ViewBag.Page = page;
            ViewBag.MaxByPage = maxByPage;
            ViewBag.SearchField = SearchField;
            return View("Index", elements);
        }

        [HttpGet]
        [Route("Details/{id}")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Thought element = _ThoughtService.FindByIdIncludes(id);
            if (element == null)
            {
                return HttpNotFound();
            }
            return View(element);
        }

        [HttpGet]
        [Route("Create")]
        public ActionResult Create()
        {
            ViewBag.PeopleComfy = new MultiSelectList(_PersonService.GetAllExcludes(), "Id", "Name", null);
            ViewBag.PeopleSecret = ViewBag.PeopleComfy;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public ActionResult Create([Bind(Include = "Id,Name")] Thought element, object[] PeopleComfy, object[] PeopleSecret)
        {
            if (ModelState.IsValid)
            {
                object owners;
                if (PeopleComfy == null)
                    owners = new PropToNull("PeopleAtEase");
                else
                    owners = _PersonService.FindManyByIdExcludes(PeopleComfy);
                object owners2;
                if (PeopleSecret == null)
                    owners2 = new PropToNull("PeopleTimid");
                else
                    owners2 = _PersonService.FindManyByIdExcludes(PeopleSecret);
                _ThoughtService.Save(element, owners, owners2);
                return RedirectToAction("Index");
            }
            ViewBag.PeopleComfy = new MultiSelectList(_PersonService.GetAllExcludes(), "Id", "Name", null);
            ViewBag.PeopleSecret = ViewBag.PeopleComfy;
            return View(element);
        }

        [HttpGet]
        [Route("Edit/{id?}")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Thought element = _ThoughtService.FindByIdIncludes(id);
            if (element == null)
            {
                return HttpNotFound();
            }
            ViewBag.PeopleComfy = new MultiSelectList(_PersonService.GetAllExcludes(), "Id", "Name", null);
            ViewBag.PeopleSecret = ViewBag.PeopleComfy;
            return View(element);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit")]
        public ActionResult Edit([Bind(Include = "Id,Name")] Thought element, int?[] PeopleComfy, int?[] PeopleSecret)
        {
            if (ModelState.IsValid)
            {
                List<Person> owners = PeopleComfy != null ? _PersonService.FindManyByIdExcludes(PeopleComfy) : null;
                List<Person> owners2 = PeopleSecret != null ? _PersonService.FindManyByIdExcludes(PeopleSecret) : null;
                if (owners == null)
                {
                    _ThoughtService.Update(element, new PropToNull("PeopleAtEase"), owners2);
                }
                else
                {
                    _ThoughtService.Update(element, owners, owners2);
                }
                return RedirectToAction("Index");
            }
            ViewBag.PeopleComfy = new MultiSelectList(_PersonService.GetAllExcludes(), "Id", "Name", null);
            ViewBag.PeopleSecret = ViewBag.PeopleComfy;
            return View(element);
        }

        [HttpGet]
        [Route("Delete/{id}")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Thought element = _ThoughtService.FindByIdIncludes(id);
            if (element == null)
            {
                return HttpNotFound();
            }
            return View(element);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Delete/{id}")]
        public ActionResult DeleteConfirmed(int id)
        {
            foreach (Person person in _PersonService.GetAllExcludes(1, int.MaxValue, null, t => t.ComfortableThoughts.Where(p => p.Id == id).Count() >= 1))
            {
                _PersonService.UpdateOne(person, "ComfortableThoughts", person.ComfortableThoughts.Where(p => p.Id != id).ToList());
            }
            foreach (Person person in _PersonService.GetAllExcludes(1, int.MaxValue, null, t => t.SecretThoughts.Where(p => p.Id == id).Count() >= 1))
            {
                _PersonService.UpdateOne(person, "SecretThoughts", person.SecretThoughts.Where(p => p.Id != id).ToList());
            }
            _ThoughtService.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("Search")]
        public ActionResult Search([Bind(Include = ("page, maxByPage, SearchField"))] int page = 1, int maxByPage = MyConstants.MAX_BY_PAGE, string searchField = "")
        {
            if (searchField.Trim().Equals(""))
                return RedirectToAction("Index");
            return Index(page, maxByPage, searchField);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
