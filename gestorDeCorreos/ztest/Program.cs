using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dataCorreos;
using logica;

namespace ztest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                logicaCorreo lc = new logicaCorreo();
                lc.getDatosTabla();
                //contexto ct = new contexto();
                //Console.Write(ct.getCredenciales());
                Console.ReadKey();
            }
            catch(Exception e)
            {
                Console.Write(e.Message);
                Console.ReadKey();
            }
            
            
        }
    }
}
