using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Probability
{
    class ExternalRunner
    {
        Logger logger;
        Rules rules;
        public string name;


        //Imported from ArenaAugmented:


        int maxGameOverPathLengthEvolution1;

        public int[][][][] mask4;
        public int[][][] mask3;
        public int[][] mask2;
        public int[] mask1;
        public int mask0;

        public int[][][] matrix3;
        public int[][] matrix2;
        public int[] matrix1;
        public int matrix0;

        public double[][] matrixCoins;

        public double[] wonCoins;
        public int wonCoinsLength;

        public int[][] path2;
        public int[] path1;
        public int path0;




        //[DllImport(@"..\..\..\debug\ExternalC.dll", CallingConvention = CallingConvention.Cdecl)]
        //public static extern void DisplayHelloFromDLL(int length, double[] dArray);


        [DllImport(@"..\..\..\debug\CudaRunner.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CudaAdd(int length, int[] a, int[] b, int[] c);


        public void initialise(int maxGameOverPathLengthEvolution1, int[][][][] mask4, int[][][] mask3, int[][] mask2, int[] mask1, int mask0, int[][][] matrix3, int[][] matrix2, int[] matrix1, int matrix0, double[][] matrixCoins, double[] wonCoins, int wonCoinsLength, int[][] path2, int[] path1, int path0)
        {
            this.maxGameOverPathLengthEvolution1 = maxGameOverPathLengthEvolution1;
            this.mask4 = mask4;
            this.mask3 = mask3;
            this.mask2 = mask2;
            this.mask1 = mask1;
            this.mask0 = mask0;
            this.matrix3 = matrix3;
            this.matrix2 = matrix2;
            this.matrix1 = matrix1;
            this.matrix0 = matrix0;
            this.matrixCoins = matrixCoins;
            this.wonCoins = wonCoins;
            this.wonCoinsLength = wonCoinsLength;
            this.path2 = path2;
            this.path1 = path1;
            this.path0 = path0;
        }

        public double calculateAntiPlayer(double[] brainCells, int allBrainCellsCount)
        {
            for (int i0 = 0; i0 < matrix0; i0++)
            {
                double sum = 0.0d;
                for (int i1 = 0; i1 < matrix1[i0]; i1++)
                {
                    double multiplication = matrixCoins[i0][i1];
                    for (int i2 = 0; i2 < matrix2[i0][i1]; i2++)
                    {
                        multiplication *= brainCells[matrix3[i0][i1][i2]];
                    }
                    sum += multiplication;
                }
                wonCoins[i0] = sum;
            }
            int pathLength = maxGameOverPathLengthEvolution1;
            for (int i0 = 0; i0 < mask0; i0++)
            {
                for (int i1 = 0; i1 < mask1[i0]; i1++)
                {
                    double? bestOptionABenefit = null;
                    int bestOptionAChoiceLocation = -1;
                    for (int i2 = 0; i2 < mask2[i0][i1]; i2++)
                    {
                        double sumOfAllDiceCombinationsBenefit = 0;
                        for (int i3 = 0; i3 < mask3[i0][i1][i2]; i3++)
                        {
                            sumOfAllDiceCombinationsBenefit += wonCoins[mask4[i0][i1][i2][i3]];
                        }
                        if (bestOptionABenefit == null || sumOfAllDiceCombinationsBenefit > bestOptionABenefit)
                        {
                            bestOptionABenefit = sumOfAllDiceCombinationsBenefit;
                            bestOptionAChoiceLocation = i2;
                        }
                    }
                    for (int i2 = 0; i2 < mask2[i0][i1]; i2++)
                    {
                        for (int i3 = 0; i3 < mask3[i0][i1][i2]; i3++)
                        {
                            double newBrainCellValue = (i2 == bestOptionAChoiceLocation) ? 1.0d : 0.0d;
                            brainCells[path2[mask4[i0][i1][i2][i3]][pathLength - 1]] = newBrainCellValue;
                            wonCoins[mask4[i0][i1][i2][i3]] *= newBrainCellValue;
                        }
                    }
                }
                pathLength--;
            }
            double sumOfCoins = 0;
            for (int i = 0; i < wonCoinsLength; i++)
            {
                sumOfCoins += wonCoins[i];
            }
            return sumOfCoins;
        }

        public ExternalRunner(Logger logger, Rules rules, string name = "")
        {
            logger.set("ExternalRunner", 10, Color.Red);
            this.logger = logger;
            this.rules = rules;
            this.name = name;

        }
        public void helloCGPU()
        {
            logger.log("Enterring C module on CPU", 5, "ExternalRunner");



            for (int j = 0; j < 1000; j += 10)
            {

                // Create new stopwatch
                Stopwatch stopwatch = new Stopwatch();

                // Begin timing
                stopwatch.Start();

                int length = j;
                int[] a = new int[length];
                int[] b = new int[length];
                int[] c = new int[length];
                for (int i = 0; i < length; i++)
                {
                    a[i] = 100 * i;
                    b[i] = 10 * i;
                }
                CudaAdd(length, a, b, c);

                // Stop timing
                stopwatch.Stop();

                // Write result
                logger.logChart(stopwatch.ElapsedMilliseconds);

            }

            //logger.log("Result " + rules.intListToString(c.ToList()), 5, "ExternalRunner");
            logger.log("Returned from C module on CPU", 5, "ExternalRunner");
        }
    }
}
