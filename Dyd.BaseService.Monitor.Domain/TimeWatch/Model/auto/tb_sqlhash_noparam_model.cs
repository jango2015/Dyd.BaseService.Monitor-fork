using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Dyd.BaseService.Monitor.Domain.TimeWatch.Model
{
    /// <summary>
    /// tb_sqlhash_noparam Data Structure.
    /// </summary>
    [Serializable]
    public partial class tb_sqlhash_noparam_model
    {
	/*�����Զ����ɹ����Զ�����,��Ҫ������д�Լ��Ĵ��룬����ᱻ�Զ�����Ŷ - ����*/
        
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string sqlhash { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string sql { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int count { get; set; }
        
    }
}