using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dyd.BaseService.Monitor.Core
{
    public class ClusterMonitorInfo 
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public double MonitorValue { get; set; }

        public ClusterMonitorInfo()
        {}

        public ClusterMonitorInfo(string name, string text,  double monitorValue)
        {
            Name = name;
            Text = text;
            MonitorValue = monitorValue;
        }

    }
}
