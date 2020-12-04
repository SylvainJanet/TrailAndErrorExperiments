using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Person : BaseEntity
    {
        [Required]
        [Display(Name = "Person Name")]
        public string Name { get; set; }

        public Brain Brain { get; set; }

        public IList<Finger> Fingers { get; set; }

        public IList<Idea> Ideas { get; set; }

        public IList<Action> Actions { get; set; }

        public WorldVision Vision { get; set; }

        public IList<Thought> ComfortableThoughts { get; set; }

        public IList<Thought> SecretThoughts { get; set; }

        public Color FavoriteColor { get; set; }

        public Color LeastLikedColor { get; set; }

        public Address Address { get; set; }

        public Person(string name) : this()
        {
            Name = name;
        }

        public Person(string name, Brain brain, IList<Finger> fingers, IList<Idea> ideas, IList<Action> actions, WorldVision vision, IList<Thought> comfortableThoughts, IList<Thought> secretThoughts, Color favoriteColor, Color leastLikedColor, Address address) : this(name)
        {
            Brain = brain;
            Fingers = fingers;
            Ideas = ideas;
            Actions = actions;
            Vision = vision;
            ComfortableThoughts = comfortableThoughts;
            SecretThoughts = secretThoughts;
            FavoriteColor = favoriteColor;
            LeastLikedColor = leastLikedColor;
            Address = address;
        }

        public Person()
        {
            Fingers = new List<Finger>();
            Ideas = new List<Idea>();
            Actions = new List<Action>();
            ComfortableThoughts = new List<Thought>();
            SecretThoughts = new List<Thought>();
        }
    }
}