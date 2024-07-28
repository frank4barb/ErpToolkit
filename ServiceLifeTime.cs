using System;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace ErpToolkit
{
    public class ServiceLifeTime : ServiceBase, IHostLifetime
    {
        public ServiceLifeTime(Microsoft.Extensions.Hosting.IApplicationLifetime applicationLifeTime)
        {
            ApplicationLifeTime = applicationLifeTime ?? throw new ArgumentNullException(nameof(applicationLifeTime));
            DelayStart = new TaskCompletionSource<object>();
        }

        public Microsoft.Extensions.Hosting.IApplicationLifetime ApplicationLifeTime { get; }
        public TaskCompletionSource<object> DelayStart { get; set; }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Stop();
            return Task.CompletedTask;
        }

        protected override void OnStart(string[] args)
        {
            DelayStart.TrySetResult(null);
            base.OnStart(args); 
        }

        protected override void OnStop()
        {
            ApplicationLifeTime.StopApplication();
            base.OnStop();  
        }

        public Task WaitForStartAsync(CancellationToken cancellationToken)
        {
            cancellationToken.Register(() => DelayStart.TrySetCanceled());
            ApplicationLifeTime.ApplicationStopping.Register(Stop);
            new Thread(Run).Start();
            return DelayStart.Task;
        }

        private void Run()
        {
            try
            {
                Run(this);
                DelayStart.TrySetException(new InvalidOperationException("Stopped, without starting the service"));
            }
            catch (Exception ex)
            {
                DelayStart.TrySetException(ex);
            }
        }
    }
}
