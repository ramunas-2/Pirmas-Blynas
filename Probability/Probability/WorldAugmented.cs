using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Probability
{
    class WorldAugmented : World
    {

        public WorldAugmented(Logger logger)
            : base(logger)
        {
        }

        public void mainScenario()
        {
            //scenarioA1();
            //scenario3();
            //scenarioA2();
            //scenarioA3();
            scenarioA4();
            //scenarioA5();
        }

        private void scenarioA5()
        {
            logger.log("Run scenarion A4 - Evolve imune player");
            logger.set("ScenarioA4", 10, Color.Green);
            logger.set("ScenarioA5", 10, Color.Blue);
            Player pA;
            double pPResult;

            Player pPUgly = new Player(logger, rules, "pP", true);

            logger.logChartReset();

            createBeutifyComponents();

            Player pP = pPUgly.copyPlayer();
            beautifyAE1M2(pP);
            printLog(pP);


            double? bestBeautifiedResult = null;

            string[] args = Environment.GetCommandLineArgs();
            int mutateCount = 1;
            if (args.Count() == 2)
            {
                mutateCount = Convert.ToInt32(args[1]);
                logger.log("mutateCount = " + mutateCount, 1, "ScenarioA5");
            }

            Player pPOld = pPUgly.copyPlayer();
            bool continueSearch = true;
            for (int i = 0; (i < 1000000) && continueSearch; i++)
            {
                if (args.Count() != 2)
                {
                    //mutateCount = rules.random.Next(16);
                    //logger.log("mutateCount = " + mutateCount, 1, "ScenarioA5");
                }
                pP = pPUgly.copyPlayer();
                double newBeautifiedResult = beautifyAE1M2(pP);
                if (bestBeautifiedResult.HasValue)
                {
                    if (newBeautifiedResult > bestBeautifiedResult)
                    {
                        logger.log("Improved   ", 1, "ScenarioA5");
                        bestBeautifiedResult = newBeautifiedResult;
                        pPOld = pPUgly.copyPlayer();

                        if (bestBeautifiedResult > -0.01)
                        {
                            printLog(pP);
                        }

                        if (bestBeautifiedResult > -0.0001)
                        {
                            continueSearch = false;
                        }
                        else
                        {
                            mutateN(pPUgly, mutateCount);
                        }
                    }
                    else
                    {
                        logger.log("Restore old ", 1, "ScenarioA5");
                        pPUgly = pPOld.copyPlayer();
                        mutateN(pPUgly, mutateCount);
                    }
                }
                else
                {
                    bestBeautifiedResult = newBeautifiedResult;
                    mutateN(pPUgly, mutateCount);
                }

            }







            printLog(pP);


        }

        public void mainScenario2()
        {
            arenaAugmented.makeAugmentedAntiplayerEvolution1(new Player(logger, rules, "Anonymous", true));
        }

        private void scenarioA4()
        {
            logger.log("Run scenarion A4 - Evolve imune player");
            logger.set("ScenarioA4", 10, Color.Blue);
            Player pA;
            double pPResult;

            Player pP = new Player(logger, rules, "pP", true);

            
            pP.brainCells[0] = 0d;
            pP.brainCells[1] = 0.6152d;
            pP.brainCells[2] = 0d;
            pP.brainCells[3] = 0.3848d;
            pP.brainCells[4] = 0d;
            pP.brainCells[5] = 0.3854d;
            pP.brainCells[6] = 0.1146d;
            pP.brainCells[7] = 0.5d;
            pP.brainCells[8] = 1d;
            pP.brainCells[9] = 0d;
            pP.brainCells[10] = 0d;
            pP.brainCells[11] = 1d;
            pP.brainCells[12] = 0d;
            pP.brainCells[13] = 1d;
            pP.brainCells[14] = 0d;
            pP.brainCells[15] = 1d;
            pP.brainCells[16] = 0d;
            pP.brainCells[17] = 0d;
            pP.brainCells[18] = 0.412d;
            pP.brainCells[19] = 0.588d;
            pP.brainCells[20] = 1d;
            pP.brainCells[21] = 0d;
            pP.brainCells[22] = 0d;
            pP.brainCells[23] = 1d;
            pP.brainCells[24] = 0d;
            pP.brainCells[25] = 0d;
            pP.brainCells[26] = 0d;
            pP.brainCells[27] = 1d;
            pP.brainCells[28] = 0d;
            pP.brainCells[29] = 0d;
            pP.brainCells[30] = 0.5391d;
            pP.brainCells[31] = 0.4609d;
            pP.brainCells[32] = 0d;
            pP.brainCells[33] = 0.1289d;
            pP.brainCells[34] = 0.8711d;
            pP.brainCells[35] = 0.9159d;
            pP.brainCells[36] = 0.0841d;
            pP.brainCells[37] = 0.4226d;
            pP.brainCells[38] = 0.4608d;
            pP.brainCells[39] = 0.1166d;
            pP.brainCells[40] = 1d;
            pP.brainCells[41] = 0d;
            pP.brainCells[42] = 0.8169d;
            pP.brainCells[43] = 0.1831d;
            pP.brainCells[44] = 0d;
            pP.brainCells[45] = 1d;
            pP.brainCells[46] = 0d;
            pP.brainCells[47] = 0d;
            pP.brainCells[48] = 0d;
            pP.brainCells[49] = 0.6563d;
            pP.brainCells[50] = 0.3437d;
            pP.brainCells[51] = 0d;
            pP.brainCells[52] = 0d;
            pP.brainCells[53] = 0.5403d;
            pP.brainCells[54] = 0.4597d;
            pP.brainCells[55] = 0d;
            pP.brainCells[56] = 1d;
            pP.brainCells[57] = 0.046d;
            pP.brainCells[58] = 0.954d;
            pP.brainCells[59] = 0d;
            pP.brainCells[60] = 1d;
            pP.brainCells[61] = 0d;
            pP.brainCells[62] = 0.912d;
            pP.brainCells[63] = 0.088d;
            pP.brainCells[64] = 0.2794d;
            pP.brainCells[65] = 0.7206d;
            pP.brainCells[66] = 0d;
            pP.brainCells[67] = 0.2305d;
            pP.brainCells[68] = 0d;
            pP.brainCells[69] = 0.7695d;
            pP.brainCells[70] = 0d;
            pP.brainCells[71] = 0d;
            pP.brainCells[72] = 0d;
            pP.brainCells[73] = 1d;
            pP.brainCells[74] = 0d;
            pP.brainCells[75] = 0d;
            pP.brainCells[76] = 1d;
            pP.brainCells[77] = 0.2056d;
            pP.brainCells[78] = 0.7944d;
            pP.brainCells[79] = 0d;
            pP.brainCells[80] = 1d;
            pP.brainCells[81] = 0d;
            pP.brainCells[82] = 0.4172d;
            pP.brainCells[83] = 0.5828d;
            pP.brainCells[84] = 0.9325d;
            pP.brainCells[85] = 0.0675d;
            pP.brainCells[86] = 0d;
            pP.brainCells[87] = 1d;


            printLog(pP);
            


            logger.logChartReset();

            createBeutifyComponents();

            //mutateN(pP, 16);
            beautifyAE1M2(pP);
            printLog(pP);


            double? bestBeautifiedResult = null;

            string[] args = Environment.GetCommandLineArgs();
            int mutateCount = 1;
            if (args.Count()==2)
            {
                mutateCount = Convert.ToInt32(args[1]);
                logger.log("mutateCount = " + mutateCount, 1, "ScenarioA4");
            }

            Player pPOld = pP.copyPlayer();
            bool continueSearch = true;
            for (int i = 0; (i < 1000000) && continueSearch; i++)
            {
                if (args.Count()!=2)
                {
                    //mutateCount = rules.random.Next(16);
                    logger.log("mutateCount = " + mutateCount, 1, "ScenarioA4");
                }
                double newBeautifiedResult = beautifyAE1M2(pP);
                if (bestBeautifiedResult.HasValue)
                {
                    if (newBeautifiedResult > bestBeautifiedResult)
                    {
                        logger.log("Improved   ", 1, "ScenarioA4");
                        bestBeautifiedResult = newBeautifiedResult;
                        pPOld = pP.copyPlayer();

                        if (bestBeautifiedResult > -0.000001)
                        {
                            //printLog(pP);
                        }

                        if (bestBeautifiedResult > -0.0001)
                        {
                            //continueSearch = false;
                        }
                        else
                        {
                            mutateN(pP, mutateCount);
                        }
                    }
                    else
                    {
                        logger.log("Restore old ", 1, "ScenarioA4");
                        pP = pPOld.copyPlayer();
                        mutateN(pP, mutateCount);
                    }
                }
                else
                {
                    bestBeautifiedResult = newBeautifiedResult;
                    mutateN(pP, mutateCount);
                }

            }







            printLog(pP);



        }

        private void printLog(Player pP)
        {
            Player pA;
            double pPResult;
            pA = arenaAugmented.makeAugmentedAntiplayerEvolution1(pP);
            pPResult = arenaAugmented.fightStatistics(pP, pA);
            logger.log("Player strenght  = " + pPResult.ToString("F12") + "     ", 1, "ScenarioA4");
            logger.log("Player             = " + pP.toString(), 1, "ScenarioA4");
            logger.log("Antiplayer  = " + pA.toString(), 1, "ScenarioA4");
        }

        private void mutateN(Player pP, int n)
        {
            for (int mutateI = 0; mutateI < n; mutateI++)
            {
                BeautifyComponent beautifyComponent = beautifyComponents[rules.random.Next(beautifyComponents.Count)];
                pP.mutateAdvanced(beautifyComponent.brainCellIndex, beautifyComponent.changeAgainst, beautifyComponent.direction * rules.random.NextDouble());
            }
        }

        private double beautifyAE1M2(Player pP)
        {
            double? pPStrenght = null;
            bool benefit = true;

            while (benefit)
            {
                double newStrenght = beautifyAE1(pP);
                if (pPStrenght.HasValue)
                {
                    if (newStrenght > pPStrenght)
                    {
                        pPStrenght = newStrenght;
                    }
                    else
                    {
                        benefit = false;
                    }
                }
                else
                {
                    pPStrenght = newStrenght;
                }
                logger.log("Strenght = " + pPStrenght.Value.ToString("F15"), 8, "ScenarioA4");
                logger.logChart(pPStrenght.Value);
            }
            return pPStrenght.Value;
        }

        private double beautifyAE1(Player pP)
        {
            //1 Finf MAX beneficial direction
            BeautifyComponent bestDirection = null;
            double microStep = 1.0E-12d;
            Player pA = arenaAugmented.makeAugmentedAntiplayerEvolution1(pP);
            double bestResult = -pA.strength;
            Player pABaseline = pA.copyPlayer();
            double baseLineStrength = pA.strength;
            pP.push();
            resetBeautifyComponents();
            foreach (BeautifyComponent beautifyComponent in beautifyComponents)
            {
                pP.mutateAdvanced(beautifyComponent.brainCellIndex, beautifyComponent.changeAgainst, beautifyComponent.direction * microStep);
                pA = arenaAugmented.makeAugmentedAntiplayerEvolution1(pP);
                beautifyComponent.newResult = pA.strength;
                //logger.log("Checking new direction, result = " + beautifyComponent.newResult.Value.ToString("F15"), 8, "ScenarioA4");
                if (bestDirection == null)
                {
                    bestDirection = beautifyComponent;
                }
                else
                {
                    if (beautifyComponent.newResult.Value < bestDirection.newResult.Value)
                    {
                        if (pA.isBrainCellsEqual(pABaseline))
                        {
                            bestDirection = beautifyComponent;
                        }
                    }
                }
                pP.pop();
            }
            double stepMax = 0.0d;
            if (bestDirection != null)
            {
                for (double stepIncrement = 0.1d; stepIncrement > microStep; stepIncrement /= 2.0d)
                {
                    double testStep = stepMax + stepIncrement;
                    pP.mutateAdvanced(bestDirection.brainCellIndex, bestDirection.changeAgainst, bestDirection.direction * testStep);
                    pA = arenaAugmented.makeAugmentedAntiplayerEvolution1(pP);
                    if ((pA.isBrainCellsEqual(pABaseline)) && ((-pA.strength) > bestResult))
                    {
                        stepMax = testStep;
                        bestResult = -pA.strength;
                    }
                    pP.pop();
                }
                pP.mutateAdvanced(bestDirection.brainCellIndex, bestDirection.changeAgainst, bestDirection.direction * stepMax);
            }
            /*
            logger.log("stepMax = " + stepMax.ToString("F15"), 8, "ScenarioA4");
            logger.logChartReset();
            for (double step = 0; step <= stepMax; step += (stepMax / 100.0d))
            {
                pP.mutateAdvanced(bestDirection.brainCellIndex, bestDirection.changeAgainst, bestDirection.direction * step);
                pA = arenaAugmented.makeAugmentedAntiplayerEvolution1(pP);
                logger.logChart(pA.strength);
                pP.pop();
            }
            */
            return bestResult;
        }



        private void scenarioA3()
        {
            logger.log("Run scenarion A3 - build anti player - Augmented direct method Evolution1 and test agaist Simple Augmented direct method");
            logger.set("ScenarioA3", 10, Color.DarkGreen);

            Player p1 = new Player(logger, rules, "p1", true);


            Player pAAugmented = arenaAugmented.makeAugmentedAntiplayer(p1);
            Player pAAugmentedEvolution1 = arenaAugmented.makeAugmentedAntiplayerEvolution1(p1);
            double compareAugmentedStatistics = arenaAugmented.fightStatistics(pAAugmented, p1);
            double compareAugmentedStatisticsEvolution1 = arenaAugmented.fightStatistics(pAAugmentedEvolution1, p1);


            if (Math.Abs(compareAugmentedStatistics - compareAugmentedStatisticsEvolution1) < 1.0E-06d)
            {
                logger.log("Result match OK", 1, "ScenarioA3");
            }
            else
            {
                logger.log("Result don't match", 5, "Error");
            }

            logger.log("AugmentedStatistics            = " + compareAugmentedStatistics.ToString("F4") + "       " + pAAugmented.toString(), 1, "ScenarioA3");
            logger.log("AugmentedStatisticsEvolution1  = " + compareAugmentedStatisticsEvolution1.ToString("F4") + "     " + pAAugmentedEvolution1.toString(), 1, "ScenarioA3");






        }

        public void scenarioA2()
        {
            logger.log("Run scenarion A2 - build anti player - Augmented direct method and test agaist Anti Beautify method");
            logger.set("ScenarioA2", 10, Color.Blue);

            Player p1 = new Player(logger, rules, "p1", true);

            /*
            p1.brainCells[0] = 0.70d;
            p1.brainCells[1] = 0.26d;
            p1.brainCells[2] = 0.04d;
            p1.brainCells[3] = 0.02d;
            p1.brainCells[4] = 0.89d;
            p1.brainCells[5] = 0.09d;
            p1.brainCells[6] = 0.48d;
            p1.brainCells[7] = 0.52d;
            p1.brainCells[8] = 0.41d;
            p1.brainCells[9] = 0.59d;
            p1.brainCells[10] = 0.19d;
            p1.brainCells[11] = 0.78d;
            p1.brainCells[12] = 0.03d;
            p1.brainCells[13] = 0.68d;
            p1.brainCells[14] = 0.08d;
            p1.brainCells[15] = 0.24d;
            p1.brainCells[16] = 0.49d;
            p1.brainCells[17] = 0.51d;
            p1.brainCells[18] = 0.00d;
            p1.brainCells[19] = 1.00d;

            p1.normaliseBrainCells();
            */

            Player pAAugmented = arenaAugmented.makeAugmentedAntiplayer(p1);
            Player pABeautified = new Player(logger, rules, "pABeautified", true);
            double compareAugmentedStatistics = arenaAugmented.fightStatistics(pAAugmented, p1);
            double compareBeautifiedStatistics = -1.0d;

            while ((compareBeautifiedStatistics + 0.00000001d) < compareAugmentedStatistics)
            {
                pABeautified.initialiseRandomise();
                beautifyAgainst(pABeautified, p1);
                compareBeautifiedStatistics = arenaAugmented.fightStatistics(pABeautified, p1);
                logger.log("BeautifiedStatistics = " + compareBeautifiedStatistics.ToString("F4"), 1, "ScenarioA2");
                logger.logChart(compareBeautifiedStatistics);
            }

            logger.log("AugmentedStatistics  = " + compareAugmentedStatistics.ToString("F8") + "       " + pAAugmented.toString(), 1, "ScenarioA2");
            logger.log("BeautifiedStatistics = " + compareBeautifiedStatistics.ToString("F8") + "  " + pABeautified.toString(), 1, "ScenarioA2");

            //searchAntiplayerBrootforce(p1);


        }


        //Calculate antiplayer
        public void scenarioA1()
        {
            logger.log("Run scenarion A1 - build anti player - Augmented direct method");
            logger.set("ScenarioA1", 10, Color.Blue);

            Player p1 = new Player(logger, rules, "p1", true);
            Player p2 = new Player(logger, rules, "p2", true);


            p1.brainCells[0] = 0.70d;
            p1.brainCells[1] = 0.26d;
            p1.brainCells[2] = 0.04d;
            p1.brainCells[3] = 0.02d;
            p1.brainCells[4] = 0.89d;
            p1.brainCells[5] = 0.09d;
            p1.brainCells[6] = 0.48d;
            p1.brainCells[7] = 0.52d;
            p1.brainCells[8] = 0.41d;
            p1.brainCells[9] = 0.59d;
            p1.brainCells[10] = 0.19d;
            p1.brainCells[11] = 0.78d;
            p1.brainCells[12] = 0.03d;
            p1.brainCells[13] = 0.68d;
            p1.brainCells[14] = 0.08d;
            p1.brainCells[15] = 0.24d;
            p1.brainCells[16] = 0.49d;
            p1.brainCells[17] = 0.51d;
            p1.brainCells[18] = 0.00d;
            p1.brainCells[19] = 1.00d;

            p2.brainCells[0] = 0.11d;
            p2.brainCells[1] = 0.12d;
            p2.brainCells[2] = 0.77d;
            p2.brainCells[3] = 0.14d;
            p2.brainCells[4] = 0.76d;
            p2.brainCells[5] = 0.10d;
            p2.brainCells[6] = 0.85d;
            p2.brainCells[7] = 0.15d;
            p2.brainCells[8] = 0.16d;
            p2.brainCells[9] = 0.84d;
            p2.brainCells[10] = 0.04d;
            p2.brainCells[11] = 0.00d;
            p2.brainCells[12] = 0.96d;
            p2.brainCells[13] = 0.06d;
            p2.brainCells[14] = 0.01d;
            p2.brainCells[15] = 0.93d;
            p2.brainCells[16] = 0.08d;
            p2.brainCells[17] = 0.92d;
            p2.brainCells[18] = 0.09d;
            p2.brainCells[19] = 0.91d;

            p1.normaliseBrainCells();
            p2.normaliseBrainCells();


            p2 = arenaAugmented.makeAugmentedAntiplayer(p1);



        }





        //Test agfainst World's antiplayer

        //Evolve perfect player
    }
}
