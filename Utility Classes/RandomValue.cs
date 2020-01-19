using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chaze.Utility_Classes
{
    class RandomValue
    {
        //Random value
        private static Random randomValue = new Random();



        public static double GetRandomValue(double max, double min)
        {

            return (randomValue.NextDouble() * (max - min)) + min;

        }
    }
}
