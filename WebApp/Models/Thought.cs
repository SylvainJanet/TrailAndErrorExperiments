using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Thought : BaseEntity
    {
        public string Name { get; set; }

        public IList<Person> PeopleAtEase { get; set; }

        public IList<Person> PeopleTimid { get; set; }

        public Thought()
        {
            PeopleAtEase = new List<Person>();
            PeopleTimid = new List<Person>();
        }

        public Thought(string name) : base()
        {
            Name = name;
        }

        public Thought(string name, List<Person> peopleAtEase, List<Person> peopleTimid) : this(name)
        {
            PeopleAtEase = peopleAtEase;
            PeopleTimid = peopleTimid;
        }
    }
}