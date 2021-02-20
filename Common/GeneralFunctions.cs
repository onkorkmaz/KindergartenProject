using System;

namespace Common
{
    public class GeneralFunctions
    {
        public static string ToString(object obje)
        {
            if (obje == null)
                return "";
            else
                return obje.ToString();
        }
        public static T GetData<T>(object data)
        {
            if (data == null || data == DBNull.Value)
            {
                return default(T);
            }
            else
            {
                return (T)ChangeType<T>(data, typeof(T));
            }
        }
        private static T ChangeType<T>(object value, Type t)
        {
            if (t.IsGenericType &&
                t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null) { return default(T); }
                t = Nullable.GetUnderlyingType(t); ;
            }
            if (value != null && string.IsNullOrEmpty(value.ToString()))
                return default(T);
            return (T)Convert.ChangeType(value, t);
        }

        public static string ValidateTextBoxtIseEmptyOrNull(string text, string errorText)
        {
            string errorMessage = "";
            if (string.IsNullOrEmpty(text))
            {
                errorMessage += errorText + "<br/>";
            }
            return errorMessage;
        }

        public static string ReplaceTurkishChar(string text)
        {
            text = text.Replace("İ", "I");
            text = text.Replace("ı", "i");
            text = text.Replace("Ğ", "G");
            text = text.Replace("ğ", "g");
            text = text.Replace("Ö", "O");
            text = text.Replace("ö", "o");
            text = text.Replace("Ü", "U");
            text = text.Replace("ü", "u");
            text = text.Replace("Ş", "S");
            text = text.Replace("ş", "s");
            text = text.Replace("Ç", "C");
            text = text.Replace("ç", "c");
            text = text.Replace(" ", "_");
            return text;
        }

    }
}