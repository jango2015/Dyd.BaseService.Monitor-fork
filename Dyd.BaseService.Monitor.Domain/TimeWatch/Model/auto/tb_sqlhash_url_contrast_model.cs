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
	/*代码自动生成工具自动生成,不要在这里写自己的代码，否则会被自动覆盖哦 - 车毅*/
        
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