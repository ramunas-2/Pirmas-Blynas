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
            construct(logger, rules, name, initialiseRandomiseNow);
        }

        private void construct(Logger logger, Rules rules, string name, bool initialiseRandomiseNow)
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

        public Player(Player model)
        {
            construct(model.logger, model.rules, model.name, false);
            model.copyPlayerNoNew(this);
        }


        //Copy Player
        public Player copyPlayer()
        {
            Player retVal = new Player(logger, rules, name, false);
            copyPlayerNoNew(retVal);
            return retVal;
        }

        public void copyPlayerNoNew(Player pDestination)
        {
            for (int i = 0; i < allBrainCellsCount; i++)
            {
                pDestination.brainCells[i] = this.brainCells[i];
            }
            pDestination.coins = this.coins;
            pDestination.dice = this.dice;
            pDestination.allBrainCellsCount = this.allBrainCellsCount;
            pDestination.strength = this.strength;
        }

        //Reset coins and dice
        public void reset()
        {
            coins = rules.playerCoins;
            dice = rules.random.Next(rules.diceCombinations);
            strength = 0.0d;
            logger.increaseSD("Reset Player = " + name + "; dice = " + dice);
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
                while ((i < scenario.possibleMoves.Count()) && (threshold <= choice))
                {
                    i++;
                    threshold += brainCells[scenario.brainCellsLocation + (dice * rules.situationBrainCellsCount) + i];
                    logger.log("--Cheching threshold = " + threshold.ToString("F4") + " decission " + scenario.possibleMoves[i].ToString(), 8, "Player");
                }
                if (threshold <= choice)
                {
                    logger.log("Don't find move", 0, "Error");
                }
                else
                {
                    retVal = scenario.possibleMoves[i];
                }
            }
            //logger.log("Player : " + name + "; dice : " + dice + "; coins : " + coins + "; move : " + retVal+"; path: "+Rules.intListToString(path));

            logger.increaseSD("Player = " + name + "; dice = " + dice + "; coins = " + coins + "; move = " + retVal + "; path = " + Rules.intListToString(path));

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
        public string toString(int precision = 25)
        {
            string s = "Player <" + name + ">; coins = " + coins.ToString() + "; dice = " + dice.ToString() + "; strength = " + strength.ToString("F" + precision.ToString()) + "; brainCells = ( ";
            for (int i = 0; i < allBrainCellsCount; i++)
            {
                s += (brainCells[i].ToString("F" + precision.ToString()) + "; ");
            }
            s += ")";
            return s;
        }

        public bool isBrainCellsEqual(Player p2)
        {
            bool brainCellsEqual = true;
            for (int i = 0; (i < allBrainCellsCount) && brainCellsEqual; i++)
            {
                if (brainCells[i] != p2.brainCells[i])
                {
                    brainCellsEqual = false;
                }
            }
            return brainCellsEqual;
        }

        public void setMinus1()
        {
            for (int i = 0; i < allBrainCellsCount; i++)
            {
                brainCells[i] = -1.0d;
            }
        }

        public void load01()
        {


            /*
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
            */



            //Result : -0.00117710437669348
            //public int blind = 1;
            //public int playerCoins = 4;
            //public int diceCombinations = 6; 

            brainCells[0] = 0d;
            brainCells[1] = 0.716882900091172d;
            brainCells[2] = 0d;
            brainCells[3] = 0d;
            brainCells[4] = 0.283117099908828d;
            brainCells[5] = 0d;
            brainCells[6] = 0.107502503700574d;
            brainCells[7] = 0d;
            brainCells[8] = 0.292497496299551d;
            brainCells[9] = 0.599999999999875d;
            brainCells[10] = 0.999999999992724d;
            brainCells[11] = 0d;
            brainCells[12] = 7.2759576141834E-12d;
            brainCells[13] = 0d;
            brainCells[14] = 1d;
            brainCells[15] = 0d;
            brainCells[16] = 0d;
            brainCells[17] = 0.430588415558724d;
            brainCells[18] = 0.569411584441276d;
            brainCells[19] = 0.535398884925711d;
            brainCells[20] = 0.464601115074289d;
            brainCells[21] = 1d;
            brainCells[22] = 0d;
            brainCells[23] = 0d;
            brainCells[24] = 1d;
            brainCells[25] = 0d;
            brainCells[26] = 1d;
            brainCells[27] = 0d;
            brainCells[28] = 0.99999999999803d;
            brainCells[29] = 0d;
            brainCells[30] = 0d;
            brainCells[31] = 1.9704238241047E-12d;
            brainCells[32] = 0d;
            brainCells[33] = 0d;
            brainCells[34] = 1d;
            brainCells[35] = 0.366985529366409d;
            brainCells[36] = 0.633014470633592d;
            brainCells[37] = 0.954340372491128d;
            brainCells[38] = 0.0456596275088717d;
            brainCells[39] = 0.989462848032417d;
            brainCells[40] = 0d;
            brainCells[41] = 0.0105371519675829d;
            brainCells[42] = 0d;
            brainCells[43] = 1d;
            brainCells[44] = 1d;
            brainCells[45] = 0d;
            brainCells[46] = 0d;
            brainCells[47] = 1d;
            brainCells[48] = 0d;
            brainCells[49] = 0d;
            brainCells[50] = 0d;
            brainCells[51] = 0d;
            brainCells[52] = 1d;
            brainCells[53] = 0d;
            brainCells[54] = 0d;
            brainCells[55] = 0d;
            brainCells[56] = 0.572818111901396d;
            brainCells[57] = 0.427181888098604d;
            brainCells[58] = 0d;
            brainCells[59] = 0d;
            brainCells[60] = 0.181560109919664d;
            brainCells[61] = 0.632810072802384d;
            brainCells[62] = 0.185629817277952d;
            brainCells[63] = 5.55111512E-17d;
            brainCells[64] = 1d;
            brainCells[65] = 1d;
            brainCells[66] = 0d;
            brainCells[67] = 0.732045883837233d;
            brainCells[68] = 0.267954116162767d;
            brainCells[69] = 0d;
            brainCells[70] = 1d;
            brainCells[71] = 0d;
            brainCells[72] = 0.996204457822387d;
            brainCells[73] = 0.00379554217761324d;
            brainCells[74] = 0.447417363056799d;
            brainCells[75] = 0.552582636943201d;
            brainCells[76] = 0d;
            brainCells[77] = 0d;
            brainCells[78] = 0d;
            brainCells[79] = 0.641992830970321d;
            brainCells[80] = 0.358007169029679d;
            brainCells[81] = 5.55111512E-17d;
            brainCells[82] = 1d;
            brainCells[83] = 5.55111512E-17d;
            brainCells[84] = 1d;
            brainCells[85] = 0.514694663938381d;
            brainCells[86] = 0.331071254199185d;
            brainCells[87] = 0.154234081862434d;
            brainCells[88] = 1d;
            brainCells[89] = 1.110223025E-16d;
            brainCells[90] = 1d;
            brainCells[91] = 2.77555756E-17d;
            brainCells[92] = 0d;
            brainCells[93] = 1d;
            brainCells[94] = 0d;
            brainCells[95] = 0d;
            brainCells[96] = 0d;
            brainCells[97] = 0d;
            brainCells[98] = 1d;
            brainCells[99] = 0d;
            brainCells[100] = 0d;
            brainCells[101] = 0d;
            brainCells[102] = 0.386535841907674d;
            brainCells[103] = 0.613464158092326d;
            brainCells[104] = 0d;
            brainCells[105] = 0d;
            brainCells[106] = 0.574263636566097d;
            brainCells[107] = 0.31145802061607d;
            brainCells[108] = 0.114278342817833d;
            brainCells[109] = 0.743786830801418d;
            brainCells[110] = 0.256213169198582d;
            brainCells[111] = 0.606270552052528d;
            brainCells[112] = 0.393729447947472d;
            brainCells[113] = 1d;
            brainCells[114] = 0d;
            brainCells[115] = 0d;
            brainCells[116] = 0.835051041019639d;
            brainCells[117] = 0.164948958980361d;
            brainCells[118] = 1d;
            brainCells[119] = 0d;
            brainCells[120] = 0.588336146667758d;
            brainCells[121] = 0.204362995076735d;
            brainCells[122] = 0d;
            brainCells[123] = 0.207300858255507d;
            brainCells[124] = 0.853461132316599d;
            brainCells[125] = 0.146538867683401d;
            brainCells[126] = 0d;
            brainCells[127] = 1d;
            brainCells[128] = 5.55111512E-17d;
            brainCells[129] = 0.266580754549513d;
            brainCells[130] = 0.733419245450487d;
            brainCells[131] = 0.70265987970036d;
            brainCells[132] = 0.29734012029964d;
            brainCells[133] = 0d;
            brainCells[134] = 1d;
            brainCells[135] = 0d;
            brainCells[136] = 1d;
            brainCells[137] = 5.55111512E-17d;
            brainCells[138] = 9.88792381E-17d;
            brainCells[139] = 1d;
            brainCells[140] = 0d;
            brainCells[141] = 0d;
            brainCells[142] = 0d;
            brainCells[143] = 0d;
            brainCells[144] = 1d;
            brainCells[145] = 0d;
            brainCells[146] = 0d;
            brainCells[147] = 0d;
            brainCells[148] = 0.472588877900692d;
            brainCells[149] = 0.527411122081119d;
            brainCells[150] = 1.81898939541434E-11d;
            brainCells[151] = 0d;
            brainCells[152] = 1d;
            brainCells[153] = 0d;
            brainCells[154] = 0d;
            brainCells[155] = 0d;
            brainCells[156] = 1d;
            brainCells[157] = 0.829245429872184d;
            brainCells[158] = 0.170754570127816d;
            brainCells[159] = 0.475746865934904d;
            brainCells[160] = 0.524253134065096d;
            brainCells[161] = 0d;
            brainCells[162] = 0.673099795676192d;
            brainCells[163] = 0.326900204323808d;
            brainCells[164] = 0.675657375359258d;
            brainCells[165] = 0.324342624640742d;
            brainCells[166] = 0d;
            brainCells[167] = 1d;
            brainCells[168] = 0d;
            brainCells[169] = 0d;
            brainCells[170] = 0.984049259211891d;
            brainCells[171] = 0d;
            brainCells[172] = 0.015950740788109d;
            brainCells[173] = 1d;
            brainCells[174] = 0d;
            brainCells[175] = 1d;
            brainCells[176] = 0d;
            brainCells[177] = 0.740107216536568d;
            brainCells[178] = 0.259892783463432d;
            brainCells[179] = 0d;
            brainCells[180] = 0.944728341859173d;
            brainCells[181] = 0.0552716581408269d;
            brainCells[182] = 0.620006676534464d;
            brainCells[183] = 0.379993323465536d;
            brainCells[184] = 0d;
            brainCells[185] = 1d;
            brainCells[186] = 0d;
            brainCells[187] = 0d;
            brainCells[188] = 0d;
            brainCells[189] = 0d;
            brainCells[190] = 0.415005007400747d;
            brainCells[191] = 0d;
            brainCells[192] = 0.584994992599253d;
            brainCells[193] = 0d;
            brainCells[194] = 0d;
            brainCells[195] = 0.395186088433609d;
            brainCells[196] = 2.77555756E-17d;
            brainCells[197] = 0.604813911566391d;
            brainCells[198] = 0.342111126213386d;
            brainCells[199] = 0.486200311504072d;
            brainCells[200] = 0.171688562282542d;
            brainCells[201] = 5.55111512E-17d;
            brainCells[202] = 1d;
            brainCells[203] = 0.20147042988656d;
            brainCells[204] = 0.79852957011344d;
            brainCells[205] = 0d;
            brainCells[206] = 0.667171350990968d;
            brainCells[207] = 0.332828649009032d;
            brainCells[208] = 0d;
            brainCells[209] = 1d;
            brainCells[210] = 0d;
            brainCells[211] = 1d;
            brainCells[212] = 0d;
            brainCells[213] = 0.621821916593035d;
            brainCells[214] = 0d;
            brainCells[215] = 0.378178083406965d;
            brainCells[216] = 0.255550318884391d;
            brainCells[217] = 0d;
            brainCells[218] = 0.744449681115609d;
            brainCells[219] = 4.401955466804E-10d;
            brainCells[220] = 0.999999999559804d;
            brainCells[221] = 0d;
            brainCells[222] = 1d;
            brainCells[223] = 0.0799510177174394d;
            brainCells[224] = 0.920048982282561d;
            brainCells[225] = 0d;
            brainCells[226] = 0.293200512087532d;
            brainCells[227] = 0.706799487912468d;
            brainCells[228] = 0d;
            brainCells[229] = 1d;
            brainCells[230] = 0d;
            brainCells[231] = 0.528138166818628d;
            brainCells[232] = 0d;
            brainCells[233] = 0d;
            brainCells[234] = 0.471861833181372d;
            brainCells[235] = 0d;
            brainCells[236] = 0d;
            brainCells[237] = 0d;
            brainCells[238] = 2.102207297128E-13d;
            brainCells[239] = 0.99999999999979d;
            brainCells[240] = 0d;
            brainCells[241] = 1.110223025E-16d;
            brainCells[242] = 0d;
            brainCells[243] = 1d;
            brainCells[244] = 0d;
            brainCells[245] = 0.867087549468078d;
            brainCells[246] = 0.132912450531922d;
            brainCells[247] = 0.477700518365847d;
            brainCells[248] = 0.522299481634153d;
            brainCells[249] = 0d;
            brainCells[250] = 1d;
            brainCells[251] = 0d;
            brainCells[252] = 0d;
            brainCells[253] = 1d;
            brainCells[254] = 0d;
            brainCells[255] = 1d;
            brainCells[256] = 5.55111512E-17d;
            brainCells[257] = 1d;
            brainCells[258] = 0d;
            brainCells[259] = 0d;
            brainCells[260] = 0d;
            brainCells[261] = 1d;
            brainCells[262] = 0.673664973896772d;
            brainCells[263] = 0.326335026103228d;
            brainCells[264] = 0d;
            brainCells[265] = 0.065662559618085d;
            brainCells[266] = 0.934337440381915d;
            brainCells[267] = 0.854837379350717d;
            brainCells[268] = 0.145162620649283d;
            brainCells[269] = 0d;
            brainCells[270] = 0d;
            brainCells[271] = 1d;
            brainCells[272] = 0.922217712235738d;
            brainCells[273] = 0.0777822877642616d;
            brainCells[274] = 1.110223025E-16d;
            brainCells[275] = 1d;






        }




        public void load02()
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

