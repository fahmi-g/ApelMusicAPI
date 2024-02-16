using ApelMusicAPI.Models;
using MySql.Data.MySqlClient;

namespace ApelMusicAPI.Data
{
    public class AccountData
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;

        public AccountData(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public List<User> GetAllUser()
        {
            List<User> users = new List<User>();
            string query = $"SELECT * FROM apelmusic_user";

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
                                users.Add(new User
                                {
                                    userId = Guid.Parse(dataReader["user_id"].ToString() ?? string.Empty),
                                    userEmail = dataReader["user_email"].ToString() ?? string.Empty,
                                    userName = dataReader["user_name"].ToString() ?? string.Empty,
                                    userPassword = dataReader["user_password"].ToString() ?? string.Empty,
                                    role = Convert.ToInt32(dataReader["role_id"]),
                                    isActivated = Convert.ToBoolean(dataReader["is_activated"])
                                });
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

            return users;
        }

        public User? GetUserById(Guid user_id)
        {
            User user = null;

            string query = $"SELECT * FROM apelmusic_user WHERE user_id = @user_id";
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

                        using (MySqlDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                user = new User
                                {
                                    userId = Guid.Parse(dataReader["user_id"].ToString() ?? string.Empty),
                                    userEmail = dataReader["user_email"].ToString() ?? string.Empty,
                                    userName = dataReader["user_name"].ToString() ?? string.Empty,
                                    userPassword = dataReader["user_password"].ToString() ?? string.Empty,
                                    role = Convert.ToInt32(dataReader["role_id"]),
                                    isActivated = Convert.ToBoolean(dataReader["is_activated"])
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

        public bool InsertAccount(User user)
        {
            bool result = false;
            string query = $"INSERT INTO apelmusic_user " +
                $"VALUES (@user_id, @user_name, @user_email, @user_password, @role_id, @is_activated)";

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
                    command.Parameters.AddWithValue("@role_id", user.role);
                    command.Parameters.AddWithValue("@is_activated", user.isActivated);

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

        public bool EditAccount(Guid user_id, User user)
        {
            bool result = false;

            string query = $"UPDATE apelmusic_user SET user_name = @user_name, user_email = @user_email, role_id = @role_id, is_activated = @is_activated " +
                $"WHERE user_id = @user_id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.Parameters.Clear();
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@user_id", user_id.ToString());
                    command.Parameters.AddWithValue("@user_name", user.userName);
                    command.Parameters.AddWithValue("@user_email", user.userEmail);
                    command.Parameters.AddWithValue("@role_id", user.role);
                    command.Parameters.AddWithValue("@is_activated", user.isActivated);

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
