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

        public override void Delete(params object[] objs)
        {
            using (MyDbContext db = new MyDbContext())
            {
                IPersonService _PersonService = new PersonService(new PersonRepository(db));
                foreach (Person person in _PersonService.GetAllExcludes(1, int.MaxValue, null, p => p.Address.Number == (int)objs[0] && p.Address.Street == (string)objs[1]))
                {
                    _PersonService.UpdateOne(person, "Address", null);
                }
            }
            _repository.Delete(objs);
        }

        public override void Delete(Address t)
        {
            using (MyDbContext db = new MyDbContext())
            {
                IPersonService _PersonService = new PersonService(new PersonRepository(db));
                foreach (Person person in _PersonService.GetAllExcludes(1, int.MaxValue, null, p => p.Address.Number == t.Number && p.Address.Street == t.Street))
                {
                    _PersonService.UpdateOne(person, "Address", null);
                }
            }
            _repository.Delete(t);
        }

        public override Expression<Func<IQueryable<Address>, IOrderedQueryable<Address>>> OrderExpression()
        {
            return null;
        }

        public override void Save(Address t)
        {
            _repository.Save(t);
        }

        public override Expression<Func<Address, bool>> SearchExpression(string searchField = "")
        {
            searchField = searchField.Trim().ToLower();
            return a => a.Street.Trim().ToLower().Contains(searchField);
        }

        public override void Update(Address t)
        {
            _repository.Update(t);
        }

        public override void UpdateOne(Address t, string propertyName, object newValue)
        {
            _repository.UpdateOne(t, propertyName, newValue);
        }
    }
}