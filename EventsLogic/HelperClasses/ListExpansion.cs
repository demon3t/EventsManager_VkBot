using EventsLogic.HeplerInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventsLogic.HelperClasses
{
    public static class ListExpansion
    {
        public static List<T> DateTimeSort<T>(this List<T> list) where T : IListDataTime
        {
            for (int i = 0; i < list.Count; i++)
                for (int j = 0; j < list.Count - i-1; j++)
                    if (list[j].SortDateTime() > list[j + 1].SortDateTime())
                    {
                        var temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;
                    }

            return list;
        }

        public static void DateTimeAdd()
        {


        }

    }
}
