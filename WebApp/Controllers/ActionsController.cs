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

namespace WebApp.Controllers
{
    [RoutePrefix("actions")]
    [Route("{action=index}")]
    public class ActionsController : Controller
    {
        private MyDbContext db = new MyDbContext();
        #region Services
        private IActionService _ActionService;
        private IPersonService _PersonService;
        #endregion

        public ActionsController()
        {
            #region Services Init
            _ActionService = new ActionService(new ActionRepository(db));
            _PersonService = new PersonService(new PersonRepository(db));
            #endregion
        }

        [HttpGet]
        [Route("{page?}/{maxByPage?}/{searchField?}")]
        public ActionResult Index(int page = 1, int maxByPage = MyConstants.MAX_BY_PAGE, string SearchField = "")
        {
            List<Models.Action> elements = _ActionService.FindAllIncludes(page, maxByPage, SearchField);
            ViewBag.NextExist = _ActionService.NextExist(page, maxByPage, SearchField);
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
            Models.Action element = _ActionService.FindByIdIncludes(id);
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
            ViewBag.People = new MultiSelectList(_PersonService.GetAllExcludes(), "Id", "Name", null);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public ActionResult Create([Bind(Include = "Id,Name")] Models.Action element, int?[] People)
        {
            #region ClassManyToMany
            if (ModelState.IsValid && People != null)
            {
                List<Person> owners = People != null ? _PersonService.FindManyByIdExcludes(People) : null;
                _ActionService.Save(element, owners);
                return RedirectToAction("Index");
            }
            #endregion

            ViewBag.People = new MultiSelectList(_PersonService.GetAllExcludes(), "Id", "Name", null);
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
            Models.Action element = _ActionService.FindByIdIncludes(id);
            if (element == null)
            {
                return HttpNotFound();
            }
            ViewBag.People = new MultiSelectList(_PersonService.GetAllExcludes(), "Id", "Name", null);
            return View(element);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit")]
        public ActionResult Edit([Bind(Include = "Id,Name")] Models.Action element, int?[] People)
        {
            #region ClassManyToMany
            if (ModelState.IsValid && People != null)
            {
                List<Person> owners = _PersonService.FindManyByIdExcludes(People);
                _ActionService.Update(element, owners);
                return RedirectToAction("Index");
            }
            #endregion

            ViewBag.People = new MultiSelectList(_PersonService.GetAllExcludes(), "Id", "Name", null);
            return View();
        }

        [HttpGet]
        [Route("Delete/{id}")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Action element = _ActionService.FindByIdIncludes(id);
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
            _ActionService.Delete(id);
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
