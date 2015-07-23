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
	/*代码自动生成工具自动生成,不要在这里写自己的代码，否则会被自动覆盖哦 - 车毅*/
        
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        
        /// <summary>
        /// 用户工号
        /// </summary>
        public string userstaffno { get; set; }
        
        /// <summary>
        /// 用户名
        /// </summary>
        public string username { get; set; }
        
        /// <summary>
        /// 用户角色
        /// </summary>
        public Byte userrole { get; set; }
        
        /// <summary>
        /// 用户创建时间
        /// </summary>
        public DateTime usercreatetime { get; set; }
        
        /// <summary>
        /// 用户手机号码
        /// </summary>
        public string usertel { get; set; }
        
        /// <summary>
        /// 用户email
        /// </summary>
        public string useremail { get; set; }
        
        /// <summary>
        /// 上一次错误发送时间
        /// </summary>
        public DateTime lastsenderrortime { get; set; }
        
        /// <summary>
        /// 错误发送时间间隔（单位:分钟）
        /// </summary>
        public int errorsendintervaltime { get; set; }
        
    }
}