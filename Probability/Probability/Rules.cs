using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Probability
{
    class Rules
    {
        Logger logger;

        public const int blind = 1;
        public const int playerCoins = 3;
        public const int diceCombinations = 6;


        public Rules(Logger logger)
        {
            this.logger = logger;
            generateScenarios();
        }

        //Scenarios
        public List<Scenario> scenarios = new List<Scenario>();
        void generateScenarios()
        {
            List<int> path = new List<int>();
            iterate(path);

        }

        void iterate(List<int> path)
        {
            for (int action = -1; action <= playerCoins - blind; action++)
            {
                bool gameOver = false;
                if (action == -1)
                {
                    gameOver = true;
                }
                bool moveAllowed = true;
                if (action == -1)
                {//Fold
                    gameOver = true;
                }
                else
                {//Check if sufficient ammount and does not exceed playerCoins
                    int p1Bet = 0;
                    int p2Bet = 0;

                    foreach (int iBet in path)
                    {
                        p1Bet += iBet;

                        int pSwap = p1Bet;
                        p1Bet = p2Bet;
                        p2Bet = pSwap;

                    }
                    int remainingCoins = playerCoins - blind - p1Bet;
                    if (action > remainingCoins)
                    {//trying to bet more than have
                        moveAllowed = false;
                    }

                    if (action < p2Bet-p1Bet)
                    {//trying to bet less than required
                        moveAllowed = false;
                    }
                    if (action == p2Bet - p1Bet)
                    {//Equalised
                        if(path.Count > 0)
                        {//Not first move
                            gameOver = true;
                        }
                    }
                }
                if (moveAllowed)
                {
                    List<int> pathNew = new List<int>();
                    foreach (int ii in path)
                    {
                        pathNew.Add(ii);
                    }
                    pathNew.Add(action);

                    if (gameOver)
                    {
                        Scenario scenario = new Scenario(pathNew, this, logger);
                        scenarios.Add(scenario);
                    }
                    else
                    {
                        iterate(pathNew);
                    }
                }
            }
        }


        //Braincells structure

        //Random generator




    }
}
