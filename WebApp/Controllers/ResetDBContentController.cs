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
        private readonly MyDbContext db = new MyDbContext();

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

            db.People.Add(p1);
            db.People.Add(p2);
            db.People.Add(p3);
            db.People.Add(p4);
            db.People.Add(p5);
            db.People.Add(p6);
            db.People.Add(p7);
            db.People.Add(p8);
            db.People.Add(p9);

            db.SaveChanges();

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

            db.SaveChanges();

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

            db.SaveChanges();

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

            Models.Action a1 = new Models.Action { Name = "Action 1 - 123", People = { p1, p2, p3 } };
            Models.Action a2 = new Models.Action { Name = "Action 2 - 234", People = { p2, p3, p4 } };
            Models.Action a3 = new Models.Action { Name = "Action 3 - 345", People = { p3, p4, p5 } };
            Models.Action a4 = new Models.Action { Name = "Action 4 - 456", People = { p4, p5, p6 } };
            Models.Action a5 = new Models.Action { Name = "Action 5 - 567", People = { p5, p6, p7 } };
            Models.Action a6 = new Models.Action { Name = "Action 6 - 678", People = { p6, p7, p8 } };
            Models.Action a7 = new Models.Action { Name = "Action 7 - 789", People = { p7, p8, p9 } };
            Models.Action a8 = new Models.Action { Name = "Action 8 - 891", People = { p8, p9, p1 } };
            Models.Action a9 = new Models.Action { Name = "Action 9 - 912", People = { p9, p1, p2 } };
            foreach (Models.Action action in db.Actions)
            {
                db.Actions.Remove(action);
            }
            db.Actions.Add(a1);
            db.Actions.Add(a2);
            db.Actions.Add(a3);
            db.Actions.Add(a4);
            db.Actions.Add(a5);
            db.Actions.Add(a6);
            db.Actions.Add(a7);
            db.Actions.Add(a8);
            db.Actions.Add(a9);

            db.SaveChanges();

            WorldVision wv1 = new WorldVision { Name = "WorldVision 1 - 1" };
            p1.Vision = wv1;
            WorldVision wv2 = new WorldVision { Name = "WorldVision 2 - 2" };
            p2.Vision = wv2;
            WorldVision wv3 = new WorldVision { Name = "WorldVision 3 - 3" };
            p3.Vision = wv3;
            WorldVision wv4 = new WorldVision { Name = "WorldVision 4 - 4" };
            p4.Vision = wv4;
            WorldVision wv5 = new WorldVision { Name = "WorldVision 5 - 5" };
            p5.Vision = wv5;
            WorldVision wv6 = new WorldVision { Name = "WorldVision 6 - 6" };
            p6.Vision = wv6;
            WorldVision wv7 = new WorldVision { Name = "WorldVision 7 - 7" };
            p7.Vision = wv7;
            WorldVision wv8 = new WorldVision { Name = "WorldVision 8 - 8" };
            p8.Vision = wv8;
            WorldVision wv9 = new WorldVision { Name = "WorldVision 9 - 9" };
            p9.Vision = wv9;
            foreach (WorldVision worldVision in db.WorldVisions)
            {
                foreach (Person person in db.People.Where(p=>p.Vision.Id == worldVision.Id))
                {
                    person.Vision=null;
                    db.Entry(person).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                db.WorldVisions.Remove(worldVision);
            }
            db.WorldVisions.Add(wv1);
            db.Entry(p1).State = System.Data.Entity.EntityState.Modified;
            db.WorldVisions.Add(wv2);
            db.Entry(p2).State = System.Data.Entity.EntityState.Modified;
            db.WorldVisions.Add(wv3);
            db.Entry(p3).State = System.Data.Entity.EntityState.Modified;
            db.WorldVisions.Add(wv4);
            db.Entry(p4).State = System.Data.Entity.EntityState.Modified;
            db.WorldVisions.Add(wv5);
            db.Entry(p5).State = System.Data.Entity.EntityState.Modified;
            db.WorldVisions.Add(wv6);
            db.Entry(p6).State = System.Data.Entity.EntityState.Modified;
            db.WorldVisions.Add(wv7);
            db.Entry(p7).State = System.Data.Entity.EntityState.Modified;
            db.WorldVisions.Add(wv8);
            db.Entry(p8).State = System.Data.Entity.EntityState.Modified;
            db.WorldVisions.Add(wv9);
            db.Entry(p9).State = System.Data.Entity.EntityState.Modified;

            db.SaveChanges();

            Thought t1 = new Thought { Name="Thought 1 - 123 - 345", PeopleAtEase = { p1, p2, p3 }, PeopleTimid = { p3, p4, p5 } };
            Thought t2 = new Thought { Name="Thought 2 - 234 - 456", PeopleAtEase = { p2, p3, p4 }, PeopleTimid = { p4, p5, p6 } };
            Thought t3 = new Thought { Name="Thought 3 - 345 - 567", PeopleAtEase = { p3, p4, p5 }, PeopleTimid = { p5, p6, p7 } };
            Thought t4 = new Thought { Name="Thought 4 - 456 - 678", PeopleAtEase = { p4, p5, p6 }, PeopleTimid = { p6, p7, p8 } };
            Thought t5 = new Thought { Name="Thought 5 - 567 - 789", PeopleAtEase = { p5, p6, p7 }, PeopleTimid = { p7, p8, p9 } };
            Thought t6 = new Thought { Name="Thought 6 - 678 - 891", PeopleAtEase = { p6, p7, p8 }, PeopleTimid = { p8, p9, p1 } };
            Thought t7 = new Thought { Name="Thought 7 - 789 - 912", PeopleAtEase = { p7, p8, p9 }, PeopleTimid = { p9, p1, p2 } };
            Thought t8 = new Thought { Name="Thought 8 - 891 - 123", PeopleAtEase = { p8, p9, p1 }, PeopleTimid = { p1, p2, p3 } };
            Thought t9 = new Thought { Name="Thought 9 - 912 - 234", PeopleAtEase = { p9, p1, p2 }, PeopleTimid = { p2, p3, p4 } };
            foreach (Thought thought in db.Thoughts)
            {
                db.Thoughts.Remove(thought);
            }
            db.Thoughts.Add(t1);
            db.Thoughts.Add(t2);
            db.Thoughts.Add(t3);
            db.Thoughts.Add(t4);
            db.Thoughts.Add(t5);
            db.Thoughts.Add(t6);
            db.Thoughts.Add(t7);
            db.Thoughts.Add(t8);
            db.Thoughts.Add(t9);

            db.SaveChanges();

            Color c1 = new Color { Name = "Color 1 - 1 - 9" };
            p1.FavoriteColor = c1;
            p9.LeastLikedColor = c1;
            Color c2 = new Color { Name = "Color 2 - 2 - 8" };
            p2.FavoriteColor = c2;
            p8.LeastLikedColor = c2;
            Color c3 = new Color { Name = "Color 3 - 3 - 7" };
            p3.FavoriteColor = c3;
            p7.LeastLikedColor = c3;
            Color c4 = new Color { Name = "Color 4 - 4 - 6" };
            p4.FavoriteColor = c4;
            p6.LeastLikedColor = c4;
            Color c5 = new Color { Name = "Color 5 - 5 - 5" };
            p5.FavoriteColor = c5;
            p5.LeastLikedColor = c5;
            Color c6 = new Color { Name = "Color 6 - 6 - 4" };
            p6.FavoriteColor = c6;
            p4.LeastLikedColor = c6;
            Color c7 = new Color { Name = "Color 7 - 7 - 3" };
            p7.FavoriteColor = c7;
            p3.LeastLikedColor = c7;
            Color c8 = new Color { Name = "Color 8 - 8 - 2" };
            p8.FavoriteColor = c8;
            p2.LeastLikedColor = c8;
            Color c9 = new Color { Name = "Color 9 - 9 - 1" };
            p9.FavoriteColor = c9;
            p1.LeastLikedColor = c9;
            foreach (Color color in db.Colors)
            {
                foreach (Person person in db.People.Where(p => p.FavoriteColor.Id == color.Id || p.LeastLikedColor.Id==color.Id))
                {
                    person.FavoriteColor = null;
                    person.LeastLikedColor = null;
                    db.Entry(person).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                db.Colors.Remove(color);
            }
            db.Colors.Add(c1);
            db.Colors.Add(c2);
            db.Colors.Add(c3);
            db.Colors.Add(c4);
            db.Colors.Add(c5);
            db.Colors.Add(c6);
            db.Colors.Add(c7);
            db.Colors.Add(c8);
            db.Colors.Add(c9);
            db.SaveChanges();

            Address ad1 = new Address { Number = 1, Street ="Person1"};
            p1.Address = ad1;
            Address ad2 = new Address { Number = 1, Street = "Person2" };
            p2.Address = ad2;
            Address ad3 = new Address { Number = 1, Street = "Person3" };
            p3.Address = ad3;
            Address ad4 = new Address { Number = 1, Street = "Person4" };
            p4.Address = ad4;
            Address ad5 = new Address { Number = 1, Street = "Person5" };
            p5.Address = ad5;
            Address ad6 = new Address { Number = 1, Street = "Person6" };
            p6.Address = ad6;
            Address ad7 = new Address { Number = 1, Street = "Person7" };
            p7.Address = ad7;
            Address ad8 = new Address { Number = 1, Street = "Person8" };
            p8.Address = ad8;
            Address ad9 = new Address { Number = 1, Street = "Person9" };
            p9.Address = ad9;
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
            db.Addresses.Add(ad1);
            db.Addresses.Add(ad2);
            db.Addresses.Add(ad3);
            db.Addresses.Add(ad4);
            db.Addresses.Add(ad5);
            db.Addresses.Add(ad6);
            db.Addresses.Add(ad7);
            db.Addresses.Add(ad8);
            db.Addresses.Add(ad9);
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