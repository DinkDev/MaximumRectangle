namespace MaximumRectangle
{
    public class IntRectangleHelper : RectangleHelperBase<int>
    {
        protected override bool IsSet(int value)
        {
            return value == 0;
        }
    }
}