using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DATABASE.View
{
    public partial class Statistic : Form
    {
        SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-DSGMDD9\SQLEXPRESS;Initial Catalog=store_management;Integrated Security=True");
        public Statistic()
        {
            InitializeComponent();
        }

        private void Statistic_Load(object sender, EventArgs e)
        {
            connection.Open();
            FillData();
        }

        public void FillData()
        {
            string query = @"
                            SELECT 
	                            Orders.OrderCode,
	                            Orders.OrderDate,
	                            Products.SellingPrice * Orders.Quantity AS TotalPrice
                            FROM
	                            Orders
                            JOIN
	                            Products ON Orders.ProductID = Products.ProductID";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dgvStatistic.DataSource = dt;
            connection.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           if (textBox1.Text == null)
           {
                return;
           }
           connection.Open();
           SearchCode(textBox1.Text);
        }

        public void SearchCode(string code)
        {
            string query = @"
                            SELECT 
	                            Orders.OrderCode,
	                            Orders.OrderDate,
	                            Products.SellingPrice * Orders.Quantity AS TotalPrice
                            FROM
	                            Orders
                            JOIN
	                            Products ON Orders.ProductID = Products.ProductID
                            WHERE
                                Orders.OrderCode LIKE @Code";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            try
            {
                adapter.SelectCommand.Parameters.AddWithValue("@Code", "%"+code+"%");
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvStatistic.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
            finally
            {
                connection.Close();
            }        
        }

        private void SearchDate(DateTime start, DateTime end)
        {
            string query = @"
                            SELECT 
                                Orders.OrderCode,
                                Orders.OrderDate,
                                Products.SellingPrice * Orders.Quantity AS TotalPrice
                            FROM
                                Orders
                            JOIN
                                Products ON Orders.ProductID = Products.ProductID
                            WHERE
                                Orders.OrderDate >= @start AND Orders.OrderDate <= @end;";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            try
            {
                //if ()
                //{
                    adapter.SelectCommand.Parameters.AddWithValue("@start", start);
                    adapter.SelectCommand.Parameters.AddWithValue("@end", end );
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvStatistic.DataSource = dt;
                //}
                //else
                //{

                //}
            }
            catch (Exception ex)
            {
               // MessageBox.Show("Error: " + ex);
            }
            finally
            {
                connection.Close( );
            }

        }

        private void dtmStart_ValueChanged(object sender, EventArgs e)
        {
            SearchDate(dtmStart.Value, dtmEnd.Value);
        }

        private void dtmEnd_ValueChanged(object sender, EventArgs e)
        {
            SearchDate(dtmStart.Value, dtmEnd.Value);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            connection.Close();
            FillData();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
