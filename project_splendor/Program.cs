using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_splendor
{
    class Program
    {
        static void Main(string[] args)
        {
            var outcomes = Game.Playout(new PlayerState(), 1, 1);
            outcomes = outcomes.Distinct().OrderBy(p => p.MovesCount).ThenBy(p => p.Score).ToList();
            Console.WriteLine($"Total outcomes simulated: {outcomes.Count()}");
            var best = outcomes.First();
            Console.WriteLine($"Best result: {best.Score} in {best.MovesCount}");
            Console.ReadKey();
            Console.ReadKey();
            Console.WriteLine("Outcome log:");
            foreach (var line in best.Log)
            {
                Console.WriteLine(line);
            }
            Console.ReadKey();
        }

        
    }
}
