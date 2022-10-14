using EventsLogic.HelperClasses;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using static Azure.Core.HttpHeader;

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

                command.Parameters.Add(ParameterRegistrer(@event.Author, "@Author"));
                command.Parameters.Add(ParameterRegistrer(@event.CreateTime, "@CreateTime"));
                command.Parameters.Add(ParameterRegistrer(@event.IsActual, "@Actual"));
                command.Parameters.Add(ParameterRegistrer(@event.Name, "@Name"));
                command.Parameters.Add(ParameterRegistrer(@event.Place, "@Place"));
                command.Parameters.Add(ParameterRegistrer(@event.Seats, "@Seats"));
                command.Parameters.Add(ParameterRegistrer(@event.Describe, "@Describe"));
                command.Parameters.Add(ParameterRegistrer(@event.StartTime, "@StartTime"));
                command.Parameters.Add(ParameterRegistrer(@event.EndTime, "@EndTime"));
                command.ExecuteScalar();
            }
        }

        public static void AddEvent(string author)
        {
            string sqlExpression = "EventAdd";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(sqlExpression, connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                command.Parameters.Add(ParameterRegistrer(author, "@Author"));
                command.Parameters.Add(ParameterRegistrer(DateTime.Now, "@CreateTime"));
                command.Parameters.Add(ParameterRegistrer(false, "@Actual"));
                command.ExecuteScalar();
            }
        }

        public static List<Event> FindEvents(int? id = null, bool? isActual = null, string? name = null, string? place = null, int? seats = null,
            string? describe = null, DateTime? startTime = null, DateTime? endTime = null, string? author = null, DateTime? createTime = null)
        {
            var result = new List<Event>();
            string sqlExpression = "EventFindByParams";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(sqlExpression, connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                if (id != null) command.Parameters.Add(ParameterRegistrer(id, "@Id"));
                if (isActual != null) command.Parameters.Add(ParameterRegistrer(isActual, "@Actual"));
                if (name != null) command.Parameters.Add(ParameterRegistrer(name, "@Name"));
                if (place != null) command.Parameters.Add(ParameterRegistrer(place, "@Place"));
                if (seats != null) command.Parameters.Add(ParameterRegistrer(seats, "@Seats"));
                if (describe != null) command.Parameters.Add(ParameterRegistrer(describe, "@Describe"));
                if (startTime != null) command.Parameters.Add(ParameterRegistrer(startTime, "@StartTime"));
                if (endTime != null) command.Parameters.Add(ParameterRegistrer(endTime, "@EndTime"));
                if (author != null) command.Parameters.Add(ParameterRegistrer(author, "@Author"));
                if (createTime != null) command.Parameters.Add(ParameterRegistrer(createTime, "@CreateTime"));

                SqlDataReader reader = command.ExecuteReader();

                for (int i = 0; i < int.MaxValue; i++)
                    if (reader.Read())
                        result.AddByDateTime(new Event()
                        {
                            Id = (int)reader["Id"],
                            IsActual = (bool)reader["Actual"],
                            Name = (string)reader["Name"],
                            Place = (string)reader["Place"],
                            Seats = (int)reader["Seats"],
                            Describe = (string)reader["Describe"],
                            StartTime = (DateTime)reader["StartTime"],
                            EndTime = (DateTime)reader["EndTime"],
                            Author = (string)reader["Author"],
                            CreateTime = (DateTime)reader["CreateTime"]
                        });
                    else break;
            }
            return result;
        }

        public static void EventSetParams(int? id = null, bool? isActual = null, string? name = null, string? place = null, int? seats = null,
            string? describe = null, DateTime? startTime = null, DateTime? endTime = null, string? author = null, DateTime? createTime = null)
        {
            string sqlExpression = "EventSetParams";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(sqlExpression, connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                if (id != null) command.Parameters.Add(ParameterRegistrer(id, "@Id"));
                if (isActual != null) command.Parameters.Add(ParameterRegistrer(isActual, "@Actual"));
                if (name != null) command.Parameters.Add(ParameterRegistrer(name, "@Name"));
                if (place != null) command.Parameters.Add(ParameterRegistrer(place, "@Place"));
                if (seats != null) command.Parameters.Add(ParameterRegistrer(seats, "@Seats"));
                if (describe != null) command.Parameters.Add(ParameterRegistrer(describe, "@Describe"));
                if (startTime != null) command.Parameters.Add(ParameterRegistrer(startTime, "@StartTime"));
                if (endTime != null) command.Parameters.Add(ParameterRegistrer(endTime, "@EndTime"));
                command.ExecuteScalar();
            }
        }

        public static int SetLastIndex()
        {
            string sqlExpression = "EventsLastId";
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
                        return (int)reader["LastId"];
            }
            return -1;
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
