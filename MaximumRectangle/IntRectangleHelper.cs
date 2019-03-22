namespace MaximumRectangle
{
    public class IntRectangleHelper : RectangleHelperBase<int[]>
    {
        protected override bool IsSet(int[] line, int column)
        {
            return line[column] == 0;
        }
    }
}
