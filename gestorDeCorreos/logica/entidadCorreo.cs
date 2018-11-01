using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logica
{
    class entidadCorreo
    {
        public string Anno { get; set; }
        public string Consecutivo { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string Remitente { get; set; }
        public string Destinatario { get; set; }
        public string Cc { get; set; }
        public string Mensaje { get; set; }
        public string Asunto { get; set; }
        public string Estado { get; set; }
        public string FechaGestion { get; set; }
        public string Gestor { get; set; }
        public string RutaAdjunto { get; set; }
        public string Error { get; set; }
        public string Notificacion { get; set; }
        public string Contrato { get; set; }

        //Constructor de objeto
        public entidadCorreo(string pAnno, string pConsecutivo, DateTime pFechaIngreso, string pRemitente, string pDestinatario, string pCc, string pMensaje, string pAsunto, string pEstado, string pFechaGestion, string pGestor, string pRutaAdjunto, string pError, string pNotificacion, string pContrato)
        {
            this.Anno = pAnno;
            this.Consecutivo = pConsecutivo;
            this.FechaIngreso = pFechaIngreso;
            this.Remitente = pRemitente;
            this.Destinatario = pDestinatario;
            this.Cc = pCc;
            this.Mensaje = pMensaje;
            this.Asunto = pAsunto;
            this.Estado = pEstado;
            this.FechaGestion = pFechaGestion;
            this.Gestor = pGestor;
            this.RutaAdjunto = pRutaAdjunto;
            this.Error = pError;
            this.Notificacion = pNotificacion;
            this.Contrato = pContrato;
        }//Fin del constructor para entidadCorreo
    }// Fin entidadCorreo
}
