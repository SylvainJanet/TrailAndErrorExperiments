﻿//using MiseEnSituation.Controllers;
//using MiseEnSituation.Repositories;
//using System.Collections.Generic;
//using System.Net;
//using System.Web.Mvc;

//namespace WebApp.Controllers
//{
//    [RoutePrefix("classes")]
//    [Route("{action=index}")]
//    public class ClassController : Controller
//    {
//        private MyDbContext db = new MyDbContext();
//        #region Services
//        //private IClassService _ClassService;
//        //private IClassManyToManyService _ClassClassManyToMany;
//        //private IClassOneRequiredToManyService _ClassOneRequiredToManyService;
//        //private IClassOneToOneRequiredService _ClassOneToOneRequiredService;
//        //private IClassManyToOneRequiredService _ClassManyToOneService;
//        //private IClassOneToManyService _ClassOneRequiredToManyService;
//        //private IClassManyNotPropToOneService _ClassManyNotPropToOneService;
//        //private IClassManyToManyMultipleService _ClassManyToManyMultipleService;
//        #endregion

//        public ClassController()
//        {
//            #region Services Init
//            //_ClassService = new ClassService(new ClassRepository(db));
//            //_ClassManyToManyService = new ClassManyToManyService(new ClassManyToManyRepository(db));
//            //_ClassOneRequiredToManyService = new ClassOneRequiredToManyService(new ClassOneRequiredToManyRepository(db));            
//            //_ClassOneToOneRequiredService = new ClassOneToOneRequiredService(new _ClassOneToOneRequiredRepository(db));
//            //_ClassManyToOneRequiredService = new ClassManyToOneRequiredService(new ClassManyToOneRepository(db));
//            //_ClassOneToManyService = new ClassOneToManyService(new ClassToManyRepository(db));
//            //_ClassManyNotPropToOneService = new ClassManyNotPropToOneService(new ClassManyNotPropToOneRepository(db));
//            //_ClassManyToManyMultipleService = new ClassManyToManyMultipleService(new ClassManyToManyMultipleRepository(db));
//            #endregion
//        }

//        [HttpGet]
//        [Route("{page?}/{maxByPage?}/{searchField?}")]
//        public ActionResult Index(int page = 1, int maxByPage = MyConstants.MAX_BY_PAGE, string SearchField = "")
//        {
//            var elements = 0;
//            //List<Class> elements = _ClassService.FindAllIncludes(page, maxByPage, SearchField);
//            //ViewBag.NextExist = _ClassService.NextExist(page, maxByPage, SearchField);
//            ViewBag.Page = page;
//            ViewBag.MaxByPage = maxByPage;
//            ViewBag.SearchField = SearchField;
//            return View("Index", elements);
//        }

//        [HttpGet]
//        [Route("Details/{id}")]
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            var element = 0;
//            //Class element = _ClassService.FindByIdIncludes(id);
//            if (element == null)
//            {
//                return HttpNotFound();
//            }
//            return View(element);
//        }

