using System;

namespace Miner49er
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            // Set up the variables
            Miner miner = new SimpleMiner();
            int secsPerTick = 1;
            Random myRandom = new Random(Environment.TickCount);

            // Longer run for clearer FSA behavior (50–59 ticks)
            int gameLengthInTics = (int)(myRandom.NextSingle() * 10) + 50;

            // Run the Miner49er loop
            for (int tick = 0; tick < gameLengthInTics; tick++)
            {
                Console.WriteLine("Tick # " + tick);
                miner.DoEvent("tick");
                miner.printStatus();
                Console.WriteLine();
                Console.WriteLine();
                System.Threading.Thread.Sleep(secsPerTick * 1000);
            }

            Console.WriteLine("Ending Wealth: " + miner.getCurrentWealth());
        }
    }
}