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
                    command.Parameters.AddWithValue("@class_schedule", userClass.classSchedule.ToString("yyyy-MM-dd"));

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

        public bool CheckoutTransaction(Orders order, int[] selectedClasses)
        {
            bool createOrderResult = false;
            bool createOrderDetailResult = false;
            bool confirmPaymentResult = false;
            bool totalPriceResult = false;

            string queryOrder = $"INSERT INTO orders (order_id, invoice_no, order_by, payment_method) " +
                $"VALUES (@order_id, @invoice_no, @order_by, @payment_method)";
            string queryOrderDetail = $"INSERT INTO order_detail (invoice_no, user_class_id) " +
                $"VALUES (@invoice_no, @user_class_id)";
            string queryConfirmPayment = $"UPDATE user_classes SET is_paid = TRUE " +
                $"WHERE user_class_id = @user_class_id";
            string queryOrderTotalPrice = $"UPDATE orders " +
                $"SET total_price = (" +
                $"SELECT SUM(c.class_price) FROM orders o " +
                $"JOIN order_detail od ON o.invoice_no = od.invoice_no " +
                $"JOIN user_classes uc ON od.user_class_id = uc.user_class_id " +
                $"JOIN class c ON uc.class_id = c.class_id " +
                $"WHERE o.order_by = @user_id " +
                $"GROUP BY o.invoice_no " +
                $"HAVING o.invoice_no = @invoice_no" +
                $") " +
                $"WHERE invoice_no = @invoice_no";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    MySqlTransaction transaction = null;

                    try
                    {
                        connection.Open();

                        transaction = connection.BeginTransaction();
                        command.Connection = connection;
                        command.Transaction = transaction;

                        command.Parameters.Clear();
                        command.CommandText = queryOrder;
                        command.Parameters.AddWithValue("@order_id", order.orderId.ToString());
                        command.Parameters.AddWithValue("@invoice_no", order.invoiceNo.ToString());
                        command.Parameters.AddWithValue("@order_by", order.orderBy.ToString());
                        command.Parameters.AddWithValue("@payment_method", order.paymentMethod.ToString());
                        createOrderResult = command.ExecuteNonQuery() > 0;

                        foreach (int selectedClass in selectedClasses)
                        {
                            command.Parameters.Clear();
                            command.CommandText = queryOrderDetail;
                            command.Parameters.AddWithValue("@invoice_no", order.invoiceNo.ToString());
                            command.Parameters.AddWithValue("@user_class_id", selectedClass);
                            createOrderDetailResult = command.ExecuteNonQuery() > 0;

                            command.Parameters.Clear();
                            command.CommandText = queryConfirmPayment;
                            command.Parameters.AddWithValue("@user_class_id", selectedClass);
                            confirmPaymentResult = command.ExecuteNonQuery() > 0;
                        }

                        command.Parameters.Clear();
                        command.CommandText = queryOrderTotalPrice;
                        command.Parameters.AddWithValue("@user_id", order.orderBy.ToString());
                        command.Parameters.AddWithValue("@invoice_no", order.invoiceNo.ToString());
                        totalPriceResult = command.ExecuteNonQuery() > 0;

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        if (transaction != null) transaction.Rollback();
                        throw;
                    }
                    finally
                    {
                        if (transaction != null) transaction.Dispose();
                        connection.Close();
                    }
                }
            }

            return createOrderResult && createOrderDetailResult && confirmPaymentResult && totalPriceResult;
        }
    }
}
