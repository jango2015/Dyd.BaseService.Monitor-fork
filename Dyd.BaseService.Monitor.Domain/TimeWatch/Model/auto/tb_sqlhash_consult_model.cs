using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Dyd.BaseService.Monitor.Domain.TimeWatch.Model
{
    /// <summary>
    /// tb_sqlhash_consult Data Structure.
    /// </summary>
    [Serializable]
    public partial class tb_sqlhash_consult_model
    {
	/*代码自动生成工具自动生成,不要在这里写自己的代码，否则会被自动覆盖哦 - 车毅*/
        
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        
        /// <summary>
        /// sql 哈希
        /// </summary>
        public string sqlhash { get; set; }
        
        /// <summary>
        /// sql 代码
        /// </summary>
        public string sql { get; set; }

        /// <summary>
        /// sql 执行次数
        /// </summary>
        public int count { get; set; }
        
    }
}