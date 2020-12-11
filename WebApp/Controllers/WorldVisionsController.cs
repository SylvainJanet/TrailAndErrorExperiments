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
    [RoutePrefix("WorldVisions")]
    [Route("{action=index}")]
    public class WorldVisionsController : Controller
    {
        private readonly MyDbContext db = new MyDbContext();
        #region Services
        private readonly IWorldVisionService _WorldVisionService;
        private readonly IPersonService _PersonService;
        #endregion

        public WorldVisionsController()
        {
            #region Services Init
            _WorldVisionService = new WorldVisionService(new WorldVisionRepository(db));
            _PersonService = new PersonService(new PersonRepository(db));
            #endregion
        }

        [HttpGet]
        [Route("{page?}/{maxByPage?}/{searchField?}")]
        public ActionResult Index(int page = 1, int maxByPage = MyConstants.MAX_BY_PAGE, string SearchField = "")
        {
            List<WorldVision> elements = _WorldVisionService.FindAllIncludes(page, maxByPage, SearchField);
            ViewBag.NextExist = _WorldVisionService.NextExist(page, maxByPage, SearchField);
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
            WorldVision element = _WorldVisionService.FindByIdIncludes(id);
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
        public ActionResult Create(WorldVision element)
        {
            #region ClassOneRequiredToMany
            if (ModelState.IsValid)
            {
                _WorldVisionService.Save(element);
                return RedirectToAction("Index");
            }
            #endregion
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
            WorldVision element = _WorldVisionService.FindByIdIncludes(id);
            if (element == null)
            {
                return HttpNotFound();
            }
            return View(element);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit")]
        public ActionResult Edit(WorldVision element)
        { 
            #region ClassOneRequiredToOne
            if (ModelState.IsValid)
            {
                _WorldVisionService.Update(element);
                return RedirectToAction("Index");
            }
            #endregion
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
            WorldVision element = _WorldVisionService.FindByIdIncludes(id);
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
            _WorldVisionService.Delete(id);
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
