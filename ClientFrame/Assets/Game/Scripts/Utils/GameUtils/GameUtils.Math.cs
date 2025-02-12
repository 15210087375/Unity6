using System;
using System.Collections;
using System.Collections.Generic;

public static partial class GameUtils
{
    public static class Math
    {
        //重写四舍五入算法
        public static int RoundToInt(float floatValue)
        {
            var intValue = (int) System.Math.Round(floatValue, MidpointRounding.AwayFromZero);
            return intValue;
        }
        
        public static int Clamp(int value, int min, int max)
        {
            if (value < min)
            {
                return min;
            }

            if (value > max)
            {
                return max;
            }

            return value;
        }
        public static long Clamp(long value, long min, long max)
        {
            if (value < min)
            {
                return min;
            }

            if (value > max)
            {
                return max;
            }

            return value;
        }
    }
}