using System;
using System.Diagnostics;
using System.Runtime.InteropServices; //Clase para estados del servicio
using System.ServiceProcess;
using System.Configuration;
using dataCorreos;
using logica;
using Oracle.DataAccess.Client;

namespace gestorDeCorreos
{
    public partial class gestorSercice : ServiceBase
    {   
        //Funcion para el estado del servicio
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(System.IntPtr handle, ref ServiceStatus serviceStatus);
       
        //Variables
        int eventRegistroId = 1;

        // Parametros de app.config
        private int tiempo = Int32.Parse(ConfigurationManager.AppSettings["timer"]);

        public gestorSercice(string[] args)
        {
            InitializeComponent();

            // Este código establece el origen del evento y el nombre del registro de acuerdo con los parámetros de inicio proporcionados,
            // o utiliza valores predeterminados si no se proporcionan argumentos.
            string eventSourceName = "SourceCorreos";
            string logName = "LogGestorCorreos";

            if (args.Length > 0)
            {
                eventSourceName = args[0];
            }

            if (args.Length > 1)
            {
                logName = args[1];
            }

            eventLogRegistro = new System.Diagnostics.EventLog();

            if (!System.Diagnostics.EventLog.SourceExists(eventSourceName))
            {
                System.Diagnostics.EventLog.CreateEventSource(eventSourceName, logName);
            }

            eventLogRegistro.Source = eventSourceName;
            eventLogRegistro.Log = logName;

        }

        protected override void OnStart(string[] args)
        {
            // Actualizar estado del inicio pendiente.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            //Se encarga de llamar al timer con el intervalo ingresado por parametro
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = tiempo; // cantidad desde app.config
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();

            try
            {
                contexto ct = new contexto();
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = ct.getCredenciales();
                conn.Open();

                eventLogRegistro.WriteEntry("Conectado correctamente a: " + ct.getCredenciales(), EventLogEntryType.Information, eventRegistroId++);
                eventLogRegistro.WriteEntry("Tiempo establecido para la carga de correos: " + tiempo + " milisegundos", EventLogEntryType.Information, eventRegistroId++);

                conn.Close();
            }
            catch (Exception e)
            {
                eventLogRegistro.WriteEntry("Error al conectar con el servidor: " + e.Message, EventLogEntryType.Information, eventRegistroId++);
                OnStop();
            }

            // Actualiza el estado en ejecución
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            //Fecha y hora del sistema
            string fecha = DateTime.Now.ToString("dd/MM/yyyy");
            string hora = DateTime.Now.ToString("hh:mm:ss");

            try
            {
                // Actividades monitoreadas
                logicaCorreo lg = new logicaCorreo();
                string correos = lg.DatosTabla();
                eventLogRegistro.WriteEntry(correos +" :fecha: " + fecha + ", hora: " + hora, EventLogEntryType.Information, eventRegistroId++);
            }
            catch (Exception e)
            {
                eventLogRegistro.WriteEntry("Error al enviar correos: " + e.Message, EventLogEntryType.Information, eventRegistroId++);
                OnStop();
            }
        }

        protected override void OnStop()
        {
            //Fecha y hora del sistema
            string fecha = DateTime.Now.ToString("dd/MM/yyyy");
            string hora = DateTime.Now.ToString("hh:mm:ss");

            // Detener el servicio.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            eventLogRegistro.WriteEntry("El servicio se detuvo. Fecha: " + fecha + " ,hora: " + hora);
        }

        //Estado del servicio
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

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public int dwServiceType;
            public ServiceState dwCurrentState;
            public int dwControlsAccepted;
            public int dwWin32ExitCode;
            public int dwServiceSpecificExitCode;
            public int dwCheckPoint;
            public int dwWaitHint;
        };
    }
}
