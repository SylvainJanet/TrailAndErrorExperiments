using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Finger : BaseEntity
    {
        [Required]
        [Display(Name = "Finger Name")]
        public string Name { get; set; }

        public int? OwnerId { get; set; }
        [Required]
        public Person Owner { get; set; }

        public Finger(string name, Person owner)
        {
            Name = name;
            Owner = owner;
        }

        public Finger()
        {
        }
    }
}