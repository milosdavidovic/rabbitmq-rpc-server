
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.InteropServices;
using System.Timers;
using System.ServiceProcess;
using System.Collections;
using System.Diagnostics;
using System.Data.SqlClient;


namespace WinSer
{
    /* Created in 2015
     * Created by Milos Davidovic
     */

    class Program
    {
        static void Main(string[] args)
        {
            //Swich between Console app and Windos service mode
            if (!Environment.UserInteractive)
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
                { 
                    new MyService(args) 
                };
                ServiceBase.Run(ServicesToRun);
            }
            else
            {
                MyService prog = new MyService();
            }
        }
    }
}