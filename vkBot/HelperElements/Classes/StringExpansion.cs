using System;
using System.Collections.Generic;
using System.Text;

namespace vkBot.HelperElements.Classes
{
    internal static class StringExpansion
    {
        internal static bool AdvansedParse(this string str, out DateTime result)
        {
            result = DateTime.Now;

            if (string.IsNullOrWhiteSpace(str)) return false;
            if (DateTime.TryParse(str, out result))
            {
                return true;
            }

            result = DateTime.Now;

            if (str.StartsWith("Сегодня"))
            {
                str = str.Replace("Сегодня", "");
            }
            else if (str.StartsWith("Завтра"))
            {
                result = result.AddDays(1);
                str = str.Replace("Завтра", "");
            }
            else if (str.StartsWith("Послезавтра"))
            {
                result = result.AddDays(2);
                str = str.Replace("Послезавтра", "");
            }
            else if (str.StartsWith("Через") && (str.Contains("дня") || str.Contains("дней")))
            {
                str = str.Replace("Через", "").Replace("дня", "").Replace("дней", "");

                if (TryParseToSeparator(out int day, ref str, ' ', 2))
                    result = result.AddDays(day + 1);
            }

            str = str.Replace(" ", "").Replace(".", ":").Replace(",", ":").Replace(";", ":");

            DateTime time;

            if (DateTime.TryParse(str, out time))
            {
                result = new DateTime(result.Year, result.Month, result.Day, time.Hour, time.Minute, time.Second);
                return true;
            }
            else if (str.Contains("в"))
            {
                str = str.Replace("в", "");
                if (DateTime.TryParse(str, out time))
                {
                    result = new DateTime(result.Year, result.Month, result.Day, time.Hour, time.Minute, time.Second);
                    return true;
                }
            }

            return false;
        }
        private static bool TryParseToSeparator(out int result, ref string str, char separator, int countSeparator)
        {
            result = 0;

            if (string.IsNullOrWhiteSpace(str)) return false;

            string resStr = string.Empty;
            int count = 0;

            for (int i = 0; i < str.Length; i++)
            {
                if (count == countSeparator)
                {
                    str = str.Remove(0, i);
                    break;
                }


                if (str[i] == separator)
                {
                    count++;
                    continue;
                }

                if (count == countSeparator - 1)
                    resStr += str[i];
            }

            int.TryParse(resStr, out result);

            return true;
        }


        internal static bool IsNaN(this string str)
        {
            return str == "NaN" || string.IsNullOrEmpty(str) ? true : false;
        }
    }
}
