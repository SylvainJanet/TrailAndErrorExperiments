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
    [RoutePrefix("colors")]
    [Route("{action=index}")]
    public class ColorsController : Controller
    {
        private readonly MyDbContext db = new MyDbContext();
        #region Services
        private readonly IColorService _ColorService;
        private readonly IPersonService _PersonService;
        #endregion

        public ColorsController()
        {
            #region Services Init
            _ColorService = new ColorService(new ColorRepository(db));
            _PersonService = new PersonService(new PersonRepository(db));
            #endregion
        }

        [HttpGet]
        [Route("{page?}/{maxByPage?}/{searchField?}")]
        public ActionResult Index(int page = 1, int maxByPage = MyConstants.MAX_BY_PAGE, string SearchField = "")
        {
            List<Color> elements = _ColorService.FindAllIncludes(page, maxByPage, SearchField);
            ViewBag.NextExist = _ColorService.NextExist(page, maxByPage, SearchField);
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
           Color element = _ColorService.FindByIdIncludes(id);
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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public ActionResult Create(Color element)
        {
            if (ModelState.IsValid)
            {
                _ColorService.Save(element);
                return RedirectToAction("Index");
            }
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
           Color element = _ColorService.FindByIdIncludes(id);
            if (element == null)
            {
                return HttpNotFound();
            }
            return View(element);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit")]
        public ActionResult Edit(Color element)
        {
            if (ModelState.IsValid)
            {
                _ColorService.Update(element);
                return RedirectToAction("Index");
            }
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
            Color element = _ColorService.FindByIdIncludes(id);
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
            _ColorService.Delete(id);
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
