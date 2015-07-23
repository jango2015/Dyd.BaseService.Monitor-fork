using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dyd.BaseService.Monitor.Domain.PlatformManage.Model
{
    public partial class tb_cluster_model
    {
        public string MonitorJson { get; set; }
    }

    public class MonitorInfoModel
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public string MonitorValue { get; set; }
    }
}
