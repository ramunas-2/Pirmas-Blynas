using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Probability
{
    class ArenaAugmented
    {
        Logger logger;
        Rules rules;

        List<FightStatisticsComponent> fightStatisticComponents;

        int maxGameOverPathLengthEvolution1;
        List<FightStatisticsComponentEvo2> fightStatisticComponentsEvo2;

        int maxGameOverPathLength = 0;

        public ArenaAugmented(Logger logger, Rules rules)
        {
            logger.set("FightS", 1, Color.Green);
            logger.set("Anti-A", 1, Color.Blue);
            logger.set("Anti-AE1", 10, Color.Blue);
            this.logger = logger;
            this.rules = rules;
            fillFightStatisticsComponents();
            fillFightStatisticsComponentsEvo2();


        }


        public Player makeAugmentedAntiplayerEvolution1(Player pP)
        {
            Stopwatch stopwatch = new Stopwatch();

            Player pA1 = null;
            Player pA2 = null;
            int repeatCount = 10;


            stopwatch.Restart();
            for (int i = 1; i < repeatCount; i++)
            {
                pA1 = antiPlayerE01(pP); //Original
            }
            stopwatch.Stop();
            logger.logChart(stopwatch.ElapsedMilliseconds);

            stopwatch.Restart();
            for (int i = 1; i < repeatCount; i++)
            {
                pA2 = antiPlayerE02(pP); //Refactored, optimised for performance
            }
            stopwatch.Stop();
            logger.logChart(stopwatch.ElapsedMilliseconds);

            if (pA1.isBrainCellsEqual(pA2))
            {
                logger.log("OK", 10, "Anti-AE1");
            }
            else
            {
                logger.log("Antiplayer do not match", 10, "Error");
            }

            return pA2;
        }

        private Player antiPlayerE02(Player pP)
        {
            //Refactored, optimised for performance
            Player pA = pP.copyPlayer();
            for (int i = 0; i < pA.allBrainCellsCount; i++)
            {
                pA.brainCells[i] = -1.0d;
            }

            int maxGameOverPathLengthEvolution1;
            List<FightStatisticsComponent> antiComponents;
            aPE02Preparation2(pP, out maxGameOverPathLengthEvolution1, out antiComponents);

            aPE02Calculation(pA, maxGameOverPathLengthEvolution1, antiComponents);

            pA.strength = aPE02CalculateSum(antiComponents); ;

            return pA;
        }


        private void aPE02Preparation(Player pP, out int maxGameOverPathLengthEvolution1, out List<FightStatisticsComponent> antiComponents)
        {
            maxGameOverPathLengthEvolution1 = 0;
            antiComponents = new List<FightStatisticsComponent>();
            foreach (FightStatisticsComponent fightStatisticComponent in fightStatisticComponents)
            {
                FightStatisticsComponent newComponent = new FightStatisticsComponent(fightStatisticComponent);
                bool lastPlayer = true;
                int initialLength = newComponent.probabilityComponents.Count;
                for (int i = newComponent.probabilityComponents.Count - 1; i >= 0; i--)
                {
                    if (lastPlayer ^ (newComponent.whoEnds == 1) ^ (initialLength % 2 == 0))
                    {
                        newComponent.wonCoins *= pP.brainCells[newComponent.probabilityComponents[i]];
                        newComponent.probabilityComponents.RemoveAt(i);
                    }
                    lastPlayer ^= true;
                }
                newComponent.whoEnds = 1;
                if (newComponent.probabilityComponents.Count > maxGameOverPathLengthEvolution1)
                {
                    maxGameOverPathLengthEvolution1 = newComponent.probabilityComponents.Count;
                }
                newComponent.setCountZero();
                antiComponents.Add(newComponent);
            }

            //int before = antiComponents.Count;

            for (int i = antiComponents.Count - 1; i >= 1; i--)
            {
                FightStatisticsComponent component = antiComponents[i];
                bool equalFound = false;
                for (int j = i - 1; (j >= 0) && (!equalFound); j--)
                {
                    FightStatisticsComponent component2 = antiComponents[j];
                    if (component.comparePC(component2))
                    {
                        equalFound = true;
                        component2.wonCoins += component.wonCoins;
                    }
                }
                if (equalFound)
                {
                    antiComponents.RemoveAt(i);
                }

            }

            //logger.log("Before: " + before + "; after: " + antiComponents.Count);


        }


        private void aPE02Preparation2(Player pP, out int maxGameOverPathLengthEvolution1, out List<FightStatisticsComponent> antiComponents)
        {
            maxGameOverPathLengthEvolution1 = this.maxGameOverPathLengthEvolution1;
            antiComponents = new List<FightStatisticsComponent>();
            foreach (FightStatisticsComponentEvo2 fightStatisticComponentEvo2 in fightStatisticComponentsEvo2)
            {
                FightStatisticsComponent newComponent = new FightStatisticsComponent(fightStatisticComponentEvo2);
                double sum = 0.0d;
                for (int i = 0; i < fightStatisticComponentEvo2.matrixLI; i++)
                {
                    double multiplication = fightStatisticComponentEvo2.matrixCoins[i];
                    for (int j = 0; j < fightStatisticComponentEvo2.matrixLJ[i]; j++)
                    {
                        multiplication *= pP.brainCells[fightStatisticComponentEvo2.matrix[i][j]]; // j,i?
                    }
                    sum += multiplication;
                }
                fightStatisticComponentEvo2.wonCoins = sum;
            }

        }


        private void aPE02Calculation(Player pA, int maxGameOverPathLengthEvolution1, List<FightStatisticsComponent> antiComponents)
        {
            for (int pathLength = maxGameOverPathLengthEvolution1; pathLength > 0; pathLength--)
            {
                foreach (FightStatisticsComponent antiComponent in antiComponents)
                {
                    if (antiComponent.probabilityComponentsCount == pathLength)
                    {
                        if (pA.brainCells[antiComponent.probabilityComponents0] == -1.0d)
                        {
                            Scenario scenario = rules.findScenarioByBCLocation(antiComponent.probabilityComponents0 % rules.situationBrainCellsCount);
                            int currentADice = antiComponent.probabilityComponents0 / rules.situationBrainCellsCount;
                            double? bestOptionABenefit = null;
                            int bestOptionAChoiceLocation = -1;
                            int choiceLocation = (currentADice * rules.situationBrainCellsCount) + scenario.brainCellsLocation;
                            foreach (int iiA in scenario.possibleMoves)
                            {
                                double sumOfAllDiceCombinationsBenefit = 0;
                                foreach (FightStatisticsComponent deepComponent in antiComponents)
                                {
                                    if ((deepComponent.probabilityComponentsCount == pathLength) && (deepComponent.probabilityComponents0 == choiceLocation))
                                    {
                                        sumOfAllDiceCombinationsBenefit += deepComponent.wonCoins;
                                    }
                                }
                                if (bestOptionABenefit.HasValue)
                                {
                                    if (sumOfAllDiceCombinationsBenefit > bestOptionABenefit)
                                    {
                                        bestOptionABenefit = sumOfAllDiceCombinationsBenefit;
                                        bestOptionAChoiceLocation = choiceLocation;
                                    }
                                }
                                else
                                {
                                    bestOptionABenefit = sumOfAllDiceCombinationsBenefit;
                                    bestOptionAChoiceLocation = choiceLocation;
                                }
                                choiceLocation++;
                            }
                            choiceLocation = (currentADice * rules.situationBrainCellsCount) + scenario.brainCellsLocation;
                            foreach (int iiA in scenario.possibleMoves)
                            {
                                pA.brainCells[choiceLocation] = ((choiceLocation == bestOptionAChoiceLocation) ? 1.0d : 0.0d);
                                foreach (FightStatisticsComponent deepComponent in antiComponents)
                                {
                                    if ((deepComponent.probabilityComponentsCount == pathLength) && (deepComponent.probabilityComponents0 == choiceLocation))
                                    {
                                        deepComponent.wonCoins *= (pA.brainCells[deepComponent.probabilityComponents0]);
                                        deepComponent.probabilityComponents.RemoveAt(0);
                                        deepComponent.setCountZero();
                                    }
                                }
                                choiceLocation++;
                            }
                        }
                    }
                }
            }
        }

        private double aPE02CalculateSum(List<FightStatisticsComponent> antiComponents)
        {
            double sumOfCoins = 0;
            foreach (FightStatisticsComponent antiComponent in antiComponents)
            {
                sumOfCoins += antiComponent.wonCoins;
            }
            sumOfCoins /= (2 * rules.diceCombinations * rules.diceCombinations);
            return sumOfCoins;
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------


        private Player antiPlayerE01(Player pP)
        {
            Player pA = pP.copyPlayer();
            pA.name = "AntiE1-" + pA.name;
            //logger.log("Antiplayer Augmented started for pleyer : " + pP.toString(), 5, "Anti-A");
            //1 Fill A braincells with -1
            for (int i = 0; i < pA.allBrainCellsCount; i++)
            {
                pA.brainCells[i] = -1.0d;
            }
            //2 Make fightStatisticComponents copy
            int maxGameOverPathLengthEvolution1 = 0;
            List<FightStatisticsComponent> antiComponents = new List<FightStatisticsComponent>();
            int ii = 0;
            foreach (FightStatisticsComponent fightStatisticComponent in fightStatisticComponents)
            {
                //antiComponents.Add(new FightStatisticsComponent(fightStatisticComponent));
                FightStatisticsComponent newComponent = new FightStatisticsComponent(fightStatisticComponent);
                //Transfer P values to coins
                bool lastPlayer = true;
                int initialLength = newComponent.probabilityComponents.Count;
                for (int i = newComponent.probabilityComponents.Count - 1; i >= 0; i--)
                {
                    if (lastPlayer ^ (newComponent.whoEnds == 1) ^ (initialLength % 2 == 0))
                    {
                        newComponent.wonCoins *= pP.brainCells[newComponent.probabilityComponents[i]];
                        newComponent.probabilityComponents.RemoveAt(i);
                    }
                    lastPlayer ^= true;
                }
                newComponent.whoEnds = 1;
                if (newComponent.probabilityComponents.Count > maxGameOverPathLengthEvolution1)
                {
                    maxGameOverPathLengthEvolution1 = newComponent.probabilityComponents.Count;
                }
                newComponent.setCountZero();
                antiComponents.Add(newComponent);

                //logger.log("PAthA["+ii.ToString("D3")+"] = ;" + rules.intListToStringReverse(newComponent.probabilityComponents), 8, "Anti-AE1");
                ii++;

            }

            //debugAntiComponents(pP, pA, antiComponents);

            //3 Filter longest (n length) paths where A ends
            for (int pathLength = maxGameOverPathLengthEvolution1; pathLength > 0; pathLength--)
            {
                foreach (FightStatisticsComponent antiComponent in antiComponents)
                {
                    if (antiComponent.probabilityComponents.Count() == pathLength)
                    {
                        //4 Scan groups of moves (A6, A7……A16, A17)
                        int lastLocation = antiComponent.probabilityComponents[0];
                        if (pA.brainCells[lastLocation] == -1.0d)
                        {//Component is not processed at this level yet, must collect a group and process
                            //logger.log("Iteration pathLength = " + pathLength + " path = ( " + rules.intListToString(antiComponent.probabilityComponents) + ")", 8, "Anti-A");
                            //Identify A group
                            Scenario scenario = rules.findScenarioByBCLocation(lastLocation % rules.situationBrainCellsCount);
                            int currentADice = lastLocation / rules.situationBrainCellsCount;
                            //Scan all other A in the same group
                            //Scan all P dice combinations
                            double? bestOptionABenefit = null;
                            int bestOptionAChoiceLocation = -1;
                            int choiceLocation = (currentADice * rules.situationBrainCellsCount) + scenario.brainCellsLocation;
                            foreach (int iiA in scenario.possibleMoves)
                            {
                                double sumOfAllDiceCombinationsBenefit = 0;
                                //5 Calculate sum of (multiplication of all P probabilites * coins in the same row) for each A in a group (same A dice!)
                                foreach (FightStatisticsComponent deepComponent in antiComponents)
                                {
                                    if ((deepComponent.probabilityComponentsCount == pathLength) && (deepComponent.probabilityComponents0 == choiceLocation))
                                    {
                                        sumOfAllDiceCombinationsBenefit += deepComponent.wonCoins;
                                    }
                                }
                                //logger.log("Sum = " + sumOfAllDiceCombinationsBenefit, 8, "Anti-A");
                                if (bestOptionABenefit.HasValue)
                                {
                                    if (sumOfAllDiceCombinationsBenefit > bestOptionABenefit)
                                    {
                                        bestOptionABenefit = sumOfAllDiceCombinationsBenefit;
                                        bestOptionAChoiceLocation = choiceLocation;
                                    }
                                }
                                else
                                {
                                    bestOptionABenefit = sumOfAllDiceCombinationsBenefit;
                                    bestOptionAChoiceLocation = choiceLocation;
                                }
                                choiceLocation++;
                            }
                            //logger.log("Benefit = " + bestOptionABenefit, 8, "Anti-A");
                            //6 Choose the best outcome, fill its A with 1, and replace all other A with 0 in the group (verify if A was -1 before replacement, otherwise - exception)
                            choiceLocation = (currentADice * rules.situationBrainCellsCount) + scenario.brainCellsLocation;
                            foreach (int iiA in scenario.possibleMoves)
                            {
                                if (pA.brainCells[choiceLocation] == -1.0d)
                                {
                                    pA.brainCells[choiceLocation] = ((choiceLocation == bestOptionAChoiceLocation) ? 1.0d : 0.0d);
                                }
                                else
                                {
                                    logger.log("Trying to fill non -1 brain cell", 1, "Error");
                                }
                                foreach (FightStatisticsComponent deepComponent in antiComponents)
                                {
                                    if ((deepComponent.probabilityComponentsCount == pathLength) && (deepComponent.probabilityComponents0 == choiceLocation))
                                    {
                                        deepComponent.wonCoins *= (pA.brainCells[deepComponent.probabilityComponents[0]]);
                                        deepComponent.probabilityComponents.RemoveAt(0);
                                        //Performance fix
                                        //deepComponent.probabilityComponentsCount--;
                                        //deepComponent.probabilityComponents0 = deepComponent.probabilityComponents[0];
                                        deepComponent.setCountZero();
                                    }
                                }
                                choiceLocation++;
                            }
                            //debugAntiComponents(pP, pA, antiComponents);
                        }
                    }


                }
                //8 While N>1, continue step 3
            }

            double sumOfCoins = 0;
            foreach (FightStatisticsComponent antiComponent in antiComponents)
            {
                sumOfCoins += antiComponent.wonCoins;
            }
            sumOfCoins /= (2 * rules.diceCombinations * rules.diceCombinations);
            pA.strength = sumOfCoins;
            //logger.log("Sum of Coins per game = " + sumOfCoins.ToString("F4"), 1, "Anti-A");

            //9 Verify if any -1 brain cell is left in A
            //10 Make a copy of A, run normalise, verify if anything is changed
            /*
            Player pACopy = pA.copyPlayer();
            pACopy.normaliseBrainCells();
            for (int i = 0; i < rules.allBrainCellsCount; i++)
            {
                if (pA.brainCells[i] == -1.0d)
                {
                    logger.log("After Anti player creatiopn found -1 brain cell", 1, "Error");
                }
                if (pA.brainCells[i] != pACopy.brainCells[i])
                {
                    logger.log("After Anti player creatiopn found not normalised brain cell", 1, "Error");
                }
            }
            */



            //logger.log("Antiplayer Augmented made : " + pA.toString(), 5, "Anti-A");
            //logger.log("Antiplayer Augmented success against player : " + fightStatistics(pA, pP).ToString("F4"), 5, "Anti-A");
            return pA;
        }


        public Player makeAugmentedAntiplayerEvolution1_Bad(Player pP)
        {
            Player pA = pP.copyPlayer();
            pA.name = "AntiE1-" + pA.name;
            logger.log("Antiplayer Augmented started for pleyer : " + pP.toString(), 5, "Anti-AE1");


            //1 Fill A braincells with -1
            for (int i = 0; i < pA.allBrainCellsCount; i++)
            {
                pA.brainCells[i] = -1.0d;
            }
            //2 Make fightStatisticComponents copy
            List<FightStatisticsComponent> antiComponents = new List<FightStatisticsComponent>();
            foreach (FightStatisticsComponent fightStatisticComponent in fightStatisticComponents)
            {
                FightStatisticsComponent newComponent = new FightStatisticsComponent(fightStatisticComponent);
                //Transfer P values to coins
                bool lastPlayer = true;
                for (int i = newComponent.probabilityComponents.Count - 1; i >= 0; i--)
                {
                    if (lastPlayer ^ (newComponent.whoEnds == 1))
                    {
                        newComponent.wonCoins *= pP.brainCells[newComponent.probabilityComponents[i]];
                        newComponent.probabilityComponents.RemoveAt(i);
                    }
                    lastPlayer ^= true;
                }
                newComponent.whoEnds = 1;
                antiComponents.Add(newComponent);
            }
            debugAntiComponents(pP, pA, antiComponents);
            //3 Filter longest (n length) paths where A ends
            for (int pathLength = maxGameOverPathLength; pathLength > 0; pathLength--)
            {
                foreach (FightStatisticsComponent antiComponent in antiComponents)
                {
                    if ((antiComponent.whoEnds == 1) && (antiComponent.probabilityComponents.Count() == pathLength))
                    {
                        //4 Scan groups of moves (A6, A7……A16, A17)
                        int lastLocation = antiComponent.probabilityComponents[0];
                        if (pA.brainCells[lastLocation] == -1.0d)
                        {//Component is not processed at this level yet, must collect a group and process
                            logger.log("Iteration pathLength = " + pathLength + " path = ( " + rules.intListToString(antiComponent.probabilityComponents) + ")", 8, "Anti-AE1");
                            //Identify A group
                            Scenario scenario = rules.findScenarioByBCLocation(lastLocation % rules.situationBrainCellsCount);
                            int currentADice = lastLocation / rules.situationBrainCellsCount;
                            //Scan all other A in the same group
                            //Scan all P dice combinations
                            double? bestOptionABenefit = null;
                            int bestOptionAChoiceLocation = -1;
                            int choiceLocation = (currentADice * rules.situationBrainCellsCount) + scenario.brainCellsLocation;
                            foreach (int iiA in scenario.possibleMoves)
                            {
                                double sumOfAllDiceCombinationsBenefit = 0;
                                //5 Calculate sum of (multiplication of all P probabilites * coins in the same row) for each A in a group (same A dice!)
                                foreach (FightStatisticsComponent deepComponent in antiComponents)
                                {
                                    if ((deepComponent.whoEnds == 1) && (deepComponent.probabilityComponents.Count() == pathLength) && (deepComponent.probabilityComponents[0] == choiceLocation))
                                    {
                                        double multiplication = (double)deepComponent.wonCoins;
                                        /*
                                        for (int i = 1; i < pathLength; i += 2)
                                        {
                                            multiplication *= pP.brainCells[deepComponent.probabilityComponents[i]];
                                        }
                                        */
                                        sumOfAllDiceCombinationsBenefit += multiplication;
                                    }
                                }
                                if (bestOptionABenefit.HasValue)
                                {
                                    if (sumOfAllDiceCombinationsBenefit > bestOptionABenefit)
                                    {
                                        bestOptionABenefit = sumOfAllDiceCombinationsBenefit;
                                        bestOptionAChoiceLocation = choiceLocation;
                                    }
                                }
                                else
                                {
                                    bestOptionABenefit = sumOfAllDiceCombinationsBenefit;
                                    bestOptionAChoiceLocation = choiceLocation;
                                }
                                choiceLocation++;
                            }
                            //6 Choose the best outcome, fill its A with 1, and replace all other A with 0 in the group (verify if A was -1 before replacement, otherwise - exception)
                            choiceLocation = (currentADice * rules.situationBrainCellsCount) + scenario.brainCellsLocation;
                            foreach (int iiA in scenario.possibleMoves)
                            {
                                if (pA.brainCells[choiceLocation] == -1.0d)
                                {
                                    pA.brainCells[choiceLocation] = ((choiceLocation == bestOptionAChoiceLocation) ? 1.0d : 0.0d);
                                }
                                else
                                {
                                    logger.log("Trying to fill non -1 brain cell", 1, "Error");
                                }
                                foreach (FightStatisticsComponent deepComponent in antiComponents)
                                {
                                    if ((deepComponent.whoEnds == 1) && (deepComponent.probabilityComponents.Count() == pathLength) && (deepComponent.probabilityComponents[0] == choiceLocation))
                                    {
                                        //7 If N>2, replace coins with coins * A(n) * p(n-1); delete A(n) and p(n-1)
                                        if (pathLength > 0)
                                        {
                                            deepComponent.wonCoins *= (pA.brainCells[deepComponent.probabilityComponents[0]]);
                                            deepComponent.probabilityComponents.RemoveAt(0);
                                        }
                                    }
                                }
                                choiceLocation++;
                            }
                            debugAntiComponents(pP, pA, antiComponents);
                        }
                    }
                    /*
                    if ((antiComponent.whoEnds != 1) && (antiComponent.probabilityComponents.Count() == pathLength))
                    {//Fix - additionally remove P probability and move to Coins

                        if (pathLength > 0)
                        {
                            antiComponent.wonCoins *= (pP.brainCells[antiComponent.probabilityComponents[0]]);
                            antiComponent.probabilityComponents.RemoveAt(0);
                        }
                    }
                    */
                }
                //8 While N>1, continue step 3
            }
            //9 Verify if any -1 brain cell is left in A
            //10 Make a copy of A, run normalise, verify if anything is changed
            Player pACopy = pA.copyPlayer();
            pACopy.normaliseBrainCells();
            for (int i = 0; i < rules.allBrainCellsCount; i++)
            {
                if (pA.brainCells[i] == -1.0d)
                {
                    logger.log("After Anti player creatiopn found -1 brain cell", 1, "Error");
                }
                if (pA.brainCells[i] != pACopy.brainCells[i])
                {
                    logger.log("After Anti player creatiopn found not normalised brain cell", 1, "Error");
                }
            }




            logger.log("Antiplayer Augmented made : " + pA.toString(), 5, "Anti-A");
            logger.log("Antiplayer Augmented success against player : " + fightStatistics(pA, pP).ToString("F4"), 5, "Anti-A");
            return pA;

        }

        public Player makeAugmentedAntiplayer(Player pP)
        {
            Player pA = pP.copyPlayer();
            pA.name = "Anti-" + pA.name;
            logger.log("Antiplayer Augmented started for pleyer : " + pP.toString(), 5, "Anti-A");



            //1 Fill A braincells with -1
            //2 Make fightStatisticComponents copy
            //3 Filter longest (n length) paths where A ends
            //4 Scan groups of moves (A6, A7……A16, A17)
            //5 Calculate sum of (multiplication of all P probabilites * coins in the same row) for each A in a group (same A dice!)
            //6 Choose the best outcome, fill its A with 1, and replace all other A with 0 in the group (verify if A was -1 before replacement, otherwise - exception)
            //7 If N>2, replace coins with coins * A(n) * p(n-1); delete A(n) and p(n-1)
            //8 While N>1, continue step 3
            //9 Verify if any -1 brain cell is left in A
            //10 Make a copy of A, run normalise, verify if anything is changed



            //1 Fill A braincells with -1
            for (int i = 0; i < pA.allBrainCellsCount; i++)
            {
                pA.brainCells[i] = -1.0d;
            }
            //2 Make fightStatisticComponents copy
            List<FightStatisticsComponent> antiComponents = new List<FightStatisticsComponent>();
            foreach (FightStatisticsComponent fightStatisticComponent in fightStatisticComponents)
            {
                //FightStatisticsComponent newComponent = new FightStatisticsComponent(fightStatisticComponent);
                antiComponents.Add(new FightStatisticsComponent(fightStatisticComponent));
            }

            debugAntiComponents(pP, pA, antiComponents);

            //3 Filter longest (n length) paths where A ends
            for (int pathLength = maxGameOverPathLength; pathLength > 0; pathLength--)
            {
                foreach (FightStatisticsComponent antiComponent in antiComponents)
                {
                    if ((antiComponent.whoEnds == 1) && (antiComponent.probabilityComponents.Count() == pathLength))
                    {
                        //4 Scan groups of moves (A6, A7……A16, A17)
                        int lastLocation = antiComponent.probabilityComponents[0];
                        if (pA.brainCells[lastLocation] == -1.0d)
                        {//Component is not processed at this level yet, must collect a group and process
                            logger.log("Iteration pathLength = " + pathLength + " path = ( " + rules.intListToString(antiComponent.probabilityComponents) + ")", 8, "Anti-A");
                            //Identify A group
                            Scenario scenario = rules.findScenarioByBCLocation(lastLocation % rules.situationBrainCellsCount);
                            int currentADice = lastLocation / rules.situationBrainCellsCount;
                            //Scan all other A in the same group
                            //Scan all P dice combinations
                            double? bestOptionABenefit = null;
                            int bestOptionAChoiceLocation = -1;
                            int choiceLocation = (currentADice * rules.situationBrainCellsCount) + scenario.brainCellsLocation;
                            foreach (int iiA in scenario.possibleMoves)
                            {
                                double sumOfAllDiceCombinationsBenefit = 0;
                                //5 Calculate sum of (multiplication of all P probabilites * coins in the same row) for each A in a group (same A dice!)
                                foreach (FightStatisticsComponent deepComponent in antiComponents)
                                {
                                    if ((deepComponent.whoEnds == 1) && (deepComponent.probabilityComponents.Count() == pathLength) && (deepComponent.probabilityComponents[0] == choiceLocation))
                                    {
                                        double multiplication = (double)deepComponent.wonCoins;
                                        for (int i = 1; i < pathLength; i += 2)
                                        {
                                            multiplication *= pP.brainCells[deepComponent.probabilityComponents[i]];
                                        }
                                        sumOfAllDiceCombinationsBenefit += multiplication;
                                    }
                                }
                                logger.log("Sum = " + sumOfAllDiceCombinationsBenefit, 8, "Anti-A");
                                if (bestOptionABenefit.HasValue)
                                {
                                    if (sumOfAllDiceCombinationsBenefit > bestOptionABenefit)
                                    {
                                        bestOptionABenefit = sumOfAllDiceCombinationsBenefit;
                                        bestOptionAChoiceLocation = choiceLocation;
                                    }
                                }
                                else
                                {
                                    bestOptionABenefit = sumOfAllDiceCombinationsBenefit;
                                    bestOptionAChoiceLocation = choiceLocation;
                                }
                                choiceLocation++;
                            }
                            logger.log("Benefit = " + bestOptionABenefit, 8, "Anti-A");
                            //6 Choose the best outcome, fill its A with 1, and replace all other A with 0 in the group (verify if A was -1 before replacement, otherwise - exception)
                            choiceLocation = (currentADice * rules.situationBrainCellsCount) + scenario.brainCellsLocation;
                            foreach (int iiA in scenario.possibleMoves)
                            {
                                if (pA.brainCells[choiceLocation] == -1.0d)
                                {
                                    pA.brainCells[choiceLocation] = ((choiceLocation == bestOptionAChoiceLocation) ? 1.0d : 0.0d);
                                }
                                else
                                {
                                    logger.log("Trying to fill non -1 brain cell", 1, "Error");
                                }
                                foreach (FightStatisticsComponent deepComponent in antiComponents)
                                {
                                    if ((deepComponent.whoEnds == 1) && (deepComponent.probabilityComponents.Count() == pathLength) && (deepComponent.probabilityComponents[0] == choiceLocation))
                                    {
                                        //7 If N>2, replace coins with coins * A(n) * p(n-1); delete A(n) and p(n-1)
                                        if (pathLength > 2)
                                        {
                                            deepComponent.wonCoins *= (pA.brainCells[deepComponent.probabilityComponents[0]] * pP.brainCells[deepComponent.probabilityComponents[1]]);
                                            deepComponent.probabilityComponents.RemoveAt(0);
                                            deepComponent.probabilityComponents.RemoveAt(0);
                                        }
                                    }
                                }
                                choiceLocation++;
                            }
                            debugAntiComponents(pP, pA, antiComponents);
                        }
                    }
                    if ((antiComponent.whoEnds != 1) && (antiComponent.probabilityComponents.Count() == pathLength))
                    {//Fix - additionally remove P probability and move to Coins

                        if (pathLength > 1)
                        {
                            antiComponent.wonCoins *= (pP.brainCells[antiComponent.probabilityComponents[0]]);
                            antiComponent.probabilityComponents.RemoveAt(0);
                            antiComponent.whoEnds = 1;
                        }


                    }
                }
                //8 While N>1, continue step 3
            }
            //9 Verify if any -1 brain cell is left in A
            //10 Make a copy of A, run normalise, verify if anything is changed
            Player pACopy = pA.copyPlayer();
            pACopy.normaliseBrainCells();
            for (int i = 0; i < rules.allBrainCellsCount; i++)
            {
                if (pA.brainCells[i] == -1.0d)
                {
                    logger.log("After Anti player creatiopn found -1 brain cell", 1, "Error");
                }
                if (pA.brainCells[i] != pACopy.brainCells[i])
                {
                    logger.log("After Anti player creatiopn found not normalised brain cell", 1, "Error");
                }
            }




            logger.log("Antiplayer Augmented made : " + pA.toString(), 5, "Anti-A");
            logger.log("Antiplayer Augmented success against player : " + fightStatistics(pA, pP).ToString("F4"), 5, "Anti-A");
            return pA;
        }

        void debugAntiComponents(Player pP, Player pA, List<FightStatisticsComponent> antiComponents)
        {
            logger.set("AntiD", 1, Color.DarkBlue);

            int ii = 0;
            foreach (FightStatisticsComponent component in antiComponents)
            {
                string ss = ii.ToString("D2") + " Path = ( ";
                string s1 = "";
                string s2 = "";


                bool antiPlayer = (component.whoEnds == 1) ^ (component.probabilityComponents.Count % 2 == 0);
                for (int i = component.probabilityComponents.Count - 1; i >= 0; i--)
                {
                    s1 += ((antiPlayer ? "A " : "P ") + component.probabilityComponents[i] + "; ");
                    s2 += ((antiPlayer ? ("A " + pA.brainCells[component.probabilityComponents[i]].ToString("F4")) : ("P " + pP.brainCells[component.probabilityComponents[i]].ToString("F4"))) + "; ");
                    antiPlayer ^= true;
                }

                string s3 = component.wonCoins.ToString("F4");
                while (s3.Length < 7)
                    s3 = " " + s3;


                while (s1.Length < 20)
                    s1 += " ";
                ss += (s1 + ")  Coins = " + s3);


                while (s2.Length < 35)
                    s2 += " ";

                ss += "  Probabilities = ( ";
                ss += (s2 + ")");



                logger.log(ss, 8, "AntiD");
                ii++;

            }





        }

        void fillFightStatisticsComponents()
        {
            fightStatisticComponents = new List<FightStatisticsComponent>();
            for (int dice1 = 0; dice1 < rules.diceCombinations; dice1++)
            {
                for (int dice2 = 0; dice2 < rules.diceCombinations; dice2++)
                {
                    for (int whoEnds = 1; whoEnds <= 2; whoEnds++)
                    {
                        logger.log("Scan scenario dice1 = " + dice1.ToString() + " dice2 = " + dice2.ToString() + " whoEnds = " + whoEnds.ToString(), 8, "FightS");
                        //foreach (Scenario scenario in rules.scenarios.TakeWhile(item => item.gameOver))
                        foreach (Scenario scenario in rules.scenarios)
                        {
                            if (scenario.gameOver)
                            {
                                logger.log("Analysing scenario ( " + rules.intListToString(scenario.path) + ")", 8, "FightS");
                                bool currentPlayerEnds = true;
                                List<int> remainingPath = new List<int>(scenario.path);
                                List<int> probabilityComponents = new List<int>();
                                foreach (int ii in scenario.path.Reverse<int>())
                                {
                                    //Get the Nr of braincell that lead to this scenatio
                                    //Split path:
                                    //beginning (path-1)


                                    //Check which one is right?
                                    //var pathMinusOne = scenarion.path.Take(scenarion.path.Count() - 1);
                                    List<int> pathMinusOne = remainingPath.Take(remainingPath.Count() - 1).ToList<int>();


                                    //Ending
                                    int endAction = remainingPath[remainingPath.Count() - 1];

                                    Scenario scenarioMinusOne = rules.findScenarioByPath(pathMinusOne);

                                    int lastMoveFoundAt = 0;
                                    bool moveFound = false;
                                    foreach (int jj in scenarioMinusOne.possibleMoves)
                                    {
                                        if (jj == endAction)
                                        {
                                            moveFound = true;
                                            break;
                                        }
                                        else
                                        {
                                            lastMoveFoundAt++;
                                        }

                                    }
                                    if (!moveFound)
                                    {
                                        logger.log("Last move not found", 1, "Error");
                                    }

                                    //construct brain cell location:
                                    int brainCellLocation = 0;

                                    //add smallest shift from last move
                                    brainCellLocation += lastMoveFoundAt;

                                    //add brain cells location
                                    brainCellLocation += scenarioMinusOne.brainCellsLocation;

                                    //add dice shift according to who moves
                                    //brainCellLocation += (rules.situationBrainCellsCount) * (((whoEnds == 1) ^ currentPlayerEnds) ? dice1 : dice2);
                                    //brainCellLocation += (rules.situationBrainCellsCount) * (((whoEnds == 1) ^ currentPlayerEnds) ? dice2 : dice1);
                                    brainCellLocation += (rules.situationBrainCellsCount) * (((whoEnds == 1) ^ currentPlayerEnds) ? dice2 : dice1);


                                    probabilityComponents.Add(brainCellLocation);


                                    currentPlayerEnds ^= true;
                                    remainingPath = pathMinusOne;
                                }
                                int wonCoins;

                                //Calculate amount of coins put by current player
                                int putCoints = rules.blind;
                                for (int pathIndex = scenario.path.Count - 1; pathIndex >= 0; pathIndex -= 2)
                                {
                                    int pathDecision = scenario.path[pathIndex];
                                    if (pathDecision > 0)
                                    {
                                        putCoints += pathDecision;
                                    }
                                }

                                if (scenario.path[scenario.path.Count() - 1] == -1)
                                {//Fold
                                    wonCoins = ((whoEnds == 1) ? (-putCoints) : (putCoints));
                                }
                                else
                                {
                                    if (dice1 == dice2)
                                    {
                                        wonCoins = 0;
                                    }
                                    else
                                    {
                                        //wonCoins = (((whoEnds == 1) ^ (dice1 > dice2)) ? (-putCoints) : (putCoints));
                                        wonCoins = ((dice1 > dice2) ? (putCoints) : (-putCoints));
                                    }
                                }
                                FightStatisticsComponent fightStatisticComponent = new FightStatisticsComponent();
                                fightStatisticComponent.wonCoins = wonCoins;
                                fightStatisticComponent.whoEnds = whoEnds;
                                fightStatisticComponent.probabilityComponents = probabilityComponents;
                                fightStatisticComponent.setCountZero();
                                fightStatisticComponents.Add(fightStatisticComponent);
                                if (probabilityComponents.Count() > maxGameOverPathLength)
                                {
                                    maxGameOverPathLength = probabilityComponents.Count();
                                }
                                logger.log("wonCoins = " + wonCoins.ToString() + " probabilityComponents = ( " + rules.intListToString(probabilityComponents) + ")", 8, "FightS");
                            }
                        }
                    }
                }
            }
        }

        private void fillFightStatisticsComponentsEvo2()
        {
            fightStatisticComponentsEvo2 = new List<FightStatisticsComponentEvo2>();
            maxGameOverPathLengthEvolution1 = 0;
            foreach (FightStatisticsComponent fightStatisticComponent in fightStatisticComponents)
            {
                FightStatisticsComponentEvo2 newComponent = new FightStatisticsComponentEvo2(fightStatisticComponent);
                bool lastPlayer = true;
                int initialLength = newComponent.probabilityComponents.Count;

                //Create matrix row 0
                //
                List<int> row0 = new List<int>();


                for (int i = newComponent.probabilityComponents.Count - 1; i >= 0; i--)
                {
                    if (lastPlayer ^ (newComponent.whoEnds == 1) ^ (initialLength % 2 == 0))
                    {
                        //newComponent.wonCoins *= pP.brainCells[newComponent.probabilityComponents[i]];

                        //Add element to matrix row 0 at the end
                        //
                        row0.Add(newComponent.probabilityComponents[i]);

                        newComponent.probabilityComponents.RemoveAt(i);
                    }
                    lastPlayer ^= true;
                }

                //Update matrix row 0
                //
                newComponent.matrixLI = 1;
                newComponent.matrixLJ = new int[1];
                newComponent.matrixLJ[0] = row0.Count;
                newComponent.matrix = new int[1][];
                newComponent.matrix[0] = row0.ToArray();
                newComponent.matrixCoins = new double[1];
                newComponent.matrixCoins[0] = newComponent.wonCoins;


                newComponent.whoEnds = 1;
                if (newComponent.probabilityComponents.Count > maxGameOverPathLengthEvolution1)
                {
                    maxGameOverPathLengthEvolution1 = newComponent.probabilityComponents.Count;
                }
                newComponent.setCountZero();
                fightStatisticComponentsEvo2.Add(newComponent);
            }

            //int before = antiComponents.Count;

            for (int i = fightStatisticComponentsEvo2.Count - 1; i >= 1; i--)
            {
                FightStatisticsComponentEvo2 component = fightStatisticComponentsEvo2[i];
                bool equalFound = false;
                for (int j = i - 1; (j >= 0) && (!equalFound); j--)
                {
                    FightStatisticsComponentEvo2 component2 = fightStatisticComponentsEvo2[j];
                    if (component.comparePC(component2))
                    {
                        equalFound = true;
                        //component2.wonCoins += component.wonCoins;

                        //Extend matrix, copy row
                        //

                        component2.matrixLI = component.matrixLI + 1;

                        List<int> lLJ = new List<int>(component.matrixLJ);
                        lLJ.Add(component2.matrixLJ[0]);
                        component2.matrixLJ = lLJ.ToArray();

                        int[][] matrix = new int[component2.matrixLI][]; ;
                        for (int ii = 0; ii < component.matrixLI; ii++)
                        {
                            matrix[ii] = component.matrix[ii].ToArray();
                        }
                        matrix[component2.matrixLI - 1] = component2.matrix[0].ToArray();
                        component2.matrix = matrix;

                        List<double> lLCoins = new List<double>(component.matrixCoins);
                        lLCoins.Add(component2.matrixCoins[0]);
                        component2.matrixCoins = lLCoins.ToArray();




                    }
                }
                if (equalFound)
                {
                    fightStatisticComponentsEvo2.RemoveAt(i);
                }

            }

            //logger.log("Before: " + before + "; after: " + antiComponents.Count);














        }


        //Virtual fight FightStatistics - return average won coins
        public double fightStatistics(Player p1, Player p2)
        {
            //double debugCheckSum = 0;
            double retVal = 0;
            foreach (FightStatisticsComponent fightStatisticComponent in fightStatisticComponents)
            {
                double component = 1.0;
                bool currentPlayer = true;
                //string debugS = "Component coins = " + fightStatisticComponent.wonCoins.ToString() + " probability = ( ";
                foreach (int ii in fightStatisticComponent.probabilityComponents)
                {
                    //debugS += ((((fightStatisticComponent.whoEnds == 1) ^ (currentPlayer)) ? p2.brainCells[ii] : p1.brainCells[ii]).ToString("F4") + "; ");
                    component *= (((fightStatisticComponent.whoEnds == 1) ^ (currentPlayer)) ? p2.brainCells[ii] : p1.brainCells[ii]);
                    currentPlayer ^= true;
                }
                //debugS += ")";
                //logger.log(debugS, 8, "FightS");
                //debugCheckSum += component;
                component *= fightStatisticComponent.wonCoins;
                retVal += component;
            }
            //logger.log("Debug check sum = " + debugCheckSum.ToString(), 8, "FightS");
            retVal /= (2 * rules.diceCombinations * rules.diceCombinations);
            return retVal;
        }





        //Test FightStatistics against FightScan
        //Done manually


    }

    class FightStatisticsComponentEvo2 : FightStatisticsComponent
    {
        public int matrixLI;
        public int[] matrixLJ;
        public int[][] matrix;

        public double[] matrixCoins;

        public FightStatisticsComponentEvo2(FightStatisticsComponent model)
            : base(model)
        {
        }



        public FightStatisticsComponentEvo2(FightStatisticsComponentEvo2 model)
            : base(model)
        {
            matrixLI = model.matrixLI;
            matrixLJ = new int[matrixLI];
            Array.Copy(model.matrixLJ, matrixLJ, matrixLI);
            Array.Copy(model.matrixCoins, matrixCoins, matrixLI);
            matrix = matrix.Select(x => x.ToArray()).ToArray();
        }


    }

    class FightStatisticsComponent
    {
        public double wonCoins;
        public int whoEnds;
        public List<int> probabilityComponents = new List<int>();
        public int probabilityComponentsCount;
        public int probabilityComponents0;

        public FightStatisticsComponent()
        {

        }

        public FightStatisticsComponent(FightStatisticsComponent model)
        {
            this.wonCoins = model.wonCoins;
            this.whoEnds = model.whoEnds;
            this.probabilityComponents = new List<int>(model.probabilityComponents);


        }

        public void setCountZero()
        {
            probabilityComponentsCount = probabilityComponents.Count;
            if (probabilityComponentsCount > 0)
            {
                probabilityComponents0 = probabilityComponents[0];
            }
            else
            {
                probabilityComponents0 = -1;
            }
        }

        public bool comparePC(FightStatisticsComponent c2)
        {
            bool matches = (probabilityComponentsCount == c2.probabilityComponentsCount);
            for (int i = 0; (i < probabilityComponentsCount) && (matches); i++)
            {
                if (probabilityComponents[i] != c2.probabilityComponents[i])
                {
                    matches = false;
                }
            }
            return matches;
        }

    }
}

