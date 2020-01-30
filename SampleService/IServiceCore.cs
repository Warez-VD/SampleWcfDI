using System.ServiceModel;

namespace SampleService
{
    [ServiceContract]
    public interface IServiceCore
    {
        [OperationContract]
        int Test();
    }
}
