using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace project_splendor
{
    public enum Color
    {
        Black = 1,
        Red = 2,
        Green = 4,
        Blue = 8,
        White = 16
    }

    public class Card
    {
        public Card(Color color, int points, int black, int white, int red, int blue, int green)
        {
            Color = color;
            Points = points;
            Cost = new Dictionary<Color, int>();
            if (black > 0)
                Cost.Add(Color.Black, black);
            if (red > 0)
                Cost.Add(Color.Red, red);
            if (green > 0)
                Cost.Add(Color.Green, green);
            if (blue > 0)
                Cost.Add(Color.Blue, blue);
            if (white > 0)
                Cost.Add(Color.White, white);
        }
        
        public Color Color { get; }
        public int Points { get; }
        public Dictionary<Color, int> Cost { get; }

        public override bool Equals(object obj)
        {
            var item = obj as Card;

            if (item == null)
            {
                return false;
            }

            return this.Color == item.Color &&
                this.Points == item.Points &&
                this.Cost[Color.Black] == item.Cost[Color.Black] &&
                this.Cost[Color.Red] == item.Cost[Color.Red] &&
                this.Cost[Color.Green] == item.Cost[Color.Green] &&
                this.Cost[Color.Blue] == item.Cost[Color.Blue] &&
                this.Cost[Color.White] == item.Cost[Color.White];
        }
    }

    public class Noble
    {
        public Noble(int black, int white, int red, int blue, int green)
        {
            Points = 3;
            Requirements = new Dictionary<Color, int>();
            if (black > 0)
                Requirements.Add(Color.Black, black);
            if (red > 0)
                Requirements.Add(Color.Red, red);
            if (green > 0)
                Requirements.Add(Color.Green, green);
            if (blue > 0)
                Requirements.Add(Color.Blue, blue);
            if (white > 0)
                Requirements.Add(Color.White, white);
        }
        public int Points { get; }
        public Dictionary<Color, int> Requirements { get; }

        public override bool Equals(object obj)
        {
            var item = obj as Noble;

            if (item == null)
            {
                return false;
            }

            return this.Requirements[Color.Black] == item.Requirements[Color.Black] &&
                this.Requirements[Color.Red] == item.Requirements[Color.Red] &&
                this.Requirements[Color.Green] == item.Requirements[Color.Green] &&
                this.Requirements[Color.Blue] == item.Requirements[Color.Blue] &&
                this.Requirements[Color.White] == item.Requirements[Color.White];
        }
    }
}
