using SearchAlgorithmsLib;
using MazeLib;
using System;
using System.Collections.Generic;

namespace All
{
    /// <summary>
    /// The Converter class will help us handle the conversion between different
    /// string formats in our application, whether it is the JSON string format or
    /// any other format that we will use in the future.
    /// </summary>
    public class Converter
    {

        /// <summary>
        /// Convert a given solution to a SearchGame to a string in the
        /// JSON format.
        /// </summary>
        /// <param name="solution">
        /// The solution that we wish to convert.
        /// </param>
        /// <returns>
        /// The solution's string representation in JSON.
        /// </returns>
        public static string ToJSON(Solution<Position> solution)
        {
            int pos = 0;
            // -1 because we have one char for each "space" between states
            char[] str;
            Position currentPosition, nextPosition;
            IEnumerator<Position> states = solution.GetEnumerator();

            // for some reason the first thing is always null
            states.MoveNext();

            // try to take the first position of the Solution.
            try
            {
                currentPosition = states.Current;
                str = new char[solution.Length - 1];
            }
            catch
            {
                // The Solution is empty/null, return "".
                return "";
            }

           while(states.MoveNext())
            {
                nextPosition = states.Current;
                if (currentPosition.Col < nextPosition.Col)
                {
                    //Right.
                    str[pos] = '1';
                }
                else if (currentPosition.Col > nextPosition.Col)
                {
                    //Left.
                    str[pos] = '0';
                }
                else if (currentPosition.Row < nextPosition.Row)
                {
                    //Down.
                    str[pos] = '3';
                }
                else if (currentPosition.Row > nextPosition.Row)
                {
                    //Up
                    str[pos] = '2';
                }
                currentPosition = nextPosition;
                ++pos;
            }
            return new string(str);
        }

        /// <summary>
        /// Convert a given string to it's matching Direction.
        /// </summary>
        /// <param name="direction">
        /// <exception>
        /// In a case where the string doesn't match any direction.
        /// </exception>
        /// The string which we will convert.
        /// </param>
        /// <returns></returns>
        public static Direction StringToDirection(string direction)
        {
            // Easier to maintain. If Direction will change the code will not.
            try
            {
                return (Direction)Enum.Parse(typeof(Direction), direction, true);
            }
            catch
            {
                return Direction.Unknown;
            }
          
        }
    }
}
