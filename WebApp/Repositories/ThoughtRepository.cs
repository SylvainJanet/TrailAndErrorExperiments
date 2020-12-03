using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Repositories
{
    public class ThoughtRepository : GenericRepository<Thought>, IThoughtRepository
    {
        public ThoughtRepository(MyDbContext context) : base(context)
        {

        }
    }
}