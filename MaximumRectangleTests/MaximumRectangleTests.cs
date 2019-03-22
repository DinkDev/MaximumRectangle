namespace MaximumRectangleTests
{
    using System.Collections.Generic;
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
            MainBody();
        }

        private List<int> _lineCache; // cache
        private Stack<PointClass> _stack; // stack

        private int _xDim;  // columns
        private int _yDim;  // rows

        void update_cache(int[] b)
        {
            for (var m = 0; m != _xDim; ++m)
            {
                if (b[m] == 0)
                {
                    _lineCache[m] = 0;
                }
                else
                {
                    _lineCache[m] = _lineCache[m] + 1;
                }
            }
        }

        void MainBody()
        {
            int m, n;
            _xDim = 16; // columns
            _yDim = 12; // rows

            var matrix = new List<int[]>
            {
                new[] {0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new[] {0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0},
                new[] {0, 0, 0, 1, 1, 0, 0, 1, 0, 0, 0, 1, 1, 0, 1, 0},
                new[] {0, 0, 0, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 0, 1, 0},
                new[] {0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 0},
                new[] {0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 1, 0},
                new[] {0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 1, 1, 1, 1, 1, 0},
                new[] {0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 1, 1, 1, 0, 1, 0},
                new[] {0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new[] {0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new[] {0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0},
                new[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0},
            };

            _lineCache = new List<int>(_xDim + 1);
            _stack = new Stack<PointClass>(_xDim + 1);
            for (m = 0; m < _xDim + 1; ++m)
            {
                _lineCache.Insert(m, 0);
                _stack.Push(new PointClass { X = 0, Y = 0 });
            }

            var bestArea = 0;
            var bestLowerLeft = new PointClass { X = 0, Y = 0 };
            var bestUpperRight = new PointClass { X = -1, Y = -1 };

            // main loop
            for (n = 0; n != _yDim; ++n)
            {
                var openRectWidth = 0;
                update_cache(matrix[n]);
                for (m = 0; m != _xDim + 1; ++m)
                {
                    if (_lineCache[m] > openRectWidth)
                    {
                        /* Open new rectangle? */
                        _stack.Push(new PointClass { X = m, Y = openRectWidth });
                        openRectWidth = _lineCache[m];
                    }
                    else /* "else" optional here */
                    if (_lineCache[m] < openRectWidth)
                    {
                        /* Close rectangle(s)? */
                        int m0, w0;
                        do
                        {
                            var val = _stack.Pop();
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
                        } while (_lineCache[m] < openRectWidth);

                        openRectWidth = _lineCache[m];
                        if (openRectWidth != 0)
                        {
                            _stack.Push(new PointClass { X = m0, Y = w0 });
                        }
                    }
                }
            }

            TestContext.WriteLine($"The maximal rectangle has area {bestArea}");
            TestContext.WriteLine($"Location: [col={bestLowerLeft.X}, row={bestLowerLeft.Y}] to [col={bestUpperRight.X}, row={bestUpperRight.Y}]");
        }
    }
}
