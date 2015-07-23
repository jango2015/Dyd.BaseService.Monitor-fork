using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Dyd.BaseService.Monitor.Domain.Cluster.Model
{
    /// <summary>
    /// tb_performance_collect Data Structure.
    /// </summary>
    [Serializable]
    public partial class tb_performance_collect_model
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
        /// cpu信息
        /// </summary>
        public decimal cpu { get; set; }
        
        /// <summary>
        /// 内存字节
        /// </summary>
        public decimal memory { get; set; }
        
        /// <summary>
        /// 网络上传字节
        /// </summary>
        public decimal networkupload { get; set; }
        
        /// <summary>
        /// 网络下载字节
        /// </summary>
        public decimal networkdownload { get; set; }
        
        /// <summary>
        /// io读字节
        /// </summary>
        public decimal ioread { get; set; }
        
        /// <summary>
        /// io写字节
        /// </summary>
        public decimal iowrite { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public decimal iisrequest { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createtime { get; set; }
        
    }
}