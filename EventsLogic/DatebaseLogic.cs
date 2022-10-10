using System;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;
using VkNet.Model;

namespace EventsLogic
{
    public enum UserFindBy
    {
        Id,
        Admin,
        Name,
        Surname,
        NameAndSurname
    }

    public enum EventFindBy
    {
        Id,
        Name,
        Actual,
        Date
    }


    public static class DatebaseLogic
    {
        private static readonly string connectionString = "Data Source = (LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Nipa\\source\\repos\\vkBot\\EventsLogic\\Database.mdf;Integrated Security = True";

        #region Работа с базой данных пользователей

        /// <summary>
        /// Добавляет пользователя в базу данных.
        /// </summary>
        /// <param name="id"> Уникальный идентификатор пользователя. </param>
        /// <param name="name"> Имя пользователя. </param>
        /// <param name="surname"> Фамилия пользователя. </param>
        /// <param name="admin"> Является ли пользователь админом (если true ,то mark = true; false аналогично). </param>
        public static void AddUser(string id, string? name, string? surname, bool admin)
        {
            string sqlExpression = "UserAdd";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(sqlExpression, connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                command.Parameters.Add(ParameterRegistrer(id, "@Id"));
                command.Parameters.Add(ParameterRegistrer(name, "@Name"));
                command.Parameters.Add(ParameterRegistrer(surname, "@Surname"));
                command.Parameters.Add(ParameterRegistrer(admin, "@Admin"));
                command.Parameters.Add(ParameterRegistrer(0, "@Major"));
                command.Parameters.Add(ParameterRegistrer(0, "@Minor"));
                var result = command.ExecuteScalar();

                Console.WriteLine($"Id добавленного объекта:{result}");
            }
        }


        public static void UserSetParams(Person person,
            string? name = null, string? surname = null, bool? admin = null, int? major = null, int? minor = null)
        {
            string sqlExpression = "UserSetParams";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(sqlExpression, connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.Add(ParameterRegistrer(person.Id, "@Id"));
                command.Parameters.Add(ParameterRegistrer(name ?? person.Name, "@Name"));
                command.Parameters.Add(ParameterRegistrer(surname ?? person.SurName, "@Surname"));
                command.Parameters.Add(ParameterRegistrer(admin ?? person.IsAdmin, "@Admin"));
                command.Parameters.Add(ParameterRegistrer(major ?? person.Major, "@Major"));
                command.Parameters.Add(ParameterRegistrer(minor ?? person.Minor, "@Minor"));

                var result = command.ExecuteScalar();
            }
        }


        /// <summary>
        /// Проверка наличия пользователя в базе данных.
        /// </summary>
        /// <param name="id"> Уникальный идентификарот пользователя. </param>
        /// <returns> True - пользователь есть в базе данных, false - нету. </returns>
        public static bool CheckUserToId(string id)
        {
            return FindUsers(UserFindBy.Id, id).Count == 0;
        }


        /// <summary>
        /// Заполнение приоритетных ролей в List.
        /// </summary>
        /// <param name="adminList"> Роль Admin. </param>
        /// <param name="makeList"> Роль Make. </param>
        /// <param name="markList"> Роль Mark. </param>
        public static List<string> FillAdminList()
        {
            var result = new List<string>();

            foreach (var user in FindUsers(UserFindBy.Admin, true))
            {
                result.Add(user.Id.ToString());
            }
            return result;
        }


