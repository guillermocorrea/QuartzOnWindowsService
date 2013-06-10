using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace Worker
{
    /// <summary>
    /// Configure, create, trigger the job
    /// </summary>
    public class ScheduledJob : IScheduledJob
    {
        private WorkerConfig workerConfig;
        private IScheduler scheduler;
        private IJobDetail jobDetail;
        private JobKey jobKey;

        public ScheduledJob(WorkerConfig config, JobKey jobKey)
        {
            this.workerConfig = config;
            this.jobKey = jobKey;
        }

        public void Run()
        {
            // TODO Initialiaze Scheduler
            scheduler = new StdSchedulerFactory().GetScheduler();
            scheduler.Start();

            // Let's generate our email job detail now
            CreateJob();

            // And finally, schedule the job
            ScheduleJob();

            // Run immediately?
            if (this.workerConfig.RunImmediately)
            {
                scheduler.TriggerJob(this.jobKey);
            }
        }

        /// <summary>
        /// Create the job detail
        /// </summary>
        private void CreateJob()
        {
            this.jobDetail = JobBuilder.Create<HelloWorldJob>()
                .WithIdentity(jobKey)
                .Build();
            this.jobDetail.JobDataMap.Put("DataKey", workerConfig.DataValue);
        }

        /// <summary>
        /// Create the trigger and schedule the job
        /// </summary>
        private void ScheduleJob()
        {
            // Let's create a trigger that fires immediately
            var trigger = (ICronTrigger)TriggerBuilder.Create()
                .WithIdentity("WriteHelloToLog", "IT")
                .WithCronSchedule("0 0/1 * 1/1 * ? *") // visit http://www.cronmaker.com/ Queues the job every minute
                .StartAt(DateTime.UtcNow)
                .WithPriority(1)
                .Build();
            scheduler.ScheduleJob(jobDetail, trigger);
        }
    }
}
