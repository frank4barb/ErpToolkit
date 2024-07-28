using ErpToolkit.Scheduler.Interfaces;
using Quartz;
using System.Threading.Tasks;

namespace ErpToolkit.Scheduler
{
    public class MinutelyJob : IMinutelyJob
    {
        public IExecService _helperService;
        public MinutelyJob(IExecService helperService)
        {
            _helperService = helperService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await _helperService.PerformService(TaskSchedule.Minutely);
        }
    }
}
