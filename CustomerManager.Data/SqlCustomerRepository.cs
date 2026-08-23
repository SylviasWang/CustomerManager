using CustomerManager.Core;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Xml.Linq;

namespace CustomerManager.Data
{
	public class SqlCustomerRepository : ICustomerRepository
	{
		private string _connectionString = @"Server=localhost\SQLEXPRESS;Database=CustomerManagerDB;Trusted_Connection=True;TrustServerCertificate=True;";

		public List<Customer> GetAll()
		{
			var result = new List<Customer>();                    // 準備一個空 List 裝結果

			using (var conn = new SqlConnection(_connectionString))  // 1. 建立連線
			{
				conn.Open();                                      // 2. 打開連線

				var sql = "SELECT ID, Name, Gender, Birthday, Phone, Email FROM Customer";
				using (var cmd = new SqlCommand(sql, conn))       // 3. 準備要送的 SQL 命令
				using (var reader = cmd.ExecuteReader())          // 4. 執行查詢,拿到讀取器
				{
					while (reader.Read())                         // 5. 一列一列讀
					{
						var customer = new Customer
						{
							ID = reader.GetInt32(0),              // 第 0 欄 ID
							Name = reader.GetString(1),           // 第 1 欄 Name
							Gender = (Gender)reader.GetInt32(2),  // 第 2 欄 Gender(INT 轉回 enum)
							Birthday = reader.GetDateTime(3),     // 第 3 欄 Birthday
							Phone = reader.GetString(4),		  // 第 4 欄
							Email = reader.GetString(5)			  // 第 5 欄
						};
						result.Add(customer);                     // 組好一個 Customer 就加進 List
					}
				}
			}                                                     // using 結束,連線自動關閉

			return result;
		}

		public void Add(Customer customer)
		{
			using (var conn = new SqlConnection(_connectionString))
			{
				conn.Open();

				var sql = @"INSERT INTO Customer (ID, Name, Gender, Birthday, Phone, Email)
                    VALUES (@ID, @Name, @Gender, @Birthday, @Phone, @Email)";

				using (var cmd = new SqlCommand(sql, conn))
				{
					cmd.Parameters.AddWithValue("@ID", customer.ID);
					cmd.Parameters.AddWithValue("@Name", customer.Name);
					cmd.Parameters.AddWithValue("@Gender", (int)customer.Gender);
					cmd.Parameters.AddWithValue("@Birthday", customer.Birthday);
					cmd.Parameters.AddWithValue("@Phone", customer.Phone);
					cmd.Parameters.AddWithValue("@Email", customer.Email);

					cmd.ExecuteNonQuery();   // 執行「不回傳資料」的 SQL(INSERT/UPDATE/DELETE 用這個)
				}
			}
		}

		public Customer? GetById(int ID)
		{
			Customer? customer = null;
			using (var conn = new SqlConnection(_connectionString))  // 1. 建立連線
			{
				conn.Open();                                      // 2. 打開連線

				var sql = @"SELECT ID, Name, Gender, Birthday, Phone, Email FROM Customer 
					WHERE ID = @ID";

				using (var cmd = new SqlCommand(sql, conn))       // 3. 準備要送的 SQL 命令
				{
					cmd.Parameters.AddWithValue("@ID", ID);
					using (var reader = cmd.ExecuteReader())          // 4. 執行查詢,拿到讀取器
					{
						while (reader.Read())                         // 5. 一列一列讀
						{
							customer = new Customer
							{
								ID = reader.GetInt32(0),              // 第 0 欄 ID
								Name = reader.GetString(1),           // 第 1 欄 Name
								Gender = (Gender)reader.GetInt32(2),  // 第 2 欄 Gender(INT 轉回 enum)
								Birthday = reader.GetDateTime(3),     // 第 3 欄 Birthday
								Phone = reader.GetString(4),          // 第 4 欄
								Email = reader.GetString(5)           // 第 5 欄
							};
							break;
						}
					}
				}
			}                                                     // using 結束,連線自動關閉

			return customer;
		}

		public List<Customer> GetByName(string Name)
		{
			var result = new List<Customer>();

			using (var conn = new SqlConnection(_connectionString))  // 1. 建立連線
			{
				conn.Open();                                      // 2. 打開連線

				var sql = @"SELECT ID, Name, Gender, Birthday, Phone, Email FROM Customer 
					WHERE Name = @Name";

				using (var cmd = new SqlCommand(sql, conn))       // 3. 準備要送的 SQL 命令
				{
					cmd.Parameters.AddWithValue("@Name", Name);
					using (var reader = cmd.ExecuteReader())          // 4. 執行查詢,拿到讀取器
					{
						while (reader.Read())                         // 5. 一列一列讀
						{
							var customer = new Customer
							{
								ID = reader.GetInt32(0),              // 第 0 欄 ID
								Name = reader.GetString(1),           // 第 1 欄 Name
								Gender = (Gender)reader.GetInt32(2),  // 第 2 欄 Gender(INT 轉回 enum)
								Birthday = reader.GetDateTime(3),     // 第 3 欄 Birthday
								Phone = reader.GetString(4),          // 第 4 欄
								Email = reader.GetString(5)           // 第 5 欄
							};
							result.Add (customer);
						}
					}
				}
			}                                                     // using 結束,連線自動關閉

			return result;
		}

		public void Update(Customer customer)
		{
			using (var conn = new SqlConnection(_connectionString))
			{
				conn.Open();

				var sql = @"Update Customer
					Set Name = @Name,
					Gender = @Gender,
					Birthday = @Birthday,
					Phone = @Phone,
					Email = @Email
					WHERE ID = @ID";

				using (var cmd = new SqlCommand(sql, conn))
				{
					cmd.Parameters.AddWithValue("@ID", customer.ID);
					cmd.Parameters.AddWithValue("@Name", customer.Name);
					cmd.Parameters.AddWithValue("@Gender", (int)customer.Gender);
					cmd.Parameters.AddWithValue("@Birthday", customer.Birthday);
					cmd.Parameters.AddWithValue("@Phone", customer.Phone);
					cmd.Parameters.AddWithValue("@Email", customer.Email);

					cmd.ExecuteNonQuery();   // 執行「不回傳資料」的 SQL(INSERT/UPDATE/DELETE 用這個)
				}
			}
		}

		public void Delete(int ID)
		{
			using (var conn = new SqlConnection(_connectionString))
			{
				conn.Open();

				var sql = @"DELETE FROM Customer 
					WHERE ID = @ID";

				using (var cmd = new SqlCommand(sql, conn))
				{
					cmd.Parameters.AddWithValue("@ID", ID);
					cmd.ExecuteNonQuery();   // 執行「不回傳資料」的 SQL(INSERT/UPDATE/DELETE 用這個)
				}
			}
		}
	}
}
