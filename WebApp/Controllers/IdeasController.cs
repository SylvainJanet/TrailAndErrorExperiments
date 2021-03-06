﻿using System;
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
    [RoutePrefix("ideas")]
    [Route("{action=index}")]
    public class IdeasController : Controller
    {
        private readonly MyDbContext db = new MyDbContext();
        private readonly IIdeaService _IdeasService;
        private readonly IPersonService _PersonService;

        public IdeasController()
        {
            _IdeasService = new IdeaService(new IdeaRepository(db));
            _PersonService = new PersonService(new PersonRepository(db));
        }

        [HttpGet] //localhost:xxx/users/1/15
        [Route("{page?}/{maxByPage?}/{searchField?}")]
        public ActionResult Index(int page = 1, int maxByPage = MyConstants.MAX_BY_PAGE, string SearchField = "")
        {
            List<Idea> ideas = _IdeasService.FindAllIncludes(page, maxByPage, SearchField);
            ViewBag.NextExist = _IdeasService.NextExist(page, maxByPage, SearchField);
            ViewBag.Page = page;
            ViewBag.MaxByPage = maxByPage;
            ViewBag.SearchField = SearchField;
            return View("Index", ideas);
        }

        [HttpGet]
        [Route("Details/{id}")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Idea idea = _IdeasService.FindByIdIncludes(id);
            if (idea == null)
            {
                return HttpNotFound();
            }
            return View(idea);
        }

        [HttpGet]
        [Route("Create")]
        public ActionResult Create()
        {
            ViewBag.Owners = new MultiSelectList(_PersonService.GetAllExcludes(), "Id", "Name", null);
            return View();
        }

        // POST: People/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public ActionResult Create([Bind(Include = "Id,Name")] Idea idea, object[] Owners)
        {
            if (ModelState.IsValid)
            {
                idea.Owners = Owners != null ? _PersonService.FindManyByIdExcludes(Owners) : null;
                _IdeasService.Save(idea);
                return RedirectToAction("Index");
            }
            ViewBag.Owners = new MultiSelectList(_PersonService.GetAllExcludes(), "Id", "Name", null);
            return View(idea);
        }

        [HttpGet]
        [Route("Edit/{id?}")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Idea idea = _IdeasService.FindByIdIncludes(id);
            if (idea == null)
            {
                return HttpNotFound();
            }
            ViewBag.Owners = new MultiSelectList(_PersonService.GetAllExcludes(), "Id", "Name", null);
            return View(idea);
        }

        // POST: People/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit")]
        public ActionResult Edit([Bind(Include = "Id,Name")] Idea idea, object[] Owners)
        {
            if (ModelState.IsValid)
            {
                idea.Owners = Owners.Length != 0 ? _PersonService.FindManyByIdExcludes(Owners) : null;
                _IdeasService.Update(idea);
                return RedirectToAction("Index");
            }
            ViewBag.Ownerss = new MultiSelectList(_PersonService.GetAllExcludes(), "Id", "Name", "Owners");
            return View(idea);
        }

        [HttpGet]
        [Route("Delete/{id}")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Idea idea = _IdeasService.FindByIdIncludes(id);
            if (idea == null)
            {
                return HttpNotFound();
            }
            return View(idea);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Delete/{id}")]
        public ActionResult DeleteConfirmed(int id)
        {
            _IdeasService.Delete(id);
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
