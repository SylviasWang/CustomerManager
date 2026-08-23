using CustomerManager.Core;
using CustomerManager.Data;

namespace CustomerManager.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Add_ShouldAddCustomer()
        {
			// Arrange:準備一個空 repository 和一筆客戶
			var repo = new InMemoryCustomerRepository();
			var customer = new Customer { ID = 1, Name = "測試客戶" };

			// Act:執行新增
			repo.Add(customer);

			// Assert:檢查結果
			var all = repo.GetAll();
			Assert.Single(all);                    // 應該只有 1 筆
			Assert.Equal("測試客戶", all[0].Name); // 那筆的 Name 應該是「測試客戶」
		}

		[Fact]
		public void Update_ShouldChangeCustomerData()
		{
			// Arrange:準備 repository,先加一筆進去
			var repo = new InMemoryCustomerRepository();
			repo.Add(new Customer { ID = 1, Name = "AAA", Email = "old@test.com" });

			// Act:用同一個 ID、不同資料去 Update
			repo.Update(new Customer { ID = 1, Name = "BBB", Email = "new@test.com" });

			// Assert:檢查那筆資料真的被改了
			var result = repo.GetById(1);
			Assert.NotNull(result);                        // 應該找得到這筆
			Assert.Equal("BBB", result.Name);			  // Name 應該變了
			Assert.Equal("new@test.com", result.Email);   // Email 應該變了
		}

		[Fact]
		public void Delete_ShouldRemoveCustomer()
		{
			// Arrange:準備 repository,先加一筆進去
			var repo = new InMemoryCustomerRepository();
			repo.Add(new Customer { ID = 1, Name = "AAA", Email = "old@test.com" });

			// Act:刪除資料
			repo.Delete(1);

			// Assert:檢查沒有任何資料
			var all = repo.GetAll();
			Assert.Empty(all);

			var Cus = repo.GetById(1);
			Assert.Null (Cus);
		}

		[Fact]
		public void GetByName_ShouldGetCustomer()
		{
			// Arrange:準備 repository,先加一筆進去
			var repo = new InMemoryCustomerRepository();
			repo.Add(new Customer { ID = 1, Name = "AAA", Email = "AAA123@test.com" });
			repo.Add(new Customer { ID = 2, Name = "BBB", Email = "BBB123@test.com" });

			// Act:取得資料
			var Cus = repo.GetByName ("BBB");

			// Assert:確認資料是否為BBB的
			Assert.Equal("BBB", Cus[0].Name);
			Assert.Equal(2, Cus[0].ID);
			Assert.Equal("BBB123@test.com", Cus[0].Email);
		}
	}
}