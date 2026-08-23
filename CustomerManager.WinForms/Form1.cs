using CustomerManager.Core;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CustomerManager.WinForms
{
	public partial class Form1 : Form
	{
		private ICustomerRepository? repository;   // 欄位:型別是「介面」

		private DataGridView Grid;
		private TextBox txtName;
		private ComboBox cbGender;
		private DateTimePicker dtpBirthday;
		private TextBox txtPhone;
		private TextBox txtMail;


		public Form1()
		{
			InitializeComponent();
			repository = new InMemoryCustomerRepository();   // 只 new 一次

			var CustomerOne = new Customer {ID = 0, Name = "王小明", Gender = Gender.Male, Birthday = new DateTime(1990, 5, 20), Phone = "0911111111", Email = "a@test.com" };
			var CustomerTwo = new Customer {ID = 1, Name = "王小明2", Gender = Gender.Female, Birthday = new DateTime(1991, 7, 15), Phone = "0922222222", Email = "a1@test.com" };
			var CustomerThree = new Customer {ID = 2, Name = "王小明3", Gender = Gender.Other, Birthday = new DateTime(1992, 3, 1), Phone = "0933333333", Email = "a2@test.com" };
			repository.Add (CustomerOne);
			repository.Add (CustomerTwo);
			repository.Add (CustomerThree);

			// 表格
			Grid = new DataGridView();
			Grid.Location = new Point(0, 0);   
			Grid.Size = new Size(800, 250);
			Grid.DataSource = new BindingList<Customer>(repository.GetAll());
			Grid.AutoGenerateColumns = false;

			Grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ID", HeaderText = "ID" });
			Grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Name", HeaderText = "姓名" });
			Grid.Columns.Add(new DataGridViewComboBoxColumn { DataPropertyName = "Gender", HeaderText = "性別", DataSource = Enum.GetValues(typeof(Gender)) });
			Grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Birthday", HeaderText = "生日" , DefaultCellStyle = new DataGridViewCellStyle { Format = "yyyy/MM/dd" } });
			Grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Phone", HeaderText = "電話" });
			Grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Email", HeaderText = "郵箱" });

			Grid.ReadOnly = true;
			Grid.AllowUserToAddRows = false;
			Grid.AllowUserToDeleteRows = false;

			Controls.Add(Grid);

			// 姓名
			Label lblName = new Label();
			lblName.Text = "姓名:";
			lblName.Location = new Point(20, 270);    
			lblName.Size = new Size(50, 25);
			Controls.Add(lblName);

			txtName = new TextBox();
			txtName.Location = new Point(75, 270);    
			txtName.Size = new Size(150, 25);
			Controls.Add(txtName);

			// 性別
			Label lblGender = new Label();
			lblGender.Text = "性別:";
			lblGender.Location = new Point(250, 270);   
			lblGender.Size = new Size(50, 25);
			Controls.Add(lblGender);

			cbGender = new ComboBox();
			cbGender.Location = new Point(300, 270);    
			cbGender.Size = new Size(150, 25);
			cbGender.DropDownStyle = ComboBoxStyle.DropDownList;
			cbGender.DataSource = Enum.GetValues(typeof(Gender));
			cbGender.SelectedItem = Gender.Male;
			Controls.Add(cbGender);

			// 生日
			Label lblBirthday = new Label();
			lblBirthday.Text = "生日:";
			lblBirthday.Location = new Point(475, 270);    
			lblBirthday.Size = new Size(50, 25);
			Controls.Add(lblBirthday);

			dtpBirthday = new DateTimePicker();
			dtpBirthday.Location = new Point(525, 270);
			dtpBirthday.Size = new Size(150, 25);
			dtpBirthday.Format = DateTimePickerFormat.Short;   // 只顯示日期,不含時間
			Controls.Add(dtpBirthday);

			// 電話
			Label lblPhone = new Label();
			lblPhone.Text = "電話:";
			lblPhone.Location = new Point(20, 300);    
			lblPhone.Size = new Size(50, 25);
			Controls.Add(lblPhone);

			txtPhone = new TextBox();
			txtPhone.Location = new Point(75, 300);   
			txtPhone.Size = new Size(150, 25);
			Controls.Add(txtPhone);

			// 信箱
			Label lblMail = new Label();
			lblMail.Text = "信箱:";
			lblMail.Location = new Point(250, 300);
			lblMail.Size = new Size(50, 25);
			Controls.Add(lblMail);

			txtMail = new TextBox();
			txtMail.Location = new Point(300, 300);   
			txtMail.Size = new Size(150, 25);
			Controls.Add(txtMail);

			// 新增
			Button btnAdd = new Button();
			btnAdd.Location = new Point(475, 305);  
			btnAdd.Size = new Size(100, 25);
			btnAdd.Text = "新增";
			Controls.Add(btnAdd);

			btnAdd.Click += (sender, e) =>
			{
				var selectedGender = cbGender.SelectedItem;
				if (selectedGender == null)
					return;

				if (txtName.Text == string.Empty)
				{
					MessageBox.Show("請輸入姓名!");
					return;
				}

				if (txtPhone.Text == string.Empty)
				{
					MessageBox.Show("請輸入電話!");
					return;
				}

				if (txtMail.Text == string.Empty)
				{
					MessageBox.Show("請輸入信箱!");
					return;
				}

				int newId = 0;
				if (repository.GetAll ().Count != 0)
					newId = repository.GetAll().Max(c => c.ID) + 1;

				repository.Add(new Customer { ID = newId, Name = txtName.Text, Gender = (Gender)selectedGender, Birthday = dtpBirthday.Value.Date, Phone = txtPhone.Text, Email = txtMail.Text });

				LoadCustomers();
				SelDataOrDefault();
			};

			// 刪除
			Button btnDelete = new Button();
			btnDelete.Location = new Point(585, 305);    
			btnDelete.Size = new Size(100, 25);
			btnDelete.Text = "刪除";
			Controls.Add(btnDelete);

			btnDelete.Click += (sender, e) =>
			{
				if (Grid.CurrentRow == null) 
					return;

				var selected = Grid.CurrentRow.DataBoundItem as Customer;
				if (selected != null)
				{
					if (Grid.CurrentRow.Index == repository.GetAll().Count - 1)
					{
						Grid.ClearSelection(); // 清除現有選取
						if ((repository.GetAll().Count - 1) != 0)
						{
							Grid.Rows[0].Selected = true; // 選取指定列
							Grid.CurrentCell = Grid.Rows[0].Cells[0]; // 將焦點移動到該列的第一個儲存格
						}
					}
					repository.Delete(selected.ID);
				}

				LoadCustomers();
				SelDataOrDefault();
			};

			// 修改
			Button btnEdit = new Button();
			btnEdit.Location = new Point(695, 305);    
			btnEdit.Size = new Size(100, 25);
			btnEdit.Text = "修改";
			Controls.Add(btnEdit);

			btnEdit.Click += (sender, e) =>
			{
				Customer? selected = Grid.CurrentRow.DataBoundItem as Customer;
				if (selected != null)
				{
					var selectedGender = cbGender.SelectedItem;
					if (selectedGender == null)
						return;

					if (txtName.Text == string.Empty)
					{
						MessageBox.Show("請輸入姓名!");
						return;
					}

					if (txtPhone.Text == string.Empty)
					{
						MessageBox.Show("請輸入電話!");
						return;
					}

					if (txtMail.Text == string.Empty)
					{
						MessageBox.Show("請輸入信箱!");
						return;
					}

					Customer edit = new Customer ();
					edit.ID = selected.ID;
					edit.Name = txtName.Text;
					edit.Gender = (Gender)selectedGender;
					edit.Birthday = dtpBirthday.Value.Date;
					edit.Phone = txtPhone.Text;
					edit.Email = txtMail.Text;

					repository.Update (edit);
					LoadCustomers();
				}
			};

			Grid.SelectionChanged += (sender, e) =>
			{
				SelDataOrDefault();
			};
		}

		public void SelDataOrDefault()
		{
			if (Grid.CurrentRow != null && Grid.CurrentRow.DataBoundItem != null)
			{ 
				var selected = Grid.CurrentRow.DataBoundItem as Customer;
				if (selected != null)
				{
					txtName.Text = selected.Name;
					cbGender.SelectedItem = selected.Gender;
					dtpBirthday.Value = selected.Birthday.Date;
					txtPhone.Text = selected.Phone;
					txtMail.Text = selected.Email;
				}
			}
			else
			{
				txtName.Text = string.Empty;
				dtpBirthday.Value = DateTime.Now.Date;
				cbGender.SelectedItem = Gender.Male;
				txtPhone.Text = string.Empty;
				txtMail.Text = string.Empty;
			}
		}

		private void LoadCustomers()
		{
			Grid.DataSource = new BindingList<Customer>(repository.GetAll());
		}
	}
}
