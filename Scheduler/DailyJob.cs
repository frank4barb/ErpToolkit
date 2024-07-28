using ErpToolkit.Scheduler.Interfaces;
using Quartz;
using System.Threading.Tasks;

namespace ErpToolkit.Scheduler
{
    public class DailyJob : IDailyJob
    {
        public IExecService _helperService;
        public DailyJob(IExecService helperService)
        {
            _helperService = helperService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await _helperService.PerformService(TaskSchedule.Daily);
        }
    }
}
