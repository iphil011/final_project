using System.Collections.Generic;

namespace AlanZucconi
{
    public static class Counting
    {
        // All distinct pairs of elements from a list
        public static IEnumerable<List<T>> DistinctPairs<T>(this IList<T> list)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                for (int j = i + 1; j < list.Count; j++)
                {
                    List<T> pair = new List<T>();
                    pair.Add(list[i]);
                    pair.Add(list[j]);

                    yield return pair;
                }
            }
        }
    }
}