using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Dyd.BaseService.Monitor.Domain.TimeWatch.Model
{
    /// <summary>
    /// tb_timewatchlog_sql_dayreport Data Structure.
    /// </summary>
    [Serializable]
    public partial class tb_timewatchlog_sql_dayreport_model
    {
	/*�����Զ����ɹ����Զ�����,��Ҫ������д�Լ��Ĵ��룬����ᱻ�Զ�����Ŷ - ����*/
        
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        
        /// <summary>
        /// sql ��ϣ
        /// </summary>
        public string sqlhash { get; set; }
        
        /// <summary>
        /// ����
        /// </summary>
        public DateTime date { get; set; }
        
        /// <summary>
        /// ƽ����ʱ
        /// </summary>
        public double avgtime { get; set; }
        
        /// <summary>
        /// ����ʱ
        /// </summary>
        public double maxtime { get; set; }
        
        /// <summary>
        /// ��С��ʱ
        /// </summary>
        public double mintime { get; set; }
        
        /// <summary>
        /// �ϴ�ɨ�����id
        /// </summary>
        public int lastmaxid { get; set; }
        
        /// <summary>
        /// �ϴ�ɨ�����ʱ��
        /// </summary>
        public DateTime lastupdatetime { get; set; }
        
        /// <summary>
        /// ���ô���
        /// </summary>
        public int count { get; set; }
        
    }
}