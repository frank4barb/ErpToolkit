using ErpToolkit.Scheduler.Interfaces;
using Quartz;
using System.Threading.Tasks;

namespace ErpToolkit.Scheduler
{
    public class WeeklyJob : IWeeklyJob
    {
        public IExecService _helperService;
        public WeeklyJob(IExecService helperService)
        {
            _helperService = helperService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await _helperService.PerformService(TaskSchedule.Weekly);
        }
    }
}
