using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace vkBot.General
{
    internal static class DbGeneral
    {
        internal static string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=vkDatabase;Integrated Security=True";
        internal static SqlParameter ParamReg(object value, string paramName)
        {
            return new SqlParameter
            {
                ParameterName = paramName,
                Value = value
            };
        }
    }
}
