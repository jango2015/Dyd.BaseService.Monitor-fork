using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class TimeChartModel
    {
        public string Title="";
        public string SubTitle = "";
        public string YTitle = "";
        public DateTime StartDate{get;set;}
        public DateTime EndDate { get; set; }
        public string MaxZoom { get; set; }
        public string PointInterval { get; set; }
        public string DataJson = "";
        public string UnitText = "";
        public string Key = "";
        public TimeChartDataType DataType { get; set; }
        public TimeChartType Type { get; set; }
    }

    public enum TimeChartType
    {
        Day,
        Month,
        Year,
    }

    public enum TimeChartDataType
    {
        min,
        max,
        avg,
        count
    }
}