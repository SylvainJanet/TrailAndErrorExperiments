using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Service
{
    public class AddressService : GenericService<Address>, IAddressService
    {
        public AddressService(IAddressRepository addressRepository) : base(addressRepository)
        {

        }
        
        public override Expression<Func<IQueryable<Address>, IOrderedQueryable<Address>>> OrderExpression()
        {
            return null;
        }

        public override Expression<Func<Address, bool>> SearchExpression(string searchField = "")
        {
            searchField = searchField.Trim().ToLower();
            return a => a.Street.Trim().ToLower().Contains(searchField);
        }
    }
}