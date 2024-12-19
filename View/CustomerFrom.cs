using DATABASE.Config;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DATABASE.View
{
    public partial class CustomerFrom : Form
    {
        private DatabaseConnection db = new DatabaseConnection();
        public CustomerFrom()
        {
            InitializeComponent();
            dgvCustomers.CellContentClick += dgvCustomers_CellContentClick;
        }

        private void dgvCustomers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                handleSelectRowOnTable(e.RowIndex);
            }
        }

        private void CustomerFrom_Load(object sender, EventArgs e)
        {
            LoadCustomerData();
        }

        private void LoadCustomerData()
        {
            try
            {
                string query = "SELECT * FROM Customers";
                DataTable dt = db.ExecuteQuery(query);
                Console.WriteLine(dt);
                dgvCustomers.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading customer data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void handleSelectRowOnTable(int index)
        {
            DataGridViewRow row = dgvCustomers.CurrentRow;
            txtCustomerCode.Text = row.Cells["CustomerCode"].Value.ToString();
            txtCustomerName.Text = row.Cells["CustomerName"].Value.ToString();
            txtPhoneNumber.Text = row.Cells["PhoneNumber"].Value.ToString();
            txtAddress.Text = row.Cells["Address"].Value.ToString();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtCustomerCode.Clear();
            txtCustomerName.Clear();
            txtPhoneNumber.Clear();
            txtAddress.Clear();
        }

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            string CustomerCode = txtCustomerCode.Text;
            string CustomerName = txtCustomerName.Text;
            string phoneNumber = txtPhoneNumber.Text;
            string address = txtAddress.Text;

            if (string.IsNullOrEmpty(CustomerCode) || string.IsNullOrEmpty(CustomerName))
            {
                MessageBox.Show("Please enter complete customer information!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateCustomerData())
            {
                return; // Stop if the data is invalid
            }

            string query = "INSERT INTO Customers (CustomerCode, CustomerName, PhoneNumber, Address) VALUES (@CustomerCode, @CustomerName, @PhoneNumber, @Address)";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CustomerCode", CustomerCode),
                new SqlParameter("@CustomerName", CustomerName),
                new SqlParameter("@PhoneNumber", phoneNumber),
                new SqlParameter("@Address", address),
            };

            try
            {
                int result = db.ExecuteNonQuery(query, parameters);
                if (result > 0)
                {
                    MessageBox.Show("Customer added successfully!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCustomerData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding customer: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateCustomer_Click(object sender, EventArgs e)
        {
            string customerCode = txtCustomerCode.Text;
            string customerName = txtCustomerName.Text;
            string phoneNumber = txtPhoneNumber.Text;
            string address = txtAddress.Text;

            if (string.IsNullOrEmpty(customerCode) || string.IsNullOrEmpty(customerName))
            {
                MessageBox.Show("Please enter complete customer information!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "UPDATE Customers SET CustomerCode = @CustomerCode, CustomerName = @CustomerName, PhoneNumber = @PhoneNumber, Address = @Address WHERE CustomerID = @CustomerID";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CustomerCode", customerCode),
                new SqlParameter("@CustomerName", customerName),
                new SqlParameter("@PhoneNumber", phoneNumber),
                new SqlParameter("@Address", address),
                new SqlParameter("@CustomerID", dgvCustomers.CurrentRow.Cells["CustomerID"].Value)
            };

            try
            {
                int result = db.ExecuteNonQuery(query, parameters);
                if (result > 0)
                {
                    MessageBox.Show("Customer updated successfully!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCustomerData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while updating customer: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteCustomer_Click(object sender, EventArgs e)
        {
            string customerCode = txtCustomerCode.Text;

            if (string.IsNullOrEmpty(customerCode))
            {
                MessageBox.Show("Please select a customer to delete!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dialogResult = MessageBox.Show($"Are you sure you want to delete the customer with the code {customerCode}?",
                                                        "Confirm deletion",
                                                        MessageBoxButtons.YesNo,
                                                        MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                string query = "DELETE FROM Customers WHERE CustomerID = @CustomerID";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@CustomerID", dgvCustomers.CurrentRow.Cells["CustomerID"].Value)
                };
                txtCustomerCode.Clear();
                txtCustomerName.Clear();
                txtPhoneNumber.Clear();
                txtAddress.Clear();

                try
                {
                    int result = db.ExecuteNonQuery(query, parameters);
                    if (result > 0)
                    {
                        MessageBox.Show("Customer was successfully deleted!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadCustomerData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while deleting customer: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool ValidateCustomerData()
        {
            // Validate the phone number (must be numeric, maximum 15 characters)
            string phoneNumber = txtPhoneNumber.Text.Trim();
            if (string.IsNullOrEmpty(phoneNumber) || !System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^\d{10,15}$"))
            {
                MessageBox.Show("Invalid phone number! Please enter a number of 10-15 digits.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim(); // Get the search term from the TextBox and remove extra spaces
            string searchQuery = ""; // SQL query for searching
            string searchColumn = "";

            // Check which radio button is selected and choose the search field
            if (radioCustomerCode.Checked)
            {
                searchColumn = "CustomerCode";
            }
            else if (radioCustomerName.Checked)
            {
                searchColumn = "CustomerName";
            }
            else
            {
                return; // Do not search if no radio button is selected
            }

            // If there is no search term, do not execute the query
            if (string.IsNullOrEmpty(searchTerm))
            {
                LoadCustomerData(); // Refresh all data if there is no search term
                if (dgvCustomers.Rows.Count > 0)
                {
                    handleSelectRowOnTable(0);
                }
                return;
            }

            // Remove extra spaces from the search term
            searchTerm = System.Text.RegularExpressions.Regex.Replace(searchTerm, "/s+", " ");

            // Create the SQL query for searching
            searchQuery = $"SELECT CustomerCode, CustomerName, PhoneNumber, Address FROM Customers " +
                          $"WHERE {searchColumn} COLLATE Latin1_General_CI_AI LIKE @SearchTerm";

            // Create parameters for the SQL query
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@SearchTerm", SqlDbType.NVarChar) { Value = "%" + searchTerm + "%" }
            };

            // Execute the query and update the DataGridView
            try
            {
                // Call the ExecuteQuery method to get the search results
                DataTable dt = db.ExecuteQuery(searchQuery, parameters);

                // Update the DataGridView with the search results
                dgvCustomers.DataSource = dt;

                if (dgvCustomers.Rows.Count > 0)
                {
                    handleSelectRowOnTable(0);
                }
                else
                {
                    // Clear the information in the TextBoxes if there are no results
                    txtCustomerCode.Clear();
                    txtCustomerName.Clear();
                    txtPhoneNumber.Clear();
                    txtAddress.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while searching: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefreshSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
        }
    }
}
