using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManager.Core
{
	public enum OrderStatus
	{
		Pending,
		Processing,
		Completed,
		Cancelled
	}

	public class Order
	{
		public int ID { get; set; }
		public int CustomerID { get; set; }
		public decimal TotalPrice { get; set; }
		public DateTime Date { get; set; }
		public OrderStatus Status { get; set;}
	}
}
