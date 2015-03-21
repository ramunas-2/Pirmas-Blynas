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
        public string toString(int precision = 25)
        {
            string s = "Player <" + name + ">; coins = " + coins.ToString() + "; dice = " + dice.ToString() + "; strength = " + strength.ToString("F" + precision.ToString()) + "; brainCells = ( ";
            for (int i = 0; i < allBrainCellsCount; i++)
            {
                s += (brainCells[i].ToString("F"+precision.ToString()) + "; ");
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



            //Result : -0,0513028506490253
            //public int blind = 1;
            //public int playerCoins = 4;
            //public int diceCombinations = 6; 

            /*
            brainCells[0] = 0.559408733331443d;
            brainCells[1] = 0d;
            brainCells[2] = 0d;
            brainCells[3] = 0d;
            brainCells[4] = 0.440591266668557d;
            brainCells[5] = 0.0651328285439376d;
            brainCells[6] = 0d;
            brainCells[7] = 0.082591408050398d;
            brainCells[8] = 0d;
            brainCells[9] = 0.852275763405664d;
            brainCells[10] = 0.422807153426515d;
            brainCells[11] = 0.0178805308005849d;
            brainCells[12] = 0.111808329892247d;
            brainCells[13] = 0.447503985880653d;
            brainCells[14] = 0.404990421220998d;
            brainCells[15] = 0d;
            brainCells[16] = 0.595009578779002d;
            brainCells[17] = 0.652291898884908d;
            brainCells[18] = 0.347708101115092d;
            brainCells[19] = 0.144075870769092d;
            brainCells[20] = 0.855924129230908d;
            brainCells[21] = 0.332624407753405d;
            brainCells[22] = 0.296839721264626d;
            brainCells[23] = 0.370535870981969d;
            brainCells[24] = 0.618243367315566d;
            brainCells[25] = 0.381756632684434d;
            brainCells[26] = 0.629186599885178d;
            brainCells[27] = 0.370813400114822d;
            brainCells[28] = 0.830892306195963d;
            brainCells[29] = 0d;
            brainCells[30] = 0d;
            brainCells[31] = 0.169107693804037d;
            brainCells[32] = 0.486234217438072d;
            brainCells[33] = 0.344207552293125d;
            brainCells[34] = 0.169558230268803d;
            brainCells[35] = 0.255357272306665d;
            brainCells[36] = 0.744642727693335d;
            brainCells[37] = 0.566236846040747d;
            brainCells[38] = 0.433763153959253d;
            brainCells[39] = 0.975725235632705d;
            brainCells[40] = 0d;
            brainCells[41] = 0.0242747643672955d;
            brainCells[42] = 0.567522894198049d;
            brainCells[43] = 0.432477105801951d;
            brainCells[44] = 1d;
            brainCells[45] = 2.77555756E-17d;
            brainCells[46] = 0.72636155264649d;
            brainCells[47] = 0d;
            brainCells[48] = 0d;
            brainCells[49] = 0d;
            brainCells[50] = 0.27363844735351d;
            brainCells[51] = 0d;
            brainCells[52] = 0.946786079032768d;
            brainCells[53] = 0.0532139209672315d;
            brainCells[54] = 0d;
            brainCells[55] = 0d;
            brainCells[56] = 0.323860735345641d;
            brainCells[57] = 0.291524804079047d;
            brainCells[58] = 0.384614460575312d;
            brainCells[59] = 0d;
            brainCells[60] = 0.264527000766802d;
            brainCells[61] = 0d;
            brainCells[62] = 0.735472999233198d;
            brainCells[63] = 0.466251359971803d;
            brainCells[64] = 0.533748640028197d;
            brainCells[65] = 1d;
            brainCells[66] = 0d;
            brainCells[67] = 0d;
            brainCells[68] = 0.331297751572574d;
            brainCells[69] = 0.668702248427426d;
            brainCells[70] = 0.80312014383779d;
            brainCells[71] = 0.19687985616221d;
            brainCells[72] = 0.48095959078158d;
            brainCells[73] = 0.51904040921842d;
            brainCells[74] = 0.332223695609203d;
            brainCells[75] = 0.62496730441166d;
            brainCells[76] = 0d;
            brainCells[77] = 0.0428089999791365d;
            brainCells[78] = 0.981821611897762d;
            brainCells[79] = 0d;
            brainCells[80] = 0.0181783881022382d;
            brainCells[81] = 0.512052978792514d;
            brainCells[82] = 0.487947021207486d;
            brainCells[83] = 0.968342393803771d;
            brainCells[84] = 0.0316576061962289d;
            brainCells[85] = 0.858767220816752d;
            brainCells[86] = 0d;
            brainCells[87] = 0.141232779183248d;
            brainCells[88] = 0.755784106502757d;
            brainCells[89] = 0.244215893497243d;
            brainCells[90] = 1d;
            brainCells[91] = 5.55111512E-17d;
            brainCells[92] = 0d;
            brainCells[93] = 0.999999999999112d;
            brainCells[94] = 8.875955526122E-13d;
            brainCells[95] = 0d;
            brainCells[96] = 0d;
            brainCells[97] = 0d;
            brainCells[98] = 1d;
            brainCells[99] = 0d;
            brainCells[100] = 0d;
            brainCells[101] = 0d;
            brainCells[102] = 0.623264561258159d;
            brainCells[103] = 0.376735438741841d;
            brainCells[104] = 0d;
            brainCells[105] = 0d;
            brainCells[106] = 0.709043200617201d;
            brainCells[107] = 0.290956799382799d;
            brainCells[108] = 0d;
            brainCells[109] = 0.443387587974251d;
            brainCells[110] = 0.556612412025749d;
            brainCells[111] = 1d;
            brainCells[112] = 0d;
            brainCells[113] = 0.737074065901941d;
            brainCells[114] = 0.262925934098059d;
            brainCells[115] = 0d;
            brainCells[116] = 0.611480135644594d;
            brainCells[117] = 0.388519864355406d;
            brainCells[118] = 0.749714759362937d;
            brainCells[119] = 0.250285240637063d;
            brainCells[120] = 0.713611962128144d;
            brainCells[121] = 0.286388037871856d;
            brainCells[122] = 0d;
            brainCells[123] = 0d;
            brainCells[124] = 0d;
            brainCells[125] = 0.328795912828667d;
            brainCells[126] = 0.671204087171333d;
            brainCells[127] = 0.378751265744309d;
            brainCells[128] = 0.621248734255691d;
            brainCells[129] = 0.161540034126314d;
            brainCells[130] = 0.838459965873686d;
            brainCells[131] = 0.692617111238144d;
            brainCells[132] = 0d;
            brainCells[133] = 0.307382888761856d;
            brainCells[134] = 0d;
            brainCells[135] = 1d;
            brainCells[136] = 0.771345695481772d;
            brainCells[137] = 0.228654304518228d;
            brainCells[138] = 0d;
            brainCells[139] = 1d;
            brainCells[140] = 0d;
            brainCells[141] = 0d;
            brainCells[142] = 0d;
            brainCells[143] = 0d;
            brainCells[144] = 0.980604540089189d;
            brainCells[145] = 0.019395459910811d;
            brainCells[146] = 0d;
            brainCells[147] = 0d;
            brainCells[148] = 0.0743766536854795d;
            brainCells[149] = 0.925623346312702d;
            brainCells[150] = 1.8189894035459E-12d;
            brainCells[151] = 0d;
            brainCells[152] = 0.7906921713011d;
            brainCells[153] = 0.2093078286989d;
            brainCells[154] = 0d;
            brainCells[155] = 1d;
            brainCells[156] = 0d;
            brainCells[157] = 0.752784528654434d;
            brainCells[158] = 0.247215471345566d;
            brainCells[159] = 0.559378169449535d;
            brainCells[160] = 0.440621830550465d;
            brainCells[161] = 0d;
            brainCells[162] = 0.735975635803601d;
            brainCells[163] = 0.264024364196399d;
            brainCells[164] = 0.898986398428689d;
            brainCells[165] = 0.101013601571311d;
            brainCells[166] = 0.07671158007316d;
            brainCells[167] = 0.773639771840454d;
            brainCells[168] = 0d;
            brainCells[169] = 0.149648648086386d;
            brainCells[170] = 0.202988201062854d;
            brainCells[171] = 0.478168203531632d;
            brainCells[172] = 0.318843595405514d;
            brainCells[173] = 1.110223025E-16d;
            brainCells[174] = 1d;
            brainCells[175] = 0.559064971989468d;
            brainCells[176] = 0.440935028010532d;
            brainCells[177] = 0.431939671116952d;
            brainCells[178] = 0.296478917494639d;
            brainCells[179] = 0.271581411388409d;
            brainCells[180] = 1d;
            brainCells[181] = 5.55111512E-17d;
            brainCells[182] = 0.400379537397757d;
            brainCells[183] = 0.599620462602243d;
            brainCells[184] = 0d;
            brainCells[185] = 0.614062139963873d;
            brainCells[186] = 1.8189894035459E-12d;
            brainCells[187] = 0d;
            brainCells[188] = 0.385937860034308d;
            brainCells[189] = 0d;
            brainCells[190] = 0.197254043742381d;
            brainCells[191] = 0.388020527124073d;
            brainCells[192] = 0d;
            brainCells[193] = 0.414725429133546d;
            brainCells[194] = 0d;
            brainCells[195] = 0.211077829853201d;
            brainCells[196] = 0d;
            brainCells[197] = 0.788922170146799d;
            brainCells[198] = 0d;
            brainCells[199] = 0.240588016102928d;
            brainCells[200] = 0.759411983897072d;
            brainCells[201] = 0.351251845896292d;
            brainCells[202] = 0.648748154103708d;
            brainCells[203] = 0d;
            brainCells[204] = 1d;
            brainCells[205] = 0d;
            brainCells[206] = 0.654795423794835d;
            brainCells[207] = 0.345204576205165d;
            brainCells[208] = 0.940745636622709d;
            brainCells[209] = 0.0592543633772913d;
            brainCells[210] = 5.55111512E-17d;
            brainCells[211] = 1d;
            brainCells[212] = 0d;
            brainCells[213] = 0.237462225125037d;
            brainCells[214] = 0.0441047998600062d;
            brainCells[215] = 0.718432975014957d;
            brainCells[216] = 0.325587705609115d;
            brainCells[217] = 0.458637100592104d;
            brainCells[218] = 0.215775193798782d;
            brainCells[219] = 1.110223025E-16d;
            brainCells[220] = 1d;
            brainCells[221] = 0.326104530737625d;
            brainCells[222] = 0.673895469262375d;
            brainCells[223] = 0d;
            brainCells[224] = 0.531606676604562d;
            brainCells[225] = 0.468393323395438d;
            brainCells[226] = 0.586914820867276d;
            brainCells[227] = 0.413085179132724d;
            brainCells[228] = 0.40782356132095d;
            brainCells[229] = 0.59217643867905d;
            brainCells[230] = 0d;
            brainCells[231] = 0.195555003330716d;
            brainCells[232] = 0d;
            brainCells[233] = 0d;
            brainCells[234] = 0.804444996669284d;
            brainCells[235] = 0d;
            brainCells[236] = 1.110223025E-16d;
            brainCells[237] = 0d;
            brainCells[238] = 0d;
            brainCells[239] = 1d;
            brainCells[240] = 0d;
            brainCells[241] = 0.510748445167204d;
            brainCells[242] = 0d;
            brainCells[243] = 0.489251554832796d;
            brainCells[244] = 0.352191260503999d;
            brainCells[245] = 0.198971007928351d;
            brainCells[246] = 0.44883773156765d;
            brainCells[247] = 2.77555756E-17d;
            brainCells[248] = 1d;
            brainCells[249] = 0.617600727524996d;
            brainCells[250] = 0.382399272475004d;
            brainCells[251] = 0d;
            brainCells[252] = 0.210286111908849d;
            brainCells[253] = 0.789713888091151d;
            brainCells[254] = 1d;
            brainCells[255] = 8.32667268E-17d;
            brainCells[256] = 0.188391680337843d;
            brainCells[257] = 0.811608319662157d;
            brainCells[258] = 0d;
            brainCells[259] = 0d;
            brainCells[260] = 0.0237467236534036d;
            brainCells[261] = 0.976253276346596d;
            brainCells[262] = 0.347800205996091d;
            brainCells[263] = 0.24315314173944d;
            brainCells[264] = 0.409046652264469d;
            brainCells[265] = 0d;
            brainCells[266] = 1d;
            brainCells[267] = 0.26358922228842d;
            brainCells[268] = 0.73641077771158d;
            brainCells[269] = 0d;
            brainCells[270] = 0.0662744965987327d;
            brainCells[271] = 0.933725503401267d;
            brainCells[272] = 0.541009424422346d;
            brainCells[273] = 0.458990575577654d;
            brainCells[274] = 0d;
            brainCells[275] = 1d;
            */






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

