using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dyd.BaseService.Monitor.Domain.Cluster.Dal;
using Dyd.BaseService.Monitor.Domain.Cluster.Model;
using Dyd.BaseService.Monitor.Domain.PlatformManage.Dal;
using Dyd.BaseService.Monitor.Domain.PlatformManage.Model;
using XXF.Db;
using XXF.Common;
using XXF.BaseService.Monitor.SystemRuntime;

namespace Dyd.BaseService.Monitor.Domain.PlatformManage.Dal
{
    public class PerformanceCollectDal
    {
        public static DateTime LastReserverdServerMonitorTime = new DateTime();

        public bool StatisServerMinitor()
        {
            List<tb_cluster_model> dataBaseList = GetEnableServer();
            tb_performance_dayreport_dal dal = new tb_performance_dayreport_dal();
            tb_performance_dayreport_model staticDayReport = new tb_performance_dayreport_model();
            DateTime dateNow = new DateTime();
            string day = "";
            string date = "";
            string ConnectionCluster = "";
            int updateMinite = 0;
            using (DbConn ManConn = DbConfig.CreateConn(XXF.Common.XXFConfig.MonitorPlatformConnectionString))
            {
                ManConn.Open();
                ConnectionCluster = new tb_database_config_dal().GetDataBaseSqlConn(ManConn, (int)DataBaseType.Cluster);
                dateNow = ManConn.GetServerDate();
                day = dateNow.ToString("yyyyMMdd");
                date = dateNow.ToString("yyyy-MM-dd");
                tb_keyvalue_config_model keymodel = new tb_keyvalue_config_dal().Get(ManConn, "SaveServerMonitorIntervalTime");
                updateMinite = Convert.ToInt32(keymodel.value);
            }
            using (DbConn PubConn = DbConfig.CreateConn(ConnectionCluster))
            {
                PubConn.Open();
                if ((dateNow - LastReserverdServerMonitorTime).TotalMinutes > updateMinite)
                {
                    int i = new tb_cluster_monitorinfo_dal().ReseverdServerMonitor(PubConn);
                    LastReserverdServerMonitorTime = dateNow;
                }
                Dictionary<int, tb_performance_dayreport_model> dayReportList = new tb_performance_dayreport_dal().GetAutoTaskList(PubConn, date);
                foreach (tb_cluster_model m in dataBaseList)
                {
                    if (dayReportList.Keys.Contains(m.id))
                    {
                        tb_performance_dayreport_model oldStaticDayReport = dayReportList[m.id];
                        staticDayReport = dal.GetStaticMonitor(PubConn, day, oldStaticDayReport.serverid, oldStaticDayReport.lastmaxid);
                        if (staticDayReport != null)
                        {
                            tb_performance_dayreport_model newStaticDayReport = new tb_performance_dayreport_model()
                            {
                                id = oldStaticDayReport.id,
                                serverid = m.id,
                                avgcpu = (oldStaticDayReport.avgcpu * oldStaticDayReport.count + staticDayReport.avgcpu * staticDayReport.count) / (oldStaticDayReport.count + staticDayReport.count),
                                maxcpu = oldStaticDayReport.maxcpu > staticDayReport.maxcpu ? oldStaticDayReport.maxcpu : staticDayReport.maxcpu,
                                mincpu = oldStaticDayReport.mincpu < staticDayReport.mincpu ? oldStaticDayReport.mincpu : staticDayReport.mincpu,
                                avgmemory = (oldStaticDayReport.avgmemory * oldStaticDayReport.count + staticDayReport.avgmemory * staticDayReport.count) / (oldStaticDayReport.count + staticDayReport.count),
                                maxmemory = oldStaticDayReport.maxmemory > staticDayReport.maxmemory ? oldStaticDayReport.maxmemory : staticDayReport.maxmemory,
                                minmemory = oldStaticDayReport.minmemory < staticDayReport.minmemory ? oldStaticDayReport.minmemory : staticDayReport.minmemory,
                                avgnetworkupload = (oldStaticDayReport.avgnetworkupload * oldStaticDayReport.count + staticDayReport.avgnetworkupload * staticDayReport.count) / (oldStaticDayReport.count + staticDayReport.count),
                                maxnetworkupload = oldStaticDayReport.maxnetworkupload > staticDayReport.maxnetworkupload ? oldStaticDayReport.maxnetworkupload : staticDayReport.maxnetworkupload,
                                minnetworkupload = oldStaticDayReport.minnetworkupload < staticDayReport.minnetworkupload ? oldStaticDayReport.minnetworkupload : staticDayReport.minnetworkupload,
                                avgnetworkdownload = (oldStaticDayReport.avgnetworkdownload * oldStaticDayReport.count + staticDayReport.avgnetworkdownload * staticDayReport.count) / (oldStaticDayReport.count + staticDayReport.count),
                                maxnetworkdownload = oldStaticDayReport.maxnetworkdownload > staticDayReport.maxnetworkdownload ? oldStaticDayReport.maxnetworkdownload : staticDayReport.maxnetworkdownload,
                                minnetworkdownload = oldStaticDayReport.minnetworkdownload < staticDayReport.minnetworkdownload ? oldStaticDayReport.minnetworkdownload : staticDayReport.minnetworkdownload,
                                avgioread = (oldStaticDayReport.avgioread * oldStaticDayReport.count + staticDayReport.avgioread * staticDayReport.count) / (oldStaticDayReport.count + staticDayReport.count),
                                maxioread = oldStaticDayReport.maxioread > staticDayReport.maxioread ? oldStaticDayReport.maxioread : staticDayReport.maxioread,
                                minioread = oldStaticDayReport.minioread < staticDayReport.minioread ? oldStaticDayReport.minioread : staticDayReport.minioread,
                                avgiowrite = (oldStaticDayReport.avgiowrite * oldStaticDayReport.count + staticDayReport.avgiowrite * staticDayReport.count) / (oldStaticDayReport.count + staticDayReport.count),
                                maxiowrite = oldStaticDayReport.maxiowrite > staticDayReport.maxiowrite ? oldStaticDayReport.maxiowrite : staticDayReport.maxiowrite,
                                miniowrite = oldStaticDayReport.miniowrite < staticDayReport.miniowrite ? oldStaticDayReport.miniowrite : staticDayReport.miniowrite,
                                avgiisrequest = (oldStaticDayReport.avgiisrequest * oldStaticDayReport.count + staticDayReport.avgiisrequest * staticDayReport.count) / (oldStaticDayReport.count + staticDayReport.count),
                                maxiisrequest = oldStaticDayReport.maxiisrequest > staticDayReport.maxiisrequest ? oldStaticDayReport.maxiisrequest : staticDayReport.maxiisrequest,
                                miniisrequest = oldStaticDayReport.miniisrequest < staticDayReport.miniisrequest ? oldStaticDayReport.miniisrequest : staticDayReport.miniisrequest,
                                lastmaxid = staticDayReport.lastmaxid,
                                count = oldStaticDayReport.count + staticDayReport.count,
                                lastupdatetime = dateNow,
                            };
                            dal.Update(PubConn, newStaticDayReport);
                        }
                    }
                    else
                    {
                        staticDayReport = dal.GetStaticMonitor(PubConn, day, m.id, 0);
                        if (staticDayReport == null)
                        {
                            staticDayReport = new tb_performance_dayreport_model();
                        }
                        staticDayReport.serverid = m.id;
                        staticDayReport.date = dateNow;
                        dal.AddStatis(PubConn, staticDayReport);
                    }
                }
                return true;
            }
        }

        private List<tb_cluster_model> GetEnableServer()
        {
            using (DbConn PubConn = DbConfig.CreateConn(XXFConfig.MonitorPlatformConnectionString))
            {
                PubConn.Open();
                List<tb_cluster_model> model = new tb_cluster_dal().GetAllList(PubConn);
                return model;
            }
        }
    }
}
