namespace MaximumRectangle
{
    using System.Drawing;

    public static class RectangleExtensions
    {
        public static int Area(this Rectangle rectangle)
        {
            return rectangle.Width * rectangle.Height;
        }
    }
}