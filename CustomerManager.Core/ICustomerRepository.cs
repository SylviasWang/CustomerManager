using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManager.Core
{
	public interface ICustomerRepository
	{
		void Add (Customer customer);
		List<Customer> GetAll ();
		Customer? GetById (int ID);
		List<Customer> GetByName (string Name);
		void Update (Customer customer);
		void Delete (int ID);
	}
}
