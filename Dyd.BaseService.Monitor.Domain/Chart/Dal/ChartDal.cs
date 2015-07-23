using Dyd.BaseService.Monitor.Domain.Chart.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Dyd.BaseService.Monitor.Domain.Chart.Dal
{
    public class ChartDal
    {
        public static Dictionary<string, object> CreateModel(DataRow dr)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            foreach (DataColumn d in dr.Table.Columns)
            {
                model.Add(d.ColumnName, dr[d.ColumnName]);
            }
            return model;
        }

        public Dictionary<string, object> CreateDicModel(DataRow dr, string Ckey, string Gkey)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            foreach (DataColumn k in dr.Table.Columns)
            {
                string name = k.ColumnName;
                if (name == Ckey)
                {
                    dic.Add(name, dr[name].ToString().Replace('/', '_'));
                }
                else if (name == Gkey)
                {
                    dic.Add(name, dr[name].ToString().Replace('/', '_'));
                }
                else
                {
                    dic.Add(name, dr[name]);
                }
            }
            return dic;
        }

        public static ChartModel CreateChartModel(DataRow dr)
        {
            ChartModel m = new ChartModel();
            m.Tkey = dr["Tkey"].ToString().Replace("/","_");
            m.Avalue = Convert.ToDecimal(dr["Avalue"]);
            m.Bvalue = Convert.ToDecimal(dr["Bvalue"]);
            if (dr.Table.Columns.Contains("Title"))
            {
                m.Title = dr["Title"].ToString() == "" ? "请自出查询" : dr["Title"].ToString();
            }
            else
            {
                m.Title = "";
            }
            m.AsubB = Convert.ToDecimal(dr["AsubB"]);
            if (Convert.ToDecimal(m.AsubB) > 0)
            {
                m.UpDown = 1;
            }
            else
            {
                m.UpDown = 0;
            }
            return m;
        }
    }
}
