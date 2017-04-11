using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SearchAlgorithmsLib;
using MazeLib;

namespace Server
{
    public class Converter
    {
        public static string ToJSON(Solution<Position> solution)
        {
            StringBuilder builder = new StringBuilder("");
            Position currentPosition = solution[0].GetState();
            Position nextPosition;
            for (int i = 1; i < solution.Size(); i++)
            {
                nextPosition = solution[i].GetState();
                if (currentPosition.Col + 1 == nextPosition.Col)
                {
                    //Right
                    builder.Append('1');
                } else if (currentPosition.Col == nextPosition.Col + 1)
                {
                    //Left.
                    builder.Append('0');
                } else if (currentPosition.Row + 1 == nextPosition.Row)
                {
                    // TODO - Check if + 1 is Really Down.
                    builder.Append('3');
                } else if ( currentPosition.Row == nextPosition.Row + 1)
                {
                    //Up
                    builder.Append('2');
                }
                currentPosition = nextPosition;
            }
            return builder.ToString();

        }

        public static Direction StringToDirection(string direction)
        {
            switch (direction)
            {
                case "up":
                    {
                        return Direction.Up;
                    }
                case "down":
                    {
                        return Direction.Down;
                    }
                case "left":
                    {
                        return Direction.Left;
                    }
                case "right":
                    {
                        return Direction.Right;
                    }
                default:
                    return Direction.Unknown;
            }
        }
    }
}
