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
    [RoutePrefix("fingers")]
    [Route("{action=index}")]
    public class FingersController : Controller
    {
        private MyDbContext db = new MyDbContext();
        private IFingerService _FingerService;
        private IPersonService _PersonService;

        public FingersController()
        {
            _FingerService = new FingerService(new FingerRepository(db));
            _PersonService = new PersonService(new PersonRepository(db));
        }

        [HttpGet] //localhost:xxx/users/1/15
        [Route("{page?}/{maxByPage?}/{searchField?}")]
        public ActionResult Index(int page = 1, int maxByPage = MyConstants.MAX_BY_PAGE, string SearchField = "")
        {
            List<Finger> fingers = _FingerService.FindAllIncludes(page, maxByPage, SearchField);
            ViewBag.NextExist = _FingerService.NextExist(page, maxByPage, SearchField);
            ViewBag.Page = page;
            ViewBag.MaxByPage = maxByPage;
            ViewBag.SearchField = SearchField;
            return View("Index", fingers);
        }

        [HttpGet]
        [Route("Details/{id}")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Finger finger = _FingerService.FindByIdIncludes(id);
            if (finger == null)
            {
                return HttpNotFound();
            }
            return View(finger);
        }

        [HttpGet]
        [Route("Create")]
        public ActionResult Create()
        {
            ViewBag.OwnerId = new SelectList(_PersonService.GetAllExcludes(), "Id", "Name");
            return View();
        }

        // POST: People/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public ActionResult Create([Bind(Include = "Id,Name, OwnerId", Exclude = "Owner")] Finger finger, int? OwnerId)
        {
            ModelState.Remove("Owner");
            if (ModelState.IsValid && OwnerId.HasValue)
            {
                finger.Owner = _PersonService.FindByIdExcludes(OwnerId);
                _FingerService.Save(finger,finger.Owner);
                return RedirectToAction("Index");
            }
            ViewBag.OwnerId = new SelectList(_PersonService.GetAllExcludes(), "Id", "Name");
            return View(finger);
        }

        [HttpGet]
        [Route("Edit/{id?}")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Finger finger = _FingerService.FindByIdIncludes(id);
            if (finger == null)
            {
                return HttpNotFound();
            }
            ViewBag.OwnerId = new SelectList(_PersonService.GetAllExcludes(), "Id", "Name");
            return View(finger);
        }

        // POST: People/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit")]
        public ActionResult Edit([Bind(Include = "Id,Name")] Finger finger, int? OwnerId)
        {
            ModelState.Remove("Owner");
            if (ModelState.IsValid && OwnerId.HasValue)
            {
                finger.Owner = _PersonService.FindByIdExcludes(OwnerId);
                _FingerService.Update(finger,finger.Owner);
                return RedirectToAction("Index");
            }
            ViewBag.OwnerId = new SelectList(_PersonService.GetAllExcludes(), "Id", "Name");
            return View(finger);
        }

        [HttpGet]
        [Route("Delete/{id}")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Finger finger = _FingerService.FindByIdIncludes(id);
            if (finger == null)
            {
                return HttpNotFound();
            }
            return View(finger);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Delete/{id}")]
        public ActionResult DeleteConfirmed(int id)
        {
            _FingerService.Delete(id);
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
