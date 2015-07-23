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
	/*�����Զ����ɹ����Զ�����,��Ҫ������д�Լ��Ĵ��룬����ᱻ�Զ�����Ŷ - ����*/
        
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        
        /// <summary>
        /// ����
        /// </summary>
        public DateTime date { get; set; }
        
        /// <summary>
        /// ������id
        /// </summary>
        public int serverid { get; set; }
        
        /// <summary>
        /// ƽ��cpu
        /// </summary>
        public decimal avgcpu { get; set; }
        
        /// <summary>
        /// ���cpu
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
        /// д����
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
        /// �ϴ�ɨ�����id
        /// </summary>
        public int lastmaxid { get; set; }
        
        /// <summary>
        /// �ϴ�ɨ��ʱ��
        /// </summary>
        public DateTime lastupdatetime { get; set; }
        
    }
}