//        [HttpGet]
//        [Route("Create")]
//        public ActionResult Create()
//        {
//            //ViewBag.ClassManyToMany = new MultiSelectList(_ClassManyToManyService.GetAllExcludes(), "Id", "Name", null);
//            //ViewBag.ClassOneToOneRequiredId = new SelectList(_ClassOneToOneRequired.GetAllExcludes(1, int.MaxValue, null, c => c.Class == null), "Id", "Name");
//            //ViewBag.ManyToOneRequiredOwnerId = new SelectList(_ClassManyToOneRequired.GetAllExcludes(), "Id", "Name");
//            //ViewBag.OneNotRequiredToMany = new SelectList(_OneNotRequiredToManyService.GetAllExcludes(), "Id", "Name", null);
//            //ViewBag.OneToManyNotPropId = new SelectList(_OneToManyNotPropService.GetAllExcludes(), "Id", "Name", null);
//            //ViewBag.ManyNotPropToOneMultiKey = new SelectList(_ManyNotPropToOneMultiKeyService.GetAllExcludes().Select(o => ManyNotPropToOneMultiKey.KeysToDisplayString(o)));
//            #region ManyToManyMultiple 
//            //ViewBag.ManyToManyMultiple1 = new MultiSelectList(_ClassManyToManyMultipleService.GetAllExcludes(), "Id", "Name", null);
//            //viewBag.ManyToManyMultiple2 = ViewBag.ManyToManyMultiple1;
//            #endregion
//            //return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        [Route("Create")]
//        #region ClassManyToMany
//        //public ActionResult Create([Bind(Include = "Id,Name")] Class element, int?[] Owners)
//        #endregion
//        #region ClassManyToManyMultiple
//        //public ActionResult Create([Bind(Include = "Id,Name")] Class element, int?[] Owners1, int?[] Owners2)
//        #endregion
//        #region OneNotRequiredToMany
//        //public ActionResult Create([Bind(Include = "Id,Name")] Class element, int? objId)
//        #endregion
//        #region ManyToOneRequired
//        //public ActionResult Edit([Bind(Include = "Id,Name")] Class element, int? OwnerId)
//        #endregion
//        #region ClassOneToOneRequired
//        //public ActionResult Create([Bind(Include = "Id, Name")] Class element, int? Id)
//        #endregion
//        //#region OneToManyNotPropId
//        //public ActionResult Create([Bind(Include = "Id,Name")] class element, int? OneToManyNotPropId)
//        //#endregion
//        public ActionResult Create(object element)
//        {
//            #region ClassManyToMany
//            //if (ModelState.IsValid && RequiredOwners!=null)
//            //{
//            //    List<ClassManyToMany> owners =  Owners != null ? _ClassManyToManyService.FindManyByIdExcludes(Owners) : null;
//            //    _ClassService.Save(element, owners);
//            //    return RedirectToAction("Index");
//            //}
//            #endregion
//            #region ClassManyToManyMultiple
//            //if (ModelState.IsValid)
//            //{
//            //    object owners;
//            //    if (Owners1 == null)
//            //        owners = new PropToNull("propertyname");
//            //    else
//            //        owners = _ManyToManyMultipleService.FindManyByIdExcludes(Owners1);
//            //    object owners2;
//            //    if (Owners2 == null)
//            //        owners2 = new PropToNull("propertyname");
//            //    else
//            //        owners2 = _ManyToManyMultipleService.FindManyByIdExcludes(Owners2s);
//            //    _ThoughtService.Save(element, owners, owners2);
//            //    return RedirectToAction("Index");
//            //}
//            #endregion
//            #region ClassManyToOneRequired
//            //ModelState.Remove("Owner");
//            //if (ModelState.IsValid && OwnerId.HasValue)
//            //{
//            //    element.Owner = _ClassManyToOneRequiredService.FindByIdExcludes(OwnerId);
//            //    _ClassService.Save(element, element.Owner);
//            //    return RedirectToAction("Index");
//            //}
//            #endregion
//            #region ClassOneToOneRequired
//            //ModelState.Remove("Owner");
//            //if (ModelState.IsValid && Id.HasValue)
//            //{
//            //    element.Owner = _ClassOneToOneRequiredService.FindByIdExcludesTracked(Id);
//            //    _classService.Save(element, element.Owner);
//            //    return RedirectToAction("Index");
//            //}
//            #endregion
//            #region ClassOneToMany
//            //if (ModelState.IsValid)
//            //{
//            //    _ClassService.Save(element);
//            //    return RedirectToAction("Index");
//            //}
//            #endregion
//            #region ClassOneNotRequiredToMany
//            //if (ModelState.IsValid)
//            //{
//            //    ClassOneNotRequiredToMany obj = objId != null ? _OneNotRequiredToManyService.FindByIdExcludesTracked(objId) : null;
//            //    _ClassService.Save(element,obj);
//            //    return RedirectToAction("Index");
//            //}
//            #endregion
//            #region OneToManyNotProp
//            //if (ModelState.IsValid)
//            //{
//            //    OneToManyNotProp obj = OneToManyNotPropId != null ? _OneToManyNotPropService.FindByIdExcludes(OneToManyNotPropId) : null;
//            //    _ClassService.Save(element, obj);
//            //    return RedirectToAction("Index");
//            //}
//            #endregion
//            #region
//            //if (ModelState.IsValid)
//            //{
//            //    ManyNotPropToOneMultiKey obj = ManyNotPropToOneMultiKey != null ? _ManyNotPropToOneMultiKeyService.FindByIdExcludes(ManyNotPropToOneMultiKey.DisplayStringToKeys(ManyNotPropToOneMultiKey)) : null;
//            //    _ClassService.Save(element, obj);
//            //}
//            #endregion

