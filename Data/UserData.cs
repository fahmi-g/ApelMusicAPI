using ApelMusicAPI.Models;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace ApelMusicAPI.Data
{
    public class UserData
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;

        public UserData(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }


        public bool CreateAccount(User user)
        {
            bool result = false;

            string query = $"INSERT INTO apelmusic_user (user_id, user_name, user_email, user_password, role_id) " +
                $"VALUES (@user_id, @user_name, @user_email, @user_password, @role_id)";


            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.Parameters.Clear();
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@user_id", user.userId);
                    command.Parameters.AddWithValue("@user_name", user.userName);
                    command.Parameters.AddWithValue("@user_email", user.userEmail);
                    command.Parameters.AddWithValue("@user_password", user.userPassword);
                    command.Parameters.AddWithValue("@role_id", 1);

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

        public string GetUserEmail(string user_email)
        {
            string userEmail = "";
            string query = $"SELECT user_email FROM apelmusic_user where user_email = @user_email";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.Parameters.Clear();

                    command.CommandText = query;
                    command.Parameters.AddWithValue("@user_email", user_email);

                    try
                    {
                        connection.Open();

                        using (MySqlDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                userEmail = dataReader["user_email"].ToString();
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

            return userEmail;
        }

        public bool GetUserActivationCheck(string user_email)
        {
            bool isActivated = false;
            string query = $"SELECT is_activated FROM apelmusic_user where user_email = @user_email";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.Parameters.Clear();

                    command.CommandText = query;
                    command.Parameters.AddWithValue("@user_email", user_email);

                    try
                    {
                        connection.Open();

                        using (MySqlDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                isActivated = Convert.ToBoolean(dataReader["is_activated"]);
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

            return isActivated;
        }

        public User? CheckUserAuth(string userEmail)
        {
            User? user = null;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * From apelmusic_user WHERE user_email = @user_email";

                    command.Parameters.Clear();

                    command.Parameters.AddWithValue("@user_email", userEmail);

                    try
                    {
                        connection.Open();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                user = new User
                                {
                                    userId = Guid.Parse(reader["user_id"].ToString() ?? string.Empty),
                                    userEmail = reader["user_email"].ToString() ?? string.Empty,
                                    userName = reader["user_name"].ToString() ?? string.Empty,
                                    userPassword = reader["user_password"].ToString() ?? string.Empty,
                                    role = Convert.ToInt32(reader["role_id"]),
                                    isActivated = Convert.ToBoolean(reader["is_activated"])
                                };
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

            return user;
        }

        //GetByID
        public string GetRoleNameById(int role_id)
        {
            string roleName = "";
            string query = "SELECT ur.role_name FROM user_roles ur " +
                "JOIN apelmusic_user u ON ur.role_id = " + "@role_id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.Parameters.Clear();
                    command.CommandText = query;
                    command.Parameters.AddWithValue("@role_id", role_id);

                    try
                    {
                        connection.Open();

                        using (MySqlDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                roleName = dataReader["role_name"].ToString();
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

            return roleName;
        }

        public bool ActivateUser(Guid user_id)
        {
            bool result = false;

            string query = $"UPDATE apelmusic_user SET is_activated = 1 WHERE user_id = @user_id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {

                    command.Connection = connection;
                    command.Parameters.Clear();
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@user_id", user_id);

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

        public bool ResetPassword(string user_email, string user_password)
        {
            bool result = false;

            string query = $"UPDATE apelmusic_user SET user_password = @user_password WHERE user_email = @user_email";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.Parameters.Clear();
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@user_password", user_password);
                    command.Parameters.AddWithValue("@user_email", user_email);

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
