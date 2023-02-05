﻿using System;
using System.Globalization;

namespace Common
{
    public class CommonFunctions
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
                try
                {
                    return (T)ChangeType<T>(data, typeof(T));
                }
                catch (Exception e)
                {
                    return default(T);
                }
                
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

        public static bool IsSameStrings(string text1, string text2)
        {
            if(!string.IsNullOrEmpty(text1) && !string.IsNullOrEmpty(text2))
            {
                string val1 = ReplaceTurkishChar(text1.Trim().ToLower());
                string val2 = ReplaceTurkishChar(text2.Trim().ToLower());
                return val1 == val2;
            }

            return false;
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

        public static string ToDateWithCulture(DateTime? date)
        {
            if (date.HasValue && date.Value > DateTime.MinValue && date.Value < DateTime.MaxValue)
            {
                CultureInfo trCulture = new CultureInfo("tr-TR");
                return "" + date.Value.Day.ToString().PadLeft(2,'0') + "/" + date.Value.Month.ToString().PadLeft(2, '0') + "/" + date.Value.Year;
            }

            return "";
        }

        public static string GetProjectType(ProjectType projectType)
        {
            switch (projectType)
            {
                case ProjectType.None:
                    return "None";
                case ProjectType.BenimDuntamKinderGartenSezenSokak:
                    return "Benim Dünyam Anaokulları";
                case ProjectType.BenimDunyamEgitimMerkeziİstiklalCaddesi:
                    return "Benim Dünyam Eğitim Merkezi";
                default:
                    return "Default";
            }
        }
    }
}