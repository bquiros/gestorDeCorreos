using System;
using Oracle.DataAccess.Client;

namespace dataCorreos
{
    public class contexto
    {
        
        public string getCredenciales()
        {
            string dataSource = System.Configuration.ConfigurationManager.AppSettings["connectionString"].ToString();
            return dataSource;

        }// fin getCredenciales

        public string getConexion()
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
    }
}
