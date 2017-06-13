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
        //// assumption: to != from
        //protected static char GetDirection(Position from, Position to)
        //{
        //    /**
        //     * on the space Col_Diff*Row_Diff*Direction would like to connect:
        //     * "(to.Col - from.Col, to.Row - from.Row, direction)
        //     * (-1,0,0)
        //     * (1,0,1)
        //     * (0,-1,2)
        //     * (0,1,3)
        //     * using Trilinear interpolation will get the function:
        //     *              f(x,y) = (x + 1 + 5 * y * y + y) / 2 = (5 * y * y + y + x + 1) / 2
        //     *
        //     * I basically just computed linear line between(x,z): (-1,0),(1,1)
        //     * and then I needed to use the Y values to "get in the game" under the need that when y=0 the
        //     * X values will set Z. So I used newton interpolation betwwn(y,z): (0,0),(-1,2),(1,3).
        //     * (Note: I should have done the x,z interpolation with (0,0) also, but it worked without it).
        //     *
        //     * (x,z) computation:
        //     * m = (z1-z0)/(x1-x0) = (1-0)/(1--1) = 1/2
        //     * z = (x--1)/2 = (x+1)/2
        //     *
        //     * (y,z) computation:
        //     *  0  0
        //     *          -2
        //     * -1  2           2.5
        //     *          1/2
        //     *  1  3
        //     *  we got: z = 0 - 2 * (y - 0) + 2.5 * (y - 0) * (y - -1) = -2y + 2.5*y*y + 2.5y = 2.5*y*y + y/2
        //     *          z = (5*y*y + y)/2
        //     */
        //    int y = to.Row - from.Row;
        //    int x = to.Col - from.Col;
        //    // The ASCII value of '0' is 48. So I inserted it into the brackets.
        //    return (char) ((5 * y * y + y + x + 97) >> 1);
        //}

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
