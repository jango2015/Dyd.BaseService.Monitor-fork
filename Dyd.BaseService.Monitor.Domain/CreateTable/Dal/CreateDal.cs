using Dyd.BaseService.Monitor.Domain.CreateTable.Model;
using Dyd.BaseService.Monitor.Domain.PlatformManage.Dal;
using Dyd.BaseService.Monitor.Domain.PlatformManage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XXF.BaseService.Monitor.SystemRuntime;
using XXF.BaseService.TaskManager.OpenOperator;
using XXF.Common;
using XXF.Db;
using XXF.ProjectTool;

namespace Dyd.BaseService.Monitor.Domain.CreateTable.Dal
{
    public class CreateDal
    {
        public bool CreateTable(out Dictionary<string, Exception> errordic)
        {
            errordic = new Dictionary<string, Exception>();
            Dictionary<string, string> configDic = new Dictionary<string, string>();
            List<tb_database_config_model> databaseList = new List<tb_database_config_model>();
            tb_database_config_dal databaseDal = new tb_database_config_dal();
            DateTime dateNow = new DateTime();
            using (DbConn PubConn = DbConfig.CreateConn(XXFConfig.MonitorPlatformConnectionString))
            {
                PubConn.Open();
                dateNow = PubConn.GetServerDate();
                configDic = new tb_keyvalue_config_dal().GetAll(PubConn);
                databaseList = databaseDal.GetModelList(PubConn);
            }
            if (configDic.Count > 0)
            {
                if (databaseList.Count > 0)
                {
                    string dayTableSuffix = dateNow.ToString("yyyyMMdd");
                    string dayTableSuffixNext = dateNow.AddDays(1).ToString("yyyyMMdd");
                    string monthTableSuffix = dateNow.ToString("yyyyMM");
                    string monthTableSuffixNext = dateNow.AddMonths(1).ToString("yyyyMM");
                    foreach (tb_database_config_model m in databaseList)
                    {
                        string Connection = databaseDal.ReplaceConnectStringTemplate(m);
                        using (DbConn PubConn = DbConfig.CreateConn(Connection))
                        {
                            PubConn.Open();
                            try
                            {
                                if (m.dbtype == (int)DataBaseType.Cluster)
                                {
                                    CreateDayTable(PubConn, "tb_performance_collect", configDic["CreatePerCollectTable"], dayTableSuffix, dayTableSuffixNext);
                                }
                                if (m.dbtype == (int)DataBaseType.Timewatch)
                                {
                                    CreateDayTable(PubConn, "tb_timewatchlog_api", configDic["CreateTimeWatchApiLogTable"], dayTableSuffix, dayTableSuffixNext);
                                    CreateDayTable(PubConn, "tb_timewatchlog", configDic["CreateTimeWatchLogTable"], dayTableSuffix, dayTableSuffixNext);
                                }
                                if (m.dbtype == (int)DataBaseType.UnityLog)
                                {
                                    CreateMonthTable(PubConn, "tb_error_log", configDic["CreateErrorLogTable"], monthTableSuffix, monthTableSuffixNext);
                                    CreateMonthTable(PubConn, "tb_log", configDic["CreateLogTable"], monthTableSuffix, monthTableSuffixNext);
                                }
                            }
                            catch (Exception ex)
                            {
                                errordic.Add("创建表失败|连接" + Connection, ex);
                            }
                        }
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private void CreateDayTable(DbConn PubConn, string name, string createsql, string dayTableSuffix, string dayTableSuffixNext)
        {
            if (!PubConn.TableIsExist(name + dayTableSuffix))
            {
                string sql = createsql.Replace("{partitionno}", dayTableSuffix).Replace("GO", " ");
                int b = Create(PubConn, sql);
            }
            if (!PubConn.TableIsExist(name + dayTableSuffixNext))
            {
                string sql = createsql.Replace("{partitionno}", dayTableSuffixNext).Replace("GO", " ");
                int b = Create(PubConn, sql);
            }
        }

        private void CreateMonthTable(DbConn PubConn, string name, string createsql, string monthTableSuffix, string monthTableSuffixNext)
        {
            if (!PubConn.TableIsExist(name + monthTableSuffix))
            {
                string sql = createsql.Replace("{partitionno}", monthTableSuffix).Replace("GO", " ");
                int b = Create(PubConn, sql);
            }
            if (!PubConn.TableIsExist(name + monthTableSuffixNext))
            {
                string sql = createsql.Replace("{partitionno}", monthTableSuffixNext).Replace("GO", " ");
                int b = Create(PubConn, sql);
            }
        }
        //private List<CreateTableModel> CreateModel(Dictionary<string,string> configdic, List<tb_database_config_model> databaselist)
        //{
        //    List<CreateTableModel> model = new List<CreateTableModel>();
        //    string dayTableSuffix = DateTime.Now.ToString("yyyyMMdd");
        //    string dayTableSuffixNext = DateTime.Now.AddDays(1).ToString("yyyyMMdd");
        //    string monthTableSuffix = DateTime.Now.ToString("yyyyMM");
        //    string monthTableSuffixNext = DateTime.Now.AddMonths(1).ToString("yyyyMM");
        //    model.Add(new CreateTableModel()
        //    {
        //        TableName = "tb_timewatchlog_api" + dayTableSuffix,
        //            SqlConn = databaselist,
        //            CreateSql = configdic["CreateTimeWatchLogApiTable"].Replace("{partitionno}", dayTableSuffix).Replace("GO", " ")
        //    });
        //        string SqlTxt = value.value;
        //        string[] splitSqlTxt = SqlTxt.Split(new[] { "[SplitSign]" }, StringSplitOptions.RemoveEmptyEntries);
        //        CreateTableModel m = new CreateTableModel()
        //        {
        //            TableName = splitSqlTxt[0].Replace("\r\n","") + tableSuffix,
        //            SqlConn = splitSqlTxt[1].Replace("\r\n", ""),
        //            CreateSql = splitSqlTxt[2].Replace("{partitionno}", tableSuffix).Replace("GO", " ")
        //        };
        //        CreateTableModel m2 = new CreateTableModel()
        //        {
        //            TableName = splitSqlTxt[0].Replace("\r\n", "") + tableSuffixNext,
        //            SqlConn = splitSqlTxt[1].Replace("\r\n", ""),
        //            CreateSql = splitSqlTxt[2].Replace("{partitionno}", tableSuffixNext).Replace("GO", " ")
        //        };
        //        model.Add(m);
        //        model.Add(m2);
        //    }
        //    return model;
        //}

        private int Create(DbConn PubConn, string sql)
        {
            return SqlHelper.Visit(ps =>
            {
                return PubConn.ExecuteSql(sql, ps.ToParameters());
            });
        }
    }
}
