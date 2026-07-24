using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManager.Core
{
	public class InMemoryCustomerRepository : ICustomerRepository
	{
		// 內部用一個 List 存資料(所有操作都對它動手)
		private List<Customer> _ListCustomers = new List<Customer>();

		public void Add(Customer customer)
		{
			_ListCustomers.Add(customer);
		}

		public List<Customer> GetAll()
		{
			return _ListCustomers;
		}

		public Customer? GetById(int ID)
		{
			return _ListCustomers.FirstOrDefault(c => c.ID == ID);
		}

		public List<Customer> GetByName(string Name)
		{
			return _ListCustomers.Where(c => c.Name == Name).ToList();
		}

		public void Update(Customer customer)
		{
			Customer? FindCustomer = GetById (customer.ID);

			if (FindCustomer != null)
			{
				FindCustomer.Name = customer.Name;
				FindCustomer.Gender = customer.Gender;
				FindCustomer.Phone = customer.Phone;
				FindCustomer.Email = customer.Email;
				FindCustomer.Birthday = customer.Birthday;
			}
		}

		public void Delete(int ID)
		{
			Customer? FindCustomer = GetById(ID);

			if (FindCustomer != null)
			{
				_ListCustomers.Remove (FindCustomer);
			}
		}
	}
}
