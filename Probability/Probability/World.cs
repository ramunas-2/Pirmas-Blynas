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
        protected Logger logger;
        protected Rules rules;
        protected Arena arena;
        protected ArenaAugmented arenaAugmented;
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
            scenario3();
        }

        public void scenario3()
        {
            logger.log("Run scenarion 3 - build anti player - brutal force with beutify");
            logger.set("Scenario3", 10, Color.Blue);

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
            for (int i = 0; i < 2; i++)
            {
                double newResult = arenaAugmented.fightStatistics(p2, p1);
                if (newResult > maxResult)
                {
                    logger.log("New max found, value = " + newResult.ToString("F4") + " " + p2.toString());
                    logger.logChart(newResult);
                    maxResult = newResult;
                }
                p2.initialiseRandomise();
                beautifyAgainst(p2, p1);








            }
        }

        protected void beautifyAgainst(Player p2, Player p1)
        {
            createBeutifyComponents();
            double bestComponent;
            //bestComponent = beautifyRepeatAgainstStep(p2, p1, 0.1d, 1000);
            bestComponent = beautifyRepeatAgainstStep(p2, p1, 0.1d, 10000);
            bestComponent = beautifyRepeatAgainstStep(p2, p1, 0.01d, 10000);
            bestComponent = beautifyRepeatAgainstStep(p2, p1, 0.001d, 10000);
            bestComponent = beautifyRepeatAgainstStep(p2, p1, 0.0001d, 10000);
            bestComponent = beautifyRepeatAgainstStep(p2, p1, 0.00001d, 10000);


        }

        protected double beautifyRepeatAgainstStep(Player p2, Player p1, double step, int repeatCount)
        {
            double? bestResult = null;
            bool stillEfficient=true;
            for (int i = 0; (i < repeatCount)&&(stillEfficient); i++)
            {
                if ((i%100==00)&&(i>100))
                {
                    logger.log("Iteration "+i+ " Precission = "+step.ToString());
                }

                stillEfficient = false;
                double? beautifyResult = beautifyOnceAgainstStep(p2, p1, step);
                if (bestResult == null)
                {
                    bestResult = beautifyResult.Value;
                    stillEfficient = true;
                }
                else
                {
                    if (beautifyResult.Value > bestResult.Value)
                    {
                        bestResult = beautifyResult;
                        stillEfficient = true;
                    }
                }
                //logger.log("Beautification attempt "+i+" step = " + step.ToString("F6") + " Best result = " + bestResult.Value.ToString("F4")+" "+(stillEfficient?"still efficient":"not efficient, exiting"), 8, "Scenario3");
                //logger.logChart(bestResult.Value);
            }

            //logger.log("Beautification step = " + step.ToString("F6") + " Best result = " + bestComponent.newResult.Value.ToString("F4"), 8, "Scenario3");
            return bestResult.Value;
        }


        protected double beautifyOnceAgainstStep(Player p2, Player p1, double step)
        {
            randomiseBeautifyComponents();
            //no need to order???
            var sorted = from beautifyComponent in beautifyComponents orderby beautifyComponent.randomOrder select beautifyComponent;
            BeautifyComponent bestComponent = null;
            p2.push();
            foreach (BeautifyComponent beautifyComponent in sorted)
            {
                p2.mutate(beautifyComponent.brainCellIndex, beautifyComponent.direction * step);

                beautifyComponent.newResult = arenaAugmented.fightStatistics(p2, p1);
                if (bestComponent == null)
                {
                    bestComponent = beautifyComponent;
                }
                else
                {
                    if (beautifyComponent.newResult.Value > bestComponent.newResult.Value)
                    {
                        bestComponent = beautifyComponent;
                    }
                }
                p2.pop();
                //logger.log("Sorted: randomOrder = " + beautifyComponent.randomOrder.ToString("F4") + " brainCellIndex = " + beautifyComponent.brainCellIndex + " direction = " + beautifyComponent.direction + " newResult = " + (beautifyComponent.newResult.HasValue ? beautifyComponent.newResult.Value.ToString("F4") : "null"), 8, "Scenario3");
            }
            p2.mutate(bestComponent.brainCellIndex, bestComponent.direction * step);
            //logger.log("Beautification once step = " + step.ToString("F6") + " Best result = " + bestComponent.newResult.Value.ToString("F4"), 8, "Scenario3");
            return bestComponent.newResult.Value;
        }

        protected List<BeautifyComponent> beautifyComponents;
        protected void createBeutifyComponents()
        {
            beautifyComponents = new List<BeautifyComponent>();
            for (int iBraiCells = 0; iBraiCells < rules.diceCombinations * rules.situationBrainCellsCount; iBraiCells++)
            {
                for (int iDirection = 0; iDirection < 2; iDirection++)
                {
                    BeautifyComponent beautifyComponent = new BeautifyComponent();
                    beautifyComponent.brainCellIndex = iBraiCells;
                    beautifyComponent.direction = -1.0d + (2.0d * ((double)iDirection));
                    //beautifyComponent.randomOrder = rules.random.NextDouble();
                    beautifyComponents.Add(beautifyComponent);
                }
            }
        }
        protected void randomiseBeautifyComponents()
        {
            foreach (BeautifyComponent beautifyComponent in beautifyComponents)
            {
                beautifyComponent.randomOrder = rules.random.NextDouble();
                beautifyComponent.newResult = null;
            }
        }

        protected class BeautifyComponent
        {
            public int brainCellIndex;
            public double direction;
            public double randomOrder;
            public double? newResult;
        }

        public void scenario2()
        {
            logger.log("Run scenarion 2 - build anti player - brutal force");

            Player p1 = new Player(logger, rules, "p1", true);
            

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



            searchAntiplayerBrootforce(p1);


        }

        public void searchAntiplayerBrootforce(Player p1)
        {
            Player pABrootForce = new Player(logger, rules, "pAntiBrrotforce", true);
            double maxResult = ((double)(-rules.playerCoins));
            logger.log("Start looking for antiPlayer for " + p1.toString());
            while (true)
            {
                double newResult = arenaAugmented.fightStatistics(pABrootForce, p1);
                if (newResult > maxResult)
                {
                    logger.log("New max found, value = " + newResult.ToString("F4") + " " + pABrootForce.toString());
                    logger.logChart(newResult);
                    maxResult = newResult;
                }
                pABrootForce.initialiseRandomise();
            }

        }

        public void scenario1()
        {

            logger.log("Run scenarion 1 - test fightScan and fightStatistics");

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
