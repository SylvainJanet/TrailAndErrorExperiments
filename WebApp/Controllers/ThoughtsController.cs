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
                element.PeopleAtEase = PeopleComfy != null ? _PersonService.FindManyByIdExcludes(PeopleComfy) : null;
                element.PeopleTimid = PeopleSecret != null ? _PersonService.FindManyByIdExcludes(PeopleSecret) : null;
                _ThoughtService.Save(element);
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
        public ActionResult Edit([Bind(Include = "Id,Name")] Thought element, object[] PeopleComfy, object[] PeopleSecret)
        {
            if (ModelState.IsValid)
            {
                element.PeopleAtEase = PeopleComfy != null ? _PersonService.FindManyByIdExcludes(PeopleComfy) : null;
                element.PeopleTimid = PeopleSecret != null ? _PersonService.FindManyByIdExcludes(PeopleSecret) : null;
                _ThoughtService.Update(element);
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
