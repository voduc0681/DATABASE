using DATABASE.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DATABASE.View
{
    public partial class HomePage : Form
    {
        private string _userRole;
        public HomePage(string userRole)
        {
            InitializeComponent();
            _userRole = userRole;
        }

        private void HomePage_Load(object sender, EventArgs e)
        {
            // Display role information
            /* lblRole.Text = $"Role: {_userRole}";*/

            // Configure UI based on role
            ConfigureUIBasedOnRole();
        }

        private void ConfigureUIBasedOnRole()
        {
            // Check user role and show/hide appropriate functions
            switch (_userRole)
            {
                case "1":
                    // Admin has full rights, show all functions
                    btnM_Product.Visible = true;
                    btnM_Employee.Visible = true;
                    btnM_Customer.Visible = true;
                    btnM_Order.Visible = true;
                    btnM_Import.Visible = true;
                    btn_Statistic.Visible = true;
                    break;

                case "2":
                    // Sales has rights to manage only products, customers, and orders
                    btnM_Product.Visible = true;
                    btnM_Customer.Visible = true;
                    btnM_Order.Visible = true;

                    // Hide functions not related to Sales
                    btnM_Employee.Visible = false;  // Hide employee management function
                    btnM_Import.Visible = false;
                    btn_Statistic.Visible = false;
                    // Hide import goods function
                    break;

                case "3":
                    // Warehouse has rights to manage only imports and product stock
                    btnM_Import.Visible = true;
                    btnM_Product.Visible = true;

                    // Hide functions not related to Warehouse
                    btnM_Employee.Visible = false;  // Hide employee management function
                    btnM_Customer.Visible = false;  // Hide customer management function
                    btnM_Order.Visible = false;     // Hide order management function
                    btn_Statistic.Visible = false;  // Hide statistics function
                    break;

                default:
                    // If an invalid role is detected
                    MessageBox.Show("Unknown role detected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
            }
        }


        private void btnM_Product_Click(object sender, EventArgs e)
        {
            ProductForm productForm = new ProductForm();
            productForm.ShowDialog();
        }

        private void btnM_Employee_Click(object sender, EventArgs e)
        {
            EmployeeForm employeeForm = new EmployeeForm();
            employeeForm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            this.Hide();
            login.ShowDialog();
            this.Close();
        }

        private void btnM_Import_Click(object sender, EventArgs e)
        {

        }

        private void btnM_Customer_Click(object sender, EventArgs e)
        {
            CustomerFrom customerForm = new CustomerFrom();
            customerForm.ShowDialog();
        }

        private void btnM_Order_Click(object sender, EventArgs e)
        {
            OrderForm orderForm = new OrderForm();
            orderForm.ShowDialog();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btn_Statistic_Click(object sender, EventArgs e)
        {
            Statistic statistic = new Statistic();
            statistic.ShowDialog();
        }
    }
}
