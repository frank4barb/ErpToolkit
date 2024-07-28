using ErpToolkit.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;
using ErpToolkit.Scheduler.Interfaces;
using ErpToolkit.Scheduler;

namespace ErpToolkit
{
    public class ServiceScheduler : IHostedService
    {
        private static NLog.ILogger _logger;
        public ServiceScheduler()
        {
            //SetUpNLog();
            NLog.LogManager.Configuration = UtilHelper.GetNLogConfig(); // Apply config
            _logger = NLog.LogManager.GetCurrentClassLogger();

        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                var scheduler = await GetScheduler();
                var serviceProvider = GetConfiguredServiceProvider();
                scheduler.JobFactory = new SchedulerJobFactory(serviceProvider);
                await scheduler.Start();
                string param = "";
                if ((param = ErpContext.Instance.GetString("#SecondlyJob")) != "") await ConfigureSecondlyJob(scheduler, param);
                if ((param = ErpContext.Instance.GetString("#MinutelyJob")) != "") await ConfigureMinutelyJob(scheduler, param);
                if ((param = ErpContext.Instance.GetString("#DailyJob")) != "") await ConfigureDailyJob(scheduler, param);
                if ((param = ErpContext.Instance.GetString("#WeeklyJob")) != "") await ConfigureWeeklyJob(scheduler, param);
            }
            catch (Exception ex)
            {
                _logger.Error(new ErpConfigurationException(ex.Message));
            }
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }


        private async Task ConfigureSecondlyJob(IScheduler scheduler, string param)
        {
            var secondlyJob = GetSecondlyJob();
            if (await scheduler.CheckExists(secondlyJob.Key))
            {
                await scheduler.ResumeJob(secondlyJob.Key);
                _logger.Info($"The job key {secondlyJob.Key} was already existed, thus resuming the same");
            }
            else
            {
                await scheduler.ScheduleJob(secondlyJob, GetSecondlyJobTrigger(param));
            }
        }
        private async Task ConfigureMinutelyJob(IScheduler scheduler, string param)
        {
            var minutelyJob = GetMinutelyJob();
            if (await scheduler.CheckExists(minutelyJob.Key))
            {
                await scheduler.ResumeJob(minutelyJob.Key);
                _logger.Info($"The job key {minutelyJob.Key} was already existed, thus resuming the same");
            }
            else
            {
                await scheduler.ScheduleJob(minutelyJob, GetMinutelyJobTrigger(param));
            }
        }
        private async Task ConfigureDailyJob(IScheduler scheduler, string param)
        {
            var dailyJob = GetDailyJob();
            if (await scheduler.CheckExists(dailyJob.Key))
            {
                await scheduler.ResumeJob(dailyJob.Key);
                _logger.Info($"The job key {dailyJob.Key} was already existed, thus resuming the same");
            }
            else
            {
                await scheduler.ScheduleJob(dailyJob, GetDailyJobTrigger(param));
            }
        }

        private async Task ConfigureWeeklyJob(IScheduler scheduler, string param)
        {
            var weklyJob = GetWeeklyJob();
            if (await scheduler.CheckExists(weklyJob.Key))
            {
                await scheduler.ResumeJob(weklyJob.Key);
                _logger.Info($"The job key {weklyJob.Key} was already existed, thus resuming the same");
            }
            else
            {
                await scheduler.ScheduleJob(weklyJob, GetWeeklyJobTrigger(param));
            }
        }


        #region "Private Functions"
        private IServiceProvider GetConfiguredServiceProvider()
        {
            var services = new ServiceCollection()
                .AddScoped<ISecondlyJob, SecondlyJob>()
                .AddScoped<IMinutelyJob, MinutelyJob>()
                .AddScoped<IDailyJob, DailyJob>()
                .AddScoped<IWeeklyJob, WeeklyJob>()
                .AddScoped<IExecService, ExecService>();
            return services.BuildServiceProvider();
        }
        
        private IJobDetail GetSecondlyJob()
        {
            return JobBuilder.Create<ISecondlyJob>()
                .WithIdentity("secondlyjob", "secondlygroup")
                .Build();
        }
        private ITrigger GetSecondlyJobTrigger(string param)
        {
            Int32 nSec = 10; Int32.TryParse(param, out nSec);
            return TriggerBuilder.Create()
                 .WithIdentity("secondlytrigger", "secondlygroup")
                 .StartNow()
                 .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(nSec)
                    .RepeatForever())
                 .Build();
        }

        private IJobDetail GetMinutelyJob()
        {
            return JobBuilder.Create<IMinutelyJob>()
                .WithIdentity("minutelyjob", "minutelygroup")
                .Build();
        }
        private ITrigger GetMinutelyJobTrigger(string param)
        {
            Int32 nMin = 3; Int32.TryParse(param, out nMin);
            return TriggerBuilder.Create()
                 .WithIdentity("Minutelytrigger", "Minutelygroup")
                 .StartNow()
                 .WithSimpleSchedule(x => x
                     .WithIntervalInMinutes(nMin)
                     .RepeatForever())
                 .Build();
        }

        private IJobDetail GetDailyJob()
        {
            return JobBuilder.Create<IDailyJob>()
                .WithIdentity("dailyjob", "dailygroup")
                .Build();
        }
        private ITrigger GetDailyJobTrigger(string param)
        {
            return TriggerBuilder.Create()
                 .WithIdentity("dailytrigger", "dailygroup")
                 .StartNow()
                 .WithSimpleSchedule(x => x
                     .WithIntervalInHours(24)
                     .RepeatForever())
                 .Build();
        }

        private IJobDetail GetWeeklyJob()
        {
            return JobBuilder.Create<IWeeklyJob>()
                .WithIdentity("weeklyjob", "weeklygroup")
                .Build();
        }
        private ITrigger GetWeeklyJobTrigger(string param)
        {
            return TriggerBuilder.Create()
                 .WithIdentity("weeklytrigger", "weeklygroup")
                 .StartNow()
                 .WithSimpleSchedule(x => x
                     .WithIntervalInHours(120)
                     .RepeatForever())
                 .Build();
        }

        private static async Task<IScheduler> GetScheduler()
        {
            // Comment this if you don't want to use database start
            //var config = (NameValueCollection)ConfigurationManager.GetSection("quartz");
            //var factory = new StdSchedulerFactory(config);
            // Comment this if you don't want to use database end

            // Uncomment this if you want to use RAM instead of database start
            var props = new NameValueCollection { { "quartz.serializer.type", "binary" } };
            var factory = new StdSchedulerFactory(props);
            // Uncomment this if you want to use RAM instead of database end
            var scheduler = await factory.GetScheduler();
            return scheduler;
        }
        #endregion
    }
}
