using MySql.Data.MySqlClient;
using ApelMusicAPI.DTOs;

namespace ApelMusicAPI.Data
{
    public class OrderData
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;

        public OrderData(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        //GetAall by user_id
        public List<OrderResponseDTO> GetOrders(Guid user_id)
        {
            List<OrderResponseDTO> orders = new List<OrderResponseDTO>();
            string query = $"SELECT o.invoice_no, o.order_date, COUNT(od.order_detail_id) AS 'total_classes', SUM(c.class_price) AS 'total_price' FROM orders o " +
                $"JOIN order_detail od ON o.invoice_no = od.invoice_no " +
                $"JOIN user_classes uc ON od.user_class_id = uc.user_class_id " +
                $"JOIN class c ON uc.class_id = c.class_id " +
                $"where o.order_by = @user_id " +
                $"GROUP BY o.invoice_no";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    try
                    {
                        connection.Open();

                        command.Connection = connection;
                        command.Parameters.Clear();

                        command.CommandText = query;
                        command.Parameters.AddWithValue("@user_id", user_id);
                        using (MySqlDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                OrderResponseDTO order = new OrderResponseDTO
                                {
                                    invoiceNo = Convert.ToString(dataReader["invoice_no"]),
                                    orderDate = Convert.ToDateTime(dataReader["order_date"]),
                                    totalClasses = Convert.ToInt32(dataReader["total_classes"]),
                                    totalPrice = Convert.ToInt32(dataReader["total_price"])
                                };

                                orders.Add(order);
                            }
                        }
                    }
                    catch (MySqlException)
                    {
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

                return orders;
        }

        //GetAall
        public List<OrderResponseDTO> GetAllOrders()
        {
            List<OrderResponseDTO> orders = new List<OrderResponseDTO>();
            string query = $"SELECT o.invoice_no, o.order_date, COUNT(od.order_detail_id) AS 'total_classes', SUM(c.class_price) AS 'total_price' FROM orders o " +
                $"JOIN order_detail od ON o.invoice_no = od.invoice_no " +
                $"JOIN user_classes uc ON od.user_class_id = uc.user_class_id " +
                $"JOIN class c ON uc.class_id = c.class_id " +
                $"GROUP BY o.invoice_no";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    try
                    {
                        connection.Open();

                        command.Connection = connection;
                        command.Parameters.Clear();

                        command.CommandText = query;
                        using (MySqlDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                OrderResponseDTO order = new OrderResponseDTO
                                {
                                    invoiceNo = Convert.ToString(dataReader["invoice_no"]),
                                    orderDate = Convert.ToDateTime(dataReader["order_date"]),
                                    totalClasses = Convert.ToInt32(dataReader["total_classes"]),
                                    totalPrice = Convert.ToInt32(dataReader["total_price"])
                                };

                                orders.Add(order);
                            }
                        }
                    }
                    catch (MySqlException)
                    {
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return orders;
        }
    }
}
