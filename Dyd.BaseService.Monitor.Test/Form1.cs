using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dyd.BaseService.Monitor.Core;
using Dyd.BaseService.Monitor.Domain.PlatformManage.Dal;
using Dyd.BaseService.Monitor.Domain.PlatformManage.Model;
using XXF.ProjectTool;
using XXF.Extensions;
using XXF.BaseService.Monitor;
using XXF.BaseService.Monitor.SystemRuntime;
using Dyd.BaseService.Monitor.Tasks;
using System.Security.Cryptography;

namespace Dyd.BaseService.Monitor.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var d = DateTime.Parse("05 19 2015  7:55AM");
            var o = 12;
            //List<CollectConfig> cc = new List<CollectConfig>();
            //cc.Add(new CollectConfig(){ CollectName= "cpu", CategoryName = "Processor", CounterName= "% Processor Time",InstanceName= "_Total",
            //    CollectType= CollectType.System, 
            //    EqualWarningValue=new EqualWarningValue(){ IsWarning=false, Value=""},
            //    LessThanWarningValue=new ContrastWarningValue(){ IsWarning=true, Value=0.2},
            //    MoreThanWarningValue=new ContrastWarningValue(){ IsWarning=true, Value=0.8}});
            //cc.Add(new CollectConfig(){CollectName="内存",CategoryName= "Memory", CounterName="Available MBytes",InstanceName= "",
            //    CollectType= CollectType.System, 
            //    EqualWarningValue=new EqualWarningValue(){ IsWarning=false, Value=""},
            //    LessThanWarningValue=new ContrastWarningValue(){ IsWarning=true, Value=0.2},
            //    MoreThanWarningValue=new ContrastWarningValue(){ IsWarning=true, Value=0.8}});
            //cc.Add(new CollectConfig()
            //{
            //    CollectName = "网络发送/s",
            //    CategoryName = "Network Interface",
            //    CounterName = "Bytes Sent/sec",
            //    InstanceName = "本地连接* 14",
            //    CollectType = CollectType.System,
            //    EqualWarningValue = new EqualWarningValue() { IsWarning = false, Value = "" },
            //    LessThanWarningValue = new ContrastWarningValue() { IsWarning = true, Value = 0.2 },
            //    MoreThanWarningValue = new ContrastWarningValue() { IsWarning = true, Value = 0.8 }
            //});
            //cc.Add(new CollectConfig()
            //{
            //    CollectName = "网络下载/s",
            //    CategoryName = "Network Interface",
            //    CounterName = "Bytes Received/sec",
            //    InstanceName = "本地连接* 14",
            //    CollectType = CollectType.System,
            //    EqualWarningValue = new EqualWarningValue() { IsWarning = false, Value = "" },
            //    LessThanWarningValue = new ContrastWarningValue() { IsWarning = true, Value = 0.2 },
            //    MoreThanWarningValue = new ContrastWarningValue() { IsWarning = true, Value = 0.8 }
            //});
            //cc.Add(new CollectConfig()
            //{
            //    CollectName = "物理磁盘读字节/s",
            //    CategoryName = "PhysicalDisk",
            //    CounterName = "Disk Read Bytes/sec",
            //    InstanceName = "_Total",
            //    CollectType = CollectType.System,
            //    EqualWarningValue = new EqualWarningValue() { IsWarning = false, Value = "" },
            //    LessThanWarningValue = new ContrastWarningValue() { IsWarning = true, Value = 0.2 },
            //    MoreThanWarningValue = new ContrastWarningValue() { IsWarning = true, Value = 0.8 }
            //});
            //cc.Add(new CollectConfig()
            //{
            //    CollectName = "物理磁盘写字节/s",
            //    CategoryName = "PhysicalDisk",
            //    CounterName = "Disk Write Bytes/sec",
            //    InstanceName = "_Total",
            //    CollectType = CollectType.System,
            //    EqualWarningValue = new EqualWarningValue() { IsWarning = false, Value = "" },
            //    LessThanWarningValue = new ContrastWarningValue() { IsWarning = true, Value = 0.2 },
            //    MoreThanWarningValue = new ContrastWarningValue() { IsWarning = true, Value = 0.8 }
            //});
            //cc.Add(new CollectConfig()
            //{
            //    CollectName = "IIS请求/s",
            //    CategoryName = "Web Service",
            //    CounterName = "Current Connections",
            //    InstanceName = "_Total",
            //    CollectType = CollectType.System,
            //    EqualWarningValue = new EqualWarningValue() { IsWarning = false, Value = "0" },
            //    LessThanWarningValue = new ContrastWarningValue() { IsWarning = true, Value = 0 },
            //    MoreThanWarningValue = new ContrastWarningValue() { IsWarning = true, Value = 100 }
            //});

            //var str = new XXF.Serialization.JsonHelper().Serializer(cc);

            SqlHelper.ExcuteSql("server=192.168.17.201;Initial Catalog=dyd_bs_monitor_platform_manage;User ID=sa;Password=Xx~!@#;", (c) =>
            {
                tb_cluster_collect_version_dal versiondal = new tb_cluster_collect_version_dal();
                versiondal.Edit(c, new tb_cluster_collect_version_model()
                {
                    id = 2,
                    versionnum = 9,
                    versioncreatetime = DateTime.Now,
                    zipfile = System.IO.File.ReadAllBytes(@"E:\collect.rar"),
                    zipfilename = "collect.rar"
                });
            });

            string text = @"aaaa";
            DateTime dt1 = DateTime.Now;
            string hash = ELFHash(text)+ "";
            var mseconds = (DateTime.Now - dt1).TotalMilliseconds;
            DateTime dt2 = DateTime.Now;
            string hash2 = text.GetHashCode() + "";
            var mseconds2 = (DateTime.Now - dt2).TotalMilliseconds;
            var a = 1;

        }
        public long ELFHash(string str)
        {
            long hash = 0;
            long x = 0;
            for (int i = 0; i < str.Length; i++)
            {
                hash = (hash << 4) + str[i];
                if ((x = hash & 0xF0000000L) != 0)
                {
                    hash ^= (x >> 24);
                    hash &= ~x;
                }
            }
            return (hash & 0x7FFFFFFF);
        }


        private void button2_Click(object sender, EventArgs e)
        {
            string msg = @"insert into tb_log(sqlservercreatetime,logcreatetime,logtype,projectname,logtag,msg)
										   values(getdate(),@logcreatetime,@logtype,@projectname,@logtag,@msg)".SubString2(900);
            int logtag = msg.GetHashCode();
            TimeWatchHelper.AddTimeWatchLog(new TimeWatchLogInfo()
            {
                fromip = "192.168.17.54",
                logcreatetime = DateTime.Now,
                logtag = logtag,
                url="/test/a/",
                logtype = (int)EnumTimeWatchLogType.SqlCmd,
                msg = msg,
                projectname = "test",
                remark = "sadfasdfsdfas",
                sqlip = "192.168.17.201",
                time = 1
            });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string tag = @"token:aaaa,";
            string url = @"/common/config";
            TimeWatchHelper.AddTimeWatchApiLog(new TimeWatchLogApiInfo()
            {
                fromip = "192.168.17.54",
                logcreatetime = DateTime.Now,
                url = url,
                msg = "",
                projectname = "test",
                time = 1,
                 tag=tag
            });
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UnityLogHelper.AddCommonLog(new CommonLogInfo()
            {
                logcreatetime = DateTime.Now,
                msg = "这是测试日志",
                projectname = "test",
                logtag = @"/common/config",
                logtype=(int)EnumLogType.CommonLog
            });
        }


        private void button5_Click(object sender, EventArgs e)
        {
            //UnityLogHelper.AddErrorLog(new ErrorLogInfo()
            //{
            //    logcreatetime = DateTime.Now,
            //    msg = "这是测试错误日志",
            //    projectname = "test",
            //    logtag = @"/common/config",
            //    logtype = (int)EnumLogType.CommonError,
            //    developer="0",
            //    remark="",
            //    tracestack=""
            //});

            XXF.Log.ErrorLog.Write("", new Exception());

        }

        private void button6_Click(object sender, EventArgs e)
        {
            TimeWatchSqlCollectTask Task = new TimeWatchSqlCollectTask();
            Task.TestRun();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            TimeWatchApiCollectTask Task = new TimeWatchApiCollectTask();
            Task.TestRun();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            SqlHashContrastCollectTask task = new SqlHashContrastCollectTask();
            task.TestRun();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            CreateTask task = new CreateTask();
            task.TestRun();
        }
    }

       public class FNV1a : HashAlgorithm
    {
        private const uint Prime = 16777619;
        private const uint Offset = 2166136261;
        protected uint CurrentHashValue;
        public FNV1a()
        {
            this.HashSizeValue = 32;
            this.Initialize();
        }
        public override void Initialize()
        {
            this.CurrentHashValue = Offset;
        }
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            int end = ibStart + cbSize;
            for (int i = ibStart; i < end; i++)
                this.CurrentHashValue = (this.CurrentHashValue ^ array[i]) * FNV1a.Prime;
        }
        protected override byte[] HashFinal()
        {
            return BitConverter.GetBytes(this.CurrentHashValue);
        }
    }
}
