namespace SampleService
{
    public class ServiceCore : IServiceCore
    {
        private readonly IDateProvider dateProvider;

        public ServiceCore(IDateProvider dateProvider)
        {
            this.dateProvider = dateProvider;
        }

        public int Test()
        {
            var currentDate = this.dateProvider.Now();
            return currentDate.Year;
        }
    }
}
