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
    [RoutePrefix("brains")]
    [Route("{action=index}")]
    public class BrainsController : Controller
    {
        private readonly MyDbContext db = new MyDbContext();
        private readonly IBrainService _BrainService;
        private readonly IPersonService _PersonService;

        public BrainsController()
        {
            _BrainService = new BrainService(new BrainRepository(db));
            _PersonService = new PersonService(new PersonRepository(db));
        }

        [HttpGet] //localhost:xxx/users/1/15
        [Route("{page?}/{maxByPage?}/{searchField?}")]
        public ActionResult Index(int page = 1, int maxByPage = MyConstants.MAX_BY_PAGE, string SearchField = "")
        {
            List<Brain> brains = _BrainService.FindAllIncludes(page, maxByPage, SearchField);
            ViewBag.NextExist = _BrainService.NextExist(page, maxByPage, SearchField);
            ViewBag.Page = page;
            ViewBag.MaxByPage = maxByPage;
            ViewBag.SearchField = SearchField;
            return View("Index", brains);
        }

        [HttpGet]
        [Route("Details/{id}")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brain brain = _BrainService.FindByIdIncludes(id);
            if (brain == null)
            {
                return HttpNotFound();
            }
            return View(brain);
        }

        [HttpGet]
        [Route("Create")]
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(_PersonService.GetAllExcludes(1,int.MaxValue, null, p => p.Brain==null), "Id", "Name");
            return View();
        }

        // POST: People/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public ActionResult Create([Bind(Include = "Id, Name")] Brain brain, int? Id)
        {
            ModelState.Remove("Owner");
            if (ModelState.IsValid && Id.HasValue)
            {
                brain.Owner = _PersonService.FindByIdExcludesTracked(Id);
                _BrainService.Save(brain,brain.Owner);
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(_PersonService.GetAllExcludes(1, int.MaxValue, null, p => p.Brain == null), "Id", "Name");
            return View(brain);
        }

        [HttpGet]
        [Route("Edit/{id?}")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brain brain = _BrainService.FindByIdIncludes(id);
            if (brain == null)
            {
                return HttpNotFound();
            }
            return View(brain);
        }

        // POST: People/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit")]
        public ActionResult Edit([Bind(Include = "Id,Name,Owner,OwnerId")] Brain brain)
        {
            ModelState.Remove("Owner");
            if (ModelState.IsValid)
            {
                brain.Owner = _PersonService.FindByIdExcludes(brain.Id);
                _BrainService.Update(brain,brain.Owner);
                return RedirectToAction("Index");
            }
            return View(brain);
        }

        [HttpGet]
        [Route("Delete/{id}")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brain brain = _BrainService.FindByIdIncludes(id);
            if (brain == null)
            {
                return HttpNotFound();
            }
            return View(brain);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Delete/{id}")]
        public ActionResult DeleteConfirmed(int id)
        {
            _BrainService.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        //[ValidateAntiForgeryToken]
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
