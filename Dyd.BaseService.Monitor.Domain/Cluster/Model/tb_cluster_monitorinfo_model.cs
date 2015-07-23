using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dyd.BaseService.Monitor.Domain.Cluster.Model
{
    public partial class tb_cluster_monitorinfo_model
    {
        public string serverip { get; set; }
        public string servername { get; set; }
    }

    public class MonitorInfoModel
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public double MonitorValue { get; set; }
    }
}
