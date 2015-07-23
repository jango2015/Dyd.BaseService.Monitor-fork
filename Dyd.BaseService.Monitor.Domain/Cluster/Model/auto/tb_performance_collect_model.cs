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
	/*�����Զ����ɹ����Զ�����,��Ҫ������д�Լ��Ĵ��룬����ᱻ�Զ�����Ŷ - ����*/
        
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        
        /// <summary>
        /// ������id
        /// </summary>
        public int serverid { get; set; }
        
        /// <summary>
        /// cpu��Ϣ
        /// </summary>
        public decimal cpu { get; set; }
        
        /// <summary>
        /// �ڴ��ֽ�
        /// </summary>
        public decimal memory { get; set; }
        
        /// <summary>
        /// �����ϴ��ֽ�
        /// </summary>
        public decimal networkupload { get; set; }
        
        /// <summary>
        /// ���������ֽ�
        /// </summary>
        public decimal networkdownload { get; set; }
        
        /// <summary>
        /// io���ֽ�
        /// </summary>
        public decimal ioread { get; set; }
        
        /// <summary>
        /// ioд�ֽ�
        /// </summary>
        public decimal iowrite { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public decimal iisrequest { get; set; }
        
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime createtime { get; set; }
        
    }
}