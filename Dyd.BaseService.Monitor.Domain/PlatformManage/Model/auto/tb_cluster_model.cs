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
	/*�����Զ����ɹ����Զ�����,��Ҫ������д�Լ��Ĵ��룬����ᱻ�Զ�����Ŷ - ����*/
        
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        
        /// <summary>
        /// ��������
        /// </summary>
        public string servername { get; set; }
        
        /// <summary>
        /// ������ip
        /// </summary>
        public string serverip { get; set; }
        
        /// <summary>
        /// �Ƿ������������
        /// </summary>
        public bool ifmonitor { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string performancecollectconfigjson { get; set; }
        
        /// <summary>
        /// ��ص�����json
        /// </summary>
        public string monitorcollectconfigjson { get; set; }
        
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime createtime { get; set; }
        
        /// <summary>
        /// �ϴθ���ʱ��
        /// </summary>
        public DateTime lastupdatetime { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime onlinetime { get; set; }
        
    }
}