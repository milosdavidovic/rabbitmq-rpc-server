
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WinSer
{

    [StructLayout(LayoutKind.Sequential)]
    public struct ServiceStatus
    {
        public long dwServiceType;
        public ServiceState dwCurrentState;
        public long dwControlsAccepted;
        public long dwWin32ExitCode;
        public long dwServiceSpecificExitCode;
        public long dwCheckPoint;
        public long dwWaitHint;
    };

    #region Enums
    //Windows Services states
    public enum ServiceState
    {
        SERVICE_STOPPED = 0x00000001,
        SERVICE_START_PENDING = 0x00000002,
        SERVICE_STOP_PENDING = 0x00000003,
        SERVICE_RUNNING = 0x00000004,
        SERVICE_CONTINUE_PENDING = 0x00000005,
        SERVICE_PAUSE_PENDING = 0x00000006,
        SERVICE_PAUSED = 0x00000007,
    }

    enum RequestStatus
    {
        NewRequest = 1,
        PendingRequest = 2,
        Sentrequest = 3,
        CompletedRequest = 4,
        ToOldRequest = 5
    }


    #endregion

    partial class MyService : ServiceBase
    {
        #region Program variables
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);
        public static int eventId = 0;
        public static System.Timers.Timer tmr1 = new System.Timers.Timer();
        public static System.Timers.Timer tmr2 = new System.Timers.Timer();
        //public static int someVariable1 = 8;
        //public static int someVariable2 = 192;
        #endregion

        #region Constants
        private const int TICK_TIME_T1 = 10000;  //Milisec interval for timer 1
        private const int TICK_TIME_T2 = 2000;  //Milisec interval for timer 2
        private const string SRV_VERSION = "0.1";
        
        //Console header - depricated
        static string consoleHeader = "Service is running!\t" + DateTime.Now.ToString() + "\n" ;
        #endregion

        #region Constructors
        // Constructor for debbuging
        public MyService()
        {
            ConsoleKeyInfo cki = new ConsoleKeyInfo();
            MyMainProcess();
            //Holds the console
            do
            {
                cki = Console.ReadKey();
            }
            while (cki.Key != ConsoleKey.Escape);
        }

        //Constructor for service deplyment
        public MyService(string[] args)
        {
            InitializeComponent();
            //Setup Event log
            SetupEventLog();
            //My service main method
            MyMainProcess();
        }
        #endregion

        #region Service menagment methods
        protected override void OnStart(string[] args)
        {
            // Update the service state to SERVICE_START_PENDING
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            eventLog.WriteEntry("In OnStart V" + SRV_VERSION);

            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        protected override void OnStop()
        {
            eventLog.WriteEntry("In onStop.");
        }

        protected override void OnContinue()
        {
            eventLog.WriteEntry("In OnContinue.");
        }
        #endregion

        #region Timer 1 - background timer for Agency alerts, etc.
        //Start timer
        private static void StartTimer1(double period_ms, System.Timers.Timer t, Config config_params)
        {
            try
            {
                //Set timer1, 5sec tick
                t.Interval = period_ms;
                //Enable timer, and pass AsynchronousSocketListener object to timer method
                t.Elapsed += delegate { t1_Tick( t, config_params); };
                //t.AutoReset = false;
                t.Enabled = true;
                //t1_Tick(ag, gr, t);
            }
            catch (Exception ex)
            {
                try { eventLog.WriteEntry("StartTimer1: " + ex.Message, EventLogEntryType.Error, eventId++); }
                catch { }
                Console.WriteLine("Error in StartTimer1: " + ex.Message);
            }
        }
        //Stop timer for active sockets status checking
        private static void StopTimer1(System.Timers.Timer t)
        {
            try
            {
                t.Stop();
                t.Close();
            }
            catch (Exception ex)
            {
                try { eventLog.WriteEntry("StopTimer1: " + ex.Message, EventLogEntryType.Error, eventId++); }
                catch { }
                Console.WriteLine("Error in StopTimer1: " + ex.Message);
            }
        }

        //Tajmer za osvezavanje grupa i slanje alarma
        static void t1_Tick( System.Timers.Timer t, Config config_params)
        {

            try
            {
                //Timer 1 code
                Debug.WriteLine("t1_Tick!");
            }
            catch (Exception ex)
            {
                try
                {
                    eventLog.WriteEntry("t1_Tick: " + ex.Message, EventLogEntryType.Error, eventId++);
                }
                catch { };
                //    Console.WriteLine("Error in t_Tick: " + ex.Message);
                try
                {
                    Debug.WriteLine("Error in t1_Tick: " + ex.Message);
                }
                catch { };
            }
        }
        #endregion

        #region Timer 2 - main timer for game management
        //Start timer
        private static void StartTimer2(double period_ms, System.Timers.Timer t, Config config_params)
        {
            try
            {
                //Set timer1, 5sec tick
                t.Interval = period_ms;
                //Enable timer, and pass AsynchronousSocketListener object to timer method
                t.Elapsed += delegate { t2_Tick( config_params); };
                t.Enabled = true;
            }
            catch (Exception ex)
            {
                try { eventLog.WriteEntry("StartTimer: " + ex.Message, EventLogEntryType.Error, eventId++); }
                catch { }
                Console.WriteLine(ex.Message);
            }
        }
        //Stop timer for active sockets status checking
        private static void StopTimer2(System.Timers.Timer t)
        {
            try
            {
                t.Stop();
                t.Close();
            }
            catch (Exception ex)
            {
                try { eventLog.WriteEntry("StopTimer: " + ex.Message, EventLogEntryType.Error, eventId++); }
                catch { }
                Console.WriteLine(ex.Message);
            }
        }
        //Active socket status check timer
        static void t2_Tick( Config config_params)
        {
            try
            {
                Console.Clear();
                Console.WriteLine(consoleHeader);
            }
            catch
            { }
            //Do Master of Time work...
            try
            {
                //Timer 2 code
                Debug.WriteLine("t2_Tick!");
            }
            catch (Exception ex)
            {
                try { eventLog.WriteEntry("t_Tick: " + ex.Message, EventLogEntryType.Error, eventId++); }
                catch { }
                try
                {
                    Debug.WriteLine("Error in t_Tick: " + ex.Message);
                }
                catch { }
            }

        }

        #endregion

        #region Private methods
        //Main method
        private void MyMainProcess()
        {
            Config config_params = new Config();

            //Start  timer 1
            StartTimer1(TICK_TIME_T1, tmr1, config_params);
            StartTimer2(TICK_TIME_T2, tmr2, config_params);
        }

        private void StartRabbitListener()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                //Create queue
                channel.QueueDeclare(queue: "rpc_queue",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                //Only one task per worker
                channel.BasicQos(0, 1, false);
                //Create consumer
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    RabbitHandleRequest(channel, ea);
                };
                channel.BasicConsume(queue: "rpc_queue",
                                     noAck: false,
                                     consumer: consumer);
                Console.WriteLine(" [x] Awaiting RPC requests");

            }
        }

        public void RabbitHandleRequest(IModel channel, BasicDeliverEventArgs ea)
        {
            Random r = new Random();
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine("[x] Recived {0}", message);
            var props = ea.BasicProperties;
            var replyProps = channel.CreateBasicProperties();
            replyProps.CorrelationId = props.CorrelationId;

            Thread.Sleep(3000);
            var response = r.Next(1, 999).ToString();
            var responseBytes = Encoding.UTF8.GetBytes(response);
            channel.BasicPublish(exchange: "",
                                 routingKey: props.ReplyTo,
                                 basicProperties: replyProps,
                                 body: responseBytes);
            channel.BasicAck(deliveryTag: ea.DeliveryTag,
                             multiple: false);
        }


        // Setup Event logging for My Service
        private void SetupEventLog()
        {
            //For service logging. Logs could be accessed using Windows's Event Viewer
            string eventSourceName = Config._company_name.ToString();
            string logName = Config._company_name.ToString() + "-MyServiceLog";
            this.AutoLog = false;

            eventLog = new EventLog();
            if (!EventLog.SourceExists(eventSourceName))
            {
                EventLog.CreateEventSource(eventSourceName, logName);
            }
            eventLog.Source = eventSourceName;
            eventLog.Log = logName;
        }

        #endregion
    }
}
