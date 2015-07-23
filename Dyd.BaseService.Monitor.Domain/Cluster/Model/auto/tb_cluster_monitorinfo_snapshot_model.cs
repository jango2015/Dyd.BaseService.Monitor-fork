using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Dyd.BaseService.Monitor.Domain.Cluster.Model
{
    /// <summary>
    /// tb_cluster_monitorinfo_snapshot Data Structure.
    /// </summary>
    [Serializable]
    public partial class tb_cluster_monitorinfo_snapshot_model
    {
	/*�����Զ����ɹ����Զ�����,��Ҫ������д�Լ��Ĵ��룬����ᱻ�Զ�����Ŷ - ����*/
        
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int serverid { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string monitorinfojson { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime lastupdatetime { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime createtime { get; set; }
        
    }
}