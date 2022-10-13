﻿using EventsLogic.HelperClasses;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventsLogic.DatabaseRequest
{
    public class UsersDatabase
    {
        private static readonly string connectionString = "Data Source = (LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Nipa\\source\\repos\\vkBot\\EventsLogic\\Database.mdf;Integrated Security = True";

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
                command.ExecuteScalar();
            }
        }

        /// <summary>
        /// Изменияе параметры Пользователя в базе данных
        /// </summary>
        /// <param name="person"> Пользователь. </param>
        /// <param name="name"> Имя,изменияемый параметр. </param>
        /// <param name="surname"> Фамилия,изменияемый параметр. </param>
        /// <param name="admin"> Администрирование,изменияемый параметр. </param>
        /// <param name="major"> Мажор,изменияемый параметр. </param>
        /// <param name="minor"> Минор,изменияемый параметр. </param>
        public static void UserSetParams(string id, string? name = null, string? surname = null, bool? admin = null, int? major = null, int? minor = null)
        {
            string sqlExpression = "UserSetParams";

            using (SqlConnection connection = new SqlConnection(connectionString))
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
                command.Parameters.Add(ParameterRegistrer(major, "@Major"));
                command.Parameters.Add(ParameterRegistrer(minor, "@Minor"));
                command.ExecuteScalar();
            }
        }

        /// <summary>
        /// Проверка наличия пользователя в базе данных.
        /// </summary>
        /// <param name="id"> Уникальный идентификарот пользователя. </param>
        /// <returns> True - пользователь есть в базе данных, false - нету. </returns>
        public static bool CheckUserToId(string id)
        {
            return FindUsers(id: id).Count == 0;
        }

        /// <summary>
        /// Нахождение всех Пользователей по заданномк параметру.
        /// </summary>
        /// <param name="findBy"> Параметр. </param>
        /// <param name="desired"> Значение параметра. </param>
        /// <returns> Список всех соответствующих пользователей </returns>
        public static List<Person> FindUsers(string? id = null, string? name = null, string? surname = null,
            bool? isAdmin = null, int? major = null, int? minor = null)
        {
            var result = new List<Person>();
            string sqlExpression = "UserFindByParams";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(sqlExpression, connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                command.Parameters.Add(ParameterRegistrer(id, "@Id"));
                command.Parameters.Add(ParameterRegistrer(name, "@Name"));
                command.Parameters.Add(ParameterRegistrer(surname, "@Surname"));
                command.Parameters.Add(ParameterRegistrer(isAdmin, "@Admin"));
                command.Parameters.Add(ParameterRegistrer(major, "@Major"));
                command.Parameters.Add(ParameterRegistrer(minor, "@Minor"));

                SqlDataReader reader = command.ExecuteReader();

                for (int i = 0; i < int.MaxValue; i++)
                    if (reader.Read())
                        result.Add(new Person()
                        {
                            Id = (string)reader["Id"],
                            Name = (string)reader["Name"],
                            Surname = (string)reader["Surname"],
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
            Person user = FindUsers(id: id).First();
            return
                $"Id: {user.Id}\n" +
                $"Имя: {user.Name}\n" +
                $"Фамилия: {user.Surname}\n" +
                $"Администратор: {user.IsAdmin}\n";
        }

        /// <summary>
        /// Регистрация параметра в SQL запросе
        /// </summary>
        /// <param name="value"> Значение параметра. </param>
        /// <param name="paramName"> Название параметра. </param>
        /// <returns></returns>
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
