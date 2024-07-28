using ErpToolkit.Helpers;
using Quartz;
using Quartz.Spi;
using System;

namespace ErpToolkit.Scheduler
{
    public class SchedulerJobFactory : IJobFactory
    {
        protected readonly IServiceProvider _serviceProvider;
        public SchedulerJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            try
            {
                return _serviceProvider.GetService(bundle.JobDetail.JobType) as IJob;
            }
            catch (Exception ex)
            {
                throw new ErpConfigurationException(ex.Message);
            }
        }

        public void ReturnJob(IJob job)
        {
            var obj = job as IDisposable;
            obj?.Dispose();
        }
    }
}
