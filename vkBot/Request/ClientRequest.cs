using EventsLogic.Basic;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using vkBot.General;
using static vkBot.General.DbGeneral;
using static vkBot.Program;

namespace vkBot.Request
{
    internal static class ClientRequest
    {
        public static void Add(long id, string name, string surname, bool? admin)
        {
            string sqlExpression = "UserAdd";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(sqlExpression, connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                command.Parameters.Add(ParamReg(id, "@Id"));
                if (name != null) command.Parameters.Add(ParamReg(name, "@Name"));
                if (surname != null) command.Parameters.Add(ParamReg(surname, "@Surname"));
                if (admin != null) command.Parameters.Add(ParamReg(admin, "@Admin"));
                command.Parameters.Add(ParamReg(0, "@Major"));
                command.Parameters.Add(ParamReg(0, "@Minor"));
                command.ExecuteScalar();
            }
        }

        public static List<Client> GetParams(string name = null, string surname = null,
            bool? isAdmin = null, int? major = null, int? minor = null)
        {
            var result = new List<Client>();
            string sqlExpression = "UserFindByParams";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(sqlExpression, connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                if (name != null) command.Parameters.Add(ParamReg(name, "@Name"));
                if (surname != null) command.Parameters.Add(ParamReg(surname, "@Surname"));
                if (isAdmin != null) command.Parameters.Add(ParamReg(isAdmin, "@Admin"));
                if (major != null) command.Parameters.Add(ParamReg(major, "@Major"));
                if (minor != null) command.Parameters.Add(ParamReg(minor, "@Minor"));

                SqlDataReader reader = command.ExecuteReader();

                for (int i = 0; i < int.MaxValue; i++)
                    if (reader.Read())
                        result.Add(new Client((long)reader["Id"])
                        {
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

        internal static Client Get(long key)
        {
            string sqlExpression = "UserFindByKey";

            Client client = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(sqlExpression, connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                command.Parameters.Add(ParamReg(key, "@Id"));

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    client = new Client((long)reader["Id"])
                    {
                        Name = (string)reader["Name"],
                        Surname = (string)reader["Surname"],
                        IsAdmin = (bool)reader["Admin"],
                        Major = (int)reader["Major"],
                        Minor = (int)reader["Minor"]
                    };
                }
            }
            return client;
        }

        public static void Set(long id, string name = null, string surname = null, bool? admin = null, int? major = null, int? minor = null)
        {
            string sqlExpression = "UserSetParams";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(sqlExpression, connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.Add(ParamReg(id, "@Id"));
                if (name != null) command.Parameters.Add(ParamReg(name, "@Name"));
                if (surname != null) command.Parameters.Add(ParamReg(surname, "@Surname"));
                if (admin != null) command.Parameters.Add(ParamReg(admin, "@Admin"));
                if (major != null) command.Parameters.Add(ParamReg(major, "@Major"));
                if (minor != null) command.Parameters.Add(ParamReg(minor, "@Minor"));
                command.ExecuteScalar();
            }
        }

        internal static bool Existence(out Client client, long id)
        {
            client = Get(id) as Client;
            if (client == null) return false;
            return true;
        }
    }
}
