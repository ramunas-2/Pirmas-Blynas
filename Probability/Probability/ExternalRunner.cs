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

        //public int[][][][] mask4;
        //public int[][][] mask3;
        //public int[][] mask2;
        public int[] mask1;
        public int mask0;

        //public int[][][] matrix3;
        //public int[][] matrix2;
        public int[] matrix1;
        public int matrix0;

        //public double[][] matrixCoins;

        public double[] wonCoins;
        public int wonCoinsLength;

        //public int[][] path2;
        public int[] path1;
        public int path0;

        int[] sMask4;
        int k0mask4;
        int k1mask4;
        int k2mask4;
        int k3mask4;
        int kAmask4;

        int[] sMask3;
        int k0mask3;
        int k1mask3;
        int k2mask3;
        int kAmask3;

        int[] sMask2;
        int k0mask2;
        int k1mask2;
        int kAmask2;

        int[] smatrix3;
        int k0matrix3;
        int k1matrix3;
        int k2matrix3;
        int kAmatrix3;

        int[] smatrix2;
        int k0matrix2;
        int k1matrix2;
        int kAmatrix2;

        double[] smatrixCoins2;
        int k0matrixCoins2;
        int k1matrixCoins2;
        int kAmatrixCoins2;

        int[] spath2;
        int k0path2;
        int k1path2;
        int kApath2;

        //[DllImport(@"..\..\..\debug\ExternalC.dll", CallingConvention = CallingConvention.Cdecl)]
        //public static extern void DisplayHelloFromDLL(int length, double[] dArray);


        [DllImport(@"..\..\..\debug\CudaRunner.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CudaAdd(int length, int[] a, int[] b, int[] c);


        [DllImport(@"..\..\..\debug\CudaRunner.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern double calculateAntiPlayerExternal(double[] brainCells, int allBrainCellsCount, double[] result, int count);

        [DllImport(@"..\..\..\debug\CudaRunner.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void initialiseAntiPlayer(

            int maxGameOverPathLengthEvolution1,

            int[] mask1,
            int mask0,

            int[] matrix1,
            int matrix0,

            int[] path1,
            int path0,

            int[] sMask4,
            int k0mask4,
            int k1mask4,
            int k2mask4,
            int k3mask4,
            int kAmask4,

            int[] sMask3,
            int k0mask3,
            int k1mask3,
            int k2mask3,
            int kAmask3,

            int[] sMask2,
            int k0mask2,
            int k1mask2,
            int kAmask2,

            int[] smatrix3,
            int k0matrix3,
            int k1matrix3,
            int k2matrix3,
            int kAmatrix3,

            int[] smatrix2,
            int k0matrix2,
            int k1matrix2,
            int kAmatrix2,

            double[] smatrixCoins2,
            int k0matrixCoins2,
            int k1matrixCoins2,
            int kAmatrixCoins2,

            int[] spath2,
            int k0path2,
            int k1path2,
            int kApath2

            );

        public void initialise(int maxGameOverPathLengthEvolution1, int[][][][] mask4, int[][][] mask3, int[][] mask2, int[] mask1, int mask0, int[][][] matrix3, int[][] matrix2, int[] matrix1, int matrix0, double[][] matrixCoins, double[] wonCoins, int wonCoinsLength, int[][] path2, int[] path1, int path0)
        {
            this.maxGameOverPathLengthEvolution1 = maxGameOverPathLengthEvolution1;
            //this.mask4 = mask4;
            //this.mask3 = mask3;
            //this.mask2 = mask2;
            this.mask1 = mask1;
            this.mask0 = mask0;
            //this.matrix3 = matrix3;
            //this.matrix2 = matrix2;
            this.matrix1 = matrix1;
            this.matrix0 = matrix0;
            //this.matrixCoins = matrixCoins;
            this.wonCoins = wonCoins;
            this.wonCoinsLength = wonCoinsLength;
            //this.path2 = path2;
            this.path1 = path1;
            this.path0 = path0;


            //logger.log("i0 = " + k0mask4 + "; i1 = " + k1mask4 + "; i2 = " + k2mask4 + "; i3 = " + k3mask4 + "; total = " + iS);

            k0mask4 = mask0;
            analyse4DArray(mask1, mask2, mask3, ref k1mask4, ref k2mask4, ref k3mask4);
            kAmask4 = k0mask4 * k1mask4 * k2mask4 * k3mask4;
            sMask4 = serialise4D(mask1, mask2, mask3, mask4, k0mask4, k1mask4, k2mask4, k3mask4);

            k0mask3 = mask0;
            analyse3DArray(mask1, mask2, ref k1mask3, ref k2mask3);
            kAmask3 = k0mask3 * k1mask3 * k2mask3;
            sMask3 = serialise3D(mask1, mask2, mask3, k0mask3, k1mask3, k2mask3);

            k0mask2 = mask0;
            analyse2DArray(mask1, ref k1mask2);
            kAmask2 = k0mask2 * k1mask2;
            sMask2 = serialise2D(mask1, mask2, k0mask2, k1mask2);

            k0matrix3 = matrix0;
            analyse3DArray(matrix1, matrix2, ref k1matrix3, ref k2matrix3);
            kAmatrix3 = k0matrix3 * k1matrix3 * k2matrix3;
            smatrix3 = serialise3D(matrix1, matrix2, matrix3, k0matrix3, k1matrix3, k2matrix3);

            k0matrix2 = matrix0;
            analyse2DArray(matrix1, ref k1matrix2);
            kAmatrix2 = k0matrix2 * k1matrix2;
            smatrix2 = serialise2D(matrix1, matrix2, k0matrix2, k1matrix2);

            k0matrixCoins2 = matrix0;
            analyse2DArray(matrix1, ref k1matrixCoins2);
            kAmatrixCoins2 = k0matrixCoins2 * k1matrixCoins2;
            smatrixCoins2 = serialise2D(matrix1, matrixCoins, k0matrixCoins2, k1matrixCoins2);

            k0path2 = path0;
            analyse2DArray(path1, ref k1path2);
            kApath2 = k0path2 * k1path2;
            spath2 = serialise2D(path1, path2, k0path2, k1path2);


            initialiseAntiPlayer(
             maxGameOverPathLengthEvolution1,

             mask1,
             mask0,

             matrix1,
             matrix0,

             path1,
             path0,

             sMask4,
             k0mask4,
             k1mask4,
             k2mask4,
             k3mask4,
             kAmask4,

             sMask3,
             k0mask3,
             k1mask3,
             k2mask3,
             kAmask3,

             sMask2,
             k0mask2,
             k1mask2,
             kAmask2,

             smatrix3,
             k0matrix3,
             k1matrix3,
             k2matrix3,
             kAmatrix3,

             smatrix2,
             k0matrix2,
             k1matrix2,
             kAmatrix2,

             smatrixCoins2,
             k0matrixCoins2,
             k1matrixCoins2,
             kAmatrixCoins2,

             spath2,
             k0path2,
             k1path2,
             kApath2

             );

        }

        private void analyse2DArray(int[] array1, ref int i1)
        {
            i1 = 0;
            for (int ii1 = 0; ii1 < array1.Count(); ii1++)
            {
                if (array1[ii1] > i1)
                {
                    i1 = array1[ii1];
                }
            }
        }

        private void analyse3DArray(int[] array1, int[][] array2, ref int i1, ref int i2)
        {
            i1 = 0;
            i2 = 0;
            for (int ii1 = 0; ii1 < array1.Count(); ii1++)
            {
                for (int ii2 = 0; ii2 < array2[ii1].Count(); ii2++)
                {
                    if (array2[ii1][ii2] > i2)
                    {
                        i2 = array2[ii1][ii2];
                    }
                }
                if (array1[ii1] > i1)
                {
                    i1 = array1[ii1];
                }
            }
        }

        private void analyse4DArray(int[] array1, int[][] array2, int[][][] array3, ref int i1, ref int i2, ref int i3)
        {
            i1 = 0;
            i2 = 0;
            i3 = 0;
            for (int ii1 = 0; ii1 < array1.Count(); ii1++)
            {
                for (int ii2 = 0; ii2 < array2[ii1].Count(); ii2++)
                {
                    for (int ii3 = 0; ii3 < array3[ii1][ii2].Count(); ii3++)
                    {
                        if (array3[ii1][ii2][ii3] > i3)
                        {
                            i3 = array3[ii1][ii2][ii3];
                        }
                    }
                    if (array2[ii1][ii2] > i2)
                    {
                        i2 = array2[ii1][ii2];
                    }
                }
                if (array1[ii1] > i1)
                {
                    i1 = array1[ii1];
                }
            }
        }

        private int[] serialise4D(int[] array1, int[][] array2, int[][][] array3, int[][][][] array4, int k0, int k1, int k2, int k3)
        {
            int[] retVal;
            int iS = k0 * k1 * k2 * k3;
            retVal = new int[iS];
            for (int i = 0; i < iS; i++)
            {
                retVal[i] = 0;
            }

            for (int ii0 = 0; ii0 < k0; ii0++)
            {
                for (int ii1 = 0; ii1 < array1[ii0]; ii1++)
                {
                    for (int ii2 = 0; ii2 < array2[ii0][ii1]; ii2++)
                    {
                        for (int ii3 = 0; ii3 < array3[ii0][ii1][ii2]; ii3++)
                        {
                            int iAddr = (((((ii0 * k1) + ii1) * k2) + ii2) * k3) + ii3;
                            if (iAddr >= iS)
                            {
                                logger.log("Array overflow " + iAddr.ToString(), 1, "Error");
                            }
                            retVal[iAddr] = array4[ii0][ii1][ii2][ii3];
                        }
                    }
                }
            }
            return retVal;
        }

        private int[] serialise3D(int[] array1, int[][] array2, int[][][] array3, int k0, int k1, int k2)
        {
            int[] retVal;
            int iS = k0 * k1 * k2;
            retVal = new int[iS];
            for (int i = 0; i < iS; i++)
            {
                retVal[i] = 0;
            }

            for (int ii0 = 0; ii0 < k0; ii0++)
            {
                for (int ii1 = 0; ii1 < array1[ii0]; ii1++)
                {
                    for (int ii2 = 0; ii2 < array2[ii0][ii1]; ii2++)
                    {
                        int iAddr = (((ii0 * k1) + ii1) * k2) + ii2;
                        if (iAddr >= iS)
                        {
                            logger.log("Array overflow " + iAddr.ToString(), 1, "Error");
                        }
                        retVal[iAddr] = array3[ii0][ii1][ii2];
                    }
                }
            }
            return retVal;
        }


        private int[] serialise2D(int[] array1, int[][] array2, int k0, int k1)
        {
            int[] retVal;
            int iS = k0 * k1;
            retVal = new int[iS];
            for (int i = 0; i < iS; i++)
            {
                retVal[i] = 0;
            }

            for (int ii0 = 0; ii0 < k0; ii0++)
            {
                for (int ii1 = 0; ii1 < array1[ii0]; ii1++)
                {
                    int iAddr = (ii0 * k1) + ii1;
                    if (iAddr >= iS)
                    {
                        logger.log("Array overflow " + iAddr.ToString(), 1, "Error");
                    }
                    retVal[iAddr] = array2[ii0][ii1];
                }
            }
            return retVal;
        }




        public double calculateAntiPlayer(double[] brainCells, int allBrainCellsCount)
        {
            double[] result = new double[1];
            calculateAntiPlayerExternal(brainCells, allBrainCellsCount, result, 1);
            return result[0] / (2 * rules.diceCombinations * rules.diceCombinations);
        }









        private double[] serialise2D(int[] array1, double[][] array2, int k0, int k1)
        {
            double[] retVal;
            int iS = k0 * k1;
            retVal = new double[iS];
            for (int i = 0; i < iS; i++)
            {
                retVal[i] = 0;
            }

            for (int ii0 = 0; ii0 < k0; ii0++)
            {
                for (int ii1 = 0; ii1 < array1[ii0]; ii1++)
                {
                    int iAddr = (ii0 * k1) + ii1;
                    if (iAddr >= iS)
                    {
                        logger.log("Array overflow " + iAddr.ToString(), 1, "Error");
                    }
                    retVal[iAddr] = array2[ii0][ii1];
                }
            }
            return retVal;
        }

        public double calculateAntiPlayer_TestMany(double[] brainCells, int allBrainCellsCount)
        {


            //int count = 384; //Tested with 576 max
            int count = 1;
            double[] result = new double[count];
            double[] brainCellsMany = new double[count * allBrainCellsCount];

            Player[] pP = new Player[count];
            Player[] pA = new Player[count];
            Player[] pT = new Player[count];

            for (int j = 0; j < count; j++)
            {
                pP[j] = new Player(logger, rules, "P" + j.ToString("D3"), true);
                pA[j] = new Player(logger, rules, "A" + j.ToString("D3"), true);
                pT[j] = new Player(logger, rules, "T" + j.ToString("D3"), true);
            }

            for (int i = 0; i < allBrainCellsCount; i++)
            {
                pP[0].brainCells[i] = brainCells[i];
            }

            for (int j = 0; j < count; j++)
            {
                for (int i = 0; i < allBrainCellsCount; i++)
                {
                    brainCellsMany[j * allBrainCellsCount + i] = pP[j].brainCells[i];
                }
            }

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Restart();
            calculateAntiPlayerExternal(brainCellsMany, allBrainCellsCount, result, count);
            stopwatch.Stop();
            //logger.log("Time = " + stopwatch.ElapsedMilliseconds, 10, "ExternalRunner");
            //logger.logChart(stopwatch.ElapsedMilliseconds);



            bool allOK = true;
            for (int j = 0; j < count; j++)
            {
                for (int i = 0; i < allBrainCellsCount; i++)
                {
                    pA[j].brainCells[i] = brainCellsMany[j * allBrainCellsCount + i];
                }
                result[j] /= (2 * rules.diceCombinations * rules.diceCombinations);
                pP[j].strength = result[j];
                pA[j].strength = -result[j];

                pT[j] = pP[j].copyPlayer();
                double strength = calculateAntiPlayerInternal(pT[j].brainCells);
                pT[j].strength = (-strength) / (2 * rules.diceCombinations * rules.diceCombinations);


                //Compare pA ws pT
                if (pA[j].isBrainCellsEqual(pT[j]) && (Math.Abs(pA[j].strength - pT[j].strength) < 1E-12d))
                {
                    //logger.log("OK", 10, "Anti-AE1");
                }
                else
                {
                    allOK = false;
                    logger.log("Antiplayer do not match", 10, "Error");
                    logger.log("Antiplayer 1 strength = " + pA[j].strength.ToString("F30"), 10, "Error");
                    logger.log("Antiplayer 2 strength = " + pT[j].strength.ToString("F30"), 10, "Error");
                    logger.log("Antiplayer strength difference = " + (pA[j].strength - pT[j].strength).ToString("F30"), 10, "Error");
                    logger.log("Antiplayer brainCell equal = " + (pA[j].isBrainCellsEqual(pT[j]) ? "True" : "False"), 10, "Error");
                    logger.log("Antiplayer 1 brainCells = " + Rules.doubleListToString(pA[j].brainCells.ToList(), 4), 10, "Error");
                    logger.log("Antiplayer 2 brainCells = " + Rules.doubleListToString(pT[j].brainCells.ToList(), 4), 10, "Error");
                }



            }


            for (int i = 0; i < allBrainCellsCount; i++)
            {
                brainCells[i] = pA[0].brainCells[i];
            }

            return -pA[0].strength;



            //return calculateAntiPlayerInternal(brainCells);


            /*
            int count = 1;
            int realData = 0;
            double[] result = new double[count];
            double[] brainCellsMany = new double[allBrainCellsCount * count];

            Player[] pMany = new Player[count];

            for (int i = 0; i < count; i++)
            {
                pMany[i] = new Player(logger, rules, "Random" + i, true);
            }
            pMany[realData].brainCells = brainCells.ToArray();

            int j = 0;
            for (int i = 0; i < count; i++)
            {
                for (int ii = 0; ii < allBrainCellsCount; ii++)
                {
                    //brainCellsMany[j] = pMany[i].brainCells[ii];
                    brainCellsMany[j] = pMany[i].brainCells[ii];
                    j++;
                }
            }



            calculateAntiPlayerExternal(brainCellsMany, allBrainCellsCount, result, count);

            for (int ii = 0; ii < allBrainCellsCount; ii++)
            {
                brainCells[ii]=brainCellsMany[realData*allBrainCellsCount+ii];
            }



            double retVal = result[realData];
            return retVal;
            */
        }

        private double calculateAntiPlayerInternal(double[] brainCells)
        {
            for (int i0 = 0; i0 < matrix0; i0++)
            {
                double sum = 0.0d;
                for (int i1 = 0; i1 < matrix1[i0]; i1++)
                {
                    double multiplication = smatrixCoins2[(i0 * k1matrix2) + i1];
                    for (int i2 = 0; i2 < smatrix2[(i0 * k1matrix2) + i1]; i2++)
                    {
                        multiplication *= brainCells[smatrix3[(((i0 * k1matrix3) + i1) * k2matrix3) + i2]];
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
                    for (int i2 = 0; i2 < sMask2[(i0 * k1mask2) + i1]; i2++)
                    {
                        double sumOfAllDiceCombinationsBenefit = 0;
                        for (int i3 = 0; i3 < sMask3[(((i0 * k1mask3) + i1) * k2mask3) + i2]; i3++)
                        {
                            sumOfAllDiceCombinationsBenefit += wonCoins[sMask4[(((((i0 * k1mask4) + i1) * k2mask4) + i2) * k3mask4) + i3]];

                        }
                        if (bestOptionABenefit == null || sumOfAllDiceCombinationsBenefit > bestOptionABenefit)
                        {
                            bestOptionABenefit = sumOfAllDiceCombinationsBenefit;
                            bestOptionAChoiceLocation = i2;
                        }
                    }
                    for (int i2 = 0; i2 < sMask2[(i0 * k1mask2) + i1]; i2++)
                    {
                        for (int i3 = 0; i3 < sMask3[(((i0 * k1mask3) + i1) * k2mask3) + i2]; i3++)
                        {
                            double newBrainCellValue = (i2 == bestOptionAChoiceLocation) ? 1.0d : 0.0d;
                            brainCells[spath2[((sMask4[(((((i0 * k1mask4) + i1) * k2mask4) + i2) * k3mask4) + i3]) * k1path2) + pathLength - 1]] = newBrainCellValue;
                            wonCoins[sMask4[(((((i0 * k1mask4) + i1) * k2mask4) + i2) * k3mask4) + i3]] *= newBrainCellValue;
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
            logger.set("ExternalRunner", 10, Color.Maroon);
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
