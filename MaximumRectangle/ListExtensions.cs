namespace MaximumRectangle
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class ListExtensions
    {
        /// <summary>
        /// Converts a list of lists to a 2D array
        /// </summary>
        /// <typeparam name="T">The inner type in the list of lists</typeparam>
        /// <param name="inputList">The list of lists</param>
        /// <returns>The list of lists transformed to T[,]</returns>
        public static T[,] ToMultidimensionalArray<T>(this IList<IList<T>> inputList)
        {
            return ConvertCollectionToMultidimensionalArray(inputList);
        }

        /// <summary>
        /// Converts a list of lists to a 2D array
        /// </summary>
        /// <typeparam name="T">The inner type in the list of lists</typeparam>
        /// <param name="inputList">The list of lists</param>
        /// <returns>The list of lists transformed to T[,]</returns>
        public static T[,] ToMultidimensionalArray<T>(this IList<List<T>> inputList)
        {
            var transformed = inputList.Select(i => i as IList<T>).ToList();
            return ConvertCollectionToMultidimensionalArray(transformed);
        }

        /// <summary>
        /// Converts a list of arrays to a 2D array
        /// </summary>
        /// <typeparam name="T">The inner type in the list of lists</typeparam>
        /// <param name="inputList">The list of lists</param>
        /// <returns>The list of lists transformed to T[,]</returns>
        public static T[,] ToMultidimensionalArray<T>(this List<T[]> inputList)
        {
            var transformed = inputList.Select(i => i as IList<T>).ToList();
            return ConvertCollectionToMultidimensionalArray(transformed);
        }

        /// <summary>
        /// Converts a list of lists to a 2D array
        /// </summary>
        /// <typeparam name="T">The inner type in the list of lists</typeparam>
        /// <param name="inputList">The list of lists</param>
        /// <returns>The list of lists transformed to T[,]</returns>
        public static T[,] ConvertCollectionToMultidimensionalArray<T>(IList<IList<T>> inputList)
        {
            var rows = inputList.Count;

            if (rows == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(inputList), @"List must have at least 1 row");
            }

            var lengths = inputList.Select(l => l.Count).Distinct().ToArray();
            if (lengths.Length > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(inputList), "All arrays must be the same length");
            }

            var columns = lengths.First();
            if (columns < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(inputList), @"List must have at least 1 column");
            }

            // build and return multi-dimension (2D) array
            var rv = new T[rows, columns];

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    rv[i, j] = inputList[i][j];
                }
            }

            return rv;
        }
    }
}