//            //ViewBag.ClassManyToMany = new MultiSelectList(_ClassManyToManyService.GetAllExcludes(), "Id", "Name", null);
//            //ViewBag.ClassOneToOneRequiredId = new SelectList(_ClassOneToOneRequired.GetAllExcludes(1, int.MaxValue, null, c => c.Class == null), "Id", "Name");
//            //ViewBag.ManyToOneRequiredOwnerId = new SelectList(_ClassManyToOneRequired.GetAllExcludes(), "Id", "Name");
//            //ViewBag.OneNotRequiredToMany = new SelectList(_OneNotRequiredToManyService.GetAllExcludes(), "Id", "Name", null);
//            //ViewBag.OneToManyNotPropId = new SelectList(_OneToManyNotPropService.GetAllExcludes(), "Id", "Name", null);
//            //ViewBag.ManyNotPropToOneMultiKey = new SelectList(_ManyNotPropToOneMultiKeyService.GetAllExcludes().Select(o => ManyNotPropToOneMultiKey.KeysToDisplayString(o)));
//            #region ManyToManyMultiple 
//            //ViewBag.ManyToManyMultiple1 = new MultiSelectList(_ClassManyToManyMultipleService.GetAllExcludes(), "Id", "Name", null);
//            //viewBag.ManyToManyMultiple2 = ViewBag.ManyToManyMultiple1;
//            #endregion
//            //return View(element);
//        }

//        [HttpGet]
//        [Route("Edit/{id?}")]
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            var element = 0;
//            //Class element = _ClassService.FindByIdIncludes(id);
//            if (element == null)
//            {
//                return HttpNotFound();
//            }
//            //ViewBag.ClassManyToMany = new MultiSelectList(_ClassManyToMany.GetAllExcludes(), "Id", "Name", null);
//            //ViewBag.ManyToOneRequiredOwnerId = new SelectList(_ClassManyToOneRequired.GetAllExcludes(), "Id", "Name");
//            //ViewBag.OneNotRequiredToMany = new SelectList(_OneNotRequiredToManyService.GetAllExcludes(), "Id", "Name", null);
//            //ViewBag.OneToManyNotPropId = new SelectList(_OneToManyNotPropService.GetAllExcludes(), "Id", "Name", null);
//            //ViewBag.ManyNotPropToOneMultiKey = new SelectList(_ManyNotPropToOneMultiKeyService.GetAllExcludes().Select(o => ManyNotPropToOneMultiKey.KeysToDisplayString(o)));
//            //TempData["OneRequiredToOne"] = element.OneRequireToOne;
//            //TempData["OneRequiredToMany"] = element.OneRequiredToMany;
//            //TempData.Keep();
//            #region ManyToManyMultiple 
//            //ViewBag.ManyToManyMultiple1 = new MultiSelectList(_ClassManyToManyMultipleService.GetAllExcludes(), "Id", "Name", null);
//            //viewBag.ManyToManyMultiple2 = ViewBag.ManyToManyMultiple1;
//            #endregion
//            return View(element);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        [Route("Edit")]
//        #region ClassManyToMany
//        //public ActionResult Edit([Bind(Include = "Id,Name")] Class element, int?[] Owners)
//        #endregion
//        #region ClassManyToManyMultiple
//        //public ActionResult Edit([Bind(Include = "Id,Name")] Class element, int?[] Owners1, int?[] Owners2)
//        #endregion
//        #region ClassOneNotRequiredToMany
//        //public ActionResult Edit([Bind(Include = "Id,Name")] Class element int? objId)
//        #endregion
//        #region ClassManyToOneRequired
//        //public ActionResult Edit([Bind(Include = "Id,Name")] Class element, int? OwnerId)
//        #endregion
//        #region ClassOneToOneRequired
//        //public ActionResult Edit([Bind(Include = "Id,Name")] Class element)
//        #endregion
//        //#region OneToManyNotPropId
//        //public ActionResult Create([Bind(Include = "Id,Name")] class element, int? OneToManyNotPropId)
//        //#endregion
//        public ActionResult Edit()
//        {
//            #region ClassManyToMany
//            //if (ModelState.IsValid && RequiredOwners!=null)
//            //{
//            //    foreach (ClassManyRequiredToMany el in _ClassManyRequiredToManyService.GetAllExcludes(1, int.MaxValue, null, t => !Owners.Contains(t.Id) && t.Owners.Count() == 1 && t.Owners.Where(s => s.Id == element.Id).Count() == 1))
//            //    {
//            //        _ClassManyRequiredToManyService.Delete(el);
//            //    }
//            //    List<ClassManyToMany> owners =  Owners != null ? _ClassManyToManyService.FindManyByIdExcludes(Owners) : null;
//            //    _ClassService.Update(element, owners);
//            //    return RedirectToAction("Index");
//            //}
//            #endregion
//            #region ClassManyToManyMultiple
//            //if (ModelState.IsValid)
//            //{
//            //    object owners;
//            //    if (Owners1 == null)
//            //        owners = new PropToNull("propertyname");
//            //    else
//            //        owners = _ManyToManyMultipleService.FindManyByIdExcludes(Owners1);
//            //    object owners2;
//            //    if (Owners2 == null)
//            //        owners2 = new PropToNull("propertyname");
//            //    else
//            //        owners2 = _ManyToManyMultipleService.FindManyByIdExcludes(Owners2);
//            //    _ThoughtService.Update(element, owners, owners2);
//            //    return RedirectToAction("Index");
//            //}
//            #endregion
//            #region ClassOneRequiredToMany
//            //if (ModelState.IsValid)
//            //{
//            //    List<ClassOneRequiredToMany> objs = TempData["ClassOneRequiredToMany"] as List<ClassOneRequiredToMany>
//            //    foreach (ClassOneRequiredToMany el in _OneRequiredToManyService.GetAllExcludes(1, int.MaxValue, null, o => o.OwnerId == element.Id && objs.Where(oo => oo.Id == o.Id).Count() == 0))
//            //    {
//            //        _OneRequiredToManyService.Delete(el);
//            //    }   
//            //    _ClassService.Update(element, objs);
//            //    return RedirectToAction("Index");
//            //}
//            #endregion

