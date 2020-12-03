using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Repositories
{
    public class FingerRepository : GenericRepository<Finger>, IFingerRepository
    {
        public FingerRepository(MyDbContext db) : base(db)
        {

        }
    }
}