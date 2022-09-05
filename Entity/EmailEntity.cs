using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public class EmailEntity
    {
    }

    public class SeasonEntity
    {
        public SeasonEntity(int id,int year,int month,string monthName)
        {
            Id = id;
            Year = year;
            Month = month;
            MonthName = monthName;
        }
        public int Id { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }
    }
}
