using System.Reflection.Metadata.Ecma335;

using ApelMusicAPI.Models;
using MySql.Data.MySqlClient;


namespace ApelMusicAPI.Data
{
    public class ClassData
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;

        public ClassData(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        //GetAll
        public List<Class> GetClasses()
        {
            List<Class> classes = new List<Class>();

            string query = "SELECT c.*, cc.category_name FROM class c " +
                "JOIN class_category cc ON c.class_category = cc.category_id";

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
                                Class newClass = new Class
                                {
                                    classId = Convert.ToInt32(dataReader["class_id"]),
                                    classCategory = Convert.ToInt32(dataReader["class_category"]),
                                    classImg = dataReader["class_img"].ToString(),
                                    className = dataReader["class_name"].ToString() ?? string.Empty,
                                    classDescription = dataReader["class_description"].ToString(),
                                    classPrice = Convert.ToInt32(dataReader["class_price"]),
                                    classStatus = dataReader["class_status"].ToString() ?? string.Empty,
                                    categoryName = dataReader["category_name"].ToString() ?? string.Empty
                                };

                                classes.Add(newClass);
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

            return classes;
        }

        //GetByID
        public Class? GetById(int class_id)
        {
            Class? classById = null;
            string query = $"SELECT c.*, cc.category_name FROM class c " +
                "JOIN class_category cc ON c.class_category = cc.category_id "+
                $"WHERE c.class_id = @class_id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.Parameters.Clear();

                    command.CommandText = query;
                    command.Parameters.AddWithValue("@class_id", class_id);

                    try
                    {
                        connection.Open();

                        using (MySqlDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                Class newClass = new Class
                                {
                                    classId = Convert.ToInt32(dataReader["class_id"]),
                                    classCategory = Convert.ToInt32(dataReader["class_category"]),
                                    classImg = dataReader["class_img"].ToString(),
                                    className = dataReader["class_name"].ToString() ?? string.Empty,
                                    classDescription = dataReader["class_description"].ToString(),
                                    classPrice = Convert.ToInt32(dataReader["class_price"]),
                                    classStatus = dataReader["class_status"].ToString() ?? string.Empty,
                                    categoryName = dataReader["category_name"].ToString() ?? string.Empty
                                };

                                classById = newClass;
                            }
                        }
                    }
                    catch(Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return classById;
        }

        //Insert/Post
        public bool InsertNewClass(Class newClass)
        {
            bool result = false;

            string query = $"INSERT INTO class(class_category, class_img, class_name, class_description, class_price, class_status) " +
                $"VALUES (@class_category, @class_img, @class_name, @class_description, @class_price, @class_status)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.Parameters.Clear();
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@class_category", newClass.classCategory);
                    command.Parameters.AddWithValue("@class_img", newClass.classImg);
                    command.Parameters.AddWithValue("@class_name", newClass.className);
                    command.Parameters.AddWithValue("@class_description", newClass.classDescription);
                    command.Parameters.AddWithValue("@class_price", newClass.classPrice);
                    command.Parameters.AddWithValue("@class_status", newClass.classStatus);

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
        public bool UpdateClass(int class_id, Class newClass)
        {
            bool result = false;

            string query = $"UPDATE class " +
                $"SET class_category = @class_category, class_img = @class_img, class_name = @class_name, class_description = @class_description, class_price = @class_price, class_status = @class_status " +
                $"WHERE class_id = @class_id;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.Parameters.Clear();
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@class_id", class_id);
                    command.Parameters.AddWithValue("@class_category", newClass.classCategory);
                    command.Parameters.AddWithValue("@class_img", newClass.classImg);
                    command.Parameters.AddWithValue("@class_name", newClass.className);
                    command.Parameters.AddWithValue("@class_description", newClass.classDescription);
                    command.Parameters.AddWithValue("@class_price", newClass.classPrice);
                    command.Parameters.AddWithValue("@class_status", newClass.classStatus);

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
        public bool DeleteById(int class_id)
        {
            bool result = false;

            string query = "DELETE FROM class WHERE class_id = @class_id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.Parameters.Clear();
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@class_id", class_id);

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
