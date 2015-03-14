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

        private void copyPlayerNoNew(Player retVal)
        {
            for (int i = 0; i < allBrainCellsCount; i++)
            {
                retVal.brainCells[i] = this.brainCells[i];
            }
            retVal.coins = this.coins;
            retVal.dice = this.dice;
            retVal.allBrainCellsCount = this.allBrainCellsCount;
            retVal.strength = this.strength;
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

            /*

            //Result : -0,0216486049180794
            //public int blind = 1;
            //public int playerCoins = 4;
            //public int diceCombinations = 6; 
             
            brainCells[0] = 0.485025902565589d;
            brainCells[1] = 0d;
            brainCells[2] = 0d;
            brainCells[3] = 0.263283376790262d;
            brainCells[4] = 0.25169072064415d;
            brainCells[5] = 0d;
            brainCells[6] = 0.0511192765610758d;
            brainCells[7] = 0.296586785109342d;
            brainCells[8] = 0d;
            brainCells[9] = 0.652293938329582d;
            brainCells[10] = 0.0161610082798456d;
            brainCells[11] = 0d;
            brainCells[12] = 0.661514597771753d;
            brainCells[13] = 0.322324393948401d;
            brainCells[14] = 0.629568279215246d;
            brainCells[15] = 0d;
            brainCells[16] = 0.370431720784754d;
            brainCells[17] = 0.397175960215378d;
            brainCells[18] = 0.602824039784622d;
            brainCells[19] = 1d;
            brainCells[20] = 0d;
            brainCells[21] = 0.281506352685939d;
            brainCells[22] = 0.429990828222562d;
            brainCells[23] = 0.2885028190915d;
            brainCells[24] = 0.98757937600166d;
            brainCells[25] = 0.0124206239983395d;
            brainCells[26] = 0.583672520111091d;
            brainCells[27] = 0.416327479888909d;
            brainCells[28] = 1d;
            brainCells[29] = 0d;
            brainCells[30] = 0d;
            brainCells[31] = 0d;
            brainCells[32] = 0d;
            brainCells[33] = 0d;
            brainCells[34] = 1d;
            brainCells[35] = 0.407414645271981d;
            brainCells[36] = 0.592585354728019d;
            brainCells[37] = 0.144675824450014d;
            brainCells[38] = 0.855324175549986d;
            brainCells[39] = 0.921340490212101d;
            brainCells[40] = 0.0786595097878996d;
            brainCells[41] = 0d;
            brainCells[42] = 1d;
            brainCells[43] = 0d;
            brainCells[44] = 0.843897015137074d;
            brainCells[45] = 0.156102984862926d;
            brainCells[46] = 0.0179364345908395d;
            brainCells[47] = 0.956305779940274d;
            brainCells[48] = 2.7755576E-18d;
            brainCells[49] = 0.0257577854688861d;
            brainCells[50] = 0d;
            brainCells[51] = 9.71445147E-17d;
            brainCells[52] = 1d;
            brainCells[53] = 0d;
            brainCells[54] = 0d;
            brainCells[55] = 0d;
            brainCells[56] = 0.547202033868132d;
            brainCells[57] = 0.452797966131868d;
            brainCells[58] = 0d;
            brainCells[59] = 0d;
            brainCells[60] = 0.262253819758129d;
            brainCells[61] = 0.491679242976933d;
            brainCells[62] = 0.246066937264938d;
            brainCells[63] = 0.92791036110149d;
            brainCells[64] = 0.0720896388985098d;
            brainCells[65] = 0.838587290077817d;
            brainCells[66] = 0.161412709922183d;
            brainCells[67] = 0.703097941654973d;
            brainCells[68] = 0.268274596112249d;
            brainCells[69] = 0.0286274622327776d;
            brainCells[70] = 0.870501634956282d;
            brainCells[71] = 0.129498365043718d;
            brainCells[72] = 1d;
            brainCells[73] = 5.55111512E-17d;
            brainCells[74] = 0.602590754280715d;
            brainCells[75] = 0.125315136519929d;
            brainCells[76] = 0.0199144082129351d;
            brainCells[77] = 0.252179700986421d;
            brainCells[78] = 1d;
            brainCells[79] = 0d;
            brainCells[80] = 0d;
            brainCells[81] = 1d;
            brainCells[82] = 0d;
            brainCells[83] = 0.836424251817821d;
            brainCells[84] = 0.163575748182179d;
            brainCells[85] = 0.936543072597392d;
            brainCells[86] = 0.0225149954040765d;
            brainCells[87] = 0.0409419319985318d;
            brainCells[88] = 1d;
            brainCells[89] = 5.55111512E-17d;
            brainCells[90] = 1d;
            brainCells[91] = 0d;
            brainCells[92] = 0d;
            brainCells[93] = 0.998783429463219d;
            brainCells[94] = 0d;
            brainCells[95] = 0.00121657053678059d;
            brainCells[96] = 2.22044605E-17d;
            brainCells[97] = 0d;
            brainCells[98] = 1d;
            brainCells[99] = 4.85722573E-17d;
            brainCells[100] = 0d;
            brainCells[101] = 0d;
            brainCells[102] = 0.497842108137663d;
            brainCells[103] = 0.473933046644854d;
            brainCells[104] = 0d;
            brainCells[105] = 0.0282248452174826d;
            brainCells[106] = 2.77555756E-17d;
            brainCells[107] = 1d;
            brainCells[108] = 0d;
            brainCells[109] = 0.0988609597232965d;
            brainCells[110] = 0.901139040276704d;
            brainCells[111] = 1d;
            brainCells[112] = 0d;
            brainCells[113] = 0.791965464607087d;
            brainCells[114] = 0.208034535392913d;
            brainCells[115] = 0d;
            brainCells[116] = 0.346436499243466d;
            brainCells[117] = 0.653563500756534d;
            brainCells[118] = 0.798429566587863d;
            brainCells[119] = 0.201570433412137d;
            brainCells[120] = 0.248099546550612d;
            brainCells[121] = 0.751203512465917d;
            brainCells[122] = 0d;
            brainCells[123] = 0.000696940983470951d;
            brainCells[124] = 0d;
            brainCells[125] = 0.0110850052867274d;
            brainCells[126] = 0.988914994713273d;
            brainCells[127] = 0.461993371094689d;
            brainCells[128] = 0.538006628905311d;
            brainCells[129] = 0.50987464547722d;
            brainCells[130] = 0.49012535452278d;
            brainCells[131] = 0.412708413143269d;
            brainCells[132] = 0.587291586856731d;
            brainCells[133] = 0d;
            brainCells[134] = 0.999999999999277d;
            brainCells[135] = 7.225210013617E-13d;
            brainCells[136] = 0.500954182229021d;
            brainCells[137] = 0.499045817770979d;
            brainCells[138] = 8.32667268E-17d;
            brainCells[139] = 1d;
            brainCells[140] = 0d;
            brainCells[141] = 0d;
            brainCells[142] = 0d;
            brainCells[143] = 0d;
            brainCells[144] = 0.977315637732681d;
            brainCells[145] = 0.0226843622673187d;
            brainCells[146] = 0d;
            brainCells[147] = 0d;
            brainCells[148] = 0.178201737287621d;
            brainCells[149] = 0.821798262712379d;
            brainCells[150] = 0d;
            brainCells[151] = 0d;
            brainCells[152] = 0.579595034059978d;
            brainCells[153] = 0.349865852375395d;
            brainCells[154] = 0.0705391135646275d;
            brainCells[155] = 0.595890834926821d;
            brainCells[156] = 0.404109165073179d;
            brainCells[157] = 1d;
            brainCells[158] = 0d;
            brainCells[159] = 0.560828031307212d;
            brainCells[160] = 0.439171968692788d;
            brainCells[161] = 0d;
            brainCells[162] = 0.42857142857194d;
            brainCells[163] = 0.57142857142806d;
            brainCells[164] = 0.656619152864169d;
            brainCells[165] = 0.343380847135831d;
            brainCells[166] = 0.166349458022253d;
            brainCells[167] = 0.783864521444063d;
            brainCells[168] = 1.4551901350579E-12d;
            brainCells[169] = 0.0497860205322292d;
            brainCells[170] = 0d;
            brainCells[171] = 0.0598792392028411d;
            brainCells[172] = 0.940120760797159d;
            brainCells[173] = 0.256574960791698d;
            brainCells[174] = 0.743425039208302d;
            brainCells[175] = 0d;
            brainCells[176] = 1d;
            brainCells[177] = 0.683193210846832d;
            brainCells[178] = 0d;
            brainCells[179] = 0.316806789153168d;
            brainCells[180] = 0.607519090064717d;
            brainCells[181] = 0.392480909935283d;
            brainCells[182] = 1d;
            brainCells[183] = 0d;
            brainCells[184] = 0d;
            brainCells[185] = 1d;
            brainCells[186] = 0d;
            brainCells[187] = 0d;
            brainCells[188] = 0d;
            brainCells[189] = 0d;
            brainCells[190] = 0.0457674430581392d;
            brainCells[191] = 0.867075993058832d;
            brainCells[192] = 0d;
            brainCells[193] = 0.0871565638830289d;
            brainCells[194] = 0d;
            brainCells[195] = 0.400280824883866d;
            brainCells[196] = 0d;
            brainCells[197] = 0.599719175116134d;
            brainCells[198] = 0d;
            brainCells[199] = 0.6604488760287d;
            brainCells[200] = 0.3395511239713d;
            brainCells[201] = 0.523204934011959d;
            brainCells[202] = 0.476795065988041d;
            brainCells[203] = 0.19460015019398d;
            brainCells[204] = 0.80539984980602d;
            brainCells[205] = 0d;
            brainCells[206] = 0.663992919364597d;
            brainCells[207] = 0.336007080635403d;
            brainCells[208] = 0.619772521686925d;
            brainCells[209] = 0.380227478313075d;
            brainCells[210] = 0d;
            brainCells[211] = 1d;
            brainCells[212] = 0d;
            brainCells[213] = 0.417384097430162d;
            brainCells[214] = 0.0995720410663448d;
            brainCells[215] = 0.483043861503493d;
            brainCells[216] = 0d;
            brainCells[217] = 0.748233147228833d;
            brainCells[218] = 0.251766852771167d;
            brainCells[219] = 0d;
            brainCells[220] = 1d;
            brainCells[221] = 0.495553226723532d;
            brainCells[222] = 0.504446773276468d;
            brainCells[223] = 2.77555756E-17d;
            brainCells[224] = 0.76298610855033d;
            brainCells[225] = 0.23701389144967d;
            brainCells[226] = 0.999999999998876d;
            brainCells[227] = 1.1238995845098E-12d;
            brainCells[228] = 0.101275220560504d;
            brainCells[229] = 0.898724779439496d;
            brainCells[230] = 0d;
            brainCells[231] = 1.4551693183762E-12d;
            brainCells[232] = 0d;
            brainCells[233] = 0.580515465592031d;
            brainCells[234] = 0.419484534406514d;
            brainCells[235] = 0d;
            brainCells[236] = 0d;
            brainCells[237] = 0d;
            brainCells[238] = 0d;
            brainCells[239] = 1d;
            brainCells[240] = 0d;
            brainCells[241] = 0.294854588012609d;
            brainCells[242] = 0d;
            brainCells[243] = 0.705145411987391d;
            brainCells[244] = 0d;
            brainCells[245] = 0.862615061699892d;
            brainCells[246] = 0.137384938300108d;
            brainCells[247] = 0.542656835373023d;
            brainCells[248] = 0.457343164626977d;
            brainCells[249] = 0.384937243573685d;
            brainCells[250] = 0.615062756426315d;
            brainCells[251] = 1.110223025E-16d;
            brainCells[252] = 0d;
            brainCells[253] = 1d;
            brainCells[254] = 0.639272783284412d;
            brainCells[255] = 0.360727216715588d;
            brainCells[256] = 0.937303757461648d;
            brainCells[257] = 0.0626962425383524d;
            brainCells[258] = 0d;
            brainCells[259] = 0d;
            brainCells[260] = 1.5396017793989E-12d;
            brainCells[261] = 0.99999999999846d;
            brainCells[262] = 0.422761010243385d;
            brainCells[263] = 0.383490163070993d;
            brainCells[264] = 0.193748826685622d;
            brainCells[265] = 6.9388939E-17d;
            brainCells[266] = 1d;
            brainCells[267] = 0.39581749592261d;
            brainCells[268] = 0.60418250407739d;
            brainCells[269] = 5.55111512E-17d;
            brainCells[270] = 0d;
            brainCells[271] = 1d;
            brainCells[272] = 0d;
            brainCells[273] = 1d;
            brainCells[274] = 6.9388939E-17d;
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

