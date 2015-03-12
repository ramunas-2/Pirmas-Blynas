using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Probability
{
    class Arena
    {
        Logger logger;
        Rules rules;

        public Arena(Logger logger, Rules rules)
        {
            logger.set("Fight", 1, Color.Blue);
            this.logger = logger;
            this.rules = rules;

        }

        //Fight
        //2 players, given dices, choieces made on braincells and random, fight to the end, return won coins
        public int fight(Player p1, Player p2)
        {
            logger.log("p1 dice = " + p1.dice.ToString() + " ; p2 dice = " + p2.dice.ToString(), 9, "Fight");
            int retVal = 0;
            Player pCurrrent = p1;
            Player pOpponent = p2;
            bool gameOver = false;
            List<int> path = new List<int>();
            int pot = 0;
            //make blinds
            pot += (rules.blind * 2);
            pCurrrent.coins -= rules.blind;
            pOpponent.coins -= rules.blind;

            while (!gameOver)
            {
                logger.log("Fight, not gameOver", 9, "Fight");
                int choice = pCurrrent.makeMove(path);
                path.Add(choice);
                logger.log("Path = ( " + Rules.intListToString(path) + ")", 9, "Fight");
                Scenario scenario = rules.findScenarioByPath(path);
                if (choice > 0)
                {//Raise coins
                    pot += choice;
                    pCurrrent.coins -= choice;
                }
                if (scenario.gameOver)
                {
                    if (choice == -1)
                    {//Fold
                        pOpponent.coins += pot;
                        pot = 0;
                    }
                    else
                    {//Show down
                        if (pCurrrent.dice == pOpponent.dice)
                        {//Draw
                            if (pot % 2 == 0)
                            {
                                pCurrrent.coins += (pot / 2);
                                pOpponent.coins += (pot / 2);
                            }
                            else
                            {//Pot odd
                                logger.log("Pot is odd", 0, "Error");
                            }
                        }
                        else
                        {
                            if (pCurrrent.dice > pOpponent.dice)
                            {//Current player wins
                                pCurrrent.coins += pot;
                            }
                            else
                            {//Opponent wins
                                pOpponent.coins += pot;
                            }
                        }
                    }
                    gameOver = true;
                }

                Player pSwap = pCurrrent;
                pCurrrent = pOpponent;
                pOpponent = pSwap;
            }
            int difference = p1.coins - p2.coins;
            if (difference % 2 == 1)
            {
                logger.log("Difference is odd", 0, "Error");
            }
            retVal = difference / 2;
            logger.log("Fight result = " + retVal.ToString(), 9, "Fight");
            return retVal;
        }

        //FightScan Scan random dices all positions (p1 starts, p2 starts), repeat given times to eliminate random deviation
        public double fightScan(Player p1, Player p2, int repeatCount)
        {
            double retVal = 0.0d;
            for (int i = 0; i < repeatCount; i++)
            {
                p1.reset();
                p2.reset();
                retVal += ((double)fight(p1, p2));
                p1.reset();
                p2.reset();
                retVal -= ((double)fight(p2, p1));
            }
            retVal /= (repeatCount * 2);
            return retVal;
        }



    }
}
