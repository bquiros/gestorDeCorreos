using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using dataCorreos; 
using System.Configuration;

namespace logica
{
    public class logicaCorreo
    {
        public string DatosTabla()
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

                    return enviarCorreo(enOb); //Se envian los campos de la tabla en un objeto

                }
                return "Todos los correos han sido enviados";
            }
            catch (Exception e)
            {
                return "Error en arhivo logicaCorreo metodo DatosTabla: " + e.Message;
            }
        }//Fin getDatosTabla

        private string enviarCorreo(entidadCorreo pOb) //parametro viene de la funcion getDatosTabla
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
            //contenido.AppendLine("Fecha: " + pOb.FechaIngreso);
            contenido.AppendLine(pOb.Mensaje + "<br>");
            contenido.AppendLine(pOb.RutaAdjunto + "<br>");
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
                return gn.setEstadoFecha(pOb.Anno, pOb.Consecutivo);
            }
            catch (Exception)
            {
                throw;
            }
        }//enviarCorreo
    }
}
