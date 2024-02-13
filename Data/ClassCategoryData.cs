using ApelMusicAPI.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace ApelMusicAPI.Data
{
    public class ClassCategoryData
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;

        public ClassCategoryData(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        //GetAll
        public List<ClassCategory> GetCategories()
        {
            List<ClassCategory> categories = new List<ClassCategory>();

            string query = "SELECT * FROM class_category WHERE is_active = TRUE";

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
                                ClassCategory category = new ClassCategory
                                {
                                    categoryId = Convert.ToInt32(dataReader["category_id"]),
                                    categoryImg = dataReader["category_img"].ToString(),
                                    categoryName = dataReader["category_name"].ToString(),
                                    categoryDescription = dataReader["category_description"].ToString(),
                                    isActive = Convert.ToBoolean(dataReader["is_active"])
                                };

                                categories.Add(category);
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

            return categories;
        }

        //GetByID
        public ClassCategory? GetById(int category_id)
        {
            ClassCategory? categoryById = null;
            string query = $"SELECT * FROM class_category WHERE category_id = @category_id AND is_active = TRUE";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.Parameters.Clear();
                    command.CommandText = query;
                    command.Parameters.AddWithValue("@category_id", category_id);

                    try
                    {
                        connection.Open();

                        using (MySqlDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                ClassCategory category = new ClassCategory
                                {
                                    categoryId = Convert.ToInt32(dataReader["category_id"]),
                                    categoryImg = dataReader["category_img"].ToString(),
                                    categoryName = dataReader["category_name"].ToString(),
                                    categoryDescription = dataReader["category_description"].ToString(),
                                    isActive = Convert.ToBoolean(dataReader["is_active"])
                                };

                                categoryById = category;
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

            return categoryById;
        }

        //Insert/Post
        public bool InsertNewCategory(ClassCategory newCategory)
        {
            bool result = false;

            string query = $"INSERT INTO class_category (category_img, category_name, category_description, is_active) VALUES(@category_img, @category_name, @category_description, @is_active)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.Parameters.Clear();
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@category_img", newCategory.categoryImg);
                    command.Parameters.AddWithValue("@category_name", newCategory.categoryName);
                    command.Parameters.AddWithValue("@category_description", newCategory.categoryDescription);
                    command.Parameters.AddWithValue("@is_active", newCategory.isActive);

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
        public bool UpdateCategory(int category_id, ClassCategory newCategory)
        {
            bool result = false;

            string query = $"UPDATE class_category SET category_img = @category_img, category_name = @category_name, category_description = @category_description, is_active = @is_active WHERE category_id = @category_id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.Parameters.Clear();
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@category_id", category_id);
                    command.Parameters.AddWithValue("@category_img", newCategory.categoryImg);
                    command.Parameters.AddWithValue("@category_name", newCategory.categoryName);
                    command.Parameters.AddWithValue("@category_description", newCategory.categoryDescription);
                    command.Parameters.AddWithValue("@is_active", newCategory.isActive);

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
        public bool DeleteById(int category_id)
        {
            bool result = false;

            string query = "DELETE FROM class_category WHERE category_id = @category_id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.Parameters.Clear();
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@category_id", category_id);

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
