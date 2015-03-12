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

            brainCells[0] = 0.462099554929786d;
            brainCells[1] = 0d;
            brainCells[2] = 0d;
            brainCells[3] = 0.13585022264617d;
            brainCells[4] = 0.402050222424044d;
            brainCells[5] = 0d;
            brainCells[6] = 0.0350375671056099d;
            brainCells[7] = 0.312668494564808d;
            brainCells[8] = 0d;
            brainCells[9] = 0.652293938329582d;
            brainCells[10] = 0.155825605118092d;
            brainCells[11] = 0d;
            brainCells[12] = 0.521850000933507d;
            brainCells[13] = 0.322324393948401d;
            brainCells[14] = 0.629568279215246d;
            brainCells[15] = 0d;
            brainCells[16] = 0.370431720784754d;
            brainCells[17] = 0.397175960215378d;
            brainCells[18] = 0.602824039784622d;
            brainCells[19] = 1d;
            brainCells[20] = 8.32667268E-17d;
            brainCells[21] = 0.281506352685939d;
            brainCells[22] = 0.429990828222562d;
            brainCells[23] = 0.2885028190915d;
            brainCells[24] = 0.430544282080873d;
            brainCells[25] = 0.569455717919127d;
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
            brainCells[37] = 0.565218227304456d;
            brainCells[38] = 0.434781772695544d;
            brainCells[39] = 0.728684292522161d;
            brainCells[40] = 0d;
            brainCells[41] = 0.271315707477839d;
            brainCells[42] = 0.704754092179035d;
            brainCells[43] = 0.295245907820965d;
            brainCells[44] = 0.828698552799935d;
            brainCells[45] = 0.171301447200065d;
            brainCells[46] = 0.68423829520043d;
            brainCells[47] = 0.23168811170317d;
            brainCells[48] = 0d;
            brainCells[49] = 0.0840735930964002d;
            brainCells[50] = 0d;
            brainCells[51] = 9.71445147E-17d;
            brainCells[52] = 1d;
            brainCells[53] = 0d;
            brainCells[54] = 0d;
            brainCells[55] = 0d;
            brainCells[56] = 0.358900992896479d;
            brainCells[57] = 0.424466478206261d;
            brainCells[58] = 0d;
            brainCells[59] = 0.21663252889726d;
            brainCells[60] = 0.262253819758129d;
            brainCells[61] = 0.491679242976933d;
            brainCells[62] = 0.246066937264938d;
            brainCells[63] = 0.92791036110149d;
            brainCells[64] = 0.0720896388985098d;
            brainCells[65] = 0.838587290077817d;
            brainCells[66] = 0.161412709922183d;
            brainCells[67] = 0.38146959446307d;
            brainCells[68] = 0.61853040553693d;
            brainCells[69] = 0d;
            brainCells[70] = 0.870501634956282d;
            brainCells[71] = 0.129498365043718d;
            brainCells[72] = 1d;
            brainCells[73] = 5.55111512E-17d;
            brainCells[74] = 0.557576411286169d;
            brainCells[75] = 0.110501093093625d;
            brainCells[76] = 0d;
            brainCells[77] = 0.331922495620206d;
            brainCells[78] = 1d;
            brainCells[79] = 0d;
            brainCells[80] = 0d;
            brainCells[81] = 0.166657910549608d;
            brainCells[82] = 0.833342089450392d;
            brainCells[83] = 0.401208012427291d;
            brainCells[84] = 0.598791987572709d;
            brainCells[85] = 0.988554911029036d;
            brainCells[86] = 0d;
            brainCells[87] = 0.0114450889709636d;
            brainCells[88] = 0.774991850263901d;
            brainCells[89] = 0.225008149736099d;
            brainCells[90] = 1d;
            brainCells[91] = 0d;
            brainCells[92] = 0d;
            brainCells[93] = 0.973695663987019d;
            brainCells[94] = 0.00786119227559539d;
            brainCells[95] = 0.0184431437286548d;
            brainCells[96] = 8.7311491370201E-12d;
            brainCells[97] = 0d;
            brainCells[98] = 0.959428839680913d;
            brainCells[99] = 0.0405711603190867d;
            brainCells[100] = 0d;
            brainCells[101] = 0d;
            brainCells[102] = 0.452748529775524d;
            brainCells[103] = 0.522974313682537d;
            brainCells[104] = 0d;
            brainCells[105] = 0.0242771565419389d;
            brainCells[106] = 0.209086138075028d;
            brainCells[107] = 0.790913861924972d;
            brainCells[108] = 0d;
            brainCells[109] = 0.0988609597232965d;
            brainCells[110] = 0.901139040276704d;
            brainCells[111] = 0.414310921586436d;
            brainCells[112] = 0.585689078413564d;
            brainCells[113] = 0.764994662774292d;
            brainCells[114] = 0.235005337225708d;
            brainCells[115] = 0d;
            brainCells[116] = 0.820808415613174d;
            brainCells[117] = 0.179191584386826d;
            brainCells[118] = 0.842228030021495d;
            brainCells[119] = 0.157771969978505d;
            brainCells[120] = 0.248099546550612d;
            brainCells[121] = 0.751900453449389d;
            brainCells[122] = 0d;
            brainCells[123] = 0d;
            brainCells[124] = 0.316968221168552d;
            brainCells[125] = 0.232581987889827d;
            brainCells[126] = 0.450449790941621d;
            brainCells[127] = 0.461993371094689d;
            brainCells[128] = 0.538006628905311d;
            brainCells[129] = 0.50987464547722d;
            brainCells[130] = 0.49012535452278d;
            brainCells[131] = 0.412708413143269d;
            brainCells[132] = 0.587291586856731d;
            brainCells[133] = 0d;
            brainCells[134] = 0.788122083075728d;
            brainCells[135] = 0.211877916924272d;
            brainCells[136] = 0.487088993041675d;
            brainCells[137] = 0.512911006958324d;
            brainCells[138] = 0d;
            brainCells[139] = 0.792281653729138d;
            brainCells[140] = 0.00015525297349086d;
            brainCells[141] = 0.207563093297371d;
            brainCells[142] = 0d;
            brainCells[143] = 0d;
            brainCells[144] = 0.977174036178635d;
            brainCells[145] = 0.0226843622612044d;
            brainCells[146] = 0d;
            brainCells[147] = 0.000141601560161051d;
            brainCells[148] = 0.493431928349732d;
            brainCells[149] = 0.372408821582089d;
            brainCells[150] = 0d;
            brainCells[151] = 0.134159250068178d;
            brainCells[152] = 0.313331615105058d;
            brainCells[153] = 0.411096387557207d;
            brainCells[154] = 0.275571997337735d;
            brainCells[155] = 0.595890834926822d;
            brainCells[156] = 0.404109165073179d;
            brainCells[157] = 0.842463037384d;
            brainCells[158] = 0.157536962616d;
            brainCells[159] = 0.618936246559499d;
            brainCells[160] = 0.252330775616235d;
            brainCells[161] = 0.128732977824266d;
            brainCells[162] = 0.42857142857194d;
            brainCells[163] = 0.57142857142806d;
            brainCells[164] = 0.757657233623632d;
            brainCells[165] = 0.242342766376368d;
            brainCells[166] = 0.166349458022253d;
            brainCells[167] = 0.775614308167875d;
            brainCells[168] = 0d;
            brainCells[169] = 0.0580362338098717d;
            brainCells[170] = 0.051818637262689d;
            brainCells[171] = 0.0598792392028411d;
            brainCells[172] = 0.88830212353447d;
            brainCells[173] = 0.256574960791698d;
            brainCells[174] = 0.743425039208302d;
            brainCells[175] = 0d;
            brainCells[176] = 1d;
            brainCells[177] = 0.683193210846832d;
            brainCells[178] = 0.316806789153168d;
            brainCells[179] = 0d;
            brainCells[180] = 0.607519090064717d;
            brainCells[181] = 0.392480909935283d;
            brainCells[182] = 1d;
            brainCells[183] = 0d;
            brainCells[184] = 0d;
            brainCells[185] = 0.749400830348592d;
            brainCells[186] = 0d;
            brainCells[187] = 0d;
            brainCells[188] = 0.250599169651408d;
            brainCells[189] = 0d;
            brainCells[190] = 0.0459090446187401d;
            brainCells[191] = 0.867075993058832d;
            brainCells[192] = 0d;
            brainCells[193] = 0.0870149623224279d;
            brainCells[194] = 0.00946456566893705d;
            brainCells[195] = 0.400280824883866d;
            brainCells[196] = 0d;
            brainCells[197] = 0.590254609447197d;
            brainCells[198] = 0d;
            brainCells[199] = 0.6604488760287d;
            brainCells[200] = 0.3395511239713d;
            brainCells[201] = 0.523204934011959d;
            brainCells[202] = 0.476795065988041d;
            brainCells[203] = 0.104115139730527d;
            brainCells[204] = 0.895884860269473d;
            brainCells[205] = 0d;
            brainCells[206] = 0.663992919364597d;
            brainCells[207] = 0.336007080635403d;
            brainCells[208] = 0.619772521686925d;
            brainCells[209] = 0.380227478313075d;
            brainCells[210] = 0d;
            brainCells[211] = 1d;
            brainCells[212] = 0d;
            brainCells[213] = 0.417384097430162d;
            brainCells[214] = 0.150122707727603d;
            brainCells[215] = 0.432493194842235d;
            brainCells[216] = 0d;
            brainCells[217] = 0.748233147228833d;
            brainCells[218] = 0.251766852771167d;
            brainCells[219] = 0d;
            brainCells[220] = 1d;
            brainCells[221] = 0.495553226723532d;
            brainCells[222] = 0.504446773276468d;
            brainCells[223] = 0.195618564234358d;
            brainCells[224] = 0d;
            brainCells[225] = 0.804381435765642d;
            brainCells[226] = 0.59240458446504d;
            brainCells[227] = 0.40759541553496d;
            brainCells[228] = 1.110223025E-16d;
            brainCells[229] = 1d;
            brainCells[230] = 0d;
            brainCells[231] = 2.9103830456734E-12d;
            brainCells[232] = 0d;
            brainCells[233] = 0.580515465592031d;
            brainCells[234] = 0.419484534405059d;
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
            brainCells[256] = 0.287338754026164d;
            brainCells[257] = 0.712661245973836d;
            brainCells[258] = 0d;
            brainCells[259] = 0d;
            brainCells[260] = 0d;
            brainCells[261] = 1d;
            brainCells[262] = 0.422761010243385d;
            brainCells[263] = 0.383490163070993d;
            brainCells[264] = 0.193748826685622d;
            brainCells[265] = 0.122046528651589d;
            brainCells[266] = 0.877953471348412d;
            brainCells[267] = 0.39581749592261d;
            brainCells[268] = 0.60418250407739d;
            brainCells[269] = 5.55111512E-17d;
            brainCells[270] = 0d;
            brainCells[271] = 1d;
            brainCells[272] = 0d;
            brainCells[273] = 1d;
            brainCells[274] = 6.9388939E-17d;
            brainCells[275] = 1d;







        }

    }
}

