using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Probability
{
    class Rules
    {
        Logger logger;

        public int blind = 1;
        public int playerCoins = 3;
<<<<<<< HEAD
        public int diceCombinations = 4;
=======
        public int diceCombinations = 6;
>>>>>>> origin/Branch-B2
        public Random random;
        public int situationBrainCellsCount;
        public int allBrainCellsCount;
        public Scenario[] locationsBC;


        public Rules(Logger logger)
        {
            logger.set("Rules", 1, Color.Brown);
            this.logger = logger;
            generateScenarios();
            generatePossibleMoves();
            generateBrainCellsCount();


            //debug

            foreach (Scenario scenario in scenarios)
            {
                logger.log(scenario.toString(), 8, "Rules");
            }


            //Random generator
            random = new Random();


        }

        //Scenarios
        public List<Scenario> scenarios = new List<Scenario>();
        void generateScenarios()
        {
            List<int> path = new List<int>();
            Scenario scenario = new Scenario(path, false, this, logger);
            scenarios.Add(scenario);
            iterate(path);
        }

        void generatePossibleMoves()
        {
            foreach (Scenario scenario in scenarios)
            {
                List<int> possibleMoves = new List<int>();
                if (!scenario.gameOver)
                {//Not game over
                    foreach (Scenario scenario2 in scenarios)
                    {
                        if (scenario.path.Count == (scenario2.path.Count - 1))
                        {//scenario2 is 1 step longer
                            if (scenario.pathMatchCount(scenario2.path) == scenario.path.Count)
                            {//path match
                                possibleMoves.Add(scenario2.path[scenario2.path.Count - 1]);
                            }
                        }
                    }
                }
                scenario.updatePossibleMoves(possibleMoves);
            }
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

                    if (action < p2Bet - p1Bet)
                    {//trying to bet less than required
                        moveAllowed = false;
                    }
                    if (action == p2Bet - p1Bet)
                    {//Equalised
                        if (path.Count > 0)
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

                    Scenario scenario = new Scenario(pathNew, gameOver, this, logger);
                    scenarios.Add(scenario);

                    if (!gameOver)
                    {
                        iterate(pathNew);
                    }
                }
            }
        }

        public Scenario findScenarioByPath(List<int> path)
        {
            Scenario retVal = null;
            foreach (Scenario scenario in scenarios)
            {
                if (scenario.path.Count == path.Count)
                {
                    if (scenario.pathMatchCount(path) == scenario.path.Count)
                    {
                        retVal = scenario;
                        break;
                    }
                }
            }
            if (retVal == null)
            {
                logger.log("Scenario by path not found.", 1, "Error");
            }
            return retVal;
        }

        public Scenario findScenarioByBCLocation(int location)
        {
            return locationsBC[location];
        }

        //Braincells structure
        void generateBrainCellsCount()
        {
            situationBrainCellsCount = 0;
            foreach (Scenario scenario in scenarios)
            {
                scenario.brainCellsLocation = situationBrainCellsCount;
                situationBrainCellsCount += scenario.possibleMoves.Count;
            }
            allBrainCellsCount = situationBrainCellsCount * diceCombinations;
            locationsBC = new Scenario[situationBrainCellsCount];
            int locationStart = 0;
            foreach (Scenario scenario in scenarios)
            {
                for(int i=locationStart; i<locationStart+scenario.possibleMoves.Count;i++)
                {
                    locationsBC[i] = scenario;
                }
                locationStart += scenario.possibleMoves.Count;
            }


        }

        public string intListToString(List<int> path)
        {
            string s = "";
            foreach (int ii in path)
            {
                s += (ii.ToString() + "; ");
            }
            return s;
        }

        public string doubleListToString(List<double> path, int precission = 4)
        {
            string s = "";
            foreach (double dd in path)
            {
                s += (dd.ToString("F" + precission.ToString()) + "; ");
            }
            return s;
        }





    }
}
