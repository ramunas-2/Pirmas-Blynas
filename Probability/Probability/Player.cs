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
            logger.log("Make Move from path ( " + rules.intListToString(path) + ")", 8, "Player");
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
            brainCells[0] = 0d;
            brainCells[1] = 0.614932611089377d;
            brainCells[2] = 0d;
            brainCells[3] = 0.385067388910623d;
            brainCells[4] = 0d;
            brainCells[5] = 0.384748012352251d;
            brainCells[6] = 0.115251987647749d;
            brainCells[7] = 0.5d;
            brainCells[8] = 0.99999999999709d;
            brainCells[9] = 0d;
            brainCells[10] = 2.9103386367524E-12d;
            brainCells[11] = 1d;
            brainCells[12] = 5.55111512E-17d;
            brainCells[13] = 1d;
            brainCells[14] = 0d;
            brainCells[15] = 0.819311554917875d;
            brainCells[16] = 0d;
            brainCells[17] = 0.180688445082125d;
            brainCells[18] = 1d;
            brainCells[19] = 0d;
            brainCells[20] = 1d;
            brainCells[21] = 0d;
            brainCells[22] = 2.08166817E-17d;
            brainCells[23] = 1d;
            brainCells[24] = 0d;
            brainCells[25] = 0d;
            brainCells[26] = 6.9388939E-18d;
            brainCells[27] = 1d;
            brainCells[28] = 0d;
            brainCells[29] = 0d;
            brainCells[30] = 0.538310870365266d;
            brainCells[31] = 0.461689129634734d;
            brainCells[32] = 0d;
            brainCells[33] = 0.766422686897754d;
            brainCells[34] = 0.233577313102246d;
            brainCells[35] = 0.961199458317424d;
            brainCells[36] = 0.0388005416825764d;
            brainCells[37] = 0.642271115869516d;
            brainCells[38] = 0.357728884130484d;
            brainCells[39] = 0d;
            brainCells[40] = 1d;
            brainCells[41] = 1.110223025E-16d;
            brainCells[42] = 0.716794824815521d;
            brainCells[43] = 0.283205175184479d;
            brainCells[44] = 0d;
            brainCells[45] = 1d;
            brainCells[46] = 0d;
            brainCells[47] = 0d;
            brainCells[48] = 0d;
            brainCells[49] = 0.654244037053071d;
            brainCells[50] = 0.345755962946929d;
            brainCells[51] = 0d;
            brainCells[52] = 0d;
            brainCells[53] = 0.41974411731062d;
            brainCells[54] = 0.58025588268938d;
            brainCells[55] = 9.02056208E-17d;
            brainCells[56] = 1d;
            brainCells[57] = 2.25514052E-17d;
            brainCells[58] = 1d;
            brainCells[59] = 5.55111512E-17d;
            brainCells[60] = 1d;
            brainCells[61] = 0d;
            brainCells[62] = 0.637520924035935d;
            brainCells[63] = 0.362479075964065d;
            brainCells[64] = 0.379392178274477d;
            brainCells[65] = 0.620607821725523d;
            brainCells[66] = 0d;
            brainCells[67] = 0.229865222177881d;
            brainCells[68] = 0d;
            brainCells[69] = 0.770134777822119d;
            brainCells[70] = 0d;
            brainCells[71] = 5.55111512E-17d;
            brainCells[72] = 0d;
            brainCells[73] = 1d;
            brainCells[74] = 0d;
            brainCells[75] = 2.77555756E-17d;
            brainCells[76] = 1d;
            brainCells[77] = 0.532729303246704d;
            brainCells[78] = 0.467270696753296d;
            brainCells[79] = 0d;
            brainCells[80] = 1d;
            brainCells[81] = 0d;
            brainCells[82] = 0.0d;
            brainCells[83] = 1.0d;
            brainCells[84] = 5.55111512E-17d;
            brainCells[85] = 1d;
            brainCells[86] = 6.24500451E-17d;
            brainCells[87] = 1d;


        }

    }
}

