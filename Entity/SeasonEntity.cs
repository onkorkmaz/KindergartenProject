using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public class SeasonEntity : BaseEntity
    {
        public SeasonEntity(int id,int year,int month,string monthName)
        {
            Id = id;
            Year = year;
            Month = month;
            MonthName = monthName;
        }
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }
    }
}
