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
            var node = sortedList.First;
            sortedList.RemoveFirst();
            equal.AddLast(node);
            while (sortedList.Count > 0)
            {
                node = sortedList.First;
                sortedList.RemoveFirst();
                int comparisionResult = comparision.Invoke(node.Value, equal.First());
                if (comparisionResult > 0)
                {
                    greater.AddLast(node);
                }
                else if (comparisionResult == 0)
                {
                    equal.AddLast(node);
                }
                else
                {
                    smaller.AddLast(node);
                }
            }
            greater.Sort(comparision);
            smaller.Sort(comparision);
            while (smaller.First != null)
            {
                node = smaller.First;
                smaller.RemoveFirst();
                sortedList.AddLast(node);
            }
            while (equal.First != null)
            {
                node = equal.First;
                equal.RemoveFirst();
                sortedList.AddLast(node);
            }
            while (greater.First != null)
            {
                node = greater.First;
                greater.RemoveFirst();
                sortedList.AddLast(node);
            }
        }
    }
}
