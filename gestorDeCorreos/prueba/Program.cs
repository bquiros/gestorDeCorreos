using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gestorDeCorreos;
using dataCorreos;

namespace prueba
{
    class Program
    {
        static void Main(string[] args)
        {
            contexto ct = new contexto();
            ct.getCredenciales();
            Console.Write(ct.getConeccion());
            Console.ReadKey();
        }
    }
}
