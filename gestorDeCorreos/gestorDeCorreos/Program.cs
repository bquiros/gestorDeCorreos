using System.ServiceProcess;

namespace gestorDeCorreos
{
    static class Program
    {
        static void Main(string[] args)
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new gestorSercice(args)
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
