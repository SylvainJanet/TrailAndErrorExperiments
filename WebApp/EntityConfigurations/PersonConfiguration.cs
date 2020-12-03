using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.EntityConfigurations
{
    public class PersonConfiguration : EntityTypeConfiguration<Person>
    {
        public PersonConfiguration()
        {
            HasMany<Idea>(p => p.Ideas)
                .WithMany(i => i.Owners)
                .Map(pi =>
                    {
                        pi.MapLeftKey("PersonId");
                        pi.MapRightKey("IdeaId");
                        pi.ToTable("PersonIdea");
                    });
            //Set will cascade on delete to false in migration

            HasMany<Finger>(p => p.Fingers)
                .WithRequired(f => f.Owner)
                .HasForeignKey<int?>(f => f.OwnerId)
                .WillCascadeOnDelete(false);

            HasOptional<Brain>(p => p.Brain)
                .WithRequired(b => b.Owner)
                .WillCascadeOnDelete(false);
            //Set foreign key in Brain class

            HasMany<Models.Action>(p => p.Actions)
                .WithMany(a => a.People)
                .Map(pa =>
                    {
                        pa.MapLeftKey("PersonId");
                        pa.MapRightKey("ActionId");
                        pa.ToTable("PersonAction");
                    });

            HasOptional<WorldVision>(p => p.Vision);

            HasMany<Thought>(p => p.ComfortableThoughts)
                .WithMany(t => t.PeopleAtEase)
                .Map(pt =>
                {
                    pt.MapLeftKey("PersonId");
                    pt.MapRightKey("ThoughtId");
                    pt.ToTable("PersonThoughtComfy");
                });

            HasMany<Thought>(p => p.SecretThoughts)
                .WithMany(t => t.PeopleTimid)
                .Map(pt =>
                {
                    pt.MapLeftKey("PersonId");
                    pt.MapRightKey("ThoughtId");
                    pt.ToTable("PersonThoughtTimid");
                });
            //Set will cascade on delete to false in migration
        }
    }
}