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
            logger.log("World Augmented Constructor OK", 1, "Error");

            stopRun = false;
        }

        public void mainScenario()
        {
            //scenarioA1();
            //scenario3();
            //scenarioA2();
            //scenarioA3();
            //scenarioA5();
            scenarioA4();
            //scenarioA6();
        }

        private void scenarioA6()
        {
            ExternalRunner externalRunner = new ExternalRunner(logger, rules, "Runner1");
            externalRunner.helloCGPU();
        }


        public bool stopRun;
        double lastMutationSize = 0;
        BeautifyComponent lastBeautifyComponent = null;

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
            beautifyAE1M2(ref pP);
            printLog(pP, "");


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
                double newBeautifiedResult = beautifyAE1M2(ref pP);
                if (bestBeautifiedResult.HasValue)
                {
                    if (newBeautifiedResult > bestBeautifiedResult)
                    {
                        logger.log("Improved   ", 1, "ScenarioA5");
                        bestBeautifiedResult = newBeautifiedResult;
                        pPOld = pPUgly.copyPlayer();

                        if (bestBeautifiedResult > -0.01)
                        {
                            printLog(pP, "");
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







            printLog(pP, "");


        }

        public void mainScenario2()
        {
            arenaAugmented.makeAugmentedAntiplayerEvolution1(new Player(logger, rules, "Anonymous", true));
        }

        private void scenarioA4()
        {
            logger.log("Run scenarion A4 - Evolve imune player");
            logger.set("ScenarioA4", 2, Color.Green);

            logger.logLabel("Started...");

            Player pPVeryBest = null;
            Player pPLast = null;

            while (!stopRun)
            {
                pPLast = beautifyAE1M4();
                if ((pPLast != null) && ((pPVeryBest == null) || (pPLast.strength > pPVeryBest.strength)))
                {
                    pPVeryBest = pPLast.copyPlayer();
                    logger.log("New very best Player found : " + pPVeryBest.toString(), 1, "ScenarioA4");
                    logger.log("Strength : " + pPVeryBest.strength.ToString("F16"), 1, "ScenarioA4");
                    logger.logLabel("Strength : " + pPVeryBest.strength.ToString("0.###E+0"));
                }
            }





        }


        private Player beautifyAE1M4()
        {
            
            int pCount = 16; //Number of parrallel players
            int maxIterations = 20; //Number of iterations (long)
            int maxBeautifyIterations = 30; //Number of iterations (long)
            
            
            /*
            int pCount = 1; //Number of parrallel players
            int maxIterations = 1; //Number of iterations (long)
            int maxBeautifyIterations = 30; //Number of iterations (long)
            */

            Player[] pP = new Player[pCount];
            for (int i = 0; i < pCount; i++)
            {
                pP[i] = new Player(logger, rules, "Lucky", true);
                //pP[i].load01();
                pP[i].normaliseBrainCells();
            }
            Player pPVeryBest = null;
            for (int iteration = 0; iteration < maxIterations; iteration++)
            {
                Player pPBest = null;
                int bestNo = -1;
                for (int i = 0; i < pCount; i++)
                {
                    logger.log("Iteration = " + iteration + "; Player = " + i, 5, "ScenarioA4");
                    pP[i] = beautifyAE1M3(-1E-12d, maxBeautifyIterations, 1, pP[i], 100, "T00", 2);
                    if ((pP[i] != null) && ((pPBest == null) || (pP[i].strength > pPBest.strength)))
                    {
                        pPBest = pP[i].copyPlayer();
                        bestNo = i;
                        logger.log("New best Player found : " + pPBest.toString(), 8, "ScenarioA4");
                    }
                }
                if (pPBest != null)
                {
                    logger.log("Iteration: "+iteration+"; Best Player was found at: " + bestNo+"; Strength: "+pPBest.strength.ToString("F16"), 2, "ScenarioA4");
                    for (int i = 0; i < pCount; i++)
                    {
                        pP[i] = pPBest.copyPlayer();
                        if (i < pCount / 2)
                        {
                            mutateN(pP[i], i);
                        }
                    }
                }
                if ((pPBest != null) && ((pPVeryBest == null) || (pPBest.strength > pPVeryBest.strength)))
                {
                    pPVeryBest = pPBest.copyPlayer();
                }
                if (pPBest != null)
                {
                    logger.logChart(pPBest.strength);
                }
            }
            return pPVeryBest;
        }

        public Player beautifyAE1M3(double targetResult = -1E-06d, int repCount = 1000, int mutateCount = 1, Player pP = null, int maxBeautifyCount = 100, string threadName = "T00", int debugL = 10)
        {
            logger.set("ScenarioA4" + threadName, debugL, Color.Blue);
            logger.set("ScenarioA4Internal", debugL, Color.Magenta);


            if (pP == null)
            {
                pP = new Player(logger, rules, "pP", true);
            }


            //printLog(pP, "ScenarioA4" + threadName);



            //logger.logChartReset();

            createBeutifyComponents();

            //mutateN(pP, 16);
            //beautifyAE1M2(pP);
            //printLog(pP);


            double? bestBeautifiedResult = null;

            string[] args = Environment.GetCommandLineArgs();

            if (args.Count() == 2)
            {
                mutateCount = Convert.ToInt32(args[1]);
                logger.log("mutateCount = " + mutateCount, 1, "ScenarioA4" + threadName);
            }

            Player pPBest = null;
            bool continueSearch = true;
            for (int i = 0; (i < repCount) && continueSearch && (!stopRun); i++)
            {
                logger.log("Iteration " + i, 3, "ScenarioA4" + threadName);
                double newBeautifiedResult = beautifyAE1M2(ref pP, maxBeautifyCount);
                if (bestBeautifiedResult.HasValue)
                {
                    if (newBeautifiedResult > bestBeautifiedResult)
                    {
                        logger.log("Improved   ", 3, "ScenarioA4" + threadName);
                        logger.log("Improvement mutation size = " + lastMutationSize.ToString("F12") + "; cell = " + lastBeautifyComponent.brainCellIndex + "; against = " + lastBeautifyComponent.changeAgainst + "; direction = " + lastBeautifyComponent.direction.ToString("F4"), 3, "ScenarioA4" + threadName);
                        bestBeautifiedResult = newBeautifiedResult;
                        pPBest = pP.copyPlayer();
                        /*
                        if (bestBeautifiedResult > -1E-05d)
                        {
                            printLog(pP);
                        }
                        */
                        if (bestBeautifiedResult > targetResult)
                        {
                            continueSearch = false;
                        }
                        else
                        {
                            mutateN(pP, mutateCount);
                        }
                    }
                    else
                    {
                        logger.log("Restore old ", 3, "ScenarioA4" + threadName);
                        pP = pPBest.copyPlayer();
                        mutateN(pP, mutateCount);
                    }
                }
                else
                {
                    bestBeautifiedResult = newBeautifiedResult;
                    pPBest = pP.copyPlayer();
                    mutateN(pP, mutateCount);
                }

            }







            //printLog(pP, "ScenarioA4" + threadName);


            return pPBest;
        }

        private void printLog(Player pP, string debugName)
        {
            Player pA;
            double pPResult;
            pA = arenaAugmented.makeAugmentedAntiplayerEvolution1(pP);
            pPResult = arenaAugmented.fightStatistics(pP, pA);
            logger.log("Player strenght  = " + pPResult.ToString("F12") + "     ", 1, debugName);
            logger.log("Player             = " + pP.toString(), 1, debugName);
            logger.log("Antiplayer  = " + pA.toString(), 1, debugName);
        }

        private void mutateN(Player pP, int n)
        {
            for (int mutateI = 0; mutateI < n; mutateI++)
            {
                lastMutationSize = rules.random.NextDouble();
                lastBeautifyComponent = beautifyComponents[rules.random.Next(beautifyComponents.Count)];
                pP.mutateAdvanced(lastBeautifyComponent.brainCellIndex, lastBeautifyComponent.changeAgainst, lastBeautifyComponent.direction * rules.random.NextDouble());
            }
        }

        private double beautifyAE1M2(ref Player pP, int maxBeautifyCount = 100)
        {
            double? pPStrenght = null;
            bool benefit = true;
            int beautifyCount = 0;
            while (benefit && (beautifyCount < maxBeautifyCount) && (!stopRun))
            {
                double newStrenght = beautifyAE1(ref pP);
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
                logger.log("Strenght = " + pPStrenght.Value.ToString("F15"), 8, "ScenarioA4Internal");
                //logger.logChart(pPStrenght.Value);
                beautifyCount++;
            }
            return pPStrenght.Value;
        }

        private double beautifyAE1(ref Player pP)
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
            pP.strength = bestResult;
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
