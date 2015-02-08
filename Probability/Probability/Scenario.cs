using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Probability
{
    class Scenario
    {
        public List<int> path = new List<int>();
        Rules rules;
        Logger logger;
        
        //Possible moves
        public List<int> possibleMoves = new List<int>();
        //remaining coins
        //search key
        //action to index
        //is last move

        public Scenario(List<int> path, Rules rules, Logger logger)
        {
            this.rules = rules;
            this.logger = logger;
            string s = "Path = ";
            foreach(int ii in path)
            {
                this.path.Add(ii);
                s += (ii + " ");
            }
            logger.log(s);
            updatePossibleMoves();

        }
        void updatePossibleMoves()
        {

        }

    }
}
