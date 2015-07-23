using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Dyd.BaseService.Monitor.Domain.TimeWatch.Model
{
    /// <summary>
    /// tb_timewatchlog_api_dayreport Data Structure.
    /// </summary>
    [Serializable]
    public partial class tb_timewatchlog_api_dayreport_model
    {
	/*代码自动生成工具自动生成,不要在这里写自己的代码，否则会被自动覆盖哦 - 车毅*/
        
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        
        /// <summary>
        /// 接口api url
        /// </summary>
        public string url { get; set; }
        
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime date { get; set; }
        
        /// <summary>
        /// 平均耗时
        /// </summary>
        public double avgtime { get; set; }
        
        /// <summary>
        /// 最大耗时
        /// </summary>
        public double maxtime { get; set; }
        
        /// <summary>
        /// 最小耗时
        /// </summary>
        public double mintime { get; set; }
        
        /// <summary>
        /// 上一次扫描最大id
        /// </summary>
        public int lastmaxid { get; set; }
        
        /// <summary>
        /// 上次扫描更新时间
        /// </summary>
        public DateTime lastupdatetime { get; set; }
        
        /// <summary>
        /// 调用次数
        /// </summary>
        public int count { get; set; }
        
    }
}