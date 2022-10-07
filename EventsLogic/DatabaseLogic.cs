using System;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;
using VkNet.Model;

namespace EventsLogic
{
    public static class DatabaseLogic
    {
        private static string connectionString = "Data Source = (LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Nipa\\source\\repos\\vkBot\\EventsLogic\\Database.mdf;Integrated Security = True";

        #region добавление пользователя в базу данных

        /// <summary>
        /// Добавляет пользователя в базу данных.
        /// </summary>
        /// <param name="id"> Уникальный идентификатор пользователя. </param>
        /// <param name="name"> Имя пользователя. </param>
        /// <param name="surname"> Фамилия пользователя. </param>
        /// <param name="admin"> Является ли пользователь админом (если true ,то mark = true; false аналогично). </param>
        public static void AddUser(string id, string? name, string? surname, bool admin)
        {
            string sqlExpression = "InsertUser";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add(ParameterRegistrer(id, "@Id"));
                command.Parameters.Add(ParameterRegistrer(name, "@Name"));
                command.Parameters.Add(ParameterRegistrer(surname, "@Surname"));
                command.Parameters.Add(ParameterRegistrer(admin, "@Admin"));
                command.Parameters.Add(ParameterRegistrer(false, "@Make"));
                command.Parameters.Add(ParameterRegistrer(0, "@MakeState"));
                command.Parameters.Add(ParameterRegistrer(admin, "@Mark"));
                var result = command.ExecuteScalar();

                Console.WriteLine($"Id добавленного объекта:{result}");
            }
        }

        #endregion

        /// <summary>
        /// Проверка наличия пользователя в базе данных.
        /// </summary>
        /// <param name="id"> Уникальный идентификарот пользователя. </param>
        /// <returns> True - пользователь есть в базе данных, false - нету. </returns>
        public static bool CheckUserToId(string id)
        {
            string sqlExpression = "CheckKeyUser";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter idParam = new SqlParameter
                {
                    ParameterName = "@Id",
                    Value = id
                };
                command.Parameters.Add(idParam);
                SqlDataReader reader = command.ExecuteReader();

                Console.WriteLine(reader.HasRows);

                return reader.HasRows;
            }
        }

        /// <summary>
        /// Заполнение приоритетных ролей в List.
        /// </summary>
        /// <param name="adminList"> Роль Admin. </param>
        /// <param name="makeList"> Роль Make. </param>
        /// <param name="markList"> Роль Mark. </param>
        public static void FillPriority(out List<string> adminList, out List<string> markList)
        {
            adminList = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlExpression = "FindUserAdmin";

                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader reader = command.ExecuteReader();

                for (int i = 0; i < int.MaxValue; i++)
                    if (reader.Read())
                    {
                        adminList.Add(reader.GetString(i));
                        Console.WriteLine($"Admin add: {reader.GetString(i)}");
                    }
                    else break;
            }

            markList = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlExpression = "FindUserMark";

                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader reader = command.ExecuteReader();

                for (int i = 0; i < int.MaxValue; i++)
                    if (reader.Read())
                    {
                        markList.Add(reader.GetString(i));
                        Console.WriteLine($"Made add: {reader.GetString(i)}");
                    }
                    else break;
            }
        }

        public static void FillActualEvents(out List<Event> actualEvents)
        {
            actualEvents = new List<Event>();
            string sqlExpression = "FindActualEvents";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader();

                for (int i = 0; i < int.MaxValue; i++)
                    if (reader.Read())
                    {
                        actualEvents.Add(new Event()
                        {
                            Id = (int)reader["Id"],
                            IsActual = (bool)reader["Actual"],
                            Name = (string)reader["Name"],
                            Place = (string)reader["Place"],
                            Count = (int)reader["Count"],
                            Describe = (string)reader["Describe"],
                            Date = (DateTime)reader["Date"],
                            Time = (DateTime)reader["Time"],
                        });
                        Console.WriteLine($"Событие \"{actualEvents.Last().Name}\" добавлено");
                    }
                    else break;
            }
        }

        public static string AboutMe(string id)
        {
            User user = GetUsersByProcedure("UserInfo", id, "@Id").First();
            return $"Id: {user.Id}\nИмя: {user.Name}\nФамилия: {user.SurName}\nАдминистратор: {user.IsAdmin}\nПомощник: {user.IsMark}";
        }

        private static SqlParameter ParameterRegistrer(object? value, string paramName)
        {
            return new SqlParameter
            {
                ParameterName = paramName,
                Value = value
            };
        }
        private static List<User> GetUsersByProcedure(string sqlExpression, object param1, string name1)
        {
            var users = new List<User>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add(ParameterRegistrer(param1, name1));

                SqlDataReader reader = command.ExecuteReader();

                for (int i = 0; i < int.MaxValue; i++)
                    if (reader.Read())
                    {
                        users.Add(new User()
                        {
                            Id = (string)reader["Id"],
                            Name = (string)reader["Name"],
                            SurName = (string)reader["Surname"],
                            IsAdmin = (bool)reader["Admin"],
                            IsMake = (bool)reader["Make"],
                            IsMark = (bool)reader["Mark"]
                        });
                        Console.WriteLine($"Пользователь {users.Last().SurName} {users.Last().Name} найден");
                    }
                    else break;
            }

            return users;
        }
    }
}
