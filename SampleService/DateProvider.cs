using System;

namespace SampleService
{
    public class DateProvider : IDateProvider
    {
        public DateTime Now()
        {
            return DateTime.Now;
        }
    }
}
