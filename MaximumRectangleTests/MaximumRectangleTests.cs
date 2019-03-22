namespace MaximumRectangleTests
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using MaximumRectangle;
    using Microsoft.VisualStudio.TestTools.UnitTesting;


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

            var sut = new IntRectangleHelper();

            var actual = sut.FindMaximalRectangle(matrix, matrix.Count, matrix.First().Length);

            TestContext.WriteLine($"The maximal rectangle has area {actual.Area()}");
            TestContext.WriteLine(
                $"Location: [col={actual.X}, row={actual.Y}] to [col={actual.Right}, row={actual.Bottom}]");

            var expected = new Rectangle(6, 4, 6, 2);
            Assert.AreEqual(expected, actual);
        }
    }
}
