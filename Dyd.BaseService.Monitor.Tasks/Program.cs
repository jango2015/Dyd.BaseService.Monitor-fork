using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dyd.BaseService.Monitor.Tasks
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.ReadLine();
            ErrorEmailSendTask Task = new ErrorEmailSendTask();
            //while (true)
            //{
                Task.TestRun();
                //string x = Console.ReadLine();
                //if (x == "b")
                //{
                //    break;
                //}
            //}
        }
    }
}
