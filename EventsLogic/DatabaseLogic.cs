using System;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

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
        /// <param name="admin"> Является ли пользователь админом. </param>
        /// <param name="make"> Может ли пользователь создавать мероприятия. </param>
        /// <param name="mark"> Может ли пользователь отмечать волонтёров. </param>
        public static void AddUser(string id, string? name, string? surname, bool admin, bool make, bool mark)
        {
            string sqlExpression = "InsertUser";

        /// <summary>
        /// Добавляет пользователя в базу данных.
        /// </summary>
        /// <param name="id"> Уникальный идентификатор пользователя. </param>
        /// <param name="name"> Имя пользователя. </param>
        /// <param name="surname"> Фамилия пользователя. </param>
        /// <param name="admin"> Является ли пользователь админом (если true ,то make = true и mark = true; false аналогично). </param>
        public static void AddUser(string id, string? name, string? surname, bool admin)
        {
            string sqlExpression = "InsertUser";

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
                SqlParameter nameParam = new SqlParameter
                {
                    ParameterName = "@Name",
                    Value = name
                };
                command.Parameters.Add(nameParam);
                SqlParameter surnameParam = new SqlParameter
                {
                    ParameterName = "@Surname",
                    Value = surname
                };
                command.Parameters.Add(surnameParam);
                SqlParameter adminParam = new SqlParameter
                {
                    ParameterName = "@Admin",
                    Value = admin
                };
                command.Parameters.Add(adminParam);
                SqlParameter makeParam = new SqlParameter
                {
                    ParameterName = "@Make",
                    Value = admin
                };
                command.Parameters.Add(makeParam);
                SqlParameter markParam = new SqlParameter
                {
                    ParameterName = "@Mark",
                    Value = admin
                };
                command.Parameters.Add(markParam);
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
            string sqlExpression = "GetKeyUser";

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
        public static void FillPriority(out List<string> adminList, out List<string> makeList, out List<string> markList)
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

            makeList = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlExpression = "FindUserMake";

                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader reader = command.ExecuteReader();

                for (int i = 0; i < int.MaxValue; i++)
                    if (reader.Read())
                    {
                        makeList.Add(reader.GetString(i));
                        Console.WriteLine($"Make add: {reader.GetString(i)}");
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

        public static void AboutMe(string id)
        {
            
        }
    }
}
