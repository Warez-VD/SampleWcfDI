using Autofac;
using Autofac.Integration.Wcf;
using SampleService;
using System;
using System.IO;
using System.Text;
using System.Web;

namespace SampleWCFService
{
    public class Global : HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            var builder = new ContainerBuilder();

            // Register your service implementations.
            builder.RegisterType<ServiceCore>().As<IServiceCore>();
            builder.RegisterType<DateProvider>().As<IDateProvider>();

            // Set the dependency resolver.
            var container = builder.Build();
            AutofacHostFactory.Container = container;

            StringBuilder sb = new StringBuilder();
            sb.Append($"{DateTime.Now}");
            // flush every 20 seconds as you do it
            File.AppendAllText(Server.MapPath("log.txt"), sb.ToString());
            sb.Clear();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}