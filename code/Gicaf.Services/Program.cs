using Gicaf.Services.SSIS_Integrations;
using System;
using System.ServiceProcess;

namespace Gicaf.Services
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var service = new ServiceSSIS())
            {
                ServiceBase.Run(service);
            }
        }
    }
}
