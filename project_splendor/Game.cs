using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_splendor
{
    public static class Game
    {
        static Game()
        {
            Cards = LoadCards();
            Nobles = LoadNobles();
            Actions = CreateActions();
        }

        public static List<Card> Cards { get; }
        public static List<Noble> Nobles { get; }
        public static List<IPlayerAction> Actions { get; }

        static List<Card> LoadCards()
        {
            var result = new List<Card>();
            string[] lines = System.IO.File.ReadAllLines(@"cards_data.txt");
            foreach (string line in lines)
            {
                var splitline = line.Split(' ');
                int[] v = new int[6];
                for (int i = 1; i < 7; i++)
                {
                    int.TryParse(splitline[i], out v[i - 1]);
                }
                Color col;
                Enum.TryParse(splitline[0], out col);
                result.Add(new Card(col, v[0], v[1], v[2], v[3], v[4], v[5]));
            }
            return result;
        }

        static List<Noble> LoadNobles()
        {
            var result = new List<Noble>();
            string[] lines = System.IO.File.ReadAllLines(@"nobles_data.txt");

            foreach (string line in lines)
            {
                var splitline = line.Split(' ');
                int[] v = new int[5];
                for (int i = 0; i < 5; i++)
                {
                    int.TryParse(splitline[i], out v[i]);
                }

                result.Add(new Noble(v[0], v[1], v[2], v[3], v[4]));
            }
            return result;
        }

        static List<IPlayerAction> CreateActions()
        {
            var result = new List<IPlayerAction>();
            var allColors = Enum.GetValues(typeof(Color));
            foreach (Color col in allColors)
            {
                result.Add(new TakeTwoAction(col));
            }

            //var temp3colors = new List<Tuple<Color, Color, Color>>();
            //for (int i = 7; i <= 28; i++)
            //{
            //    var colors = GetColors(i);
            //    if (colors.Count == 3)
            //        temp3colors.Add(new Tuple<Color, Color, Color>(colors[0], colors[1], colors[2]));

            //}
            //foreach (var tuple in temp3colors.Distinct())
            //{
            //    result.Add(new TakeThreeAction(tuple.Item1, tuple.Item2, tuple.Item3));
            //}

            foreach (Card card in Cards)
            {
                result.Add(new TakeCardAction(card));
            }

            return result;
        }

        static List<Color> GetColors(int i)
        {
            List<Color> result = new List<Color>();
            while (i > 0)
            {
                if (i >= 16)
                {
                    result.Add(Color.White);
                    i -= 16;
                }
                if (i >= 8)
                {
                    result.Add(Color.Blue);
                    i -= 8;
                }
                if (i >= 4)
                {
                    result.Add(Color.Green);
                    i -= 4;
                }
                if (i >= 2)
                {
                    result.Add(Color.Red);
                    i -= 2;
                }
                if (i >= 1)
                {
                    result.Add(Color.Black);
                    i -= 1;
                }
            }

            return result;
        }

        public static IEnumerable<PlayerState> Playout(PlayerState player, int MaxMoves, int RequiredPoints)
        {
            if (player.MovesCount >= MaxMoves || player.Score >= RequiredPoints)
                return new List<PlayerState>() { player };

            var result = new List<PlayerState>();
            var availableActions = Actions.Where(a => a.IsAvailable(player)).ToList();
            foreach (var action in availableActions)
            {
                result.AddRange(action.Execute(player));
            }
            var newResult = new List<PlayerState>();
            foreach (var state in result.Distinct())
            {
                newResult.AddRange(Playout(state, MaxMoves, RequiredPoints));
            }

            return newResult.Distinct();
        }
    }
}
