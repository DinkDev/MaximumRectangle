﻿namespace MaximumRectangleTests
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class PointClass
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    /// <summary>
    /// Finally found this: https://en.wikipedia.org/wiki/Largest_empty_rectangle
    /// This is a historic discrete mathematics problem!
    /// After some related searching, I found the Dr. Dobb's article "The Maximal Rectangle Problem"
    /// by David Vandevoorde at http://www.drdobbs.com/database/the-maximal-rectangle-problem/184410529
    /// David also posted the code at: https://stackoverflow.com/questions/7245/puzzle-find-largest-rectangle-maximal-rectangle-problem
    /// </summary>
    [TestClass]
    public class MaximumRectangleTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void MaximumRectangle_Test1()
        {
            var matrix = new List<int[]>
            {
                //     0  1  2  3  4  5  6  7  8  9 10 11 12 13 14 15
                new[] {0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, // 0
                new[] {0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0}, // 1
                new[] {0, 0, 0, 1, 1, 0, 0, 1, 0, 0, 0, 1, 1, 0, 1, 0}, // 2
                new[] {0, 0, 0, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 0, 1, 0}, // 3
                new[] {0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 0}, // 4
                new[] {0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 1, 0}, // 5
                new[] {0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 1, 1, 1, 1, 1, 0}, // 6
                new[] {0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 1, 1, 1, 0, 1, 0}, // 7
                new[] {0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, // 8
                new[] {0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, // 9
                new[] {0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0}, // 10
                new[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0}, // 11
            };

            var result = MainBody(matrix);

            Assert.AreEqual(new Rectangle(6, 4, 6, 2), result);
        }

        private Rectangle MainBody(IList<int[]> matrix)
        {
            int m;
            int n;

            var rows = matrix.Count();
            var columns = matrix.First().Count();

            var lineCache = Enumerable.Repeat(0, columns + 1).ToList();
            var widthStack = new Stack<PointClass>(columns + 1);
            for (m = 0; m < columns + 1; ++m)
            {
                widthStack.Push(new PointClass {X = 0, Y = 0});
            }

            var bestArea = 0;
            var bestLowerLeft = new PointClass {X = 0, Y = 0};
            var bestUpperRight = new PointClass {X = -1, Y = -1};

            // main loop
            for (n = 0; n != rows; ++n)
            {
                var openRectWidth = 0;
                UpdateCache(lineCache, matrix[n]);
                for (m = 0; m != columns + 1; ++m)
                {
                    if (lineCache[m] > openRectWidth)
                    {
                        /* Open new rectangle? */
                        widthStack.Push(new PointClass {X = m, Y = openRectWidth});
                        openRectWidth = lineCache[m];
                    }
                    else /* "else" optional here */
                    if (lineCache[m] < openRectWidth)
                    {
                        /* Close rectangle(s)? */
                        int m0, w0;
                        do
                        {
                            var val = widthStack.Pop();
                            m0 = val.X;
                            w0 = val.Y;

                            //pop(&m0, &w0);
                            var area = openRectWidth * (m - m0);
                            if (area > bestArea)
                            {
                                bestArea = area;
                                bestLowerLeft.X = m0;
                                bestLowerLeft.Y = n;
                                bestUpperRight.X = m - 1;
                                bestUpperRight.Y = n - openRectWidth + 1;
                            }

                            openRectWidth = w0;
                        } while (lineCache[m] < openRectWidth);

                        openRectWidth = lineCache[m];
                        if (openRectWidth != 0)
                        {
                            widthStack.Push(new PointClass {X = m0, Y = w0});
                        }
                    }
                }
            }

            TestContext.WriteLine($"The maximal rectangle has area {bestArea}");
            TestContext.WriteLine(
                $"Location: [col={bestLowerLeft.X}, row={bestLowerLeft.Y}] to [col={bestUpperRight.X}, row={bestUpperRight.Y}]");

            return new Rectangle(bestLowerLeft.X, bestUpperRight.Y,
                Math.Abs(bestUpperRight.X - bestLowerLeft.X) + 1, Math.Abs(bestUpperRight.Y - bestLowerLeft.Y) + 1);
        }

        private void UpdateCache(IList<int> cache, IList<int> row)
        {
            for (var x = 0; x != row.Count; ++x)
            {
                // TODO: will need to invert for images
                cache[x] = row[x] == 0 ? 0 : cache[x] + 1;
            }
        }
    }
}
