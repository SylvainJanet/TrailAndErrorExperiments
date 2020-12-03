using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Repositories
{
    public class ActionRepository : GenericRepository<Models.Action>, IActionRepository
    { 
        public ActionRepository(MyDbContext db) : base(db)
        {

        }
    }
}