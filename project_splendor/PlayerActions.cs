using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_splendor
{
    public interface IPlayerAction
    {
        bool IsAvailable(PlayerState player);
        IEnumerable<PlayerState> Execute(PlayerState player);
    }

    public abstract class TakeAction : IPlayerAction
    {
        public IEnumerable<PlayerState> CheckAfter(PlayerState player)
        {
            if (player.Stash.Sum(s => s.Value) <= 10)
                return new List<PlayerState>() { player };
            else
            {
                List<PlayerState> firstPassResults = new List<PlayerState>();
                foreach (Color clr in Enum.GetValues(typeof(Color)))
                    if (player.Stash[clr] > 0)
                    {
                        var newState = new PlayerState(player);
                        newState.Stash[clr] = newState.Stash[clr] - 1;
                        newState.Log[newState.Log.Count - 1] = newState.Log.Last() + $", return {clr}";
                        firstPassResults.Add(newState);
                    }
                List<PlayerState> secondPassResults = new List<PlayerState>();
                foreach (var result in firstPassResults)
                {
                    secondPassResults.AddRange(CheckAfter(result));
                }

                return secondPassResults.Distinct().ToList();
            }
        }

        public abstract IEnumerable<PlayerState> Execute(PlayerState player);

        public abstract bool IsAvailable(PlayerState player);
    }

    public class TakeTwoAction : TakeAction
    {
        public Color Color { get; }

        public TakeTwoAction(Color color)
        {
            Color = color;
        }

        public override IEnumerable<PlayerState> Execute(PlayerState player)
        {
            player.Log.Add($"Take 2 {Color}");
            player.Stash[Color] = player.Stash[Color] + 2;
            player.MovesCount++;
            return CheckAfter(player);
        }

        public override bool IsAvailable(PlayerState player)
        {
            return player.Stash[Color] < 4;
        }
    }

    public class TakeThreeAction : TakeAction
    {
        public Color Color1 { get; }
        public Color Color2 { get; }
        public Color Color3 { get; }

        public TakeThreeAction(Color color1, Color color2, Color color3)
        {
            Color1 = color1;
            Color2 = color2;
            Color3 = color3;
        }

        public override IEnumerable<PlayerState> Execute(PlayerState player)
        {
            StringBuilder sb = new StringBuilder("Take ");
            if (player.Stash[Color1] < 7)
            {
                player.Stash[Color1] = player.Stash[Color1] + 1;
                sb.Append($" 1{Color1},");
            }
            if (player.Stash[Color2] < 7)
            {
                player.Stash[Color2] = player.Stash[Color2] + 1;
                sb.Append($" 1{Color2},");
            }

            if (player.Stash[Color3] < 7)
            {
                player.Stash[Color3] = player.Stash[Color3] + 1;
                sb.Append($" 1{Color3},");
            }
            sb.Remove(sb.Length - 1, 1);
            player.Log.Add(sb.ToString());

            player.MovesCount++;
            return CheckAfter(player);
        }

        public override bool IsAvailable(PlayerState player)
        {
            return (player.Stash.Sum(s => s.Value) < 10) &&
                (player.Stash[Color1] < 7 || player.Stash[Color2] < 7 || player.Stash[Color3] < 7);
        }
    }

    public class TakeCardAction : IPlayerAction
    {
        public TakeCardAction(Card card)
        {
            Card = card;
        }
        public Card Card { get; }

        public void CheckAfter(PlayerState player)
        {
            foreach (Noble noble in Game.Nobles)
            {
                if (!player.TakenNobles.Contains(noble))
                {
                    foreach (var req in noble.Requirements)
                    {
                        if (req.Value <= player.TakenCards.Where(c => c.Color == req.Key).Count())
                        {
                            player.TakenNobles.Add(noble);
                        }
                    }
                }
            }
        }

        public IEnumerable<PlayerState> Execute(PlayerState player)
        {
            StringBuilder sb = new StringBuilder($"Taken {Card.Color} {Card.Points} card");
            foreach (var cost in Card.Cost)
            {
                sb.Append($" for {cost.Value} {cost.Key},");
                if (player.TakenCards.Where(c => c.Color == cost.Key).Count() < cost.Value)
                    player.Stash[cost.Key] = player.Stash[cost.Key] - cost.Value + player.TakenCards.Where(c => c.Color == cost.Key).Count();
            }
            sb.Remove(sb.Length - 1, 1);
            player.TakenCards.Add(Card);
            player.Log.Add(sb.ToString());

            return new List<PlayerState>() { player };
        }

        public bool IsAvailable(PlayerState player)
        {
            if (player.TakenCards.Contains(Card))
                return false;

            foreach (var cost in Card.Cost)
            {
                if (cost.Value > (player.TakenCards.Where(c => c.Color == cost.Key).Count() + player.Stash[cost.Key]))
                    return false;
            }
            return true;
        }
    }
}
