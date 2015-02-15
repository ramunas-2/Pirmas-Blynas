using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Probability
{
    class WorldAugmented
    {
        Logger logger;
        Rules rules;
        Arena arena;
        ArenaAugmented arenaAugmented;
        public WorldAugmented(Logger logger)
        {
            this.logger = logger;
            rules = new Rules(logger);
            arena = new Arena(logger, rules);
            arenaAugmented = new ArenaAugmented(logger, rules);
        }


        //Calculate antiplayer
        //Test agfainst World's antiplayer

        //Evolve perfect player
    }
}
