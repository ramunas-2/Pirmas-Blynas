using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Probability
{
    class World
    {
        Logger logger;
        Rules rules;
        public World(Logger logger)
        {
            this.logger = logger;
            rules = new Rules(logger);
        }

        //Create players
        //Create Arena
        //Live different scenarious
        //Mutation rules
        //Evolution
        //Evolve antiplayer

    }
}
