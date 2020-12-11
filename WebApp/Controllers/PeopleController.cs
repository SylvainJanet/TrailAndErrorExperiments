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
    [RoutePrefix("people")]
    [Route("{action=index}")]
    public class PeopleController : Controller
    {
        private readonly MyDbContext db = new MyDbContext();
        private readonly IPersonService _PersonService;
        private readonly IIdeaService _IdeaService;
        private readonly IFingerService _FingerService;
        private readonly IBrainService _BrainService;
        private readonly IActionService _ActionService;
        private readonly IWorldVisionService _WorldVisionService;
        private readonly IThoughtService _ThoughtService;
        private readonly IColorService _ColorService;
        private readonly IAddressService _AddressService;

        public PeopleController()
        {
            _PersonService = new PersonService(new PersonRepository(db));
            _IdeaService = new IdeaService(new IdeaRepository(db));
            _FingerService = new FingerService(new FingerRepository(db));
            _BrainService = new BrainService(new BrainRepository(db));
            _ActionService = new ActionService(new ActionRepository(db));
            _WorldVisionService = new WorldVisionService(new WorldVisionRepository(db));
            _ThoughtService = new ThoughtService(new ThoughtRepository(db));
            _ColorService = new ColorService(new ColorRepository(db));
            _AddressService = new AddressService(new AddressRepository(db));
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
            ViewBag.ThoughtsComfy = new MultiSelectList(_ThoughtService.GetAllExcludes(), "Id", "Name", null);
            ViewBag.ThoughtsSecret = ViewBag.ThoughtsComfy;
            ViewBag.FavoriteColorId = new SelectList(_ColorService.GetAllExcludes(), "Id", "Name", null);
            ViewBag.LeastLikedColorId = ViewBag.FavoriteColorId;
            ViewBag.Addresskeysstring = new SelectList(_AddressService.GetAllExcludes().Select(a => Address.KeysToDisplayString(a)));
            return View();
        }

        

        // POST: People/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public ActionResult Create([Bind(Include = "Id,Name")] Person person, object[] Ideas, object[] Actions, int? WorldVisionId, object[] ThoughtsComfy, object[] ThoughtsSecret, int? FavoriteColorId, int? LeastLikedColorId, string Addresskeysstring)
        {
            if (ModelState.IsValid)
            {
                person.Address = Addresskeysstring != null ? _AddressService.FindByIdExcludes(Address.DisplayStringToKeys(Addresskeysstring)) : null;
                person.Ideas = Ideas != null ? _IdeaService.FindManyByIdExcludes(Ideas) : null;
                person.Actions = Actions != null ? _ActionService.FindManyByIdExcludes(Actions) : null;
                person.Vision = WorldVisionId != null ? _WorldVisionService.FindByIdExcludes(WorldVisionId) : null;
                person.ComfortableThoughts = ThoughtsComfy != null ? _ThoughtService.FindManyByIdExcludes(ThoughtsComfy) : null;
                person.SecretThoughts = ThoughtsSecret != null ? _ThoughtService.FindManyByIdExcludes(ThoughtsSecret) : null;
                person.FavoriteColor = FavoriteColorId != null ? _ColorService.FindByIdExcludes(FavoriteColorId) : null;
                person.LeastLikedColor = LeastLikedColorId != null ? _ColorService.FindByIdExcludes(LeastLikedColorId) : null;
                _PersonService.Save(person);
                return RedirectToAction("Index");
            }
            ViewBag.Ideas = new MultiSelectList(_IdeaService.GetAllExcludes(), "Id", "Name", null);
            ViewBag.Actions = new MultiSelectList(_ActionService.GetAllExcludes(), "Id", "Name", null);
            ViewBag.WorldVisionId = new SelectList(_WorldVisionService.GetAllExcludes(), "Id", "Name", null);
            ViewBag.ThoughtsComfy = new MultiSelectList(_ThoughtService.GetAllExcludes(), "Id", "Name", null);
            ViewBag.ThoughtsSecret = ViewBag.ThoughtsComfy;
            ViewBag.FavoriteColorId = new SelectList(_ColorService.GetAllExcludes(), "Id", "Name", null);
            ViewBag.LeastLikedColorId = ViewBag.FavoriteColorId;
            ViewBag.Addresskeysstring = new SelectList(_AddressService.GetAllExcludes().Select(a => EntityWithKeys.KeysToDisplayString(a)));
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
            ViewBag.ThoughtsComfy = new MultiSelectList(_ThoughtService.GetAllExcludes(), "Id", "Name", null);
            ViewBag.ThoughtsSecret = ViewBag.ThoughtsComfy;
            ViewBag.FavoriteColorId = new SelectList(_ColorService.GetAllExcludes(), "Id", "Name", null);
            ViewBag.LeastLikedColorId = ViewBag.FavoriteColorId;
            ViewBag.Addresskeysstring = new SelectList(_AddressService.GetAllExcludes().Select(a => Address.KeysToDisplayString(a)));
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
        public ActionResult Edit([Bind(Include = "Id,Name")] Person person, object[] Ideas, object[] Actions, int? WorldVisionId, object[] ThoughtsComfy, object[] ThoughtsSecret, int? FavoriteColorId, int? LeastLikedColorId, string Addresskeysstring)
        {
            if (ModelState.IsValid)
            {
                person.Address = Addresskeysstring != null ? _AddressService.FindByIdExcludes(Address.DisplayStringToKeys(Addresskeysstring)) : null;
                person.Ideas = Ideas != null ? _IdeaService.FindManyByIdExcludes(Ideas) : null;
                person.Actions = Actions != null ? _ActionService.FindManyByIdExcludes(Actions) : null;
                person.Vision = WorldVisionId != null ? _WorldVisionService.FindByIdExcludes(WorldVisionId) : null;
                person.Brain = TempData["Brain"] as Brain;
                person.Fingers = TempData["Fingers"] as List<Finger>;
                person.ComfortableThoughts = ThoughtsComfy != null ? _ThoughtService.FindManyByIdExcludes(ThoughtsComfy) : null;
                person.SecretThoughts = ThoughtsSecret != null ? _ThoughtService.FindManyByIdExcludes(ThoughtsSecret) : null;
                person.FavoriteColor = FavoriteColorId != null ? _ColorService.FindByIdExcludes(FavoriteColorId) : null;
                person.LeastLikedColor = LeastLikedColorId != null ? _ColorService.FindByIdExcludes(LeastLikedColorId) : null;
                _PersonService.Update(person);
                return RedirectToAction("Index");
            }
            ViewBag.Ideas = new MultiSelectList(_IdeaService.GetAllExcludes(), "Id", "Name", null);
            ViewBag.Actions = new MultiSelectList(_ActionService.GetAllExcludes(), "Id", "Name", null);
            ViewBag.WorldVisionId = new SelectList(_WorldVisionService.GetAllExcludes(), "Id", "Name", null);
            ViewBag.ThoughtsComfy = new MultiSelectList(_ThoughtService.GetAllExcludes(), "Id", "Name", null);
            ViewBag.ThoughtsSecret = ViewBag.ThoughtsComfy;
            ViewBag.FavoriteColorId = new SelectList(_ColorService.GetAllExcludes(), "Id", "Name", null);
            ViewBag.LeastLikedColorId = ViewBag.FavoriteColorId;
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
