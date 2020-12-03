using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Action : BaseEntity
    {
        [Required]
        [Display(Name = "Action Name")]
        public string Name { get; set; }

        [Required]
        public IList<Person> People { get; set; }

        public Action()
        {
            People = new List<Person>();
        }

        public Action(string name, IList<Person> people)
        {
            Name = name;
            People = people;
        }
    }
}