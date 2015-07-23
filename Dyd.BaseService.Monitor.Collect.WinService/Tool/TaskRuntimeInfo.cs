using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dyd.BaseService.Monitor.Domain.PlatformManage.Model;
using XXF.BaseService.Monitor;
using XXF.BaseService.Monitor.SystemRuntime;

namespace Dyd.BaseService.Monitor.Collect.WinService.Tool
{
    public class TaskRuntimeInfo
    {
        /// <summary>
        /// 任务所在的应用程序域
        /// </summary>
        public AppDomain Domain;
        /// <summary>
        /// 任务当前版本信息
        /// </summary>
        public tb_cluster_collect_version_model TaskVersionModel;
        /// <summary>
        /// 应用程序域中任务dll实例引用
        /// </summary>
        public BaseCollectMonitorDll DllTask;
    }

}
