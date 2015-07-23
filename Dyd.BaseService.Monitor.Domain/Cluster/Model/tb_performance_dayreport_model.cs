using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Dyd.BaseService.Monitor.Domain.Cluster.Model
{

    public partial class tb_performance_dayreport_model
    {
        public string serverip { get; set; }
        public string servername { get; set; }
    }
}