using MySql.Data.MySqlClient;
using ApelMusicAPI.DTOs;

namespace ApelMusicAPI.Data
{
    public class OrderDetailData
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;

        public OrderDetailData(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        //GetAll by invoice_no
        public List<OrderDetailResponseDTO> GetOrderDetails(string invoice_no)
        {
            List<OrderDetailResponseDTO> orderDetails = new List<OrderDetailResponseDTO>();
            string query = $"SELECT c.class_name, cc.category_name, uc.class_schedule, c.class_price FROM orders o " +
                $"JOIN order_detail od ON o.invoice_no = od.invoice_no " +
                $"JOIN user_classes uc ON od.user_class_id = uc.user_class_id " +
                $"JOIN class c ON uc.class_id = c.class_id " +
                $"JOIN class_category cc ON c.class_category = cc.category_id " +
                $"WHERE o.invoice_no = @invoice_no";

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
                        command.Parameters.AddWithValue("@invoice_no", invoice_no);
                        using (MySqlDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                OrderDetailResponseDTO orderDetail = new OrderDetailResponseDTO
                                {
                                    className = Convert.ToString(dataReader["class_name"]),
                                    category = Convert.ToString(dataReader["category_name"]),
                                    schedule = Convert.ToDateTime(dataReader["class_schedule"]),
                                    classPrice = Convert.ToInt32(dataReader["class_price"])
                                };

                                orderDetails.Add(orderDetail);
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

            return orderDetails;
        }

    }
}
