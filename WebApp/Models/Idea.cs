using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Idea : BaseEntity
    {
        [Required]
        [Display(Name = "Idea Name")]
        public string Name { get; set; }

        public IList<Person> Owners { get; set; }

        public Idea(string name) : this()
        {
            Name = name;
        }

        public Idea(string name, IList<Person> owners) : this(name)
        {
            Owners = owners;
        }

        public Idea()
        {
            Owners = new List<Person>();
        }
    }
}