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
              
            try
            {
                contexto ct = new contexto();
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = ct.getConexion();
                conn.Open();

                DataTable dt = new DataTable();
                string qry = string.Empty;
                string tbNombre = ConfigurationManager.AppSettings["tbName"];
                string tbAlias = ConfigurationManager.AppSettings["tbAlias"];

                qry = "SELECT * from " + tbNombre + " " + tbAlias + " WHERE " + tbAlias + ".ESTADO = 'P'";
                OracleDataAdapter dtsOra = new OracleDataAdapter(qry, ct.getConexion());
                dtsOra.Fill(dt);

                conn.Close();
                return dt;
            }
            catch (Exception)
            {
                throw;
            }

        }//Fin de datosTabla

        public string setEstadoFecha(string pAnno, string pConsecutivo)
        {
            try
            {
                try
                {
                    contexto ct = new contexto();
                    OracleConnection conn = new OracleConnection();
                    conn.ConnectionString = ct.getConexion();
                    conn.Open();

                    string tbNombre = ConfigurationManager.AppSettings["tbName"];
                    string tbAlias = ConfigurationManager.AppSettings["tbAlias"];

                    OracleCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "update " + tbNombre + " " + tbAlias + " set " + tbAlias + ".estado = 'E', " + tbAlias + ".fecha_gestion = to_date(Sysdate) where " + tbAlias + ".ano = " + pAnno + " and " + tbAlias + ".consecutivo = " + pConsecutivo + "";
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    return "Correo enviado, tabla actualizada correctamente";
                }
                catch
                {
                    return "No hay correos por enviar";
                }
            }
            catch (Exception e)
            {
                return "Error en archivo gestion, metodo setEstadoFecha: " + e.Message;
            }
        }
    }
}
