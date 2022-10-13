using EventsLogic.HeplerInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventsLogic.HelperClasses
{
    public static class ListExpansion
    {
        public static List<T> SortByDateTime<T>(this List<T> obj)
            where T : IListDataTime
        {
            for (int i = 0; i < obj.Count; i++)
                for (int j = 0; j < obj.Count - i - 1; j++)
                    if (obj[j].SortDateTime() > obj[j + 1].SortDateTime())
                    {
                        var temp = obj[j];
                        obj[j] = obj[j + 1];
                        obj[j + 1] = temp;
                    }

            return obj;
        }
        public static List<T> AddByDateTime<T>(this List<T> obj, T item)
            where T : IListDataTime
        {
            for (int i = 0; i < obj.Count; i++)
                if (item.SortDateTime() < obj[i].SortDateTime())
                {
                    obj.Insert(i, item);
                    return obj;
                }
            obj.Add(item);
            return obj;
        }

    }
}
