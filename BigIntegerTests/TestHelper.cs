using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigIntegerTests
{
    public class TestHelper
    {
        private static int randSeed = 999;
        public static int NUM_OF_TIMES_FOR_RANDOM = 999;

        public static string GenRandNumStr(int numOfDigit)
        {
            if (numOfDigit <= 0) return "0";

            var rand = new Random(randSeed);
            randSeed = rand.Next();
            string str = "";
            if (rand.Next(100) > 50)
                 str = "-";
            str += rand.Next(1, 9).ToString();
            for (int i = 1; i < numOfDigit - 1; i++)
            {
                str += rand.Next(9);
            }
            return str;
        }

        public static int CompareNumStr(string val1, string val2)
        {
            bool isVal1Pos = true, isVal2Pos = true;
            if (val1[0] == '-')
            {
                isVal1Pos = false;
                val1 = val1.Substring(1);
            }

            if (val2[0] == '-')
            {
                isVal2Pos = false;
                val2 = val2.Substring(1);
            }

            if (isVal1Pos != isVal2Pos)
                return isVal1Pos ? 1 : -1;


            if (val1.Length != val2.Length)
            {
                return (val1.Length > val2.Length) == isVal1Pos ? 1 : -1;
            }
            else
            {
                for (int i = 0; i < val1.Length; i++)
                {
                    int c1 = Convert.ToInt32(val1[i]),
                        c2 = Convert.ToInt32(val2[i]);
                    if (c1 > c2)
                        return isVal1Pos ? 1 : -1;
                    else if (c1 < c2)
                        return isVal1Pos ? -1 : 1;
                }
            }

            return 0;
        }
    }
}