using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.ServiceProcess;
using System.Text;

namespace Gicaf.Services.SSIS_Integrations
{
    internal class ServiceSSIS : ServiceBase
    {
        public ServiceSSIS()
        {
            ServiceName = "TestService";
        }

        protected override void OnStart(string[] args)
        {
            //IntegrationServices integrationServices = new IntegrationServices(new SqlConnection());

            string filename = CheckFileExists();
            ///IntegrationServices integrationServices = new IntegrationServices();
            //integrationServices;
            //Microsoft.SqlServer.Management.
        }

        protected override void OnStop()
        {
            string filename = CheckFileExists();
            //File.AppendAllText(filename, .../../src/"{DateTime.Now} stopped.{Environment.NewLine}");
        }

        private static string CheckFileExists()
        {
            return "";
        }

    }
}
