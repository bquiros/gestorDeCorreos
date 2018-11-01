﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;
using System.Data;
using System.Configuration;

namespace dataCorreos
{
    public class gestion
    {
        string tbNombre = ConfigurationManager.AppSettings["tbName"];
        string tbAlias = ConfigurationManager.AppSettings["tbAlias"];

        public DataTable getDatosTabla()
        {
            DataTable dt = new DataTable();
            contexto ct = new contexto();
            string qry = string.Empty;
            
            try
            {
                //qry = "SELECT * from GC_CORREOS GC WHERE GC.ESTADO = 'P'";
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

        public void setEstadoFecha(string pAnno, string pConsecutivo)
        {
            contexto ct = new contexto();

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ct.getCredenciales();

            try
            {
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                //cmd.CommandText = "update gc_correos gc set gc.estado = 'E', gc.fecha_gestion = to_date(Sysdate) where gc.ano = " + pAnno + " and gc.consecutivo = " + pConsecutivo + "";
                cmd.CommandText = "update " + tbNombre + " " + tbAlias + " set " + tbAlias + ".estado = 'E', " + tbAlias + ".fecha_gestion = to_date(Sysdate) where " + tbAlias + ".ano = " + pAnno + " and " + tbAlias + ".consecutivo = " + pConsecutivo + "";
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
