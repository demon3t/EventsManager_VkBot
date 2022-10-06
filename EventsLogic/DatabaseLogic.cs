

using Microsoft.Data.SqlClient;
using System;

namespace EventsLogic
{
    public static class DatabaseLogic
    {
        private static string connectionString = "Data Source = (LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Nipa\\source\\repos\\vkBot\\EventsLogic\\Database.mdf;Integrated Security = True";
        public static void AddUser(string id, string? name, string? surname, bool? admin, bool? make, bool? mark)
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
                    Value = make
                };
                command.Parameters.Add(makeParam);
                SqlParameter markParam = new SqlParameter
                {
                    ParameterName = "@Mark",
                    Value = mark
                };
                command.Parameters.Add(markParam);
                var result = command.ExecuteScalar();
                Console.WriteLine($"Id добавленного объекта:{result}");
            }
        }
    }
}
