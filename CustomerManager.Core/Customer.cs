namespace CustomerManager.Core
{
	public enum Gender
	{
		Male,
		Female,
		Other
	}

	public class Customer
    {
		public int ID { get; set; }
		public string Name { get; set; } = string.Empty;
		public Gender Gender { get; set; }
		public DateTime Birthday { get; set; }
		public string? Phone { get; set; }
		public string? Email { get; set; }
	}
}
