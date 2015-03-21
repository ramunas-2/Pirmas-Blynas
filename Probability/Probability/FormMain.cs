using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Probability
{
    public partial class FormMain : Form
    {
        const int noOfThreads = 4;
        Logger[] logger = new Logger[noOfThreads];
        WorldAugmented[] worldAugmented = new WorldAugmented[noOfThreads];
        bool[] threadRuns = new bool[noOfThreads];

        System.Windows.Forms.RichTextBox[] richTextLog = new System.Windows.Forms.RichTextBox[noOfThreads];
        System.Windows.Forms.DataVisualization.Charting.Chart[] chartLog = new System.Windows.Forms.DataVisualization.Charting.Chart[noOfThreads];
        System.Windows.Forms.Label[] labelResult = new System.Windows.Forms.Label[noOfThreads];
        System.ComponentModel.BackgroundWorker[] backgroundWorker = new System.ComponentModel.BackgroundWorker[noOfThreads];

        Player pBest;
        Logger loggerMain;

        public FormMain()
        {
            InitializeComponent();
        }

        void run()
        {
            /*
            for (int i = 0; i < noOfThreads; i++)
            {
                worldAugmented[i].mainScenario();
            }
            */

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < noOfThreads; i++)
            {
                threadRuns[i] = false;
            }

            loggerMain = new Logger(richTextBoxMain, chartMain, labelMain, "logMain.txt", 2);
            pBest = new Player(loggerMain, new Rules(loggerMain), "Best", true);
            pBest.strength = -1E12d;

            int j = 0;
            if (noOfThreads > j)
            {
                richTextLog[j] = richTextBoxMainLogger0;
                chartLog[j] = chartLog0;
                labelResult[j] = labelResult0;
                backgroundWorker[j] = backgroundWorker0;
            }

            j = 1;
            if (noOfThreads > j)
            {
                richTextLog[j] = richTextBoxMainLogger1;
                chartLog[j] = chartLog1;
                labelResult[j] = labelResult1;
                backgroundWorker[j] = backgroundWorker1;
            }

            j = 2;
            if (noOfThreads > j)
            {
                richTextLog[j] = richTextBoxMainLogger2;
                chartLog[j] = chartLog2;
                labelResult[j] = labelResult2;
                backgroundWorker[j] = backgroundWorker2;
            }

            j = 3;
            if (noOfThreads > j)
            {
                richTextLog[j] = richTextBoxMainLogger3;
                chartLog[j] = chartLog3;
                labelResult[j] = labelResult3;
                backgroundWorker[j] = backgroundWorker3;
            }

            for (int i = 0; i < noOfThreads; i++)
            {
                logger[i] = new Logger(richTextLog[i], chartLog[i], labelResult[i], "log" + i.ToString("d2") + ".txt", 5, loggerMain, pBest);
                logger[i].log("Hello");
                logger[i].set("Error", 10, Color.Red);
                worldAugmented[i] = new WorldAugmented(logger[i]);

            }





            //run();
            //change3
        }


        private void buttonRun01_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < noOfThreads; i++)
            {
                if (!threadRuns[i])
                {
                    worldAugmented[i].stopRun = false;
                    threadRuns[i] = true;
                    backgroundWorker[i].RunWorkerAsync();
                }
            }


        }

        private void buttonRun2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < noOfThreads; i++)
            {
                worldAugmented[i].mainScenario2();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < noOfThreads; i++)
            {
                if (threadRuns[i])
                {
                    worldAugmented[i].stopRun = true;
                }
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (noOfThreads > 0)
                worldAugmented[0].mainScenario();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (noOfThreads > 0)
                threadRuns[0] = false;
        }

        private void backgroundWorker1_DoWork_1(object sender, DoWorkEventArgs e)
        {
            if (noOfThreads > 1)
                worldAugmented[1].mainScenario();
        }

        private void backgroundWorker1_RunWorkerCompleted_1(object sender, RunWorkerCompletedEventArgs e)
        {
            if (noOfThreads > 1)
                threadRuns[1] = false;
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            if (noOfThreads > 2)
                worldAugmented[2].mainScenario();
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (noOfThreads > 2)
                threadRuns[2] = false;
        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            if (noOfThreads > 3)
                worldAugmented[3].mainScenario();
        }

        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (noOfThreads > 3)
                threadRuns[3] = false;
        }



    }
}
