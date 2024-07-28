using System.Threading.Tasks;

namespace ErpToolkit.Scheduler.Interfaces
{
    public interface IExecService
    {
        Task PerformService(string schedule);
    }
}
