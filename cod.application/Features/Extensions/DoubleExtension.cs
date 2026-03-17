using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Features.Extensions
{
    public static class DoubleExtension
    {
        public static bool IsValid(this double value)
        {
            return value > 0;
        }

        public static bool OnlyPositiveNumbers(this double value)
        {
            return value > 0;
        }

        public static bool OnlyPositiveNumbersWithZero(this double value)
        {
            return value >= 0;
        }

        public static double RoundTwo(this double number)
        {
            return Math.Round(number, 2);
        }

        public static string ToCurrency(this double value)
        {
            value = value.RoundTwo();
            return string.Concat("L", " ", value.ToString("N"));
        }
    }
}
