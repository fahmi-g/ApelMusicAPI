using ApelMusicAPI.DTOs;
using ApelMusicAPI.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace ApelMusicAPI.Data
{
    public class CheckoutStateData
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;

        public CheckoutStateData(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public bool InsertToUserClass(UserClasses userClass)
        {
            bool result = false;

            string query = $"INSERT INTO user_classes (user_id, class_id, class_schedule, is_paid) " +
                $"VALUES (@user_id, @class_id, @class_schedule, FALSE)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                { 
                    command.Connection = connection;
                    command.Parameters.Clear();
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@user_id", userClass.userId.ToString());
                    command.Parameters.AddWithValue("@class_id", userClass.classId);
                    command.Parameters.AddWithValue("@class_schedule", userClass.classSchedule.ToString("yyyy-MM-dd HH:mm:ss"));

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

        public bool AddToOrders(Orders orders)
        {
            bool result = false;

            string query = $"INSERT INTO orders (order_id, invoice_no, order_by, payment_method) " +
                $"VALUES (@order_id, @invoice_no, @order_by, @payment_method)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.Parameters.Clear();
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@order_id", orders.orderId.ToString());
                    command.Parameters.AddWithValue("@invoice_no", orders.invoiceNo.ToString());
                    command.Parameters.AddWithValue("@order_by", orders.orderBy.ToString());
                    command.Parameters.AddWithValue("@payment_method", orders.paymentMethod.ToString());

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

        public bool AddToOrderDetail(OrderDetail orderDetail)
        {
            bool result = false;

            string query = $"INSERT INTO order_detail (invoice_no, class_id) " +
                $"VALUES (@invoice_no, @class_id)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.Parameters.Clear();
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@invoice_no", orderDetail.invoiceNo);
                    command.Parameters.AddWithValue("@class_id", orderDetail.classId);

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

        /*public bool CheckoutTransaction(Orders orders, OrderDetail orderDetail)
        {
            bool result = false;

            string queryOrder = $"INSERT INTO orders (order_id, invoice_no, order_by, payment_method) " +
                $"VALUES (@order_id, @invoice_no, @order_by, @payment_method)";
            string queryOrderDetail = $"INSERT INTO order_detail (invoice_no, class_id) " +
                $"VALUES (@invoice_no, @class_id)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlTransaction transaction = connection.BeginTransaction();

                MySqlCommand commandOrder = new MySqlCommand();
                commandOrder.Connection = connection;
                commandOrder.Transaction = transaction;
                commandOrder.Parameters.Clear();
                commandOrder.CommandText = queryOrder;

                commandOrder.Parameters.AddWithValue("@order_id", orders.orderId.ToString());
                commandOrder.Parameters.AddWithValue("@invoice_no", orders.invoiceNo.ToString());
                commandOrder.Parameters.AddWithValue("@order_by", orders.orderBy.ToString());
                commandOrder.Parameters.AddWithValue("@payment_method", orders.paymentMethod.ToString());

                MySqlCommand commandOrderDetail = new MySqlCommand();
                commandOrderDetail.Connection = connection;
                commandOrderDetail.Transaction = transaction;
                commandOrderDetail.Parameters.Clear();
                commandOrderDetail.CommandText = queryOrderDetail;

                commandOrderDetail.Parameters.AddWithValue("@invoice_no", orderDetail.invoiceNo);
                commandOrderDetail.Parameters.AddWithValue("@class_id", orderDetail.classId);

                try
                {
                    result = (commandOrder.ExecuteNonQuery() > 0 && commandOrderDetail.ExecuteNonQuery() > 0) ? true : false;
                    
                    transaction.Commit();
                }
                catch(Exception)
                {
                    transaction.Rollback();
                }
                finally
                {
                    if(transaction != null) transaction.Dispose();
                    if(commandOrder != null) commandOrder.Dispose();
                    if(commandOrderDetail != null) commandOrderDetail.Dispose();
                    connection.Close();
                }
            }

            return result;
        }*/
    }
}