        /// <summary>
        /// Нахождение всех Пользователей по заданномк параметру.
        /// </summary>
        /// <param name="findBy"> Параметр. </param>
        /// <param name="desired"> Значение параметра. </param>
        /// <returns> Список всех соответствующих пользователей </returns>
        public static List<Person> FindUsers(UserFindBy findBy, object desired)
        {
            string sqlExpression = $"UserFindBy{findBy}";
            List<Person> result = new List<Person>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(sqlExpression, connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                switch (findBy)
                {
                    case UserFindBy.Id:
                        command.Parameters.Add(ParameterRegistrer(desired, "@Id"));
                        break;
                    case UserFindBy.Admin:
                        command.Parameters.Add(ParameterRegistrer(desired, "@Admin"));
                        break;
                    case UserFindBy.Name:
                        command.Parameters.Add(ParameterRegistrer(desired, "@Name"));
                        break;
                    case UserFindBy.Surname:
                        command.Parameters.Add(ParameterRegistrer(desired, "@Surname"));
                        break;
                    case UserFindBy.NameAndSurname:
                        {
                            int scSymbol = ((string)desired).IndexOf('|');
                            command.Parameters.Add(ParameterRegistrer(((string)desired)
                                .Remove(0, scSymbol), "@Param1"));
                            command.Parameters.Add(ParameterRegistrer(((string)desired)
                                .Remove(scSymbol + 1, ((string)desired).Length - scSymbol), "@Param2"));
                        }
                        break;
                }
                SqlDataReader reader = command.ExecuteReader();

                for (int i = 0; i < int.MaxValue; i++)
                    if (reader.Read())
                        result.Add(new Person()
                        {
                            Id = (string)reader["Id"],
                            Name = (string)reader["Name"],
                            SurName = (string)reader["Surname"],
                            IsAdmin = (bool)reader["Admin"],
                            Major = (int)reader["Major"],
                            Minor = (int)reader["Minor"]
                        });
                    else break;
            }
            return result;
        }


        /// <summary>
        /// Вывод данных о пользователе
        /// </summary>
        /// <param name="id"> Индефикатор пользователя. </param>
        /// <returns> Строковое представление информации о пользователе. </returns>
        public static string AboutMe(string id)
        {
            Person user = FindUsers(UserFindBy.Id, id).First();
            return
                $"Id: {user.Id}\n" +
                $"Имя: {user.Name}\n" +
                $"Фамилия: {user.SurName}\n" +
                $"Администратор: {user.IsAdmin}\n";
        }


        #endregion

        #region Работа с базой данных событий


        public static List<Event> FindEvents(EventFindBy findBy, object desired)
        {
            string sqlExpression = $"EventFindBy{findBy}";
            var result = new List<Event>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(sqlExpression, connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                switch (findBy)
                {
                    case EventFindBy.Id:
                        command.Parameters.Add(ParameterRegistrer(desired, "@Id"));
                        break;
                    case EventFindBy.Name:
                        command.Parameters.Add(ParameterRegistrer(desired, "@Name"));
                        break;
                    case EventFindBy.Actual:
                        command.Parameters.Add(ParameterRegistrer(desired, "@Actual"));
                        break;
                    case EventFindBy.Date:
                        command.Parameters.Add(ParameterRegistrer(desired, "@Date"));
                        break;
                }
                SqlDataReader reader = command.ExecuteReader();

                for (int i = 0; i < int.MaxValue; i++)
                    if (reader.Read())
                        result.Add(new Event()
                        {
                            Id = (int)reader["Id"],
                            IsActual = (bool)reader["Actual"],
                            Name = (string)reader["Name"],
                            Place = (string)reader["Place"],
                            Seats = (int)reader["Count"],
                            Describe = (string)reader["Describe"],
                            Date = (DateTime)reader["Date"],
                            Time = (DateTime)reader["Time"],
                        });
                    else break;
            }
            return result;
        }

        public static List<Event> FillActualEvents()
        {
            var result = new List<Event>();
            string sqlExpression = "FindActualEvents";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(sqlExpression, connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                SqlDataReader reader = command.ExecuteReader();

                for (int i = 0; i < int.MaxValue; i++)
                    if (reader.Read())
                        result.Add(new Event()
                        {
                            Id = (int)reader["Id"],
                            IsActual = (bool)reader["Actual"],
                            Name = (string)reader["Name"],
                            Place = (string)reader["Place"],
                            Seats = (int)reader["Count"],
                            Describe = (string)reader["Describe"],
                            Date = (DateTime)reader["Date"],
                            Time = (DateTime)reader["Time"],
                        });
                    else break;
            }
            return result;
        }


        #endregion
        private static SqlParameter ParameterRegistrer(object? value, string paramName)
        {
            return new SqlParameter
            {
                ParameterName = paramName,
                Value = value
            };
        }
    }
}
