using ApelMusicAPI.Models;
using MySql.Data.MySqlClient;

namespace ApelMusicAPI.Data
{
    public class UserRolesData
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;

        public UserRolesData(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        //GetAll
        public List<UserRoles> GetAll()
        {
            List<UserRoles> userRoles = new List<UserRoles>();

            string query = "SELECT * FROM user_roles";

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
                                UserRoles userRole = new UserRoles
                                {
                                    roleId = Convert.ToInt32(dataReader["role_id"]),
                                    roleName = dataReader["role_name"].ToString()
                                };

                                userRoles.Add(userRole);
                            }
                        }
                    }
                    catch (MySqlException)
                    {

                    }
                    finally
                    {
                        connection?.Close();
                    }
                }
            }

                return userRoles;
        }

        //GetById
        public UserRoles? GetById(int role_id)
        {
            UserRoles? userRole = null;
            string query = $"SELECT * FROM user_roles WHERE role_id = @role_id";

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
                                UserRoles userRoleById = new UserRoles
                                {
                                    roleId = role_id,
                                    roleName = dataReader["role_name"].ToString()
                                };

                                userRole = userRoleById;
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

            return userRole;
        }

        //Insert/Post
        public bool InsertNewRole(UserRoles newRole)
        {
            bool result = false;

            string query = $"INSERT INTO user_roles (role_name) VALUES(@role_name)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.Parameters.Clear();
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@role_name", newRole.roleName);

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
        public bool UpdateRole(int role_id, UserRoles newRole)
        {
            bool result = false;

            string query = $"UPDATE user_roles SET role_name = @role_name WHERE role_id = @role_id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.Parameters.Clear();
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@role_id", role_id);
                    command.Parameters.AddWithValue("@role_name", newRole.roleName);

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
        public bool DeleteById(int role_id)
        {
            bool result = false;

            string query = "DELETE FROM user_roles WHERE role_id = @role_id";

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
