using Dyd.BaseService.Monitor.Domain.TimeWatch.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dyd.BaseService.Monitor.Domain.Chart.Model
{
    public class ChartDataModel
    {
        public List<Dictionary<string,object>> GeneralData { get; set; }
        public string CategoryKey { get; set; }
        public string GroupKey { get; set; }
        public List<Dictionary<string, object>> AllData = new List<Dictionary<string, object>>();
    }

    public class ChartModel
    {
        public string Tkey { get; set; }
        public decimal Avalue { get; set; }
        public decimal Bvalue { get; set; }
        public string Title { get; set; }
        public decimal AsubB { get; set; }
        public int UpDown { get; set; }
    }

    public class ChartMainModel
    {
        public string Tkey { get; set; }
        public decimal Tvalue { get; set; }
    }

    public class ChartSubModel
    {
        public string Tkey { get; set; }
        public decimal Tvalue { get; set; }
    }
}
