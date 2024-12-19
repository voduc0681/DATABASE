using DATABASE.Config;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DATABASE.View
{
    public partial class EmployeeForm : Form
    {
        private DatabaseConnection db = new DatabaseConnection();
        public EmployeeForm()
        {
            InitializeComponent();
            dgvEmployee.CellClick += dgvEmployee_CellContentClick;
        }
        private void EmployeeForm_Load(object sender, EventArgs e)
        {
            LoadEmployeeData();
        }

        private void LoadEmployeeData()
        {
            try
            {
                string query = "SELECT * FROM Employees";
                DataTable dt = db.ExecuteQuery(query);
                Console.WriteLine(dt);
                dgvEmployee.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading employee data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvEmployee_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                handleSelectRowOnTable(e.RowIndex);
            }
        }

        private void handleSelectRowOnTable(int index)
        {
            DataGridViewRow row = dgvEmployee.Rows[index];
            txtEmployeeCode.Text = row.Cells["EmployeeCode"].Value.ToString();
            txtEmployeeName.Text = row.Cells["EmployeeName"].Value.ToString();
            txtAddress.Text = row.Cells["Address"].Value.ToString();
            txtEmail.Text = row.Cells["Email"].Value.ToString();
            txtPhoneNumber.Text = row.Cells["PhoneNumber"].Value.ToString();
            txtAccount.Text = row.Cells["UserName-Login"].Value.ToString();
            txtPassword.Text = row.Cells["Password-Login"].Value.ToString();
            dtpDateOfBirth.Value = DateTime.Parse(row.Cells["DateOfBirth"].Value.ToString());

            btnUpdateEmployee.Enabled = true;
            btnDeleteEmployee.Enabled = true;
            int role = Int32.Parse(row.Cells["RoleID"].Value.ToString()) ;
            cmbRole.SelectedIndex = role - 1;
        }

        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            string employeeCode = txtEmployeeCode.Text;
            string employeeName = txtEmployeeName.Text;
            string address = txtAddress.Text;
            string email = txtEmail.Text;
            string phoneNumber = txtPhoneNumber.Text;
            string account = txtAccount.Text;
            string password = txtPassword.Text;
            int role = cmbRole.SelectedIndex + 1;

            DateTime dateOfBirth = dtpDateOfBirth.Value;

            // Check required fields
            if (string.IsNullOrEmpty(employeeCode))
            {
                MessageBox.Show("Please enter Employee Code!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmployeeCode.Focus();
                return;
            }

            if (string.IsNullOrEmpty(employeeName))
            {
                MessageBox.Show("Please enter Employee Name!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmployeeName.Focus();
                return;
            }

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Please enter Email!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            if (!ValidateEmployeeData())
            {
                return; // Stop if the data is invalid
            }

            // Check if the employee code already exists in the database
            string checkQuery = "SELECT COUNT(*) FROM Employees WHERE EmployeeCode = @EmployeeCode";
            SqlParameter[] checkParameters = new SqlParameter[]
            {
                new SqlParameter("@EmployeeCode", employeeCode)
            };

            try
            {
                int exists = (int)db.ExecuteScalar(checkQuery, checkParameters);
                if (exists > 0)
                {
                    MessageBox.Show("The Employee Code already exists. Please use a different code!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmployeeCode.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking Employee Code: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // If no duplicate, add the employee
            string query = "INSERT INTO Employees (EmployeeCode, EmployeeName, RoleID, Address, Email, PhoneNumber, DateOfBirth, [UserName-Login], [Password-Login]) " +
                           "VALUES (@EmployeeCode, @EmployeeName, @RoleID, @Address, @Email, @PhoneNumber, @DateOfBirth, @UserNameLogin, @PasswordLogin)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@EmployeeCode", employeeCode),
                new SqlParameter("@EmployeeName", employeeName),
                new SqlParameter("@RoleID", role),
                new SqlParameter("@Address", address),
                new SqlParameter("@Email", email),
                new SqlParameter("@PhoneNumber", phoneNumber),
                new SqlParameter("@DateOfBirth", dateOfBirth),
                new SqlParameter("@UserNameLogin", account),
                new SqlParameter("@PasswordLogin", password),
            };

            try
            {
                int result = db.ExecuteNonQuery(query, parameters);
                if (result > 0)
                {
                    MessageBox.Show("Add employee successfully!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadEmployeeData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding employee: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateEmployee_Click(object sender, EventArgs e)
        {
            string employeeCode = txtEmployeeCode.Text;
            string employeeName = txtEmployeeName.Text;
            string address = txtAddress.Text;
            string email = txtEmail.Text;
            string phoneNumber = txtPhoneNumber.Text;
            string account = txtAccount.Text;
            string password = txtPassword.Text;
            int role = cmbRole.SelectedIndex + 1;
            DateTime dateOfBirth = dtpDateOfBirth.Value;

            if (string.IsNullOrEmpty(employeeCode) || string.IsNullOrEmpty(employeeName) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Please enter complete employee information!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateEmployeeData())
            {
                return; // Stop if the data is invalid
            }

            string query = "UPDATE Employees SET EmployeeName = @EmployeeName, RoleID = @RoleID, Address = @Address, Email = @Email, PhoneNumber = @PhoneNumber, DateOfBirth = @DateOfBirth, [UserName-Login] = @UserNameLogin, [Password-Login] = @PasswordLogin WHERE EmployeeID = @EmployeeID";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@EmployeeID", dgvEmployee.CurrentRow.Cells["EmployeeID"].Value),
                new SqlParameter("@EmployeeName", employeeName),
                new SqlParameter("@RoleID", role),
                new SqlParameter("@Address", address),
                new SqlParameter("@Email", email),
                new SqlParameter("@PhoneNumber", phoneNumber),
                new SqlParameter("@DateOfBirth", dateOfBirth),
                new SqlParameter("@UserNameLogin", account),
                new SqlParameter("@PasswordLogin", password)
            };

            try
            {
                int result = db.ExecuteNonQuery(query, parameters);
                if (result > 0)
                {
                    MessageBox.Show("Employee update successful!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadEmployeeData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while updating employee: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteEmployee_Click(object sender, EventArgs e)
        {
            string employeeCode = txtEmployeeCode.Text;

            if (string.IsNullOrEmpty(employeeCode))
            {
                MessageBox.Show("Please select an employee to delete!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dialogResult = MessageBox.Show($"Are you sure you want to delete the employee with the code {employeeCode}?",
                                                        "Confirm deletion",
                                                        MessageBoxButtons.YesNo,
                                                        MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                string query = "DELETE FROM Employees WHERE EmployeeID = @EmployeeID";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@EmployeeID", dgvEmployee.CurrentRow.Cells["EmployeeID"].Value)
                };

                try
                {
                    int result = db.ExecuteNonQuery(query, parameters);
                    if (result > 0)
                    {
                        MessageBox.Show("Employee was successfully deleted!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadEmployeeData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while deleting employee: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtEmployeeCode.Enabled = true;
            txtEmployeeCode.Clear();
            txtEmployeeName.Clear();
            txtAddress.Clear();
            txtEmail.Clear();
            txtPhoneNumber.Clear();
            txtAccount.Clear();
            txtPassword.Clear();
            dtpDateOfBirth.Value = DateTime.Now;
            /* LoadEmployeeData();*/

            btnAddEmployee.Enabled = true;
            btnUpdateEmployee.Enabled = false;
            btnDeleteEmployee.Enabled = false;
        }

        private bool ValidateEmployeeData()
        {
            // Validate phone number (must be a number, up to 15 digits)
            string phoneNumber = txtPhoneNumber.Text.Trim();
            if (string.IsNullOrEmpty(phoneNumber) || !System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^\d{10,15}$"))
            {
                MessageBox.Show("Invalid phone number! Please enter a number of 10-15 digits.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Validate email format
            string email = txtEmail.Text.Trim();
            if (string.IsNullOrEmpty(email) || !System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Invalid email address!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Validate date of birth (must be over 18 years old)
            DateTime dateOfBirth = dtpDateOfBirth.Value;
            DateTime today = DateTime.Today;
            if (today.Year - dateOfBirth.Year < 18 || (today.Year - dateOfBirth.Year == 18 && dateOfBirth.Date > today.AddYears(-18)))
            {
                MessageBox.Show("Employees must be 18 years of age or older!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim(); // Get the search term from TextBox
            string searchQuery = ""; // SQL search query
            string searchColumn = "";

            // Check which radio button is selected to determine the search column
            if (radioEmployeeCode.Checked)
            {
                searchColumn = "EmployeeCode";
            }
            else if (radioEmployeeName.Checked)
            {
                searchColumn = "EmployeeName";
            }
            else
            {
                return; // Do not search if no radio button is selected
            }

            // If no search term, reload all data
            if (string.IsNullOrEmpty(searchTerm))
            {
                LoadEmployeeData(); // Reload all data
                if (dgvEmployee.Rows.Count > 0)
                {
                    handleSelectRowOnTable(0);
                }
                return;
            }

            // Create the SQL search query
            searchQuery = $"SELECT EmployeeCode, EmployeeName, Address, Email, PhoneNumber, DateOfBirth, [UserName-Login],  [Password-Login], RoleID " +
                          $"FROM Employees WHERE {searchColumn} COLLATE Latin1_General_CI_AI LIKE @SearchTerm";

            // Create parameters for the SQL query
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@SearchTerm", SqlDbType.NVarChar) { Value = "%" + searchTerm + "%" }
            };

            // Execute the query and update the DataGridView
            try
            {
                DataTable dt = db.ExecuteQuery(searchQuery, parameters); // db is the database connection object
                dgvEmployee.DataSource = dt;

                if (dgvEmployee.Rows.Count > 0)
                {
                    handleSelectRowOnTable(0); // Select the first row if there are results
                }
                else
                {
                    ClearEmployeeInputFields(); // Clear the input fields if no results
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while searching.: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearEmployeeInputFields()
        {
            txtEmployeeCode.Clear();
            txtEmployeeName.Clear();
            txtAddress.Clear();
            txtEmail.Clear();
            txtPhoneNumber.Clear();
            dtpDateOfBirth.Value = DateTime.Now;
        }

        private void btnRefreshSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
        }

        private void btnAccInfor_Click(object sender, EventArgs e)
        {
            string employeeId = txtEmployeeCode.Text; // Get EmployeeID from the input field

            if (string.IsNullOrEmpty(employeeId))
            {
                MessageBox.Show("Please select an employee to view account information.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            /*AccountInformationForm accountInformationForm = new AccountInformationForm(employeeId);
            accountInformationForm.ShowDialog();*/
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

