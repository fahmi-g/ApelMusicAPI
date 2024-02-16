using ApelMusicAPI.Models;
using MySql.Data.MySqlClient;

namespace ApelMusicAPI.Data
{
    public class PaymentMethodData
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;

        public PaymentMethodData(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        //GetAll
        public List<PaymentMethods> GetPaymentMethods()
        {
            List<PaymentMethods> paymentMethods = new List<PaymentMethods>();

            string query = "SELECT * FROM payment_methods " +
                "WHERE is_active = TRUE";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();

                        using (MySqlDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                PaymentMethods paymentMethod = new PaymentMethods
                                { 
                                    paymentId = Convert.ToInt32(dataReader["payment_id"]),
                                    paymentName = dataReader["payment_name"].ToString(),
                                    paymentImg = dataReader["payment_img"].ToString(),
                                    isActive = Convert.ToBoolean(dataReader["is_active"])
                                };

                                paymentMethods.Add(paymentMethod);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return paymentMethods;
        }

        //GetAll
        public List<PaymentMethods> GetAllPaymentMethod()
        {
            List<PaymentMethods> paymentMethods = new List<PaymentMethods>();

            string query = "SELECT * FROM payment_methods";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();

                        using (MySqlDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                PaymentMethods paymentMethod = new PaymentMethods
                                {
                                    paymentId = Convert.ToInt32(dataReader["payment_id"]),
                                    paymentName = dataReader["payment_name"].ToString(),
                                    paymentImg = dataReader["payment_img"].ToString(),
                                    isActive = Convert.ToBoolean(dataReader["is_active"])
                                };

                                paymentMethods.Add(paymentMethod);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return paymentMethods;
        }

        //GetByID
        public PaymentMethods? GetPaymentMethodById(int payment_id)
        {
            PaymentMethods? paymentMethodById = null;
            string query = $"SELECT * FROM payment_methods " +
                $"WHERE payment_id = @payment_id AND is_active = TRUE";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.Parameters.Clear();

                    command.CommandText = query;
                    command.Parameters.AddWithValue("@payment_id", payment_id);

                    try
                    {
                        connection.Open();

                        using (MySqlDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                PaymentMethods paymentMethod = new PaymentMethods
                                {
                                    paymentId = payment_id,
                                    paymentName = dataReader["payment_name"].ToString(),
                                    paymentImg = dataReader["payment_img"].ToString(),
                                    isActive = Convert.ToBoolean(dataReader["is_active"])
                                };

                                paymentMethodById = paymentMethod;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return paymentMethodById;
        }

        //GetByID
        public PaymentMethods? GetActiveInactivePaymentMethodById(int payment_id)
        {
            PaymentMethods? paymentMethodById = null;
            string query = $"SELECT * FROM payment_methods " +
                $"WHERE payment_id = @payment_id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.Parameters.Clear();

                    command.CommandText = query;
                    command.Parameters.AddWithValue("@payment_id", payment_id);

                    try
                    {
                        connection.Open();

                        using (MySqlDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                PaymentMethods paymentMethod = new PaymentMethods
                                {
                                    paymentId = payment_id,
                                    paymentName = dataReader["payment_name"].ToString(),
                                    paymentImg = dataReader["payment_img"].ToString(),
                                    isActive = Convert.ToBoolean(dataReader["is_active"])
                                };

                                paymentMethodById = paymentMethod;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return paymentMethodById;
        }

        //Insert/Post
        public bool InsertNewPaymentMethod(PaymentMethods newPaymentMethod)
        {
            bool result = false;

            string query = $"INSERT INTO payment_methods (payment_name, payment_img, is_active) " +
                $"VALUES (@payment_name, @payment_img, @is_active)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.Parameters.Clear();
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@payment_name", newPaymentMethod.paymentName);
                    command.Parameters.AddWithValue("@payment_img", newPaymentMethod.paymentImg);
                    command.Parameters.AddWithValue("@is_active", newPaymentMethod.isActive);

                    try
                    {
                        connection.Open();

                        result = command.ExecuteNonQuery() > 0 ? true : false;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return result;
        }

        //Update/Put
        public bool UpdatePaymentMethod(int payment_id, PaymentMethods paymentMethod)
        {
            bool result = false;

            string query = $"UPDATE payment_methods " +
                $"SET payment_name = @payment_name, payment_img = @payment_img, is_active = @is_active " +
                $"WHERE payment_id = @payment_id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.Parameters.Clear();
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@payment_id", payment_id);
                    command.Parameters.AddWithValue("@payment_name", paymentMethod.paymentName);
                    command.Parameters.AddWithValue("@payment_img", paymentMethod.paymentImg);
                    command.Parameters.AddWithValue("@is_active", paymentMethod.isActive);

                    try
                    {
                        connection.Open();

                        result = command.ExecuteNonQuery() > 0 ? true : false;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return result;
        }

        //Delete
        public bool DeletePaymentMethodById(int payment_id)
        {
            bool result = false;

            string query = "DELETE FROM payment_methods WHERE payment_id = @payment_id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.Parameters.Clear();
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@payment_id", payment_id);

                    try
                    {
                        connection.Open();

                        result = command.ExecuteNonQuery() > 0 ? true : false;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return result;
        }
    }
}
