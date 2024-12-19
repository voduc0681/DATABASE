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
    public partial class ProductForm : Form
    {
        private DatabaseConnection db = new DatabaseConnection();
        public ProductForm()
        {
            InitializeComponent();
            dgvProduct.CellClick += dgvProduct_CellContentClick;
        }
        private void ProductForm_Load(object sender, EventArgs e)
        {
            LoadProductData();
            txtQuantityAll.Text = GetTotalQuantity().ToString();

            if (dgvProduct.Rows.Count > 0)
            {
                handleSelectRowOnTable(0);
            }
        }
        private void LoadProductData()
        {
            try
            {
                // SQL query to get product data
                string query = "SELECT ProductCode, ProductName, SellingPrice, InventoryQuantity, ImportPrice FROM Products";
                DataTable dt = db.ExecuteQuery(query);

                // Set data into DataGridView
                dgvProduct.DataSource = dt;
                btnAddProduct.Enabled = true;
                btnUpdateProduct.Enabled = true;
                btnDeleteProduct.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading product data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                handleSelectRowOnTable(e.RowIndex);
            }
        }
        private void handleSelectRowOnTable(int index)
        {
            DataGridViewRow row = dgvProduct.Rows[index];
            txtProductCode.Text = row.Cells["ProductCode"].Value.ToString();
            txtProductName.Text = row.Cells["ProductName"].Value.ToString();
            txtSellingPrice.Text = row.Cells["SellingPrice"].Value.ToString();
            txtInventoryQuantity.Text = row.Cells["InventoryQuantity"].Value.ToString();
            txtImportPrice.Text = row.Cells["ImportPrice"].Value.ToString();
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            // Get data from TextBoxes
            string productCode = txtProductCode.Text;
            string productName = txtProductName.Text;
            decimal sellingPrice = 0;
            int inventoryQuantity = 0;
            decimal importPrice = 0;

            // Check the validity of input data
            if (string.IsNullOrEmpty(productCode) || string.IsNullOrEmpty(productName) ||
                !decimal.TryParse(txtSellingPrice.Text, out sellingPrice) ||
                !int.TryParse(txtInventoryQuantity.Text, out inventoryQuantity) ||
                !decimal.TryParse(txtImportPrice.Text, out importPrice))
            {
                MessageBox.Show("Please enter complete and correct product information!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; //Stop if data is invalid
            }

            // Check if ProductCode already exists
            string checkQuery = "SELECT COUNT(*) FROM Products WHERE ProductCode = @ProductCode";
            SqlParameter[] checkParameters = new SqlParameter[]
            {
               new SqlParameter("@ProductCode", SqlDbType.VarChar) { Value = productCode }
            };

            try
            {
                // Perform existence check
                int existingCount = (int)db.ExecuteScalar(checkQuery, checkParameters);

                if (existingCount > 0)
                {
                    MessageBox.Show("Product code already exists. Please check the product information or use a different code.",
                                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Stop if product code already exists
                }

                // If the product code does not exist, add a new one.
                string query = "INSERT INTO Products (ProductCode, ProductName, SellingPrice, InventoryQuantity, ImportPrice) " +
                               "VALUES (@ProductCode, @ProductName, @SellingPrice, @InventoryQuantity, @ImportPrice)";

                // Create parameters for SQL statements
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@ProductCode", SqlDbType.VarChar) { Value = productCode },
                    new SqlParameter("@ProductName", SqlDbType.NVarChar) { Value = productName },
                    new SqlParameter("@SellingPrice", SqlDbType.Decimal) { Value = sellingPrice },
                    new SqlParameter("@InventoryQuantity", SqlDbType.Int) { Value = inventoryQuantity },
                    new SqlParameter("@ImportPrice", SqlDbType.Decimal) { Value = importPrice }
                };

                // Execute SQL statement
                int result = db.ExecuteNonQuery(query, parameters);

                if (result > 0) // If at least 1 row is affected
                {
                    MessageBox.Show("Product added successfully!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Update DataGridView after adding product
                    LoadProductData();
                    txtQuantityAll.Text = GetTotalQuantity().ToString();
                }
                else
                {
                    MessageBox.Show("Cannot add product.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while adding the product: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnUpdateProduct_Click(object sender, EventArgs e)
        {
            // Get data from TextBoxes
            string productCode = txtProductCode.Text;
            string productName = txtProductName.Text;
            decimal sellingPrice = 0;
            int inventoryQuantity = 0;
            decimal importPrice = 0;

            // Check the validity of input data
            if (string.IsNullOrEmpty(productCode) || string.IsNullOrEmpty(productName) ||
                !decimal.TryParse(txtSellingPrice.Text, out sellingPrice) ||
                !int.TryParse(txtInventoryQuantity.Text, out inventoryQuantity) ||
                !decimal.TryParse(txtImportPrice.Text, out importPrice))
            {
                MessageBox.Show("Please enter complete and correct product information.!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;  // Stop if data is invalid
            }

            // Create SQL statement to update products into database
            string query = "UPDATE Products SET ProductName = @ProductName, SellingPrice = @SellingPrice, " +
                           "InventoryQuantity = @InventoryQuantity, ImportPrice = @ImportPrice WHERE ProductCode = @ProductCode";

            // Create parameters for SQL statements
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ProductCode", SqlDbType.VarChar) { Value = productCode },
                new SqlParameter("@ProductName", SqlDbType.NVarChar) { Value = productName },
                new SqlParameter("@SellingPrice", SqlDbType.Decimal) { Value = sellingPrice },
                new SqlParameter("@InventoryQuantity", SqlDbType.Int) { Value = inventoryQuantity },
                new SqlParameter("@ImportPrice", SqlDbType.Decimal) { Value = importPrice }
            };

            // Execute SQL statement
            try
            {
                // Call the ExecuteNonQuery method to execute the SQL statement
                int result = db.ExecuteNonQuery(query, parameters);

                if (result > 0) // If at least 1 row is affected
                {
                    MessageBox.Show("Product update successful!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Update DataGridView after product update
                    LoadProductData();
                    txtQuantityAll.Text = GetTotalQuantity().ToString();
                }
                else
                {
                    MessageBox.Show("Unable to update product. Check information again.!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updating the product.: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtProductCode.Enabled = true;
            txtProductCode.Clear();
            txtProductName.Clear();
            txtSellingPrice.Clear();
            txtInventoryQuantity.Clear();
            txtImportPrice.Clear();

            // If you want to reset the data in the DataGridView (or if necessary)
            LoadProductData();
            btnAddProduct.Enabled = true;
            btnUpdateProduct.Enabled = true;
            btnDeleteProduct.Enabled = true;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Get product code from TextBox
            string productCode = txtProductCode.Text;

            // Check if product code is empty
            if (string.IsNullOrEmpty(productCode))
            {
                MessageBox.Show("Please select a product to delete!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Show confirmation dialog before deleting
            DialogResult dialogResult = MessageBox.Show($"Are you sure you want to delete the product with the code {productCode}?",
                                                        "Confirm deletion",
                                                        MessageBoxButtons.YesNo,
                                                        MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                // Create SQL statement to delete product from database
                string query = "DELETE FROM Products WHERE ProductCode = @ProductCode";

                // Create parameters for SQL statements
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@ProductCode", SqlDbType.VarChar) { Value = productCode }
                };

                // Execute SQL statement
                try
                {
                    // Call the ExecuteNonQuery method to execute the SQL statement
                    int result = db.ExecuteNonQuery(query, parameters);

                    if (result > 0) // If at least 1 row is affected
                    {
                        MessageBox.Show("Product has been deleted successfully!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Update DataGridView after deleting product
                        LoadProductData();
                        txtQuantityAll.Text = GetTotalQuantity().ToString();

                        // Delete information in TextBoxes after deleting products
                        txtProductCode.Clear();
                        txtProductName.Clear();
                        txtSellingPrice.Clear();
                        txtInventoryQuantity.Clear();
                        txtImportPrice.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Cannot delete product. Please check product code again.!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while deleting the product.: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim(); // Get search keyword from TextBox and remove extra spaces
            string searchQuery = ""; // SQL search statement
            string searchColumn = "";

            // Check which radio button is selected and select the search field
            if (radioProductCode.Checked)
            {
                searchColumn = "ProductCode";
            }
            else if (radioProductName.Checked)
            {
                searchColumn = "ProductName";
            }
            else
            {
                return; // Do not search if no radio button is selected
            }

            // If there is no search keyword, do not execute the query.
            if (string.IsNullOrEmpty(searchTerm))
            {
                LoadProductData(); // Refresh all data if no search keyword is found
                if (dgvProduct.Rows.Count > 0)
                {
                    handleSelectRowOnTable(0);
                }
                return;
            }

            // Create SQL statement to search
            searchQuery = $"SELECT ProductCode, ProductName, SellingPrice, InventoryQuantity, ImportPrice FROM Products " +
                          $"WHERE {searchColumn} COLLATE Latin1_General_CI_AI LIKE @SearchTerm";

            // Create parameters for SQL statements
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@SearchTerm", SqlDbType.NVarChar) { Value = "%" + searchTerm + "%" }
            };

            // Execute query and update DataGridView
            try
            {
                // Call the ExecuteQuery method to get the search results.
                DataTable dt = db.ExecuteQuery(searchQuery, parameters);
                Console.WriteLine("do1");
                // Update DataGridView with search results
                dgvProduct.DataSource = dt;
                Console.WriteLine(dgvProduct.Rows.Count);
                if (dgvProduct.Rows.Count > 0)
                {
                    Console.WriteLine("do");
                    handleSelectRowOnTable(0);
                }
                else
                {
                    // Clear information in TextBoxes if no results
                    txtProductCode.Clear();
                    txtProductName.Clear();
                    txtSellingPrice.Clear();
                    txtInventoryQuantity.Clear();
                    txtImportPrice.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while searching.: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefreshSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
        }
        private int GetTotalQuantity()
        {
            int totalQuantity = 0;

            try
            {
                // Create SQL statement to calculate total quantity of products
                string query = "SELECT SUM(InventoryQuantity) FROM Products";

                // Execute SQL statement and get result
                object result = db.ExecuteScalar(query);

                if (result != DBNull.Value)
                {
                    totalQuantity = Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating total quantity: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return totalQuantity;
        }
    }
}
