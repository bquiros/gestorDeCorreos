﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices; //Clase para estados del servicio
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace gestorDeCorreos
{
    public partial class gestorSercice : ServiceBase
    {   
        //Funcion para el estado del servicio
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(System.IntPtr handle, ref ServiceStatus serviceStatus);
       
        //Variables
        private int eventRegistroId = 1;

        // Parametros de app.config
        private int tiempo = Int32.Parse(ConfigurationManager.AppSettings["timer"]);

        public gestorSercice(string[] args)
        {
            InitializeComponent();

            // Este código establece el origen del evento y el nombre del registro de acuerdo con los parámetros de inicio proporcionados,
            // o utiliza valores predeterminados si no se proporcionan argumentos.
            string eventSourceName = "SourceCorreos";
            string logName = "LogCorreos";

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
            eventLogRegistro.WriteEntry("Dentro de onStart, tiempo de espera: " +tiempo);

            //Se encarga de llamar al timer con el intervalo ingresado por parametro
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = tiempo; // cantidad desde app.config
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();

            // Actualiar estado del inicio pendiente.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            // Actualizar el estado del servicio en ejecución.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            // Actividades monitoreadas
            eventLogRegistro.WriteEntry("Monitoreado", EventLogEntryType.Information, eventRegistroId++);
        }

        protected override void OnStop()
        {
            eventLogRegistro.WriteEntry("El servicio se detuvo");
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