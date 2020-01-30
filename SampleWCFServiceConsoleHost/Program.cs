using System;
using System.Collections.Generic;
using System.ServiceModel;
using SampleService;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.ServiceModel.Description;
using System.Net;
using Autofac.Integration.Wcf;
using Autofac;
using System.Text;
using System.IO;

namespace SampleWCFServiceConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<ServiceCore>().As<IServiceCore>();
            builder.RegisterType<DateProvider>().As<IDateProvider>();

            StringBuilder sb = new StringBuilder();
            sb.Append($"{DateTime.Now}");
            // flush every 20 seconds as you do it
            File.AppendAllText("log.txt", sb.ToString());
            sb.Clear();


            using (var container = builder.Build())
            {
                List<ServiceHost> hosts = new List<ServiceHost>();
                try
                {
                    ServiceHost host = null;

                    List<Type> servicesToStart = new List<Type>() {
                    typeof(ServiceCore)
                };

#if DEBUG
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("#########################################");
                    Console.WriteLine("#            DEBUG VERSION              #");
                    Console.WriteLine("#########################################");
                    Console.WriteLine();
                    Console.ResetColor();
#endif

                    foreach (Type type in servicesToStart)
                    {
                        host = new ServiceHost(type);
                        host.AddDependencyInjectionBehavior<IServiceCore>(container);
                        host.Open();

                        PrintServiceInfo(host);
                        hosts.Add(host);
                    }
                    ServicePointManager.ServerCertificateValidationCallback += customXertificateValidation;
#if DEBUG
                    // Вади connection string-a
                    if (ConfigurationManager.ConnectionStrings["ECheckModelContainer"] != null)
                    {
                        string connectionString = ConfigurationManager.ConnectionStrings["ECheckModelContainer"].ConnectionString;
                        Console.WriteLine("\n{0}\n", connectionString);
                    }
#endif

                    Console.ReadLine();
                    foreach (ServiceHost h in hosts)
                    {
                        h.Close();
                    }
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc);
                    foreach (ServiceHost h in hosts)
                    {
                        h.Abort();
                    }
                    Console.ReadLine();
                }
            }
        }

        private static bool customXertificateValidation(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            X509Certificate2 certificate = (X509Certificate2)cert;
            if (!String.IsNullOrEmpty(certificate.Thumbprint) && certificate.Thumbprint.ToLower() == "16d4753ac155d02572c4b73aaeaca05c340d4600")
            {
                return true;
            }
            return false;
        }

        private static void PrintServiceInfo(ServiceHost host)
        {
            Console.Write("Host: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(host.Description.ServiceType);
            Console.ResetColor();
            Console.WriteLine(" running.");
            Console.WriteLine("Endpoints:");

            foreach (ServiceEndpoint e in host.Description.Endpoints)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(e.Address);
            }
            Console.WriteLine();
            Console.ResetColor();
        }
    }
}
