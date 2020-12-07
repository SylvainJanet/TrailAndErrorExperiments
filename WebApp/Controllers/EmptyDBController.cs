using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Controllers
{
    public class EmptyDBController : Controller
    {
        private readonly MyDbContext db = new MyDbContext();

        public ActionResult Index()
        {
            foreach (Finger finger in db.Fingers)
            {
                db.Fingers.Remove(finger);
            }
            foreach (Models.Action el in db.Actions)
            {
                db.Actions.Remove(el);
            }
            foreach (Thought thought in db.Thoughts)
            {
                thought.PeopleAtEase = new List<Person>();
                thought.PeopleTimid = new List<Person>();
                db.Entry(thought).State = System.Data.Entity.EntityState.Modified;
            }
            foreach (Person person in db.People.Include("Brain").Include("Fingers").Include("Ideas").Include("ComfortableThoughts").Include("SecretThoughts").Include("Vision"))
            {
                db.People.Remove(person);
            }
            db.SaveChanges();

            foreach (Brain brain in db.Brains.Include("Owner"))
            {
                db.Brains.Remove(brain);
            }
            db.SaveChanges();

            foreach (Finger finger in db.Fingers.Include("Owner"))
            {
                db.Fingers.Remove(finger);
            }
            db.SaveChanges();

            foreach (Idea idea in db.Ideas.Include("Owners"))
            {
                db.Ideas.Remove(idea);
            }
            db.SaveChanges();

            foreach (Models.Action action in db.Actions)
            {
                db.Actions.Remove(action);
            }
            db.SaveChanges();

            foreach (WorldVision worldVision in db.WorldVisions)
            {
                foreach (Person person in db.People.Where(p => p.Vision.Id == worldVision.Id))
                {
                    person.Vision = null;
                    db.Entry(person).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                db.WorldVisions.Remove(worldVision);
            }
            db.SaveChanges();

            foreach (Thought thought in db.Thoughts)
            {
                db.Thoughts.Remove(thought);
            }
            db.SaveChanges();

            foreach (Color color in db.Colors)
            {
                foreach (Person person in db.People.Where(p => p.FavoriteColor.Id == color.Id || p.LeastLikedColor.Id == color.Id))
                {
                    person.FavoriteColor = null;
                    person.LeastLikedColor = null;
                    db.Entry(person).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                db.Colors.Remove(color);
            }
            db.SaveChanges();

            foreach (Address address in db.Addresses)
            {
                Person p = db.People.Where(pp => pp.Address.Number == address.Number && pp.Address.Street == address.Street).SingleOrDefault();
                if (p != null)
                {
                    p.Address = null;
                    db.Entry(p).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                db.Addresses.Remove(address);
            }
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
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