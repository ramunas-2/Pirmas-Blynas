using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Probability
{
    class ArenaAugmented
    {
        Logger logger;
        Rules rules;
        List<FightStatisticsComponent> fightStatisticComponents = new List<FightStatisticsComponent>();

        public ArenaAugmented(Logger logger, Rules rules)
        {
            logger.set("FightS", 1, Color.Green);
            this.logger = logger;
            this.rules = rules;
            fillFightStatisticsComponents();


        }

        void fillFightStatisticsComponents()
        {
            for (int dice1 = 0; dice1 < rules.diceCombinations; dice1++)
            {
                for (int dice2 = 0; dice2 < rules.diceCombinations; dice2++)
                {
                    for (int whoEnds = 1; whoEnds <= 2; whoEnds++)
                    {
                        logger.log("Scan scenario dice1 = " + dice1.ToString() + " dice2 = " + dice2.ToString() + " whoEnds = " + whoEnds.ToString(), 8, "FightS");
                        //foreach (Scenario scenario in rules.scenarios.TakeWhile(item => item.gameOver))
                        foreach (Scenario scenario in rules.scenarios)
                        {
                            if (scenario.gameOver)
                            {
                                logger.log("Analysing scenario ( " + rules.intListToString(scenario.path) + ")", 8, "FightS");
                                bool currentPlayerEnds = true;
                                List<int> remainingPath = new List<int>(scenario.path);
                                List<int> probabilityComponents = new List<int>();
                                foreach (int ii in scenario.path.Reverse<int>())
                                {
                                    //Get the Nr of braincell that lead to this scenatio
                                    //Split path:
                                    //beginning (path-1)


                                    //Check which one is right?
                                    //var pathMinusOne = scenarion.path.Take(scenarion.path.Count() - 1);
                                    List<int> pathMinusOne = remainingPath.Take(remainingPath.Count() - 1).ToList<int>();


                                    //Ending
                                    int endAction = remainingPath[remainingPath.Count() - 1];

                                    Scenario scenarioMinusOne = rules.findScenarioByPath(pathMinusOne);

                                    int lastMoveFoundAt = 0;
                                    bool moveFound = false;
                                    foreach (int jj in scenarioMinusOne.possibleMoves)
                                    {
                                        if (jj == endAction)
                                        {
                                            moveFound = true;
                                            break;
                                        }
                                        else
                                        {
                                            lastMoveFoundAt++;
                                        }

                                    }
                                    if (!moveFound)
                                    {
                                        logger.log("Last move not found", 1, "Error");
                                    }

                                    //construct brain cell location:
                                    int brainCellLocation = 0;

                                    //add smallest shift from last move
                                    brainCellLocation += lastMoveFoundAt;

                                    //add brain cells location
                                    brainCellLocation += scenarioMinusOne.brainCellsLocation;

                                    //add dice shift according to who moves
                                    //brainCellLocation += (rules.situationBrainCellsCount) * (((whoEnds == 1) ^ currentPlayerEnds) ? dice1 : dice2);
                                    //brainCellLocation += (rules.situationBrainCellsCount) * (((whoEnds == 1) ^ currentPlayerEnds) ? dice2 : dice1);
                                    brainCellLocation += (rules.situationBrainCellsCount) * (((whoEnds == 1) ^ currentPlayerEnds) ? dice2 : dice1);


                                    probabilityComponents.Add(brainCellLocation);


                                    currentPlayerEnds ^= true;
                                    remainingPath = pathMinusOne;
                                }
                                int wonCoins;

                                //Calculate amount of coins put by current player
                                int putCoints = rules.blind;
                                for (int pathIndex = scenario.path.Count - 1; pathIndex >= 0; pathIndex -= 2)
                                {
                                    int pathDecision = scenario.path[pathIndex];
                                    if (pathDecision > 0)
                                    {
                                        putCoints += pathDecision;
                                    }
                                }

                                if (scenario.path[scenario.path.Count() - 1] == -1)
                                {//Fold
                                    wonCoins = ((whoEnds == 1) ? (-putCoints) : (putCoints));
                                }
                                else
                                {
                                    if (dice1 == dice2)
                                    {
                                        wonCoins = 0;
                                    }
                                    else
                                    {
                                        //wonCoins = (((whoEnds == 1) ^ (dice1 > dice2)) ? (-putCoints) : (putCoints));
                                        wonCoins = ((dice1 > dice2) ? (putCoints) : (-putCoints));
                                    }
                                }
                                FightStatisticsComponent fightStatisticComponent = new FightStatisticsComponent();
                                fightStatisticComponent.wonCoins = wonCoins;
                                fightStatisticComponent.whoEnds = whoEnds;
                                fightStatisticComponent.probabilityComponents = probabilityComponents;
                                fightStatisticComponents.Add(fightStatisticComponent);
                                logger.log("wonCoins = " + wonCoins.ToString() + " probabilityComponents = ( " + rules.intListToString(probabilityComponents) + ")", 8, "FightS");
                            }
                        }
                    }
                }
            }
        }

        //Virtual fight FightStatistics - return average won coins
        public double fightStatistics(Player p1, Player p2)
        {
            double debugCheckSum = 0;
            double retVal = 0;
            foreach (FightStatisticsComponent fightStatisticComponent in fightStatisticComponents)
            {
                double component = 1.0;
                bool currentPlayer = true;
                string debugS = "Component coins = " + fightStatisticComponent.wonCoins.ToString() + " probability = ( ";
                foreach (int ii in fightStatisticComponent.probabilityComponents)
                {
                    debugS+=((((fightStatisticComponent.whoEnds == 1) ^ (currentPlayer)) ? p2.brainCells[ii] : p1.brainCells[ii]).ToString("F4")+"; ");
                    component *= (((fightStatisticComponent.whoEnds == 1) ^ (currentPlayer)) ? p2.brainCells[ii] : p1.brainCells[ii]);
                    currentPlayer ^= true;
                }
                debugS += ")";
                logger.log(debugS, 8, "FightS");
                debugCheckSum += component;
                component *= ((double)fightStatisticComponent.wonCoins);
                retVal += component;
            }
            logger.log("Debug check sum = " + debugCheckSum.ToString(),8, "FightS");
            retVal /= (2 * rules.diceCombinations * rules.diceCombinations);
            return retVal;
        }





        //Test FightStatistics against FightScan
        //Done manually


    }
    class FightStatisticsComponent
    {
        public int wonCoins;
        public int whoEnds;
        public List<int> probabilityComponents = new List<int>();
    }
}

