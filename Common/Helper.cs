using System;

namespace Common
{
    public class Helper
    {
        public static int ToInt32(object obje)
        {
            if (obje == null)
                return 0;

            int current = 0;

            bool isValid =  Int32.TryParse(obje.ToString(), out current);
            if (isValid)
                return current;
            else
                return 0;
        }

        public static decimal ToDecimal(object obje)
        {
            if (obje == null)
                return 0;

            decimal current = 0;

            bool isValid = decimal.TryParse(obje.ToString(), out current);
            if (isValid)
                return current;
            else
                return 0;
        }
    }
}
