using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Service
{
    public class PersonService : GenericService<Person>, IPersonService
    {
        public PersonService(IPersonRepository personRepository) : base(personRepository)
        {

        }

        public override Expression<Func<IQueryable<Person>, IOrderedQueryable<Person>>> OrderExpression()
        {
            return null;
        }

        public override Expression<Func<Person, bool>> SearchExpression(string searchField = "")
        {
            searchField = searchField.Trim().ToLower();
            return b => b.Name.Contains(searchField);
        }
    }
}