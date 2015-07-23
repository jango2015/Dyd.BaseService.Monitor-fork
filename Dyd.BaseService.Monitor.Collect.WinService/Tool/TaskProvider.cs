using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dyd.BaseService.Monitor.Core;
using Dyd.BaseService.Monitor.Domain.PlatformManage.Dal;
using XXF.BaseService.Monitor;
using XXF.BaseService.Monitor.SystemRuntime;
using XXF.ProjectTool;

namespace Dyd.BaseService.Monitor.Collect.WinService.Tool
{
    /// <summary>
    /// 任务操作提供者
    /// 提供任务的开始，关闭,重启，卸载
    /// </summary>
    public class TaskProvider
    {
        private static object _locktag = new object();
        TaskRuntimeInfo TaskRunTimeInfo = null;
        public bool IsStart()
        {
            lock (_locktag)
            {
                if (TaskRunTimeInfo != null)
                    return true;
                else
                    return false;
            }
        }
        /// <summary>
        /// 任务的开启
        /// </summary>
        /// <param name="taskid"></param>
        /// <returns></returns>
        public bool Start()
        {
            lock (_locktag)
            {
                if (TaskRunTimeInfo != null)
                    throw new Exception("采集任务已启动，启动失败");
                TaskRunTimeInfo = new TaskRuntimeInfo();
                SqlHelper.ExcuteSql(CoreGlobalConfig.PlatformManageConnectString, (c) =>
                {
                    tb_cluster_collect_version_dal versiondal = new tb_cluster_collect_version_dal();
                    TaskRunTimeInfo.TaskVersionModel = versiondal.GetCurrentVersion(c);
                });
                string filelocalcachepath = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\" + GlobalConfig.TaskDllCompressFileCacheDir + @"\" + TaskRunTimeInfo.TaskVersionModel.versionnum + @"\" +
                    TaskRunTimeInfo.TaskVersionModel.zipfilename;
                string fileinstallpath = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\" + GlobalConfig.TaskDllDir + @"\";
                string fileinstallmainclassdllpath = fileinstallpath + @"\" + "Dyd.BaseService.Monitor.Collect.exe";

                XXF.Common.IOHelper.CreateDirectory(filelocalcachepath);
                XXF.Common.IOHelper.CreateDirectory(fileinstallpath);
                System.IO.File.WriteAllBytes(filelocalcachepath, TaskRunTimeInfo.TaskVersionModel.zipfile);

                CompressHelper.UnCompress(filelocalcachepath, fileinstallpath);

                try
                {
                    var dlltask = new AppDomainLoader<BaseCollectMonitorDll>().Load(fileinstallmainclassdllpath, "Dyd.BaseService.Monitor.Collect.CollectMonitorDll", out TaskRunTimeInfo.Domain);
                    dlltask.PlatformManageConnectString = CoreGlobalConfig.PlatformManageConnectString;
                    dlltask.ServerIP = GlobalConfig.ServerIP;
                    dlltask.TryStart();
                    LogHelper.Log("采集任务启动成功");
                    return true;
                }
                catch (Exception exp)
                {
                    DisposeTask();
                    throw exp;
                }
            }
        }
        /// <summary>
        /// 任务的关闭
        /// </summary>
        /// <param name="taskid"></param>
        /// <returns></returns>
        public bool Stop()
        {
            lock (_locktag)
            {
                if (TaskRunTimeInfo == null)
                {
                    throw new Exception("任务不在运行中,停止失败");
                }

                var r = DisposeTask();

                LogHelper.Log("采集任务关闭成功");
                return r;
            }
        }

        /// <summary>
        /// 任务的资源释放
        /// </summary>
        /// <param name="taskid"></param>
        /// <param name="taskruntimeinfo"></param>
        /// <returns></returns>
        private bool DisposeTask()
        {
            if (TaskRunTimeInfo != null && TaskRunTimeInfo.DllTask != null)
                try { TaskRunTimeInfo.DllTask.Dispose(); TaskRunTimeInfo.DllTask = null; }
                catch (Exception e) { LogHelper.Error("强制资源释放之任务资源释放", e); }
            if (TaskRunTimeInfo != null && TaskRunTimeInfo.Domain != null)
                try { new AppDomainLoader<BaseCollectMonitorDll>().UnLoad(TaskRunTimeInfo.Domain); TaskRunTimeInfo.Domain = null; }
                catch (Exception e) { LogHelper.Error("强制资源释放之应用程序域释放", e); }
            TaskRunTimeInfo = null;
            LogHelper.Log("已对采集任务进行资源释放");
            return true;
        }
    }
}
