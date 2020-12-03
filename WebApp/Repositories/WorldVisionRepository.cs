using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Repositories
{
    public class WorldVisionRepository : GenericRepository<WorldVision> , IWorldVisionRepository
    {
        public WorldVisionRepository(MyDbContext db) : base(db)
        {

        }
    }
}