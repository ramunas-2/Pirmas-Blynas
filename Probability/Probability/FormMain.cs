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

        bool terminateWhenAllClosed = false;

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

            Random rnd = new Random();
            for (int i = 0; i < noOfThreads; i++)
            {
                logger[i] = new Logger(richTextLog[i], chartLog[i], labelResult[i], "log" + i.ToString("d2") + ".txt", 5, loggerMain, pBest);
                logger[i].log("Hello");
                logger[i].set("Error", 10, Color.Red);
                worldAugmented[i] = new WorldAugmented(logger[i], rnd.Next(0x00010000));

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
            stopApplication();
        }

        private void stopApplication()
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
            terminateCheck();
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
            terminateCheck();
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
            terminateCheck();
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
            terminateCheck();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            stopApplication();
            terminateWhenAllClosed = true;
            if (!checkAllClosed())
            {
                e.Cancel = true;
            }
        }

        private bool checkAllClosed()
        {
            bool allClosed = false;
            if (terminateWhenAllClosed)
            {
                allClosed = true;
                for (int i = 0; i < noOfThreads; i++)
                {
                    allClosed &= (!threadRuns[i]);
                }
            }
            return allClosed;
        }

        private void terminateCheck()
        {
            if (terminateWhenAllClosed)
            {
                if (checkAllClosed())
                {
                    Close();
                }
            }
        }

        private void buttonTest1_Click(object sender, EventArgs e)
        {
            //loggerMain = new Logger(richTextBoxMain, chartMain, labelMain, "logTest.txt", 2);
            Random rnd = new Random();
            WorldAugmented world = new WorldAugmented(loggerMain, (rnd.Next(0x00010000)));
            Rules rules = new Rules(loggerMain, rnd.Next(0x00010000));
            Arena arena = new Arena(loggerMain, rules);
            ArenaAugmented arenaAugmented = new ArenaAugmented(loggerMain, rules);

            Player pCalculated = new Player(loggerMain, rules, "pCalculated", true);
            Player pRandom = new Player(loggerMain, rules, "pRandom", true);
            Player pA = new Player(loggerMain, rules, "pA", true);
            Player pManual = new Player(loggerMain, rules, "pManual", true);


            pManual.brainCells[0] = 0d;
            pManual.brainCells[1] = 1d;
            pManual.brainCells[2] = 0d;
            pManual.brainCells[3] = 0d;
            pManual.brainCells[4] = 1d;
            pManual.brainCells[5] = 0d;
            pManual.brainCells[6] = 1d;
            pManual.brainCells[7] = 0d;
            pManual.brainCells[8] = 1d;
            pManual.brainCells[9] = 0d;
            pManual.brainCells[10] = 0d;
            pManual.brainCells[11] = 1d;
            pManual.brainCells[12] = 0d;
            pManual.brainCells[13] = 0d;
            pManual.brainCells[14] = 1d;
            pManual.brainCells[15] = 0d;
            pManual.brainCells[16] = 0d;
            pManual.brainCells[17] = 1d;
            pManual.brainCells[18] = 0d;
            pManual.brainCells[19] = 1d;


            pCalculated.brainCells[0] = 0d;
            pCalculated.brainCells[1] = 2.77555756E-17d;
            pCalculated.brainCells[2] = 1d;
            pCalculated.brainCells[3] = 0d;
            pCalculated.brainCells[4] = 1d;
            pCalculated.brainCells[5] = 0d;
            pCalculated.brainCells[6] = 1d;
            pCalculated.brainCells[7] = 0d;
            pCalculated.brainCells[8] = 1d;
            pCalculated.brainCells[9] = 5.55111512E-17d;
            pCalculated.brainCells[10] = 0d;
            pCalculated.brainCells[11] = 0d;
            pCalculated.brainCells[12] = 1d;
            pCalculated.brainCells[13] = 0d;
            pCalculated.brainCells[14] = 0.637411158460914d;
            pCalculated.brainCells[15] = 0.362588841539086d;
            pCalculated.brainCells[16] = 0d;
            pCalculated.brainCells[17] = 1d;
            pCalculated.brainCells[18] = 0d;
            pCalculated.brainCells[19] = 1d;



            pA = arenaAugmented.antiPlayerE01(pCalculated);

            /*
            for (long i = 0; i < 1000000000; i++)
            {
                if (i % 1000000 ==0)
                {
                    loggerMain.log(""+i/1000000);
                }
                pCalculated.reset();
                pManual.reset();
            }
            */


            for (int i = 0; i < 1; i++)
            {

                //double resultAgainstManual = arena.fightScan(pCalculated, pManual, 1000000000);  //for Random testing
                //double resultAgainstManual = arena.fightScan(pCalculated, pManual, 10000);


                double resultAgainstRandom = arenaAugmented.fightStatistics(pCalculated, pRandom);
                double resultAgainstA = arenaAugmented.fightStatistics(pCalculated, pA);
                double resultAgainstManual = arenaAugmented.fightStatistics(pCalculated, pManual);


                loggerMain.log("Fight against pRandom = " + resultAgainstRandom.ToString("E4"));
                loggerMain.log("Fight against pA      = " + resultAgainstA.ToString("E4"));
                loggerMain.log("Fight against pManual = " + resultAgainstManual.ToString("E4"));

            }


            loggerMain.printSD();




        }

        private void EquationButton_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            WorldAugmented world = new WorldAugmented(loggerMain, (rnd.Next(0x00010000)));
            Rules rules = new Rules(loggerMain, rnd.Next(0x00010000));
            Arena arena = new Arena(loggerMain, rules);
            ArenaAugmented arenaAugmented = new ArenaAugmented(loggerMain, rules);

            Player p = new Player(loggerMain, rules, "Player", true);




            string s1 = "Solve[\n";
            for (int dice = 0; dice < rules.diceCombinations; dice++)
            {
                int brainCellsBegin = dice * rules.situationBrainCellsCount;
                foreach (Scenario scenario in rules.scenarios)
                {
                    if (scenario.possibleMoves.Count > 0)
                    {
                        String s2 = "(";
                        for (int i = brainCellsBegin; i < brainCellsBegin + scenario.possibleMoves.Count; i++)
                        {
                            s1 += "(0 <= p"+i+" <=1) && ";
                            s2 += "p"+i + " + ";
                        }
                        s2 = s2.Substring(0, s2.Length - 3);
                        s2 += " == 1)";
                        s1 += s2+" && \n";
                    }
                    brainCellsBegin += scenario.possibleMoves.Count;
                }
            }

            s1 += arenaAugmented.antiPlayerE03PrintEquation(p);

            s1 += ", \n{";

            for (int i = 0; i < p.allBrainCellsCount; i++)
            {
                s1 += "p" + i + ", ";
            }
            s1 = s1.Substring(0, s1.Length - 2);
            s1 += "}]";


            loggerMain.log("Equation : \n"+ s1);










        }



    }
}
