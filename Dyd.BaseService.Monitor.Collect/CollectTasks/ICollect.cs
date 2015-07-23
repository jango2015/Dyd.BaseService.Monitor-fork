using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dyd.BaseService.Monitor.Collect.CollectTasks
{
    interface ICollect<T>
    {
        T Collect();
    }

    public abstract class BaseCollectTask
    {
        public string Name { get; set; }
        public bool IsCollect = false;
    }
}
