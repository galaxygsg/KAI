using System;
using KAI.FSA; // Use the namespace from your FSAImpl definition

namespace Miner49er
{
    /// <summary>
    /// This class implements the Miner using a simple 3-state FSA:
    /// Mining -> Banking -> Drinking
    /// Option A change: drink earlier (preemptive hydration).
    /// </summary>
    public class SimpleMiner : FSAImpl, Miner
    {
        /// Amount of gold nuggets in the miner's pockets
        public int gold = 0;

        /// How thirsty the miner is
        public int thirst = 0;

        /// How many gold nuggets the miner has in the bank
        public int bank = 0;

        // States
        State miningState;
        State drinkingState;
        State bankingState;

        public SimpleMiner() : base("SimpleMiner")
        {
            miningState = MakeNewState("Mining");
            drinkingState = MakeNewState("Drinking");
            bankingState = MakeNewState("Banking");

            // --------------------
            // Mining transitions
            // --------------------
            // If thirsty enough, go drink (Option A threshold)
            miningState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.parched) },
                new ActionDelegate[] { }, drinkingState);

            // If pockets are full, go to bank
            miningState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.pocketsFull) },
                new ActionDelegate[] { }, bankingState);

            // Otherwise, dig
            miningState.addTransition("tick",
                new ConditionDelegate[] { },
                new ActionDelegate[] { new ActionDelegate(this.dig) }, miningState);

            // --------------------
            // Drinking transitions
            // --------------------
            // Keep drinking until thirst reaches 0
            drinkingState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.thirsty) },
                new ActionDelegate[] { new ActionDelegate(this.takeDrink) }, drinkingState);

            // If no longer thirsty, go back to mining
            drinkingState.addTransition("tick",
                new ConditionDelegate[] { },
                new ActionDelegate[] { }, miningState);

            // --------------------
            // Banking transitions
            // --------------------
            // Deposit all gold one nugget per tick
            bankingState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.pocketsNotEmpty) },
                new ActionDelegate[] { new ActionDelegate(this.depositGold) }, bankingState);

            // If thirsty, go drink
            bankingState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.parched) },
                new ActionDelegate[] { }, drinkingState);

            // Otherwise, go back to mining
            bankingState.addTransition("tick",
                new ConditionDelegate[] { },
                new ActionDelegate[] { }, miningState);

            SetCurrentState(miningState);
        }

        /// <summary>
        /// Option A: preemptive hydration.
        /// Trigger drinking before the hard stop at 15.
        /// </summary>
        private Boolean parched(FSA fsa)
        {
            if (thirst >= 12)
            {
                Console.WriteLine("Getting thirsty, time to drink.");
            }
            return thirst >= 12;
        }

        /// <summary>
        /// Drink reduces thirst by 1 per tick.
        /// </summary>
        private void takeDrink(FSA fsa)
        {
            thirst -= 1;
            Console.WriteLine("Glug glug glug");
        }

        /// <summary>
        /// Deposit moves 1 nugget from pockets to bank per tick.
        /// </summary>
        private void depositGold(FSA fsa)
        {
            gold -= 1;
            bank += 1;
            Console.WriteLine("deposit a gold nugget");
        }

        /// <summary>
        /// Total wealth is bank + pockets.
        /// </summary>
        public int getCurrentWealth()
        {
            return bank + gold;
        }

        /// <summary>
        /// Digging earns gold but increases thirst.
        /// </summary>
        private void dig(FSA fsa)
        {
            gold++;
            thirst++;
            Console.WriteLine("Miner is digging.");
        }

        private Boolean pocketsFull(FSA fsa) => gold >= 5;
        private Boolean pocketsNotEmpty(FSA fsa) => gold > 0;
        private Boolean thirsty(FSA fsa) => thirst > 0;

        public void printStatus()
        {
            Console.WriteLine("Thirst: " + thirst + " Gold: " + gold + " Bank: " + bank);
        }
    }
}
