namespace MaximumRectangleTests
{
    using System.Collections.Generic;
    using MaximumRectangle;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ListExtensionsTests
    {
        [TestMethod]
        public void ListExtensions_ListOfList_Test1()
        {
            var sut = new List<List<int>>
            {
                new List<int> { 1, 2, 3, 4 },
                new List<int> { 9, 8, 7, 6 }
            };

            var result = sut.ToMultidimensionalArray();

            for (var y = 0; y < sut.Count; y++)
            {
                for (var x = 0; x < sut[y].Count; x++)
                {
                    Assert.AreEqual(sut[y][x], result[y,x]);
                }
            }
        }

        [TestMethod]
        public void ListExtensions_ListOfArray_Test1()
        {
            var sut = new List<int[]>
            {
                new List<int> { 1, 2, 3, 4 }.ToArray(),
                new List<int> { 9, 8, 7, 6 }.ToArray()
            };

            var result = sut.ToMultidimensionalArray();

            for (var y = 0; y < sut.Count; y++)
            {
                for (var x = 0; x < sut[y].Length; x++)
                {
                    Assert.AreEqual(sut[y][x], result[y, x]);
                }
            }
        }
    }
}
