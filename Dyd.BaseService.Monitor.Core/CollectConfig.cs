using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Dyd.BaseService.Monitor.Core
{
    public class CollectConfig
    {
        public CollectConfig() { }
        //public CollectConfig(string collectname, string categoryname, string countername, string instancename, CollectType collecttype = Core.CollectType.Custom)
        //{
        //    this.CollectName = collectname;
        //    this.CollectType = collecttype;
        //    this.CategoryName = categoryname;
        //    this.CounterName = countername;
        //    this.InstanceName = instancename;
        //}
        //public CollectConfig(string collectname, string categoryname,  CollectType collecttype = Core.CollectType.System)
        //{
        //    this.CollectName = collectname;
        //    this.CollectType = collecttype;
        //    this.CategoryName = categoryname;
        //    this.CounterName = "";
        //    this.InstanceName = "";
        //}
        public string CollectName { get; set; }
        public string CategoryName { get; set; }
        public string CounterName { get; set; }
        public string InstanceName { get; set; }
        public CollectType CollectType { get; set; }
        public ContrastWarningValue MoreThanWarningValue { get; set; }
        public ContrastWarningValue LessThanWarningValue { get; set; }
        public ContrastWarningValue EqualWarningValue { get; set; }
    }

    public enum CollectType
    {
        [Description("系统")]
        System,
        [Description("自定义")]
        Custom
    }

    public class ContrastWarningValue
    {
        public double Value { get; set; }
        public bool IsWarning { get; set; }
    }

    //public class EqualWarningValue
    //{
    //    public string Value { get; set; }
    //    public bool IsWarning { get; set; }
    //}
}
