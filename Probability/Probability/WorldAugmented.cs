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
            scenarioA3();
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


            if (Math.Abs(compareAugmentedStatistics-compareAugmentedStatisticsEvolution1)<1.0E-06d)
            {
                logger.log("Result match OK", 1, "ScenarioA3");
            }
            else
            {
                logger.log("Result don't match", 5, "Error");
            }

            logger.log("AugmentedStatistics            = " + compareAugmentedStatistics.ToString("F4")           + "       " + pAAugmented.toString()          , 1, "ScenarioA3");
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
            double compareBeautifiedStatistics =-1.0d;

            while ((compareBeautifiedStatistics + 0.0001d) < compareAugmentedStatistics)
            {
                pABeautified.initialiseRandomise();
                beautifyAgainst(pABeautified, p1);
                compareBeautifiedStatistics = arenaAugmented.fightStatistics(pABeautified, p1);
                logger.log("BeautifiedStatistics = " + compareBeautifiedStatistics.ToString("F4"), 1, "ScenarioA2");
                logger.logChart(compareBeautifiedStatistics);
            }

            logger.log("AugmentedStatistics  = " + compareAugmentedStatistics.ToString("F4") + "       " + pAAugmented.toString(), 1, "ScenarioA2");
            logger.log("BeautifiedStatistics = " + compareBeautifiedStatistics.ToString("F4") + "  " + pABeautified.toString(), 1, "ScenarioA2");

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
