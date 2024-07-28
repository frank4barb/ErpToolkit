using ErpToolkit.Scheduler.Interfaces;
using Quartz;
using System.Threading.Tasks;

namespace ErpToolkit.Scheduler
{
    public class SecondlyJob : ISecondlyJob
    {
        public IExecService _helperService;
        public SecondlyJob(IExecService helperService)
        {
            _helperService = helperService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await _helperService.PerformService(TaskSchedule.Secondly);
        }
    }
}
