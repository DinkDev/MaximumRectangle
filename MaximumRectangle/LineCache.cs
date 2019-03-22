namespace MaximumRectangle
{
    using System.Collections.Generic;
    using System.Linq;

    public class LineCache
    {
        public readonly List<int> _lineCache; // cache

        public LineCache(int capacity)
        {
            _lineCache = new List<int>();
            _lineCache.AddRange(Enumerable.Repeat(0, capacity));
        }

        private void update_cache(IList<int> b)
        {
            for (var m = 0; m != b.Count; ++m)
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
    }
}
