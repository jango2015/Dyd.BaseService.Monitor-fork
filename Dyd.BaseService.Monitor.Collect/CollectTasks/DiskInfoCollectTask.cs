using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Dyd.BaseService.Monitor.Core;
using XXF.Extensions;

namespace Dyd.BaseService.Monitor.Collect.CollectTasks
{
    public class DiskInfoCollectTask : BaseCollectTask, ICollect<List<HardDiskInfo>>
    {
        public DiskInfoCollectTask()
        {
            this.Name = "磁盘使用信息";
        }
        public List<HardDiskInfo> Collect()
        {
            if (IsCollect == false)
                return null;
            try
            {
                return GetAllHardDiskInfo();
            }
            catch (Exception exp)
            {
                throw new Exception(string.Format("服务器[id:{0}] ",GlobalConfig.ServerIP.NullToEmpty()) + this.Name + "采集出错");
            }
        }

        /// <summary>
        /// 根据盘符获取磁盘信息
        /// </summary>
        /// <param name="diskName"></param>
        /// <returns>一个自定义类对象</returns>
        public HardDiskInfo GetHardDiskInfoByName(string diskName)
        {
            DriveInfo drive = new DriveInfo(diskName);         
            return new HardDiskInfo { FreeSpace = GetDriveData(drive.AvailableFreeSpace),TotalSpace =GetDriveData (drive.TotalSize  ),Name =drive.Name  };
        }
        /// <summary>
        /// 获取所有驱动盘信息
        /// </summary>
        /// <returns></returns>
        public List<HardDiskInfo> GetAllHardDiskInfo()
        {
            List<HardDiskInfo> list = new List<HardDiskInfo>();
            foreach (DriveInfo d in DriveInfo.GetDrives())
            {
                if (d.IsReady)
                {
                    list.Add(new HardDiskInfo { Name = d.Name, FreeSpace = this.GetDriveData(d.AvailableFreeSpace), TotalSpace = this.GetDriveData(d.TotalSize) });
                }
            }
            return list;
        }
   
        private double GetDriveData(long data)//将磁盘大小的单位由byte转化为G
        {
            return Convert.ToDouble( (data / Convert.ToDouble(1024) / Convert.ToDouble(1024) / Convert.ToDouble(1024)));
        }
   
        
    
    }

    public class HardDiskInfo//自定义类
    {
        public string Name { get; set; }
        public double FreeSpace { get; set; }
        public double TotalSpace { get; set; }
    }
}
