using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATABASE.Config
{
    internal class DatabaseConnection
    {
        // Connection string - Thay đổi theo thông tin kết nối của bạn
        private readonly string _connectionString = @"Data Source=DESKTOP-DSGMDD9\SQLEXPRESS;Initial Catalog=store_management;Integrated Security=True";

        // Hàm mở kết nối
        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        // Hàm thực thi câu lệnh INSERT, UPDATE, DELETE
        public int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection connection = GetConnection())
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    connection.Open();
                    return command.ExecuteNonQuery(); // Trả về số hàng bị ảnh hưởng
                }
            }
        }

        // Hàm thực thi câu lệnh SELECT (trả về DataTable)
        public DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection connection = GetConnection())
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
        }

        // Hàm thực thi câu lệnh SELECT (trả về một giá trị duy nhất)
        public object ExecuteScalar(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection connection = GetConnection())
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    connection.Open();
                    return command.ExecuteScalar(); // Trả về giá trị đầu tiên của cột đầu tiên
                }
            }
        }
    }
}
