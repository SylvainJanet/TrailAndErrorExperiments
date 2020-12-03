using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Repositories
{
    public class BrainRepository : GenericRepository<Brain>, IBrainRepository
    {
        public BrainRepository(MyDbContext db) : base(db)
        {

        }
    }
}