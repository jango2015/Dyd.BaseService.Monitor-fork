using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Dyd.BaseService.Monitor.Domain.TimeWatch.Model
{
    /// <summary>
    /// tb_sqlhash_url_contrast Data Structure.
    /// </summary>
    [Serializable]
    public partial class tb_sqlhash_url_contrast_model
    {
	/*�����Զ����ɹ����Զ�����,��Ҫ������д�Լ��Ĵ��룬����ᱻ�Զ�����Ŷ - ����*/
        
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int sqlhash { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string url { get; set; }
        
    }
}