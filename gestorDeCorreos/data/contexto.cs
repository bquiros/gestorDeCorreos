using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;
using System.Configuration;

namespace dataCorreos
{
    public class contexto
    {
        
        public string getCredenciales()
        {
            //string vsBd = ConfigurationManager.AppSettings["bd"];
            //string vsUsuario = ConfigurationManager.AppSettings["bdUser"];
            //string vsContrasenna = ConfigurationManager.AppSettings["bdPass"];

            //return "Data Source=" + vsBd + ";User Id=" + vsUsuario + ";Password=" + vsContrasenna + ";";

            string dataSource = System.Configuration.ConfigurationManager.AppSettings["connectionString"].ToString();
            return dataSource;

        }// fin getCredenciales

        public string getConeccion()
        {
            
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = getCredenciales();

            try
            {
                conn.Open();
                conn.Close();
                return getCredenciales();
            }
            catch (Exception)
            {
                throw;
            }
        }// Fin de getConection

        public void cerrarConexion(OracleConnection pOConexion)
        {
            pOConexion.Dispose();
            pOConexion.Close();

        }// Fin de CerrarConexion
    }
}
