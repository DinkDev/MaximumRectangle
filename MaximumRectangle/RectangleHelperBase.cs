namespace MaximumRectangle
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    /// <summary>
    /// Finally found this: https://en.wikipedia.org/wiki/Largest_empty_rectangle
    /// This is a historic discrete mathematics problem!
    /// After some related searching, I found the Dr. Dobb's article "The Maximal Rectangle Problem"
    /// by David Vandevoorde at http://www.drdobbs.com/database/the-maximal-rectangle-problem/184410529
    /// David also posted the code at: https://stackoverflow.com/questions/7245/puzzle-find-largest-rectangle-maximal-rectangle-problem
    /// </summary>
    public abstract class RectangleHelperBase<TLine>
    {
        public Rectangle FindMaximalRectangle(IEnumerable<TLine> matrix, int lines, int columns)
        {
            var lineCache = Enumerable.Repeat(0, columns + 1).ToList();
            // note that the Y value of Point is used for Height
            var heightStack = new Stack<Point>(Enumerable.Repeat(new Point(0, 0), columns + 1));

            var bestRectangle = Rectangle.Empty;

            var line = 0;
            foreach (var nextLine in matrix)
            { 
                UpdateCache(lineCache, nextLine, columns);
                var openRectHeight = 0;

                for (var col = 0; col < columns + 1; ++col)
                {
                    if (lineCache[col] > openRectHeight)
                    {
                        // Open new rectangle?
                        heightStack.Push(new Point(col, openRectHeight));
                        openRectHeight = lineCache[col];
                    }
                    else if (lineCache[col] < openRectHeight)
                    {
                        // Close rectangle(s)?
                        Point val;

                        do
                        {
                            val = heightStack.Pop();

                            var area = openRectHeight * (col - val.X);
                            if (area > bestRectangle.Area())
                            {
                                bestRectangle = new Rectangle(val.X, line - openRectHeight + 1, col - val.X, openRectHeight);
                            }

                            openRectHeight = val.Y;

                        } while (lineCache[col] < openRectHeight);

                        openRectHeight = lineCache[col];
                        if (openRectHeight != 0)
                        {
                            heightStack.Push(val);
                        }
                    }
                }

                line++;
            }

            return bestRectangle;
        }

        private void UpdateCache(IList<int> cache, TLine line, int columns)
        {
            for (var x = 0; x < columns; ++x)
            {
                cache[x] = IsSet(line, x) ? 0 : cache[x] + 1;
            }
        }

        protected abstract bool IsSet(TLine line, int column);
    }
}
