using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Dyd.BaseService.Monitor.Domain.PlatformManage.Model
{
    /// <summary>
    /// tb_keyvalue_config Data Structure.
    /// </summary>
    [Serializable]
    public partial class tb_keyvalue_config_model
    {
	/*代码自动生成工具自动生成,不要在这里写自己的代码，否则会被自动覆盖哦 - 车毅*/
        
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string key { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string remark { get; set; }
    }
}