//            #region ClassManyToOneRequired
//            //ModelState.Remove("Owner");
//            //if (ModelState.IsValid && OwnerId.HasValue)
//            //{
//            //    element.Owner = _ClassManyToOneRequiredService.FindByIdExcludes(OwnerId);
//            //    _ClassService.Update(element, element.Owner);
//            //    return RedirectToAction("Index");
//            //}
//            #endregion

//            #region ClassOneToOneRequired
//            //ModelState.Remove("Owner");
//            //if (ModelState.IsValid)
//            //{
//            //    element.Owner = _ClassOneToOneRequiredService.FindByIdExcludes(element.Id);
//            //    _ClassService.Update(element, element.Owner);
//            //    return RedirectToAction("Index");
//            //}
//            #endregion

//            #region ClassOneRequiredToOne
//            //if (ModelState.IsValid)
//            //{
//            //    ClassOneRequiredToOne obj = TempData["ClassOneRequiredToOne"] as ClassOneRequiredToOne;
//            //    _PersonService.Update(element, obj);
//            //    return RedirectToAction("Index");
//            //}
//            #endregion

//            #region OneNotRequiredToMany 
//            //if (ModelState.IsValid)
//            //{
//            //    ClassOneNotRequiredToMany obj = _OneNotRequiredToManyService.FindByIdExcludesTracked(objId);
//            //    _ClassService.Update(element,obj);
//            //    return RedirectToAction("Index");
//            //}
//            #endregion

//            #region OneToManyNotProp
//            //if (ModelState.IsValid)
//            //{
//            //    OneToManyNotProp obj = OneToManyNotPropId != null ? _OneToManyNotPropService.FindByIdExcludes(OneToManyNotPropId) : null;
//            //    _PersonService.Update(person, obj);
//            //    return RedirectToAction("Index");
//            //}
//            #endregion

//            #region
//            //if (ModelState.IsValid)
//            //{
//            //    ManyNotPropToOneMultiKey obj = ManyNotPropToOneMultiKey != null ? _ManyNotPropToOneMultiKeyService.FindByIdExcludes(ManyNotPropToOneMultiKey.DisplayStringToKeys(ManyNotPropToOneMultiKey)) : null;
//            //    _ClassService.Save(element, obj);
//            //}
//            #endregion

