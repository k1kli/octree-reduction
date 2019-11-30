using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Octree_reduction
{
    public static class ExtensionMethods
    {
        public static void Sort<T>(this LinkedList<T> sortedList, Comparison<T> comparision)
        {
            if (sortedList.Count <= 1) return;
            LinkedList<T> greater = new LinkedList<T>();
            LinkedList<T> smaller = new LinkedList<T>();
            LinkedList<T> equal = new LinkedList<T>();
            equal.AddLast(sortedList.First());
            sortedList.RemoveFirst();
            while (sortedList.Count > 0)
            {
                T element = sortedList.First();
                sortedList.RemoveFirst();
                int comparisionResult = comparision.Invoke(element, equal.First());
                if (comparisionResult > 0)
                {
                    greater.AddLast(element);
                }
                else if (comparisionResult == 0)
                {
                    equal.AddLast(element);
                }
                else
                {
                    smaller.AddLast(element);
                }
            }
            greater.Sort(comparision);
            smaller.Sort(comparision);
            foreach (T el in smaller)
            {
                sortedList.AddLast(el);
            }
            foreach (T el in equal)
            {
                sortedList.AddLast(el);
            }
            foreach (T el in greater)
            {
                sortedList.AddLast(el);
            }
        }
    }
}
