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


        //[DllImport(@"..\..\..\debug\ExternalC.dll", CallingConvention = CallingConvention.Cdecl)]
        //public static extern void DisplayHelloFromDLL(int length, double[] dArray);


        [DllImport(@"..\..\..\debug\CudaRunner.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CudaAdd(int length, int[] a, int[] b, int[] c);

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



            for (int j = 0; j < 1000; j+=10)
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
