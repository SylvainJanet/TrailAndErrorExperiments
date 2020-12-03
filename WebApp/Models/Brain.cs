using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class Brain : BaseEntity
    {
        [Required]
        [Display(Name = "Brain Name")]
        public string Name { get; set; }

        public int? OwnerId { get { if (Owner == null) return null; return Owner.Id; } }
        [Required]
        public Person Owner { get; set; }

        public Brain(string name, Person owner)
        {
            Name = name;
            Owner = owner;
        }

        public Brain()
        {
        }
    }
}