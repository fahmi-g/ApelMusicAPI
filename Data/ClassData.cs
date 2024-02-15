using System.Reflection.Metadata.Ecma335;
using ApelMusicAPI.DTOs;
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
                "JOIN class_category cc ON c.class_category = cc.category_id " +
                "WHERE c.class_status = 'active'";

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
                $"WHERE c.class_id = @class_id AND c.class_status = 'active'";

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
                                    classId = class_id,
                                    classCategory = Convert.ToInt32(dataReader["class_category"]),
                                    classImg = dataReader["class_img"].ToString(),
                                    className = dataReader["class_name"].ToString() ?? string.Empty,
                                    classDescription = dataReader["class_description"].ToString(),
                                    classPrice = Convert.ToInt32(dataReader["class_price"]),
                                    classStatus = dataReader["class_status"].ToString() ?? string.Empty,
                                    categoryName = dataReader["category_name"].ToString() ?? string.Empty,
                                    classSchedules = GetClassSchedules(dataReader["class_name"].ToString() ?? string.Empty)
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

        //GetByCategoryId
        public List<Class> GetClassesByCategoryId(int category_id)
        {
            List<Class> classesByCateogryId = new List<Class>();
            string query = $"SELECT c.*, cc.category_name FROM class c " +
                $"JOIN class_category cc ON c.class_category = cc.category_id " +
                $"WHERE c.class_category = @category_id AND c.class_status = 'active'";

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
                                Class newClass = new Class
                                {
                                    classId = Convert.ToInt32(dataReader["class_id"]),
                                    classCategory = category_id,
                                    classImg = dataReader["class_img"].ToString(),
                                    className = dataReader["class_name"].ToString() ?? string.Empty,
                                    classDescription = dataReader["class_description"].ToString(),
                                    classPrice = Convert.ToInt32(dataReader["class_price"]),
                                    classStatus = dataReader["class_status"].ToString() ?? string.Empty,
                                    categoryName = dataReader["category_name"].ToString() ?? string.Empty,
                                    classSchedules = GetClassSchedules(dataReader["class_name"].ToString() ?? string.Empty)
                                };

                                classesByCateogryId.Add(newClass);
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

            return classesByCateogryId;
        }

        //GetUserClassesByUserID
        public List<UserClassesPaidUnpaidDTO> GetUserClassesByUserId(Guid user_id, bool is_paid)
        {
            List<UserClassesPaidUnpaidDTO> userClasses = new List<UserClassesPaidUnpaidDTO>();
            string query = $"SELECT uc.user_class_id, uc.class_schedule, c.*, cc.category_name FROM user_classes uc " +
                $"JOIN class c ON uc.class_id = c.class_id " +
                $"JOIN class_category cc ON c.class_category = cc.category_id " +
                $"WHERE uc.is_paid = @is_paid AND uc.user_id = @user_id AND c.class_status = 'active'";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.Parameters.Clear();

                    command.CommandText = query;
                    command.Parameters.AddWithValue("@user_id", user_id.ToString());
                    command.Parameters.AddWithValue("@is_paid", is_paid);

                    try
                    {
                        connection.Open();

                        using (MySqlDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                userClasses.Add(new UserClassesPaidUnpaidDTO
                                {
                                    userClassId = Convert.ToInt32(dataReader["user_class_id"]),
                                    classId = Convert.ToInt32(dataReader["class_id"]),
                                    classCategory = Convert.ToInt32(dataReader["class_category"]),
                                    categoryName = dataReader["category_name"].ToString() ?? string.Empty,
                                    classImg = dataReader["class_img"].ToString(),
                                    className = dataReader["class_name"].ToString() ?? string.Empty,
                                    classDescription = dataReader["class_description"].ToString(),
                                    classPrice = Convert.ToInt32(dataReader["class_price"]),
                                    classStatus = dataReader["class_status"].ToString() ?? string.Empty,
                                    classSchedule = Convert.ToDateTime(dataReader["class_schedule"])
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

            return userClasses;
        }

        //GetClassSchedules
        public List<DateTime> GetClassSchedules(string class_name)
        {
            List<DateTime> classSchedules = new List<DateTime>();
            string query = $"SELECT * FROM class_schedules " +
                $"WHERE class_name = @class_name " +
                $"AND schedule_id NOT IN (SELECT schedule_id FROM class_schedules " +
                $"WHERE class_name IN (SELECT c.class_name FROM user_classes uc JOIN class c ON uc.class_id = c.class_id WHERE is_paid = TRUE) " +
                $"AND class_schedule IN (SELECT class_schedule FROM user_classes WHERE is_paid = TRUE))";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.Parameters.Clear();

                    command.CommandText = query;
                    command.Parameters.AddWithValue("@class_name", class_name);

                    try
                    {
                        connection.Open();

                        using (MySqlDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                classSchedules.Add(Convert.ToDateTime(dataReader["class_schedule"]));
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

            return classSchedules;
        }

        //Insert/Post
        public bool InsertNewClass(Class newClass)
        {
            bool result = false;
            bool classSchedulesResult = false;

            string query = $"INSERT INTO class(class_category, class_img, class_name, class_description, class_price, class_status) " +
                $"VALUES (@class_category, @class_img, @class_name, @class_description, @class_price, @class_status)";
            string queryClassSchedules = $"INSERT INTO class_schedules (class_name, class_schedule) " +
                $"VALUES (@class_name, @class_schedule)";

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
                        command.CommandText = query;
                        command.Parameters.AddWithValue("@class_category", newClass.classCategory);
                        command.Parameters.AddWithValue("@class_img", newClass.classImg);
                        command.Parameters.AddWithValue("@class_name", newClass.className);
                        command.Parameters.AddWithValue("@class_description", newClass.classDescription);
                        command.Parameters.AddWithValue("@class_price", newClass.classPrice);
                        command.Parameters.AddWithValue("@class_status", newClass.classStatus);
                        result = command.ExecuteNonQuery() > 0;

                        foreach (DateTime classSchedule in newClass.classSchedules)
                        {
                            command.Parameters.Clear();
                            command.CommandText = queryClassSchedules;
                            command.Parameters.AddWithValue("@class_name", newClass.className);
                            command.Parameters.AddWithValue("@class_schedule", classSchedule.ToString("yyyy-MM-dd"));
                            classSchedulesResult = command.ExecuteNonQuery() > 0;
                        }

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

            return result && classSchedulesResult;
        }

        //Update/Put
        public bool UpdateClass(int class_id, Class newClass)
        {
            bool result = false;
            bool classSchedulesResult = false;
            bool deleteSheduleResult = false;

            string query = $"UPDATE class " +
                $"SET class_category = @class_category, class_img = @class_img, class_name = @class_name, class_description = @class_description, class_price = @class_price, class_status = @class_status " +
                $"WHERE class_id = @class_id;";
            string queryDeleteClassSchedules = $"DELETE FROM class_schedules WHERE class_name = @class_name";
            string queryClassSchedules = $"INSERT INTO class_schedules (class_name, class_schedule) " +
                $"VALUES (@class_name, @class_schedule)";

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
                        command.CommandText = query;
                        command.Parameters.AddWithValue("@class_id", class_id);
                        command.Parameters.AddWithValue("@class_category", newClass.classCategory);
                        command.Parameters.AddWithValue("@class_img", newClass.classImg);
                        command.Parameters.AddWithValue("@class_name", newClass.className);
                        command.Parameters.AddWithValue("@class_description", newClass.classDescription);
                        command.Parameters.AddWithValue("@class_price", newClass.classPrice);
                        command.Parameters.AddWithValue("@class_status", newClass.classStatus);
                        result = command.ExecuteNonQuery() > 0;

                        command.Parameters.Clear();
                        command.CommandText = queryDeleteClassSchedules;
                        command.Parameters.AddWithValue("@class_name", newClass.className);
                        deleteSheduleResult = command.ExecuteNonQuery() > 0;

                        foreach (DateTime classSchedule in newClass.classSchedules)
                        {
                            command.Parameters.Clear();
                            command.CommandText = queryClassSchedules;
                            command.Parameters.AddWithValue("@class_name", newClass.className);
                            command.Parameters.AddWithValue("@class_schedule", classSchedule.ToString("yyyy-MM-dd"));
                            classSchedulesResult = command.ExecuteNonQuery() > 0;
                        }

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

            return result && classSchedulesResult && deleteSheduleResult;
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
