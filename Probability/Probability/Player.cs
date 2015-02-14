﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Probability
{
    class Player
    {
        Logger logger;
        Rules rules;

        //Braincells
        public double[] brainCells;
        public int coins;
        public int dice;
        public int allBrainCellsCount;
        public string name;


        //New  player (constructor)
        public Player(Logger logger, Rules rules, string name = "", bool initialiseRandomiseNow = false)
        {
            this.logger = logger;
            this.rules = rules;
            this.name = name;
            allBrainCellsCount = this.rules.situationBrainCellsCount * this.rules.diceCombinations;
            brainCells = new double[allBrainCellsCount];
            if (initialiseRandomiseNow)
            {
                initialiseRandomise();
            }
        }

        //Copy Player
        public Player copyPlayer()
        {
            Player retVal = new Player(logger, rules, name, false);
            for (int i = 0; i < allBrainCellsCount; i++)
            {
                retVal.brainCells[i] = this.brainCells[i];
            }
            retVal.coins = this.coins;
            retVal.dice = this.dice;
            retVal.allBrainCellsCount = this.allBrainCellsCount;


            return retVal;
        }

        //Reset coins and dice
        public void reset()
        {
            coins = rules.playerCoins;
            dice = rules.random.Next(rules.diceCombinations);
        }

        //Initialise and randomise Player
        public void initialiseRandomise()
        {
            reset();
            for (int i = 0; i < allBrainCellsCount; i++)
            {
                brainCells[i] = rules.random.NextDouble();
            }
            normaliseBrainCells();
        }

        public void normaliseBrainCells()
        {
            int brainCellsBegin = 0;
            foreach (Scenario scenario in rules.scenarios)
            {
                double brainCellsSum = 0.0d;
                for (int i = brainCellsBegin; i < brainCellsBegin + scenario.possibleMoves.Count; i++)
                {
                    brainCellsSum += brainCells[i];
                }
                for (int i = brainCellsBegin; i < brainCellsBegin + scenario.possibleMoves.Count; i++)
                {
                    if (brainCellsSum > 0.0d)
                    {
                        brainCells[i] /= brainCellsSum;
                    }
                    else
                    {
                        brainCells[i] = 1.0d / ((double)scenario.possibleMoves.Count);
                    }
                }
                brainCellsBegin += scenario.possibleMoves.Count;
            }
        }



        //Make Move
        public int makeMove(List<int> path)
        {
            int retVal = -2;
            Scenario scenario = rules.findScenarioByPath(path);
            if (scenario.gameOver)
            {
                logger.log("Can't make move, gameOver",0,"Error");
            }
            else
            {
                double choice = rules.random.NextDouble();
                int i = 0;
                double threshold = brainCells[scenario.brainCellsLocation ];
                while ((i < scenario.possibleMoves.Count()) && (threshold < choice))
                {
                    i++;
                    threshold += brainCells[scenario.brainCellsLocation + i];
                }
                if (threshold < choice)
                {
                    logger.log("Don't find move", 0, "Error");
                }
                else
                {
                    retVal = scenario.possibleMoves[i];
                }
            }
            return retVal;
        }


        //toString
        public string toString()
        {
            string s = "Player <" + name + ">; coins = " + coins.ToString() + "; dice = " + dice.ToString() + "; brainCells = ( ";
            for (int i = 0; i < allBrainCellsCount; i++)
            {
                s += (brainCells[i].ToString("F4") + "; ");
            }
            s += ")";
            return s;
        }


    }
}
