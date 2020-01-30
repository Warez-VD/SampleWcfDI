using System;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace SampleService
{
    public class ServiceFactory : WebServiceHostFactory
    {
        public ServiceFactory()
            : base()
        {
        }

        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            ServiceHost host = new RestServiceHost(serviceType, baseAddresses);
            if (serviceType == typeof(IServiceCore))
                host.AddServiceEndpoint(typeof(SampleService.IServiceCore), new NetTcpBinding(), baseAddresses[1]);

            return host;
        }

        public class RestServiceHost : ServiceHost
        {
            public RestServiceHost() : base()
            {
            }

            public RestServiceHost(Type serviceType, params Uri[] baseAddresses)
                : base(serviceType, baseAddresses)
            {
            }
        }
    }
}
