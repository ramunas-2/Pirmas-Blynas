using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Probability
{
    class World
    {
        Logger logger;
        Rules rules;
        Arena arena;
        public World(Logger logger)
        {
            this.logger = logger;
            rules = new Rules(logger);
            arena = new Arena(logger, rules);
        }

        //Create players
        //Create Arena
        //Live different scenarious

        public void scenario1()
        {

            logger.log("Run scenarion 1");

            Player p1 = new Player(logger, rules, "p1", true);
            Player p2 = new Player(logger, rules, "p2", true);

            logger.log(p1.toString());
            logger.log(p2.toString());

            
            double fightResult=((double)arena.fightScan(p1,p2,1000));
            logger.log("Fight result = "+fightResult.ToString("F4"));
            

        }


        //Mutation rules
        //Evolution
        //Evolve antiplayer

    }
}
