namespace MaximumRectangleTests
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using MaximumRectangle;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

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

            var actual = MainBody(matrix);

            TestContext.WriteLine($"The maximal rectangle has area {actual.Area()}");
            TestContext.WriteLine(
                $"Location: [col={actual.X}, row={actual.Y}] to [col={actual.Right}, row={actual.Bottom}]");

            var expected = new Rectangle(6, 4, 6, 2);
            Assert.AreEqual(expected, actual);
        }

        private Rectangle MainBody(IList<int[]> matrix)
        {
            var columns = matrix.First().Length;

            var lineCache = Enumerable.Repeat(0, columns + 1).ToList();
            var widthStack = new Stack<Point>(Enumerable.Repeat(new Point(0, 0), columns + 1));

            var bestRectangle = Rectangle.Empty;

            for (var line = 0; line != matrix.Count; ++line)
            {
                var openRectWidth = 0;
                UpdateCache(lineCache, matrix[line]);

                for (var m = 0; m != columns + 1; ++m)
                {
                    if (lineCache[m] > openRectWidth)
                    {
                        /* Open new rectangle? */
                        widthStack.Push(new Point(m, openRectWidth));
                        openRectWidth = lineCache[m];
                    }
                    else if (lineCache[m] < openRectWidth)
                    {
                        /* Close rectangle(s)? */
                        int m0, w0;
                        do
                        {
                            var val = widthStack.Pop();
                            m0 = val.X;
                            w0 = val.Y;

                            var area = openRectWidth * (m - m0);
                            if (area > bestRectangle.Area())
                            {
                                bestRectangle = new Rectangle(m0, line - openRectWidth + 1, m - m0, openRectWidth);
                            }

                            openRectWidth = w0;

                        } while (lineCache[m] < openRectWidth);

                        openRectWidth = lineCache[m];
                        if (openRectWidth != 0)
                        {
                            widthStack.Push(new Point(m0, w0));
                        }
                    }
                }
            }

            return bestRectangle;
        }

        private void UpdateCache(IList<int> cache, IList<int> newLine)
        {
            for (var x = 0; x != newLine.Count; ++x)
            {
                // TODO: will need to invert for images
                cache[x] = IsSet(newLine[x]) ? 0 : cache[x] + 1;
            }
        }

        public bool IsSet(int value)
        {
            return value == 0;
        }
    }
}
