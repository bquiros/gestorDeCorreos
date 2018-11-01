using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;
using System.Configuration;

namespace dataCorreos
{
    class contexto
    {
        
        public string getCredenciales()
        {
            string vsBd = ConfigurationManager.AppSettings["bd"];
            string vsUsuario = ConfigurationManager.AppSettings["bdUser"];
            string vsContrasenna = ConfigurationManager.AppSettings["bdPass"];

            return "Data Source=" + vsBd + ";User Id=" + vsUsuario + ";Password=" + vsContrasenna + ";"; ;
        }// fin getCredenciales

        public bool getConeccion()
        {
            bool vbConectar = false;

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = getCredenciales();

            try
            {
                conn.Open();
                vbConectar = true;

                return vbConectar;
            }
            catch (Exception)
            {
                return vbConectar;
            }
        }// Fin de getConection

        public void cerrarConexion(OracleConnection pOConexion)
        {
            pOConexion.Dispose();
            pOConexion.Close();

        }// Fin de CerrarConexion
    }
}
