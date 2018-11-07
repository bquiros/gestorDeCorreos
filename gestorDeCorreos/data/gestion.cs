using System;
using Oracle.DataAccess.Client;
using System.Data;
using System.Configuration;

namespace dataCorreos
{
    public class gestion
    {
        public DataTable getDatosTabla()
        {
            string tbNombre = ConfigurationManager.AppSettings["tbName"];
            string tbAlias = ConfigurationManager.AppSettings["tbAlias"];

            DataTable dt = new DataTable();
            contexto ct = new contexto();
            string qry = string.Empty;
            
            try
            {
                qry = "SELECT * from " + tbNombre + " " + tbAlias + " WHERE " + tbAlias + ".ESTADO = 'P'";
                OracleDataAdapter dtsOra = new OracleDataAdapter(qry, ct.getConeccion());
                dtsOra.Fill(dt);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }

        }//Fin de datosTabla

        public string setEstadoFecha(string pAnno, string pConsecutivo)
        {
            string tbNombre = ConfigurationManager.AppSettings["tbName"];
            string tbAlias = ConfigurationManager.AppSettings["tbAlias"];

            try
            {
                contexto ct = new contexto();
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = ct.getCredenciales();
                conn.Open();

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update " + tbNombre + " " + tbAlias + " set " + tbAlias + ".estado = 'E', " + tbAlias + ".fecha_gestion = to_date(Sysdate) where " + tbAlias + ".ano = " + pAnno + " and " + tbAlias + ".consecutivo = " + pConsecutivo + "";
                cmd.ExecuteNonQuery();
                conn.Close();
                return "Correo enviado y tabla actualizada correctamente";
               
            }
            catch (Exception e)
            {
                return "Error en archivo gestion, metodo setEstadoFecha: " + e.Message;
            }
        }
    }
}
