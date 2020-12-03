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
    [RoutePrefix("people")]
    [Route("{action=index}")]
    public class PeopleController : Controller
    {
        private MyDbContext db = new MyDbContext();
        private IPersonService _PersonService;
        private IIdeaService _IdeaService;
        private IFingerService _FingerService;
        private IBrainService _BrainService;
        private IActionService _ActionService;
        private IWorldVisionService _WorldVisionService;

        public PeopleController()
        {
            _PersonService = new PersonService(new PersonRepository(db));
            _IdeaService = new IdeaService(new IdeaRepository(db));
            _FingerService = new FingerService(new FingerRepository(db));
            _BrainService = new BrainService(new BrainRepository(db));
            _ActionService = new ActionService(new ActionRepository(db));
            _WorldVisionService = new WorldVisionService(new WorldVisionRepository(db));
        }
        
        [HttpGet] //localhost:xxx/users/1/15
        [Route("{page?}/{maxByPage?}/{searchField?}")]
        public ActionResult Index(int page = 1, int maxByPage = MyConstants.MAX_BY_PAGE, string SearchField = "")
        {
            List<Person> persons = _PersonService.FindAllIncludes(page, maxByPage, SearchField);
            ViewBag.NextExist = _PersonService.NextExist(page, maxByPage, SearchField);
            ViewBag.Page = page;
            ViewBag.MaxByPage = maxByPage;
            ViewBag.SearchField = SearchField;
            return View("Index",persons);
        }

        [HttpGet]
        [Route("Details/{id}")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = _PersonService.FindByIdIncludes(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        [HttpGet]
        [Route("Create")]
        public ActionResult Create()
        {
            ViewBag.Ideas = new MultiSelectList(_IdeaService.GetAllExcludes(), "Id", "Name", null);
            ViewBag.Actions = new MultiSelectList(_ActionService.GetAllExcludes(), "Id", "Name", null);
            ViewBag.WorldVisionId = new SelectList(_WorldVisionService.GetAllExcludes(), "Id", "Name", null);
            return View();
        }

        // POST: People/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public ActionResult Create([Bind(Include = "Id,Name")] Person person, int?[] Ideas, int?[] Actions, int? WorldVisionId)
        {
            if (ModelState.IsValid)
            {
                List<Idea> ideas = Ideas != null ? _IdeaService.FindManyByIdExcludes(Ideas) : null;
                List<Models.Action> actions = Actions != null ? _ActionService.FindManyByIdExcludes(Actions) : null;
                WorldVision worldVision = WorldVisionId != null ? _WorldVisionService.FindByIdExcludes(WorldVisionId) : null;
                _PersonService.Save(person, ideas, actions, worldVision);
                return RedirectToAction("Index");
            }
            ViewBag.Ideas = new MultiSelectList(_IdeaService.GetAllExcludes(), "Id", "Name", null);
            ViewBag.Actions = new MultiSelectList(_ActionService.GetAllExcludes(), "Id", "Name", null);
            ViewBag.WorldVisionId = new SelectList(_WorldVisionService.GetAllExcludes(), "Id", "Name", null);
            return View(person);
        }

        [HttpGet]
        [Route("Edit/{id?}")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = _PersonService.FindByIdIncludes(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            ViewBag.Ideas = new MultiSelectList(_IdeaService.GetAllExcludes(), "Id", "Name", null);
            ViewBag.Actions = new MultiSelectList(_ActionService.GetAllExcludes(), "Id", "Name", null);
            ViewBag.WorldVisionId = new SelectList(_WorldVisionService.GetAllExcludes(), "Id", "Name", null);
            TempData["Brain"] = person.Brain;
            TempData["Fingers"] = person.Fingers;
            TempData.Keep();
            return View(person);
        }

        // POST: People/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit")]
        public ActionResult Edit([Bind(Include = "Id,Name")] Person person, int?[] Ideas, int?[] Actions, int? WorldVisionId )
        {
            if (ModelState.IsValid)
            {
                List<Idea> ideas = Ideas != null ? _IdeaService.FindManyByIdExcludes(Ideas) : null;
                List<Models.Action> actions = Actions != null ? _ActionService.FindManyByIdExcludes(Actions) : null;
                WorldVision worldVision = WorldVisionId != null ? _WorldVisionService.FindByIdExcludes(WorldVisionId) : null;
                Brain brain = TempData["Brain"] as Brain;
                List<Finger> fingers = TempData["Fingers"] as List<Finger>;
                foreach (Finger finger in _FingerService.GetAllExcludes(1, int.MaxValue, null, f => f.OwnerId == person.Id && fingers.Where(ff => ff.Id == f.Id).Count()==0))
                {
                    _FingerService.Delete(finger);
                }
                foreach (Models.Action el in _ActionService.GetAllExcludes(1, int.MaxValue, null, t => t.People.Count() == 1 && t.People.Where(s => s.Id == person.Id).Count() == 1).Where(t => !Actions.Contains(t.Id)))
                {
                    _ActionService.Delete(el);
                }
                _PersonService.Update(person, ideas, brain, fingers, actions, worldVision);
                return RedirectToAction("Index");
            }
            ViewBag.Ideas = new MultiSelectList(_IdeaService.GetAllExcludes(), "Id", "Name", null);
            ViewBag.Actions = new MultiSelectList(_ActionService.GetAllExcludes(), "Id", "Name", null);
            ViewBag.WorldVisionId = new SelectList(_WorldVisionService.GetAllExcludes(), "Id", "Name", null);
            TempData.Keep();
            return View(person);
        }

        [HttpGet]
        [Route("Delete/{id}")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = _PersonService.FindByIdIncludes(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Delete/{id}")]
        public ActionResult DeleteConfirmed(int id)
        {
            foreach (Finger finger in _FingerService.GetAllExcludes(1, int.MaxValue, null, f => f.OwnerId == id))
            {
                _FingerService.Delete(finger);
            }   
            foreach (Models.Action el in _ActionService.GetAllExcludes(1, int.MaxValue, null, t => t.People.Count() == 1 && t.People.Where(p => p.Id == id).Count()==1))
            {
                _ActionService.Delete(el);
            }
            _PersonService.Delete(id);
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
