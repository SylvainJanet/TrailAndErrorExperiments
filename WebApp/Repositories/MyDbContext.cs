using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using WebApp.EntityConfigurations;
using WebApp.Models;

namespace WebApp.Repositories
{
    public class MyDbContext : DbContext
    {
        public MyDbContext()
            : base("name=MyDbContext")
        {
            Database.Log = l => Debug.Write(l);
        }

        public virtual DbSet<Brain> Brains { get; set; }
        public virtual DbSet<Finger> Fingers { get; set; }
        public virtual DbSet<Idea> Ideas { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<Models.Action> Actions { get; set; }
        public virtual DbSet<WorldVision> WorldVisions { get; set; }
        public virtual DbSet<Thought> Thoughts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new PersonConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}