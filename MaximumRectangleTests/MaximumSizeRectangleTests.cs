namespace MaximumRectangleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MaximumSizeRectangleTests
    {
        public TestContext TestContext { get; set; }

        /// <summary>
        /// Maximum size rectangle binary sub-matrix with all 1s
        /// https://www.geeksforgeeks.org/maximum-size-rectangle-binary-sub-matrix-1s/
        /// </summary>
        /// <remarks>
        /// Will be difficult to extend this to finding coordinates.
        /// </remarks>
        [TestMethod]
        public void MaximumSizeRectangle_Test1()
        {
            var matrix = new[,]
            {
                { 0, 1, 1, 0 },
                { 1, 1, 1, 1 },
                { 1, 1, 1, 1 },
                { 1, 1, 0, 0 },
                { 0, 1, 0, 0 },
            };

            TestContext.WriteLine($"Area of maximum rectangle is {LargestRectangle(matrix)}");
        }

        /// <summary>
        /// Returns area of the largest rectangle with all 1s in matrix.
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns>Area of the largest rectangle</returns>
        private int LargestRectangle(int[,] matrix)
        {
            var rows = matrix.GetLength(0);
            var columns = matrix.GetLength(1);

            // Calculate area for first row and initialize it as the result 
            var result = MaxHistogram(columns, matrix, 0);

            PrintRow(columns, matrix, 0, result);

            // iterate over row to find maximum rectangular area 
            // considering each row as histogram 
            for (var r = 1; r < rows; r++)
            {
                for (var c = 0; c < columns; c++)
                {
                    if (matrix[r, c] == 1)
                    {
                        matrix[r, c] += matrix[r - 1, c];
                    }
                }

                // Update result if area with current row (as last 
                // row of rectangle) is more 
                var rowHistogram = MaxHistogram(columns, matrix, r);

                PrintRow(columns, matrix, r, rowHistogram);

                result = Math.Max(result, rowHistogram);
            }

            return result;
        }

        private void PrintRow(int columns, int[,] matrix, int r, int rowHistogram)
        {
            var sb = new StringBuilder();

            for (var c = 0; c < columns; c++)
            {
                sb.Append($"{matrix[r, c]}, ");
            }

            Console.WriteLine($"{sb} - row histogram: {rowHistogram}");
        }

        // Finds the maximum area under the histogram represented 
        // by histogram.  See below article for details. 
        // https://www.geeksforgeeks.org/largest-rectangle-under-histogram/ 
        private int MaxHistogram(int columns, int[,] matrix, int rowNum)
        {
            // Create an empty stack. The stack holds indexes of 
            // hist[] array/ The bars stored in stack are always 
            // in increasing order of their heights. 
            var result = new Stack<int>();

            int topValue; // Top of stack 

            var maxArea = 0; // Initialize max area in current 
            // row (or histogram) 

            int area; // Initialize area with current top 

            // Run through all bars of given histogram (or row) 
            var c = 0;
            while (c < columns)
            {
                // If this bar is higher than the bar on top stack, 
                // push it to stack 
                if (!result.Any() || matrix[rowNum, result.Peek()] <= matrix[rowNum, c])
                {
                    result.Push(c);
                    c++;
                }
                else
                {
                    // If this bar is lower than top of stack, then 
                    // calculate area of rectangle with stack top as 
                    // the smallest (or minimum height) bar. 'i' is 
                    // 'right index' for the top and element before 
                    // top in stack is 'left index' 
                    topValue = matrix[rowNum, result.Peek()];
                    result.Pop();
                    area = topValue * c;

                    if (result.Any())
                    {
                        area = topValue * (c - result.Peek() - 1);
                    }
                    maxArea = Math.Max(area, maxArea);
                }
            }

            // Now pop the remaining bars from stack and calculate  
            // area with every popped bar as the smallest bar 
            while (result.Any())
            {
                topValue = matrix[rowNum, result.Peek()];
                result.Pop();
                area = topValue * c;
                if (result.Any())
                {
                    area = topValue * (c - result.Peek() - 1);
                }

                maxArea = Math.Max(area, maxArea);
            }

            return maxArea;
        }
    }
}
