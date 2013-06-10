using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using Worker;

namespace WindowsServiceWorkerHost
{
    public partial class QuartzService : ServiceBase
    {
        private IScheduledJob ScheduledJob;

        public QuartzService()
        {
            InitializeComponent();
            var config = new WorkerConfig() { RunImmediately = true, DataValue = "Passed value" };
            ScheduledJob = new ScheduledJob(config, new JobKey("HelloWorldJob"));
        }

        protected override void OnStart(string[] args)
        {
            //ScheduledJob.Run();
            // TODO Initialiaze Scheduler
            var scheduler = new StdSchedulerFactory().GetScheduler();
            scheduler.Start();

            var jobDetail = JobBuilder.Create<HelloWorldJob>()
                .WithIdentity(new JobKey("HelloWorldJob"))
                .Build();
            jobDetail.JobDataMap.Put("DataKey", "Passed value");
            jobDetail.JobDataMap.Put("FilePath", System.Configuration.ConfigurationManager.AppSettings["FilePath"] as string);

            // Let's create a trigger that fires immediately
            var trigger = (ICronTrigger)TriggerBuilder.Create()
                .WithIdentity("WriteHelloToLog", "IT")
                .WithCronSchedule("0 0/1 * 1/1 * ? *") // visit http://www.cronmaker.com/ Queues the job every minute
                .StartAt(DateTime.UtcNow)
                .WithPriority(1)
                .Build();
            scheduler.ScheduleJob(jobDetail, trigger);

            scheduler.TriggerJob(new JobKey("HelloWorldJob"));
        }

        protected override void OnStop()
        {
        }
    }
}