//            //ViewBag.ClassManyToMany = new MultiSelectList(_ClassManyToManyService.GetAllExcludes(), "Id", "Name", null);
//            //ViewBag.ManyToOneRequiredOwnerId = new SelectList(_ClassManyToOneRequired.GetAllExcludes(), "Id", "Name");
//            //ViewBag.OneNotRequiredToMany = new SelectList(_OneNotRequiredToManyService.GetAllExcludes(), "Id", "Name", null);
//            //ViewBag.OneToManyNotPropId = new SelectList(_OneToManyNotPropService.GetAllExcludes(), "Id", "Name", null);
//            //ViewBag.ManyNotPropToOneMultiKey = new SelectList(_ManyNotPropToOneMultiKeyService.GetAllExcludes().Select(o => ManyNotPropToOneMultiKey.KeysToDisplayString(o)));
//            #region ClassOneRequiredToMany, ClassOneRequiredToOne
//            //TempDate.Keep()
//            #endregion
//            #region ManyToManyMultiple 
//            //ViewBag.ManyToManyMultiple1 = new MultiSelectList(_ClassManyToManyMultipleService.GetAllExcludes(), "Id", "Name", null);
//            //viewBag.ManyToManyMultiple2 = ViewBag.ManyToManyMultiple1;
//            #endregion
//            return View();
//        }

//        [HttpGet]
//        [Route("Delete/{id}")]
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            var element = 0;
//            //Class element = _ClassService.FindByIdIncludes(id);
//            if (element == null)
//            {
//                return HttpNotFound();
//            }
//            return View(element);
//        }

//        // POST: People/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        [Route("Delete/{id}")]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            //foreach (ClassOneRequiredToMany elementOneRequiredToMany in _ClassOneRequiredToManyService.GetAllExcludes(1, int.MaxValue, null, c => c.OwnerId == id))
//            //{
//            //    _ClassOneRequiredToManyService.Delete(elementOneRequiredToMany);
//            //}
//            //foreach (ClassManyRequiredToMany el in _ManyRequiredToManyService.GetAllExcludes(1, int.MaxValue, null, t => t.Owners.Count() == 1 && t.Owners.Where(s => s.Id == id).Count() == 1))
//            //{
//            //    _ManyRequiredToManyService.Delete(el);
//            //}
//            //foreach (ManyNotPropToOne el in _ManyNotPropToOneService.GetAllExcludes(1, int.MaxValue, null, p => p.propname.Id == id))
//            //{
//            //    _ManyNotPropToOneService.UpdateOne(el, "propame", null);
//            //}
//            //foreach (ManyToManyMultiple obj in _ManyToManyMultipleService.GetAllExcludes(1, int.MaxValue, null, t => t.Prop1.Where(p => p.Id == id).Count() >= 1))
//            //{
//            //    _ManyToManyMultipleService.UpdateOne(person, "Prop1", obj.Prop1.Where(p => p.Id != id).ToList());
//            //}
//            //foreach (ManyToManyMultiple obj in _ManyToManyMultipleService.GetAllExcludes(1, int.MaxValue, null, t => t.Prop2.Where(p => p.Id == id).Count() >= 1))
//            //{
//            //    _ManyToManyMultipleService.UpdateOne(person, "Prop2", obj.Prop2.Where(p => p.Id != id).ToList());
//            //}
//            //foreach (ManyNotPropToOneMultiple obj in _ManyNotPropToOneMultipleService.GetAllExcludes(1, int.MaxValue, null, p => p.Prop1.Id == id))
//            //{
//            //    _ManyNotPropToOneMultipleService.UpdateOne(obj, "Prop1", null);
//            //}
//            //foreach (ManyNotPropToOneMultiple obj in _ManyNotPropToOneMultipleService.GetAllExcludes(1, int.MaxValue, null, p => p.Prop2.Id == id))
//            //{
//            //    _ManyNotPropToOneMultipleService.UpdateOne(obj, "Prop2", null);
//            //}
//            //_ClassService.Delete(id);
//            return RedirectToAction("Index");
//        }

//        [HttpGet]
//        [Route("Search")]
//        public ActionResult Search([Bind(Include = ("page, maxByPage, SearchField"))] int page = 1, int maxByPage = MyConstants.MAX_BY_PAGE, string searchField = "")
//        {
//            if (searchField.Trim().Equals(""))
//                return RedirectToAction("Index");
//            return Index(page, maxByPage, searchField);
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}
