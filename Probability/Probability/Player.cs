using System;
using System.Collections.Generic;
using System.Drawing;
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
        public double[] storeBrainCells;
        public int coins;
        public int dice;
        public int allBrainCellsCount;
        public string name;
        public double strength;


        //New  player (constructor)
        public Player(Logger logger, Rules rules, string name = "", bool initialiseRandomiseNow = false)
        {
            logger.set("Player", 1, Color.Green);
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
            retVal.strength = this.strength;


            return retVal;
        }

        //Reset coins and dice
        public void reset()
        {
            coins = rules.playerCoins;
            dice = rules.random.Next(rules.diceCombinations);
            strength = 0.0d;
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
            for (int dice = 0; dice < rules.diceCombinations; dice++)
            {
                int brainCellsBegin = dice * rules.situationBrainCellsCount;
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
        }



        //Make Move
        public int makeMove(List<int> path)
        {
            int retVal = -2;
            Scenario scenario = rules.findScenarioByPath(path);
            logger.log("Make Move from path ( " + Rules.intListToString(path) + ")", 8, "Player");
            if (scenario.gameOver)
            {
                logger.log("Can't make move, gameOver", 0, "Error");
            }
            else
            {
                double choice = rules.random.NextDouble();
                logger.log("Random choice = " + choice.ToString("F4"), 8, "Player");
                int i = 0;
                double threshold = brainCells[scenario.brainCellsLocation + (dice * rules.situationBrainCellsCount)];
                logger.log("--Cheching threshold = " + threshold.ToString("F4") + " decission " + scenario.possibleMoves[i].ToString(), 8, "Player");
                while ((i < scenario.possibleMoves.Count()) && (threshold < choice))
                {
                    i++;
                    threshold += brainCells[scenario.brainCellsLocation + (dice * rules.situationBrainCellsCount) + i];
                    logger.log("--Cheching threshold = " + threshold.ToString("F4") + " decission " + scenario.possibleMoves[i].ToString(), 8, "Player");
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

        public void mutate(int brainCellToMutate, double delta)
        {
            brainCells[brainCellToMutate] += (delta);
            brainCells[brainCellToMutate] = Math.Min(brainCells[brainCellToMutate], 1.0d);
            brainCells[brainCellToMutate] = Math.Max(brainCells[brainCellToMutate], 0.0d);
            normaliseBrainCells();
        }

        public void mutateAdvanced(int brainCellToMutate, int brainCellToCompensate, double delta)
        {



            double b1 = brainCells[brainCellToMutate];
            double b2 = brainCells[brainCellToCompensate];
            double b1Old = b1;
            double b2Old = b2;

            b1 += delta;
            b2 -= delta;

            b1 = Math.Min(b1, 1.0d);
            b1 = Math.Max(b1, 0.0d);
            b2 = Math.Min(b2, 1.0d);
            b2 = Math.Max(b2, 0.0d);

            double delta1 = b1 - b1Old;
            double delta2 = b2Old - b2;


            double deltaNew = (Math.Abs(delta1) < Math.Abs(delta2)) ? delta1 : delta2;


            brainCells[brainCellToMutate] += (deltaNew);

            brainCells[brainCellToCompensate] -= (deltaNew);


            //remove?
            normaliseBrainCells();
        }


        public void push()
        {
            storeBrainCells = new double[allBrainCellsCount];
            for (int i = 0; i < allBrainCellsCount; i++)
            {
                storeBrainCells[i] = brainCells[i];
            }
        }

        public void pop()
        {
            for (int i = 0; i < allBrainCellsCount; i++)
            {
                brainCells[i] = storeBrainCells[i];
            }
        }

        //toString
        public string toString()
        {
            string s = "Player <" + name + ">; coins = " + coins.ToString() + "; dice = " + dice.ToString() + "; brainCells = ( ";
            for (int i = 0; i < allBrainCellsCount; i++)
            {
                s += (brainCells[i].ToString("F25") + "; ");
            }
            s += ")";
            return s;
        }

        public bool isBrainCellsEqual(Player p2)
        {
            bool brainCellsEqual = true;
            for (int i = 0; (i < allBrainCellsCount) && brainCellsEqual; i++)
            {
                if (brainCells[i]!=p2.brainCells[i])
                {
                    brainCellsEqual = false;
                }
            }
            return brainCellsEqual;
        }

        public void load01()
        {


            //Result : -0,0000000000000727
            //public int blind = 1;
            //public int playerCoins = 3;
            //public int diceCombinations = 4; 

            brainCells[0] = 0d;
            brainCells[1] = 0.615384615384305d;
            brainCells[2] = 0d;
            brainCells[3] = 0.384615384615695d;
            brainCells[4] = 0d;
            brainCells[5] = 0.384615384616235d;
            brainCells[6] = 0.115384615384198d;
            brainCells[7] = 0.499999999999568d;
            brainCells[8] = 1d;
            brainCells[9] = 0d;
            brainCells[10] = 0d;
            brainCells[11] = 1d;
            brainCells[12] = 5.55111512E-17d;
            brainCells[13] = 1d;
            brainCells[14] = 0d;
            brainCells[15] = 0.771364174667987d;
            brainCells[16] = 0d;
            brainCells[17] = 0.228635825332013d;
            brainCells[18] = 1d;
            brainCells[19] = 0d;
            brainCells[20] = 1d;
            brainCells[21] = 0d;
            brainCells[22] = 2.08166817E-17d;
            brainCells[23] = 1d;
            brainCells[24] = 0d;
            brainCells[25] = 0d;
            brainCells[26] = 0d;
            brainCells[27] = 1d;
            brainCells[28] = 5.55111512E-17d;
            brainCells[29] = 0d;
            brainCells[30] = 0.538461538461756d;
            brainCells[31] = 0.461538461538244d;
            brainCells[32] = 0d;
            brainCells[33] = 0.766422686897754d;
            brainCells[34] = 0.233577313102246d;
            brainCells[35] = 0.961538461538467d;
            brainCells[36] = 0.038461538461533d;
            brainCells[37] = 0.65820568260242d;
            brainCells[38] = 0.34179431739758d;
            brainCells[39] = 0d;
            brainCells[40] = 1d;
            brainCells[41] = 0d;
            brainCells[42] = 0.735701051884098d;
            brainCells[43] = 0.264298948115902d;
            brainCells[44] = 0d;
            brainCells[45] = 1d;
            brainCells[46] = 0d;
            brainCells[47] = 1.387778781E-16d;
            brainCells[48] = 0d;
            brainCells[49] = 0.65384615384593d;
            brainCells[50] = 0.346153846152341d;
            brainCells[51] = 1.7287837827951E-12d;
            brainCells[52] = 0d;
            brainCells[53] = 0.649848677137742d;
            brainCells[54] = 0.350151322862258d;
            brainCells[55] = 9.02056208E-17d;
            brainCells[56] = 1d;
            brainCells[57] = 0d;
            brainCells[58] = 1d;
            brainCells[59] = 0d;
            brainCells[60] = 1d;
            brainCells[61] = 5.55111512E-17d;
            brainCells[62] = 1d;
            brainCells[63] = 0d;
            brainCells[64] = 0.360452794269914d;
            brainCells[65] = 0.639547205730086d;
            brainCells[66] = 0d;
            brainCells[67] = 0.230769230768585d;
            brainCells[68] = 0d;
            brainCells[69] = 0.769230769231415d;
            brainCells[70] = 0d;
            brainCells[71] = 5.55111512E-17d;
            brainCells[72] = 0d;
            brainCells[73] = 1d;
            brainCells[74] = 0d;
            brainCells[75] = 0d;
            brainCells[76] = 1d;
            brainCells[77] = 2.77555756E-17d;
            brainCells[78] = 1d;
            brainCells[79] = 0d;
            brainCells[80] = 1d;
            brainCells[81] = 0d;
            brainCells[82] = 8.32667268E-17d;
            brainCells[83] = 1d;
            brainCells[84] = 5.55111512E-17d;
            brainCells[85] = 1d;
            brainCells[86] = 6.24500451E-17d;
            brainCells[87] = 1d;







        }

    }
}

