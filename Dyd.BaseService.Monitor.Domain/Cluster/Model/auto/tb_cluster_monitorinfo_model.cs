using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Dyd.BaseService.Monitor.Domain.Cluster.Model
{
    /// <summary>
    /// tb_cluster_monitorinfo Data Structure.
    /// </summary>
    [Serializable]
    public partial class tb_cluster_monitorinfo_model
    {
	/*代码自动生成工具自动生成,不要在这里写自己的代码，否则会被自动覆盖哦 - 车毅*/
        
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        
        /// <summary>
        /// 服务器id
        /// </summary>
        public int serverid { get; set; }
        
        /// <summary>
        /// 服务器监控得到的各项指标信息
        /// </summary>
        public string monitorinfojson { get; set; }
        
        /// <summary>
        /// 上次服务器更新时间
        /// </summary>
        public DateTime lastupdatetime { get; set; }
        
    }
}