using DATABASE.Config;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DATABASE.View
{
    public partial class OrderForm : Form
    {
        private DatabaseConnection db = new DatabaseConnection();
        SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-DSGMDD9\SQLEXPRESS;Initial Catalog=store_management;Integrated Security=True");

        public OrderForm()
        {
            InitializeComponent();
            dgvOrder.AutoGenerateColumns = true;


        }
        private void txtCustomerID_TextChanged(object sender, EventArgs e)
        {
            LoadCustomerName(txtCustomerID.Text);
        }
        private void LoadCustomerName(string customerCode)
        {
            try
            {
                if (string.IsNullOrEmpty(customerCode))
                {
                    txtCustomerName.Text = ""; // Delete customer name without entering CustomerCode
                    return;
                }

                // Query to get CustomerName

                string query = "SELECT CustomerName FROM Customers WHERE CustomerID = @code";

                using (SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-DSGMDD9\SQLEXPRESS;Initial Catalog=store_management;Integrated Security=True"))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn)) // Make sure to release the SqlCommand
                    {
                        // Use Parameters.Add and specify the data type
                        cmd.Parameters.AddWithValue("@code", txtCustomerID.Text);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())  // If product found
                            {
                                txtCustomerName.Text = reader["CustomerName"].ToString();  // Show product name

                            }
                            else
                            {
                                txtCustomerName.Text = "Customer not found";  //    Notification when product not found

                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading customer name: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProductInfo(string productID)
        {
            try
            {
                if (string.IsNullOrEmpty(productID))
                {
                    txtProductName.Text = "";  //Delete product name without entering ProductID
                    txtProductPrice.Text = ""; //Remove product price without ProductID
                    return;
                }

                // Query to get product information (ProductName and SellingPrice)
                string query = "SELECT ProductName, SellingPrice FROM Products WHERE ProductID = @id";
                using (SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-DSGMDD9\SQLEXPRESS;Initial Catalog=store_management;Integrated Security=True"))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", productID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())  // If product found
                        {
                            txtProductName.Text = reader["ProductName"].ToString();  //Show product name
                            txtProductPrice.Text = reader["SellingPrice"].ToString();  //Show product price
                        }
                        else
                        {
                            txtProductName.Text = "Product not found";  //Notification when product not found
                            txtProductPrice.Text = "0.00";  // If no price found, default price to 0.00
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading product info: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void txtProductID_TextChanged(object sender, EventArgs e)
        {
            LoadProductInfo(txtProductID.Text);
        }

        private void GenerateOrderCode()
        {
            try
            {
                // Query the current largest order code
                string query = "SELECT TOP 1 OrderID FROM Orders ORDER BY OrderID DESC";
                using (SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-DSGMDD9\SQLEXPRESS;Initial Catalog=store_management;Integrated Security=True"))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        // Current order code, convert it to integer
                        int lastCode = Convert.ToInt32(result);
                        int nextCode = lastCode + 1; // Increase number by 1

                        // Assign new order code
                        txtOrderCode.Text = nextCode.ToString(); // Show new code as integer
                    }
                    else
                    {
                        // If there is no code in the table, initialize the first code to 1
                        txtOrderCode.Text = "1";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating order code: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OrderForm_Load_1(object sender, EventArgs e)
        {
            GenerateOrderCode();
            dgvOrder.Columns.Clear();

            /*dgvOrder.Columns.Add("OrderCode", "Order Code");
            dgvOrder.Columns.Add("OrderDate", "Order Date");
            dgvOrder.Columns.Add("Quantity", "Quantity");
            dgvOrder.Columns.Add("TotalAmount", "Total Amount");
            dgvOrder.Columns.Add("CustomerID", "Customer ID");
            dgvOrder.Columns.Add("CustomerName", "Customer Name");
            dgvOrder.Columns.Add("ProductID", "Product ID");
            dgvOrder.Columns.Add("ProductName", "Product Name");
            dgvOrder.Columns.Add("ProductPrice", "Product Price");

            // */
            LoadOrderData();
        }

        private void dgvOrder_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void txtTotalAmount_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalAmount();
        }

        private void CalculateTotalAmount()
        {
            try
            {
                // Check if the product quantity and price are valid
                if (decimal.TryParse(txtQuantity.Text, out decimal quantity) && decimal.TryParse(txtProductPrice.Text, out decimal price))
                {
                    // Tính tổng tiền
                    decimal totalAmount = quantity * price;

                    // Display total amount in txtTotalAmount cell
                    txtTotalAmount.Text = totalAmount.ToString("F2");  //Display with 2 decimal places
                }
                else
                {
                    // If the data is invalid, set the total value to 0
                    txtTotalAmount.Text = "0.00";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating total amount: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadOrderData()
        {
            try
            {
                // Query to get all orders from database
                string query = @"
                                SELECT 
                                    Orders.OrderCode, 
                                    Orders.OrderDate, 
                                    Orders.Quantity, 
                                    Orders.CustomerID, 
                                    Customers.CustomerName, 
                                    Orders.ProductID,
                                    Products.ProductName, 
                                    Products.SellingPrice,
                                    Products.SellingPrice * Orders.Quantity AS TotalAmount
                                FROM 
                                    Orders
                                JOIN
                                    Customers ON Customers.CustomerID = Orders.CustomerID
                                JOIN
                                    Products ON Products.ProductID = Orders.ProductID";

                using (SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-DSGMDD9\SQLEXPRESS;Initial Catalog=store_management;Integrated Security=True"))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);

                    // Use SqlDataAdapter to get data from database into DataTable
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable); // Fill data into DataTable

                    // Assign DataTable to DataGridView
                    dgvOrder.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading order data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteOrder_Click(object sender, EventArgs e)
        {
            if (dgvOrder.CurrentRow == null)
            {
                MessageBox.Show("Please select an order to delete!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string orderCode = dgvOrder.CurrentRow.Cells["OrderCode"].Value.ToString();

            // Confirm before deleting
            DialogResult dialogResult = MessageBox.Show($"Are you sure you want to delete the order with code {orderCode}?",
                                                        "Confirm Deletion",
                                                        MessageBoxButtons.YesNo,
                                                        MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    // Execute the DELETE statement to delete the order.
                    string query = "DELETE FROM Orders WHERE OrderCode = @OrderCode";
                    using (SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-DSGMDD9\SQLEXPRESS;Initial Catalog=store_management;Integrated Security=True"))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@OrderCode", orderCode);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Order deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadOrderData(); // Update display data
                        }
                        else
                        {
                            MessageBox.Show("Order deletion failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting order: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btnAddOrder_Click(object sender, EventArgs e)
        {
            try
            {
                //Check input data
                if (string.IsNullOrEmpty(txtOrderCode.Text) ||
                    string.IsNullOrEmpty(txtQuantity.Text) ||
                    string.IsNullOrEmpty(txtCustomerID.Text) ||
                    string.IsNullOrEmpty(txtProductID.Text) ||
                    string.IsNullOrEmpty(txtProductPrice.Text))
                {
                    MessageBox.Show("All fields must be filled!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txtQuantity.Text, out decimal quantity) || quantity <= 0)
                {
                    MessageBox.Show("Invalid quantity. Please enter a positive number.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txtProductPrice.Text, out decimal productPrice) || productPrice <= 0)
                {
                    MessageBox.Show("Invalid product price. Please enter a valid price.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string orderCode = txtOrderCode.Text;
                DateTime orderDate = DateTime.Now; 
                decimal totalAmount = quantity * productPrice;
                string customerID = txtCustomerID.Text;
                string customerName = txtCustomerName.Text;
                string productID = txtProductID.Text;
                string productName = txtProductName.Text;
                MessageBox.Show("Order added successfully" + customerID);
                string query = @"
                                INSERT INTO Orders (OrderCode, OrderDate, Quantity, ProductID, CustomerID)
VALUES (@OrderCode, @OrderDate, @Quantity, @ProductID,  @CustomerID);
";
                using (SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-DSGMDD9\SQLEXPRESS;Initial Catalog=store_management;Integrated Security=True"))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@OrderCode", orderCode);
                        cmd.Parameters.AddWithValue("@OrderDate", orderDate);
                        cmd.Parameters.AddWithValue("@Quantity", quantity);
                        cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                        cmd.Parameters.AddWithValue("@CustomerID", customerID);
                        cmd.Parameters.AddWithValue("@CustomerName", customerName);
                        cmd.Parameters.AddWithValue("@ProductID", productID);
                        cmd.Parameters.AddWithValue("@ProductName", productName);
                        cmd.Parameters.AddWithValue("@ProductPrice", productPrice);

                        cmd.ExecuteNonQuery();
                    }
                }

                // Update data and clean form
                LoadOrderData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding order: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvOrder_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvOrder.Rows[e.RowIndex].Cells.Count > 0)
            {
                DataGridViewRow selectedRow = dgvOrder.Rows[e.RowIndex];

                txtOrderCode.Text = selectedRow.Cells["OrderCode"].Value?.ToString() ?? "";
                dateTimePicker1.Value = DateTime.TryParse(selectedRow.Cells["OrderDate"].Value?.ToString(), out DateTime orderDate) ? orderDate : DateTime.Now;
                txtQuantity.Text = selectedRow.Cells["Quantity"].Value?.ToString() ?? "0";
                txtCustomerID.Text = selectedRow.Cells["CustomerID"].Value?.ToString() ?? "";
                txtCustomerName.Text = selectedRow.Cells["CustomerName"].Value?.ToString() ?? "";
                txtProductID.Text = selectedRow.Cells["ProductID"].Value?.ToString() ?? "";
                txtProductName.Text = selectedRow.Cells["ProductName"].Value?.ToString() ?? "";
                txtProductPrice.Text = selectedRow.Cells["SellingPrice"].Value?.ToString() ?? "0.00";
                txtTotalAmount.Text = selectedRow.Cells["TotalAmount"].Value?.ToString() ?? "0.00";
            }
        }

        private void btnUpdateOrder_Click(object sender, EventArgs e)
        {
            try
            {
                // Check input data
                if (string.IsNullOrEmpty(txtOrderCode.Text) ||
                    string.IsNullOrEmpty(txtQuantity.Text) ||
                    string.IsNullOrEmpty(txtCustomerID.Text) ||
                    string.IsNullOrEmpty(txtProductID.Text) ||
                    string.IsNullOrEmpty(txtProductPrice.Text))
                {
                    MessageBox.Show("All fields must be filled!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txtQuantity.Text, out decimal quantity) || quantity <= 0)
                {
                    MessageBox.Show("Invalid quantity. Please enter a positive number.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txtProductPrice.Text, out decimal productPrice) || productPrice <= 0)
                {
                    MessageBox.Show("Invalid product price. Please enter a valid price.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string orderCode = txtOrderCode.Text;
                DateTime orderDate = dateTimePicker1.Value;
                decimal totalAmount = quantity * productPrice;
                string customerID = txtCustomerID.Text;
                string productID = txtProductID.Text;

                using (SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-DSGMDD9\SQLEXPRESS;Initial Catalog=store_management;Integrated Security=True"))
                {
                    conn.Open();

                    // Check if OrderCode exists
                    string checkQuery = "SELECT COUNT(1) FROM Orders WHERE OrderCode = @OrderCode";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@OrderCode", orderCode);

                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count > 0)
                    {
                        //If OrderCode exists, execute UPDATE
                        string updateQuery = @"
                    UPDATE Orders
                    SET OrderDate = @OrderDate, 
                        Quantity = @Quantity, 
                        ProductID = @ProductID, 
                        CustomerID = @CustomerID
                    WHERE OrderCode = @OrderCode";

                        SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                        updateCmd.Parameters.AddWithValue("@OrderCode", orderCode);
                        updateCmd.Parameters.AddWithValue("@OrderDate", orderDate);
                        updateCmd.Parameters.AddWithValue("@Quantity", quantity);
                        updateCmd.Parameters.AddWithValue("@ProductID", productID);
                        updateCmd.Parameters.AddWithValue("@CustomerID", customerID);

                        updateCmd.ExecuteNonQuery();

                        MessageBox.Show("Order updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadOrderData(); // Reload data
                    }
                    else
                    {
                        // If not present, error message
                        MessageBox.Show("Cannot update. OrderCode does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating order: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtCustomerID.Clear();
            txtCustomerName.Clear();
            txtProductID.Clear();
            txtProductName.Clear();
            txtProductPrice.Clear();
            txtOrderCode.Clear();
            txtTotalAmount.Clear();
            txtQuantity.Clear();

        }
    }
}


