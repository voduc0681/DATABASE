using DATABASE.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DATABASE.View
{
    public partial class Login : Form
    {
        private DatabaseConnection db = new DatabaseConnection();
        public Login()
        {
            InitializeComponent();

        }

        private void btnLogin_Click_1(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            // Check if username or password is empty
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Query to check login information

                string query = "SELECT * FROM [Employees] WHERE [UserName-Login] = @username AND [Password-Login] = @password";
                SqlParameter[] parameters = new SqlParameter[]
                {
            new SqlParameter("@username", username),
            new SqlParameter("@password", password) // In practice, passwords should be encrypted
                };

                DataTable result = db.ExecuteQuery(query, parameters);

                // If account is found
                if (result.Rows.Count > 0)
                {
                    // Get user information
                    string role = result.Rows[0]["RoleID"].ToString();
                    string fullName = result.Rows[0]["EmployeeName"].ToString();

                    MessageBox.Show($"Welcome, {fullName}!", "Login Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Switch to HomePage
                    this.Hide(); // Hide login form
                    HomePage homePage = new HomePage(role); // Pass role to manage permissions
                    homePage.ShowDialog();
                    this.Close(); // Close the form after HomePage is closed
                }
                else
                {
                    MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ckbPasswordVisible_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = true;
        }


        //private void ckbPasswordVisible_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (ckbPasswordVisible.Checked)
        //    {
        //        txtPassword.UseSystemPasswordChar = false;
        //    }
        //    else
        //    {
        //        txtPassword.UseSystemPasswordChar = true;
        //    }
        //}

        private void ckbPasswordVisible_CheckedChanged_1(object sender, EventArgs e)
        {
            if (ckbPasswordVisible.Checked)
            {
                txtPassword.UseSystemPasswordChar = false;
            }
            else
            {
                txtPassword.UseSystemPasswordChar = true;
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
