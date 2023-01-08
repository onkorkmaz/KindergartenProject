using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KindergartenProject
{
    public class CommonUIFunction
    {
        public static List<SeasonEntity> GetSeasonList(int year)
        {
            List<SeasonEntity> monthList = new List<SeasonEntity>();
            monthList.Add(new SeasonEntity(0, year, 9, "Eylül"));
            monthList.Add(new SeasonEntity(1, year, 10, "Ekim"));
            monthList.Add(new SeasonEntity(2, year, 11, "Kasım"));
            monthList.Add(new SeasonEntity(3, year, 12, "Aralık"));
            monthList.Add(new SeasonEntity(4, year + 1, 1, "Ocak"));
            monthList.Add(new SeasonEntity(5, year + 1, 2, "Şubat"));
            monthList.Add(new SeasonEntity(6, year + 1, 3, "Mart"));
            monthList.Add(new SeasonEntity(7, year + 1, 4, "Nisan"));
            monthList.Add(new SeasonEntity(8, year + 1, 5, "Mayıs"));
            monthList.Add(new SeasonEntity(9, year + 1, 6, "Haziran"));
            monthList.Add(new SeasonEntity(10, year + 1, 7, "Temmuz"));
            monthList.Add(new SeasonEntity(11, year + 1, 8, "Ağustus"));

            return monthList;
        }

    }
}