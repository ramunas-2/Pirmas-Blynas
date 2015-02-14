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
        public List<int> posssibleMoves = new List<int>();
        public int brainCellsLocation;
        Rules rules;
        Logger logger;

        //Possible moves
        public List<int> possibleMoves;
        public bool gameOver;
        //remaining coins
        //search key
        //action to index
        //is last move

        public Scenario(List<int> path, bool gameOver, Rules rules, Logger logger)
        {
            this.rules = rules;
            this.logger = logger;
            this.gameOver = gameOver;

            foreach (int ii in path)
            {
                this.path.Add(ii);
            }

            /*
            //debug
            string s = "Path = ";
            foreach (int ii in this.path)
            {
                s += (ii + " ");
            }
            s += (gameOver ? "." : "");
            logger.log(s);
            */

        }


        public void updatePossibleMoves(List<int> possibleMoves)
        {
            this.possibleMoves = new List<int>();
            foreach (int ii in possibleMoves)
            {
                this.possibleMoves.Add(ii);
            }
        }

        public int pathMatchCount(List<int> pathCheck)
        {
            int retval = 0;
            int checkedItem = 0;
            bool match = true;

            while (match && (checkedItem < pathCheck.Count) && (checkedItem < path.Count))
            {
                if (pathCheck[checkedItem]==path[checkedItem])
                {
                    checkedItem++;
                }
                else
                {
                    match = false;
                }
            }

            retval = checkedItem;

            return retval;
        }


        public string toString()
        {
            string s = "Scenario path = ( ";
            s += rules.intListToString(path);
            s += "); possibleMoves = ( ";
            s += rules.intListToString(possibleMoves);
            s += "); brainCellsLocation = "+brainCellsLocation;
            return s;
        }




    }
}
