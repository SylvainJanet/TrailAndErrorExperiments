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
    [RoutePrefix("addresses")]
    [Route("{action=index}")]
    public class AddressesController : Controller
    {
        private readonly MyDbContext db = new MyDbContext();
        private readonly IAddressService _AddressService;
        private readonly IPersonService _PersonService;

        public AddressesController()
        {
            _AddressService = new AddressService(new AddressRepository(db));
            _PersonService = new PersonService(new PersonRepository(db));
        }

        [HttpGet]
        [Route("{page?}/{maxByPage?}/{searchField?}")]
        public ActionResult Index(int page = 1, int maxByPage = MyConstants.MAX_BY_PAGE, string SearchField = "")
        {
            List<Address> elements = _AddressService.FindAllIncludes(page, maxByPage, SearchField);
            ViewBag.NextExist = _AddressService.NextExist(page, maxByPage, SearchField);
            ViewBag.Page = page;
            ViewBag.MaxByPage = maxByPage;
            ViewBag.SearchField = SearchField;
            return View("Index", elements);
        }

        [HttpGet]
        [Route("Details/{number?}/{street?}")]
        public ActionResult Details(int? number, string street)
        {
            if (number == null || street == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address element = _AddressService.FindByIdIncludes(number, street);
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
        public ActionResult Create([Bind(Include = "Number,Street")] Address address)
        {
            if (ModelState.IsValid)
            {
                _AddressService.Save(address);
                return RedirectToAction("Index");
            }

            return View(address);
        }

        [HttpGet]
        [Route("Edit/{number?}/{street?}")]
        public ActionResult Edit(int? number, string street)
        {
            if (number == null || street == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address element = _AddressService.FindByIdIncludes(number, street);
            if (element == null)
            {
                return HttpNotFound();
            }
            return View(element);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit")]
        public ActionResult Edit([Bind(Include = "Number,Street")] Address address)
        {
            if (ModelState.IsValid)
            {
                _AddressService.Update(address);
                return RedirectToAction("Index");
            }
            return View(address);
        }

        [HttpGet]
        [Route("Delete/{number?}/{street?}")]
        public ActionResult Delete(int? number, string street)
        {
            if (number == null || street == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address element = _AddressService.FindByIdIncludes(number, street);
            if (element == null)
            {
                return HttpNotFound();
            }
            return View(element);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Delete/{number}/{street}")]
        public ActionResult DeleteConfirmed(int number, string street)
        {
            _AddressService.Delete(number,street);
            return RedirectToAction("Index");
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
