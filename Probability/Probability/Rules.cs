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

            for (int i=-1; i<=playerCoins; i++)
            {
                List<int> path = new List<int>();
                path.Add(-99);
                path.Add(i);
                Scenario scenario=new Scenario(path, this, logger);
                scenarios.Add(scenario);
            }

        }

        //Braincells structure

        //Random generator


        

    }
}
