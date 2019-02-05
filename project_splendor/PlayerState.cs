using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_splendor
{
    public class PlayerState
    {
        public PlayerState()
        {
            TakenCards = new List<Card>();
            TakenNobles = new List<Noble>();
            Stash = new Dictionary<Color, int>();
            Stash.Add(Color.Black, 0);
            Stash.Add(Color.Red, 0);
            Stash.Add(Color.Green, 0);
            Stash.Add(Color.Blue, 0);
            Stash.Add(Color.White, 0);
            Log = new List<string>();
            MovesCount = 0;
        }

        public PlayerState(List<Card> cards, List<Noble> nobles, Dictionary<Color, int> stash, int moves, List<string> log = null)
        {
            TakenCards = cards.ToList();
            TakenNobles = nobles.ToList();
            Stash = new Dictionary<Color, int>();
            foreach (KeyValuePair<Color, int> kvp in stash)
            {
                Stash.Add(kvp.Key, kvp.Value);
            }
            MovesCount = moves;
            Log = log.ToList();
        }

        public PlayerState(PlayerState player) : this(player.TakenCards, player.TakenNobles, player.Stash, player.MovesCount, player.Log)
        {
        }

        public int Score
        {
            get
            {
                return TakenCards.Sum(c => c.Points) + TakenNobles.Sum(n => n.Points);
            }
        }
        public List<Card> TakenCards { get; }
        public List<Noble> TakenNobles { get; }

        public int MovesCount { get; set; }
        public Dictionary<Color, int> Stash { get; }

        public override bool Equals(object obj)
        {
            var state = obj as PlayerState;

            if (state == null)
            {
                return false;
            }

            var result = this.Stash[Color.Black] == state.Stash[Color.Black] &&
                this.Stash[Color.Red] == state.Stash[Color.Red] &&
                this.Stash[Color.Green] == state.Stash[Color.Green] &&
                this.Stash[Color.Blue] == state.Stash[Color.Blue] &&
                this.Stash[Color.White] == state.Stash[Color.White] &&
                this.MovesCount == state.MovesCount &&
                this.TakenCards.Count == state.TakenCards.Count &&
                this.TakenCards.TrueForAll(c => state.TakenCards.Contains(c));

            return result;
        }

        public List<string> Log { get; }
    }
}
