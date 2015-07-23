using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Dyd.BaseService.Monitor.Domain.Cluster.Model
{
    /// <summary>
    /// tb_performance_dayreport Data Structure.
    /// </summary>
    [Serializable]
    public partial class tb_performance_dayreport_model
    {
	/*代码自动生成工具自动生成,不要在这里写自己的代码，否则会被自动覆盖哦 - 车毅*/
        
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime date { get; set; }
        
        /// <summary>
        /// 服务器id
        /// </summary>
        public int serverid { get; set; }
        
        /// <summary>
        /// 平均cpu
        /// </summary>
        public decimal avgcpu { get; set; }
        
        /// <summary>
        /// 最大cpu
        /// </summary>
        public decimal maxcpu { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public decimal mincpu { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public decimal avgmemory { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public decimal maxmemory { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public decimal minmemory { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public decimal avgnetworkupload { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public decimal maxnetworkupload { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public decimal minnetworkupload { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public decimal avgnetworkdownload { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public decimal maxnetworkdownload { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public decimal minnetworkdownload { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public decimal avgioread { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public decimal maxioread { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public decimal minioread { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public decimal avgiowrite { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public decimal maxiowrite { get; set; }
        
        /// <summary>
        /// 写磁盘
        /// </summary>
        public decimal miniowrite { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public decimal avgiisrequest { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public decimal maxiisrequest { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public decimal miniisrequest { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int count { get; set; }
        
        /// <summary>
        /// 上次扫描最大id
        /// </summary>
        public int lastmaxid { get; set; }
        
        /// <summary>
        /// 上次扫描时间
        /// </summary>
        public DateTime lastupdatetime { get; set; }
        
    }
}