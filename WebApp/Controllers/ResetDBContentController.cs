using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Controllers
{
    public class ResetDBContentController : Controller
    {
        private MyDbContext db = new MyDbContext();

        public ActionResult Index()
        {
            Person p1 = new Person("Person1");
            Person p2 = new Person("Person2");
            Person p3 = new Person("Person3");
            Person p4 = new Person("Person4");
            Person p5 = new Person("Person5");
            Person p6 = new Person("Person6");
            Person p7 = new Person("Person7");
            Person p8 = new Person("Person8");
            Person p9 = new Person("Person9");

            foreach (Person person in db.People.Include("Brain").Include("Fingers").Include("Ideas"))
            {
                db.People.Remove(person);
            }

            db.People.Add(p1);
            db.People.Add(p2);
            db.People.Add(p3);
            db.People.Add(p4);
            db.People.Add(p5);
            db.People.Add(p6);
            db.People.Add(p7);
            db.People.Add(p8);
            db.People.Add(p9);

            Brain b1 = new Brain("Brain 1 - 1", p1);
            Brain b2 = new Brain("Brain 2 - 2", p2);
            Brain b3 = new Brain("Brain 3 - 3", p3);
            Brain b4 = new Brain("Brain 4 - 4", p4);
            Brain b5 = new Brain("Brain 5 - 5", p5);
            Brain b6 = new Brain("Brain 6 - 6", p6);
            Brain b7 = new Brain("Brain 7 - 7", p7);
            Brain b8 = new Brain("Brain 8 - 8", p8);
            Brain b9 = new Brain("Brain 9 - 9", p9);

            foreach (Brain brain in db.Brains.Include("Owner"))
            {
                db.Brains.Remove(brain);
            }

            db.Brains.Add(b1);
            db.Brains.Add(b2);
            db.Brains.Add(b3);
            db.Brains.Add(b4);
            db.Brains.Add(b5);
            db.Brains.Add(b6);
            db.Brains.Add(b7);
            db.Brains.Add(b8);
            db.Brains.Add(b9);

            Finger f11 = new Finger("Finger 1 - 1", p1);
            Finger f12 = new Finger("Finger 2 - 1", p1);
            Finger f13 = new Finger("Finger 3 - 1", p1);
            Finger f14 = new Finger("Finger 4 - 1", p1);
            Finger f15 = new Finger("Finger 5 - 1", p1);
            Finger f16 = new Finger("Finger 6 - 1", p1);
            Finger f17 = new Finger("Finger 7 - 1", p1);
            Finger f18 = new Finger("Finger 8 - 1", p1);
            Finger f19 = new Finger("Finger 9 - 1", p1);

            Finger f21 = new Finger("Finger 1 - 2", p2);
            Finger f22 = new Finger("Finger 2 - 2", p2);
            Finger f23 = new Finger("Finger 3 - 2", p2);
            Finger f24 = new Finger("Finger 4 - 2", p2);
            Finger f25 = new Finger("Finger 5 - 2", p2);
            Finger f26 = new Finger("Finger 6 - 2", p2);
            Finger f27 = new Finger("Finger 7 - 2", p2);
            Finger f28 = new Finger("Finger 8 - 2", p2);
            Finger f29 = new Finger("Finger 9 - 2", p2);

            Finger f31 = new Finger("Finger 1 - 3", p3);
            Finger f32 = new Finger("Finger 2 - 3", p3);
            Finger f33 = new Finger("Finger 3 - 3", p3);
            Finger f34 = new Finger("Finger 4 - 3", p3);
            Finger f35 = new Finger("Finger 5 - 3", p3);
            Finger f36 = new Finger("Finger 6 - 3", p3);
            Finger f37 = new Finger("Finger 7 - 3", p3);
            Finger f38 = new Finger("Finger 8 - 3", p3);
            Finger f39 = new Finger("Finger 9 - 3", p3);

            Finger f41 = new Finger("Finger 1 - 4", p4);
            Finger f42 = new Finger("Finger 2 - 4", p4);
            Finger f43 = new Finger("Finger 3 - 4", p4);
            Finger f44 = new Finger("Finger 4 - 4", p4);
            Finger f45 = new Finger("Finger 5 - 4", p4);
            Finger f46 = new Finger("Finger 6 - 4", p4);
            Finger f47 = new Finger("Finger 7 - 4", p4);
            Finger f48 = new Finger("Finger 8 - 4", p4);
            Finger f49 = new Finger("Finger 9 - 4", p4);

            Finger f51 = new Finger("Finger 1 - 5", p5);
            Finger f52 = new Finger("Finger 2 - 5", p5);
            Finger f53 = new Finger("Finger 3 - 5", p5);
            Finger f54 = new Finger("Finger 4 - 5", p5);
            Finger f55 = new Finger("Finger 5 - 5", p5);
            Finger f56 = new Finger("Finger 6 - 5", p5);
            Finger f57 = new Finger("Finger 7 - 5", p5);
            Finger f58 = new Finger("Finger 8 - 5", p5);
            Finger f59 = new Finger("Finger 9 - 5", p5);

            Finger f61 = new Finger("Finger 1 - 6", p6);
            Finger f62 = new Finger("Finger 2 - 6", p6);
            Finger f63 = new Finger("Finger 3 - 6", p6);
            Finger f64 = new Finger("Finger 4 - 6", p6);
            Finger f65 = new Finger("Finger 5 - 6", p6);
            Finger f66 = new Finger("Finger 6 - 6", p6);
            Finger f67 = new Finger("Finger 7 - 6", p6);
            Finger f68 = new Finger("Finger 8 - 6", p6);
            Finger f69 = new Finger("Finger 9 - 6", p6);

            Finger f71 = new Finger("Finger 1 - 7", p7);
            Finger f72 = new Finger("Finger 2 - 7", p7);
            Finger f73 = new Finger("Finger 3 - 7", p7);
            Finger f74 = new Finger("Finger 4 - 7", p7);
            Finger f75 = new Finger("Finger 5 - 7", p7);
            Finger f76 = new Finger("Finger 6 - 7", p7);
            Finger f77 = new Finger("Finger 7 - 7", p7);
            Finger f78 = new Finger("Finger 8 - 7", p7);
            Finger f79 = new Finger("Finger 9 - 7", p7);

            Finger f81 = new Finger("Finger 1 - 8", p8);
            Finger f82 = new Finger("Finger 2 - 8", p8);
            Finger f83 = new Finger("Finger 3 - 8", p8);
            Finger f84 = new Finger("Finger 4 - 8", p8);
            Finger f85 = new Finger("Finger 5 - 8", p8);
            Finger f86 = new Finger("Finger 6 - 8", p8);
            Finger f87 = new Finger("Finger 7 - 8", p8);
            Finger f88 = new Finger("Finger 8 - 8", p8);
            Finger f89 = new Finger("Finger 9 - 8", p8);

            Finger f91 = new Finger("Finger 1 - 9", p9);
            Finger f92 = new Finger("Finger 2 - 9", p9);
            Finger f93 = new Finger("Finger 3 - 9", p9);
            Finger f94 = new Finger("Finger 4 - 9", p9);
            Finger f95 = new Finger("Finger 5 - 9", p9);
            Finger f96 = new Finger("Finger 6 - 9", p9);
            Finger f97 = new Finger("Finger 7 - 9", p9);
            Finger f98 = new Finger("Finger 8 - 9", p9);
            Finger f99 = new Finger("Finger 9 - 9", p9);

            foreach (Finger finger in db.Fingers.Include("Owner"))
            {
                db.Fingers.Remove(finger);
            }

            db.Fingers.Add(f11);
            db.Fingers.Add(f12);
            db.Fingers.Add(f13);
            db.Fingers.Add(f14);
            db.Fingers.Add(f15);
            db.Fingers.Add(f16);
            db.Fingers.Add(f17);
            db.Fingers.Add(f18);
            db.Fingers.Add(f19);

            db.Fingers.Add(f21);
            db.Fingers.Add(f22);
            db.Fingers.Add(f23);
            db.Fingers.Add(f24);
            db.Fingers.Add(f25);
            db.Fingers.Add(f26);
            db.Fingers.Add(f27);
            db.Fingers.Add(f28);
            db.Fingers.Add(f29);

            db.Fingers.Add(f31);
            db.Fingers.Add(f32);
            db.Fingers.Add(f33);
            db.Fingers.Add(f34);
            db.Fingers.Add(f35);
            db.Fingers.Add(f36);
            db.Fingers.Add(f37);
            db.Fingers.Add(f38);
            db.Fingers.Add(f39);

            db.Fingers.Add(f41);
            db.Fingers.Add(f42);
            db.Fingers.Add(f43);
            db.Fingers.Add(f44);
            db.Fingers.Add(f45);
            db.Fingers.Add(f46);
            db.Fingers.Add(f47);
            db.Fingers.Add(f48);
            db.Fingers.Add(f49);

            db.Fingers.Add(f51);
            db.Fingers.Add(f52);
            db.Fingers.Add(f53);
            db.Fingers.Add(f54);
            db.Fingers.Add(f55);
            db.Fingers.Add(f56);
            db.Fingers.Add(f57);
            db.Fingers.Add(f58);
            db.Fingers.Add(f59);

            db.Fingers.Add(f61);
            db.Fingers.Add(f62);
            db.Fingers.Add(f63);
            db.Fingers.Add(f64);
            db.Fingers.Add(f65);
            db.Fingers.Add(f66);
            db.Fingers.Add(f67);
            db.Fingers.Add(f68);
            db.Fingers.Add(f69);

            db.Fingers.Add(f71);
            db.Fingers.Add(f72);
            db.Fingers.Add(f73);
            db.Fingers.Add(f74);
            db.Fingers.Add(f75);
            db.Fingers.Add(f76);
            db.Fingers.Add(f77);
            db.Fingers.Add(f78);
            db.Fingers.Add(f79);

            db.Fingers.Add(f81);
            db.Fingers.Add(f82);
            db.Fingers.Add(f83);
            db.Fingers.Add(f84);
            db.Fingers.Add(f85);
            db.Fingers.Add(f86);
            db.Fingers.Add(f87);
            db.Fingers.Add(f88);
            db.Fingers.Add(f89);

            db.Fingers.Add(f91);
            db.Fingers.Add(f92);
            db.Fingers.Add(f93);
            db.Fingers.Add(f94);
            db.Fingers.Add(f95);
            db.Fingers.Add(f96);
            db.Fingers.Add(f97);
            db.Fingers.Add(f98);
            db.Fingers.Add(f99);

            Idea idea1_123 = new Idea("Idea 1 - 123", new List<Person> { p1, p2, p3 });
            Idea idea2_234 = new Idea("Idea 2 - 234", new List<Person> { p2, p3, p4 });
            Idea idea3_345 = new Idea("Idea 3 - 345", new List<Person> { p3, p4, p5 });
            Idea idea4_456 = new Idea("Idea 4 - 456", new List<Person> { p4, p5, p6 });
            Idea idea5_567 = new Idea("Idea 5 - 567", new List<Person> { p5, p6, p7 });
            Idea idea6_678 = new Idea("Idea 6 - 678", new List<Person> { p6, p7, p8 });
            Idea idea7_789 = new Idea("Idea 7 - 789", new List<Person> { p7, p8, p9 });
            Idea idea8_891 = new Idea("Idea 8 - 891", new List<Person> { p8, p9, p1 });
            Idea idea9_912 = new Idea("Idea 9 - 912", new List<Person> { p9, p1, p2 });

            foreach (Idea idea in db.Ideas.Include("Owners"))
            {
                db.Ideas.Remove(idea);
            }

            db.Ideas.Add(idea1_123);
            db.Ideas.Add(idea2_234);
            db.Ideas.Add(idea3_345);
            db.Ideas.Add(idea4_456);
            db.Ideas.Add(idea5_567);
            db.Ideas.Add(idea6_678);
            db.Ideas.Add(idea7_789);
            db.Ideas.Add(idea8_891);
            db.Ideas.Add(idea9_912);

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