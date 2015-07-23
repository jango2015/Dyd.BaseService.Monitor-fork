using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Dyd.BaseService.Monitor.Domain.PlatformManage.Model
{
    /// <summary>
    /// tb_user Data Structure.
    /// </summary>
    [Serializable]
    public partial class tb_user_model
    {
	/*�����Զ����ɹ����Զ�����,��Ҫ������д�Լ��Ĵ��룬����ᱻ�Զ�����Ŷ - ����*/
        
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        
        /// <summary>
        /// �û�����
        /// </summary>
        public string userstaffno { get; set; }
        
        /// <summary>
        /// �û���
        /// </summary>
        public string username { get; set; }
        
        /// <summary>
        /// �û���ɫ
        /// </summary>
        public Byte userrole { get; set; }
        
        /// <summary>
        /// �û�����ʱ��
        /// </summary>
        public DateTime usercreatetime { get; set; }
        
        /// <summary>
        /// �û��ֻ�����
        /// </summary>
        public string usertel { get; set; }
        
        /// <summary>
        /// �û�email
        /// </summary>
        public string useremail { get; set; }
        
        /// <summary>
        /// ��һ�δ�����ʱ��
        /// </summary>
        public DateTime lastsenderrortime { get; set; }
        
        /// <summary>
        /// ������ʱ��������λ:���ӣ�
        /// </summary>
        public int errorsendintervaltime { get; set; }
        
    }
}