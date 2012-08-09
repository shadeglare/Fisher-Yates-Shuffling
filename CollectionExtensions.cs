using System;
using System.Collections.Generic;
using System.Linq;

namespace Shadeglare.Helpers
{
    /// <summary>
    /// Contains array extensions methods.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Shuffles an array by Fisher-Yates algorithm.
        /// </summary>
        /// <typeparam name="T">A type of a given array.</typeparam>
        /// <param name="arr">An array for a shuffle.</param>
        public static void Shuffle<T>(this IList<T> collection)
        {
            Shuffle<T>(collection, 0, collection.Count - 1);
        }

        /// <summary>
        /// Shuffles an array by Fisher-Yates algorithm.
        /// </summary>
        /// <typeparam name="T">A type of a given array.</typeparam>
        /// <param name="arr">An array for a shuffle.</param>
        /// <param name="beginPosition">A start position in an array from where to shuffle.</param>
        /// <param name="endPosition">A end position in an array from where to shuffle.</param>
        public static void Shuffle<T>(this IList<T> collection, Int32 beginPosition, Int32 endPosition)
        {
            Shuffle<T>(collection, beginPosition, endPosition, null);
        }

        /// <summary>
        /// Shuffles an array by Fisher-Yates algorithm.
        /// </summary>
        /// <typeparam name="T">A type of a given array.</typeparam>
        /// <param name="arr">An array for a shuffle.</param>
        /// <param name="beginPosition">A start position in an array from where to shuffle.</param>
        /// <param name="endPosition">A end position in an array from where to shuffle.</param>
        /// <param name="movableElementsIndicies">Indicies of movable elements. 
        /// Required only sorted by ascendent array without repeated indicies.
        /// </param>
        public static void Shuffle<T>(this IList<T> collection, Int32 beginPosition, Int32 endPosition, IEnumerable<Int32> movableElementsIndicies)
        {
            //Check bounds
            if (beginPosition < 0 || endPosition >= collection.Count || beginPosition > endPosition)
            {
                throw new IndexOutOfRangeException("Bounds are out of range.");
            }

            Random shuffleRandom = new Random();

            if (movableElementsIndicies == null)
            {
                for (Int32 i = endPosition + 1; i > beginPosition + 1; --i)
                {
                    int j = shuffleRandom.Next(beginPosition, i);
                    T temp = collection[j];
                    collection[j] = collection[i - 1];
                    collection[i - 1] = temp;
                }
            }
            else
            {
                Int32 indiciesCount = movableElementsIndicies.Count();

                if (indiciesCount == 0)
                {
                    return;
                }
                else if (indiciesCount == 1)
                {
                    //Check a movable index
                    if (movableElementsIndicies.ElementAt(0) < beginPosition ||
                          movableElementsIndicies.ElementAt(0) > endPosition)
                    {
                        throw new IndexOutOfRangeException("An index of a movable element is out of range or in a wrong position");
                    }
                    return;
                }
                else
                {
                    //Check movable indicies
                    for (Int32 i = 0; i < indiciesCount - 1; i++)
                    {
                        if (movableElementsIndicies.ElementAt(i) < beginPosition ||
                           movableElementsIndicies.ElementAt(i) > endPosition ||
                           movableElementsIndicies.ElementAt(i) >= movableElementsIndicies.ElementAt(i + 1))
                        {
                            throw new IndexOutOfRangeException("An index of a movable element is out of range or in a wrong position");
                        }
                    }

                    for (Int32 i = indiciesCount; i > 0; i--)
                    {
                        Int32 j = -1;
                        while (!movableElementsIndicies.Contains(j))
                        {
                            j = shuffleRandom.Next(movableElementsIndicies.ElementAt(0), movableElementsIndicies.ElementAt(i - 1));

                        }
                        T temp = collection[j];
                        collection[j] = collection[movableElementsIndicies.ElementAt(i - 1)];
                        collection[movableElementsIndicies.ElementAt(i - 1)] = temp;
                    }
                }
            }
        }
    }
}
