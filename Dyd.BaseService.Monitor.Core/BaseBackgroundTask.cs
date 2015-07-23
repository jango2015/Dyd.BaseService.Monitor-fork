using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dyd.BaseService.Monitor.Core
{
    public abstract class BaseBackgroundTask
    {
        private System.Threading.Thread _thread = null;
        public int TimeSleep = 5000;
        public BaseBackgroundTask()
        {
           
        }

        public virtual void Start()
        {
            _thread = new System.Threading.Thread(TryRun);
            _thread.IsBackground = true;
            _thread.Start();
        }

        protected void TryRun()
        {
            while (true)
            {
                try
                {
                    Run();
                }
                catch (Exception exp)
                {
                    LogHelper.Error("当前后台任务出现严重错误:", exp);
                }
                System.Threading.Thread.Sleep(TimeSleep);
            }
        }

        protected virtual void Run()
        {

        }
    }
}
