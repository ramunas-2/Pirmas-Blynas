using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Probability
{
    class World
    {
        Logger logger;
        Rules rules;
        Arena arena;
        ArenaAugmented arenaAugmented;
        public World(Logger logger)
        {
            this.logger = logger;
            rules = new Rules(logger);
            arena = new Arena(logger, rules);
            arenaAugmented = new ArenaAugmented(logger, rules);
        }

        //Create players
        //Create Arena
        //Live different scenarious

        public void mainScenario()
        {
            scenario2();
        }

        public void scenario2()
        {
            logger.log("Run scenarion 2");

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


            double maxResult = ((double)(-rules.playerCoins));

            logger.log("Start looking for antiPlayer for " + p1.toString());
            while (true)
            {
                double newResult = arenaAugmented.fightStatistics(p2, p1);
                if (newResult>maxResult)
                {
                    logger.log("New max found, value = "+newResult.ToString("F4")+" "+p2.toString());
                    logger.logChart(newResult);
                    maxResult = newResult;
                }
                p2.initialiseRandomise();
                
            }




        }
        public void scenario1()
        {

            logger.log("Run scenarion 1");

            Player p1 = new Player(logger, rules, "p1", true);
            Player p2 = new Player(logger, rules, "p2", true);

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
            

            p1.dice = 1;
            p2.dice = 0;
            */



            logger.log(p1.toString());
            logger.log(p2.toString());


            int fightResult2 = (arena.fight(p1, p2));
            logger.log("Single fight result = " + fightResult2.ToString());



            double fightResult = ((double)arena.fightScan(p1, p2, 100000));
            logger.log("Fight scan result = " + fightResult.ToString("F5"));




            double fightResultStatistics = ((double)arenaAugmented.fightStatistics(p1, p2));
            logger.log("Fight result statistics = " + fightResultStatistics.ToString("F5"));


        }


        //Mutation rules
        //Evolution
        //Evolve antiplayer

    }
}
