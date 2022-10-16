using EventsLogic.Basic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using VkNet.Model;
using static vkBot.General.DbGeneral;

namespace vkBot.Request
{
    internal static class EventRequest
    {
        public static void Add(out Event @event, long author)
        {
            string sqlExpression = "EventAdd";

            @event = null;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(sqlExpression, connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                command.Parameters.Add(ParamReg(author, "@Author"));
                command.Parameters.Add(ParamReg(DateTime.Now, "@CreateTime"));

                command.ExecuteScalar();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    @event = Get((int)(decimal)reader[0]);
                }
            }
        }

        internal static Event Get(int key)
        {
            string sqlExpression = "EventFindByKey";

            var @event = new Event(0);
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
                    @event.Id = (int)reader["Id"];
                    @event.Author = (long)reader["Author"];
                    @event.CreateTime = (DateTime)reader["CreateTime"];
                    @event.IsActual = (bool)reader["Actual"];
                    @event.Name = (string)reader["Name"];
                    @event.Place = (string)reader["Place"];
                    @event.Describe = (string)reader["Describe"];
                    @event.StartTime = (DateTime)reader["StartTime"];
                    @event.EndTime = (DateTime)reader["EndTime"];
                }
            }
            return @event;
        }

        internal static List<Event> GetParam(bool? isActual = null, string name = null, string place = null, int? seats = null,
            string describe = null, DateTime? startTime = null, DateTime? endTime = null, string author = null, DateTime? createTime = null)
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

                if (isActual != null) command.Parameters.Add(ParamReg(isActual, "@Actual"));
                if (name != null) command.Parameters.Add(ParamReg(name, "@Name"));
                if (place != null) command.Parameters.Add(ParamReg(place, "@Place"));
                if (seats != null) command.Parameters.Add(ParamReg(seats, "@Seats"));
                if (describe != null) command.Parameters.Add(ParamReg(describe, "@Describe"));
                if (startTime != null) command.Parameters.Add(ParamReg(startTime, "@StartTime"));
                if (endTime != null) command.Parameters.Add(ParamReg(endTime, "@EndTime"));
                if (author != null) command.Parameters.Add(ParamReg(author, "@Author"));
                if (createTime != null) command.Parameters.Add(ParamReg(createTime, "@CreateTime"));

                SqlDataReader reader = command.ExecuteReader();

                for (int i = 0; i < int.MaxValue; i++)
                    if (reader.Read())
                        result.Add(new Event((long)reader["Author"])
                        {
                            Id = (int)reader["Id"],
                            IsActual = (bool)reader["Actual"],
                            Name = (string)reader["Name"],
                            Place = (string)reader["Place"],
                            Seats = (int)reader["Seats"],
                            Describe = (string)reader["Describe"],
                            StartTime = (DateTime)reader["StartTime"],
                            EndTime = (DateTime)reader["EndTime"],
                            CreateTime = (DateTime)reader["CreateTime"]
                        });
                    else break;
            }
            return result;
        }

        internal static void Set(int id, bool? isActual = null, string name = null, string place = null, int? seats = null,
            string describe = null, DateTime? startTime = null, DateTime? endTime = null, string author = null, DateTime? createTime = null)
        {

            var result = new List<Event>();
            string sqlExpression = "EventSetParams";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(sqlExpression, connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                command.Parameters.Add(ParamReg(id, "@Id"));
                if (isActual != null) command.Parameters.Add(ParamReg(isActual, "@Actual"));
                if (name != null) command.Parameters.Add(ParamReg(name, "@Name"));
                if (place != null) command.Parameters.Add(ParamReg(place, "@Place"));
                if (seats != null) command.Parameters.Add(ParamReg(seats, "@Seats"));
                if (describe != null) command.Parameters.Add(ParamReg(describe, "@Describe"));
                if (startTime != null) command.Parameters.Add(ParamReg(startTime, "@StartTime"));
                if (endTime != null) command.Parameters.Add(ParamReg(endTime, "@EndTime"));
                if (author != null) command.Parameters.Add(ParamReg(author, "@Author"));
                if (createTime != null) command.Parameters.Add(ParamReg(createTime, "@CreateTime"));

                SqlDataReader reader = command.ExecuteReader();
            }
        }

    }
}
