using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Dyd.BaseService.Monitor.Domain.PlatformManage.Model
{
    /// <summary>
    /// tb_cluster Data Structure.
    /// </summary>
    [Serializable]
    public partial class tb_cluster_model
    {
	/*代码自动生成工具自动生成,不要在这里写自己的代码，否则会被自动覆盖哦 - 车毅*/
        
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        
        /// <summary>
        /// 服务器名
        /// </summary>
        public string servername { get; set; }
        
        /// <summary>
        /// 服务器ip
        /// </summary>
        public string serverip { get; set; }
        
        /// <summary>
        /// 是否开启服务器监控
        /// </summary>
        public bool ifmonitor { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string performancecollectconfigjson { get; set; }
        
        /// <summary>
        /// 监控的配置json
        /// </summary>
        public string monitorcollectconfigjson { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createtime { get; set; }
        
        /// <summary>
        /// 上次更新时间
        /// </summary>
        public DateTime lastupdatetime { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime onlinetime { get; set; }
        
    }
}