using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using dataCorreos; 
using System.Configuration;
using System.Net.Mail;
using System.Net.Mime;

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
            string correo = ConfigurationManager.AppSettings["mail"];
            string contrasenna = ConfigurationManager.AppSettings["mailPass"];

            // Se crea mail
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            // Se define ruta de origen
            msg.From = new System.Net.Mail.MailAddress(correo);
            // Se agrega el asunto
            msg.Subject = pOb.Asunto;
            // Tipo de codificación
            msg.SubjectEncoding = System.Text.Encoding.UTF8;

            //Ruta de archivo adjunto (Si lo lleva)
            string PathFile = pOb.RutaAdjunto;
            if (PathFile.Contains(" ") == false)
            {
                //Agrego el archivo que puse en la ruta anterior "PathFile", y su tipo.
                Attachment Data = new Attachment(PathFile, MediaTypeNames.Application.Zip);

                //Obtengo las propiedades del archivo.
                ContentDisposition disposition = Data.ContentDisposition;
                disposition.CreationDate = System.IO.File.GetCreationTime(PathFile);
                disposition.ModificationDate = System.IO.File.GetLastWriteTime(PathFile);
                disposition.ReadDate = System.IO.File.GetLastAccessTime(PathFile);

                //Agrego el archivo al mensaje
                msg.Attachments.Add(Data);
            }

            string correosDestino = pOb.Destinatario;
            string[] listaCorreosDestino = correosDestino.Split(';');
            // Se recorren los destinatarios para agregarlos 
            foreach (string destinatario in listaCorreosDestino)
            {
                // se agregan destinatarios
                msg.To.Add(destinatario);
            }

            string correosCC = pOb.Cc;
            if (correosCC!="")
            {
                string[] listaCorreosCC = correosCC.Split(';');
                // Se recorren los cc para agregarlos 
                foreach (string destinos in listaCorreosCC)
                {
                    // Se agrega destinatarios
                    msg.CC.Add(destinos);
                }
            }
           
            msg.Bcc.Add(pOb.Remitente);
            //Se crea string que contiene el cuerpo del correo
            StringBuilder contenido = new StringBuilder();
            contenido.AppendLine("Remitente: " + pOb.Remitente + "<br>");
            contenido.AppendLine("<br>");
            contenido.AppendLine(pOb.Mensaje + "<br>");
            contenido.AppendLine("<br>");
            contenido.AppendLine("Ruta archivo: " + pOb.RutaAdjunto + "<br>");
            contenido.AppendLine("<br>");
            contenido.AppendLine("Gestor: " + pOb.Gestor);  
            //Se envia el string creado como cuerpo del correo
            msg.Body = contenido.ToString();
            // Tipo de codificación del mensaje
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            // Se especifica que va a ser interpretado como HTML
            msg.IsBodyHtml = true;
            //Creo objeto cliente (Por donde se va a enviar el correo)
            System.Net.Mail.SmtpClient cliente = new System.Net.Mail.SmtpClient();
            //credenciales de la cuenta que enviara los correos
            cliente.Credentials = new System.Net.NetworkCredential(correo, contrasenna);
            // Se especifica el puerto
            cliente.Port = 587;
            // sertificado de seguridad
            cliente.EnableSsl = true;
            // Se identifica el cliente
            cliente.Host = "smtp.office365.com";

            try
            {   
                //se envia el correo
                cliente.Send(msg);
                //Referencio clase consulta
                gestion gn = new gestion();
                //Clase que cambia estado de la tabla
                return gn.setEstadoFecha(pOb.Anno, pOb.Consecutivo);
            }
            catch (Exception)
            {
                throw;
            }
        }//enviarCorreo
    }
}
