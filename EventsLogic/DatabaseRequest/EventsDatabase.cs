using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventsLogic.DatabaseRequest
{
    public enum EventFindBy { Id, Name, Actual, Date }

    public class EventsDatabase
    {
        private static readonly string connectionString = "Data Source = (LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Nipa\\source\\repos\\vkBot\\EventsLogic\\Database.mdf;Integrated Security = True";

        public static void AddEvent(Event @event)
        {
            string sqlExpression = "EventAdd";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(sqlExpression, connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                command.Parameters.Add(ParameterRegistrer(@event.IsActual, "@Actual"));
                command.Parameters.Add(ParameterRegistrer(@event.Name, "@Name"));
                command.Parameters.Add(ParameterRegistrer(@event.Place ?? "", "@Place"));
                command.Parameters.Add(ParameterRegistrer(@event.Seats, "@Seats"));
                command.Parameters.Add(ParameterRegistrer(@event.Describe ?? "", "@Describe"));
                command.Parameters.Add(ParameterRegistrer(@event.StartTime, "@StartTime"));
                command.Parameters.Add(ParameterRegistrer(@event.EndTime, "@EndTime"));
                command.ExecuteScalar();
            }
        }


        public static List<Event> FillActualEvents()
        {
            var result = new List<Event>();
            string sqlExpression = "EventFindByActual";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(sqlExpression, connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                command.Parameters.Add(ParameterRegistrer(true, "@Actual"));

                SqlDataReader reader = command.ExecuteReader();

                for (int i = 0; i < int.MaxValue; i++)
                    if (reader.Read())
                        result.Add(new Event()
                        {
                            Id = (int)reader["Id"],
                            IsActual = (bool)reader["Actual"],
                            Name = (string)reader["Name"],
                            Place = (string)reader["Place"],
                            Seats = (int)reader["Seats"],
                            Describe = (string)reader["Describe"],
                            StartTime = (DateTime)reader["StartTime"],
                            EndTime = (DateTime)reader["EndTime"],
                        });
                    else break;
            }
            return result;
        }
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
