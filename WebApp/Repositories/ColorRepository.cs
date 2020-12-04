using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Repositories
{
    public class ColorRepository : GenericRepository<Models.Color>, IColorRepository
    {
        public ColorRepository(MyDbContext myDbContext) : base(myDbContext)
        {
        }
    }
}