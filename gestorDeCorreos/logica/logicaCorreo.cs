using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Oracle.DataAccess;
using dataCorreos; 
using System.Configuration;


namespace logica
{
    public class logicaCorreo
    {
        public void getDatosTabla()
        {
            List<entidadCorreo> lstCorreo = new List<entidadCorreo>();

            try
            {
                gestion gn = new gestion();
                DataTable dt = gn.getDatosTabla();
                entidadCorreo enOb;

                foreach (DataRow dr in dt.Rows)
                {
                    enOb = new entidadCorreo(dr[0].ToString(), dr[1].ToString(), Convert.ToDateTime(dr[2]), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString(), dr[8].ToString(), dr[9].ToString(), dr[10].ToString(), dr[11].ToString(), dr[12].ToString(), dr[13].ToString(), dr[14].ToString());

                    enviarCorreo(enOb); //Se envian los campos de la tabla en un objeto
                }
            }
            catch (Exception)
            {
                throw;
            }
        }//Fin getDatosTabla

        private void enviarCorreo(entidadCorreo pOb) //parametro viene de la funcion getDatosTabla
        {   
            // Contraseña adquirida desde el archivo app.config
            string contrasenna = ConfigurationManager.AppSettings["mailPass"];
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();

            msg.To.Add(pOb.Destinatario);
            msg.Subject = pOb.Asunto;
            msg.SubjectEncoding = System.Text.Encoding.UTF8;
            msg.Bcc.Add(pOb.Cc);

            //Se crea string que contiene el cuerpo del correo
            StringBuilder contenido = new StringBuilder();
            contenido.AppendLine("Fecha: " + pOb.FechaIngreso);
            contenido.AppendLine(pOb.Mensaje);
            contenido.AppendLine(pOb.RutaAdjunto);
            contenido.AppendLine("Gestor: " + pOb.Gestor);

            //Se envia el string creado como cuerpo del correo
            msg.Body = contenido.ToString();
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.IsBodyHtml = true;
            msg.From = new System.Net.Mail.MailAddress(pOb.Remitente);

            System.Net.Mail.SmtpClient cliente = new System.Net.Mail.SmtpClient();
            cliente.Credentials = new System.Net.NetworkCredential(pOb.Remitente, contrasenna); //credenciales de la cuenta que enviara los correos

            cliente.Port = 587;
            cliente.EnableSsl = true;

            cliente.Host = "smtp.office365.com";

            try
            {
                cliente.Send(msg);

                //Referencio clase consulta
                gestion gn = new gestion();
                gn.setEstadoFecha(pOb.Anno, pOb.Consecutivo);
            }
            catch (Exception)
            {
                throw;
            }
        }//enviarCorreo
    }
}
