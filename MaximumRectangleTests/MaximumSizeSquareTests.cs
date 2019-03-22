namespace MaximumRectangleTests
{
    using System;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MaximumSizeSquareTests
    {
        public TestContext TestContext { get; set; }

        /// <summary>
        /// Maximum size square sub-matrix with all 1s
        /// https://www.geeksforgeeks.org/maximum-size-sub-matrix-with-all-1s-in-a-binary-matrix/
        /// </summary>
        /// <remarks>
        /// Not likely to be able to extend to rectangular area.
        /// </remarks>
        [TestMethod]
        public void MaximumSizeSquare_Test1()
        {
            var M = new[,]
            {
                {0, 1, 1, 0, 0, 0, 1},
                {1, 1, 0, 1, 1, 1, 0},
                {0, 1, 1, 1, 1, 1, 0},
                {1, 1, 1, 1, 1, 1, 0},
                {1, 1, 1, 1, 1, 1, 1},
                {0, 0, 0, 0, 0, 0, 0}
            };

            PrintMaxSubSquare(M);
        }

        // method for Maximum size square sub-matrix with all 1s 
        private void PrintMaxSubSquare(int[,] M)
        {
            int i, j;
            //no of rows in M[,] 
            var R = M.GetLength(0);
            //no of columns in M[,] 
            var C = M.GetLength(1);
            var S = new int[R, C];

            /* Set first column of S[,]*/
            for (i = 0; i < R; i++)
            {
                S[i, 0] = M[i, 0];
            }

            /* Set first row of S[][]*/
            for (j = 0; j < C; j++)
            {
                S[0, j] = M[0, j];
            }

            /* Construct other entries of S[,]*/
            for (i = 1; i < R; i++)
            {
                for (j = 1; j < C; j++)
                {
                    if (M[i, j] == 1)
                        S[i, j] = Math.Min(S[i, j - 1],
                                      Math.Min(S[i - 1, j], S[i - 1, j - 1])) + 1;
                    else
                        S[i, j] = 0;
                }
            }

            /* Find the maximum entry, and indexes of  
                maximum entry in S[,] */
            var maxOfS = S[0, 0];
            var maxI = 0;
            var maxJ = 0;
            for (i = 0; i < R; i++)
            {
                for (j = 0; j < C; j++)
                {
                    if (maxOfS < S[i, j])
                    {
                        maxOfS = S[i, j];
                        maxI = i;
                        maxJ = j;
                    }
                }
            }

            TestContext.WriteLine("Maximum size sub-matrix is: ");
            for (i = maxI; i > maxI - maxOfS; i--)
            {
                var sb = new StringBuilder();

                for (j = maxJ; j > maxJ - maxOfS; j--)
                {
                    sb.Append(M[i, j] + " ");
                }

                TestContext.WriteLine(sb.ToString());
            }
        }
    }
}
