using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Worker;

namespace WorkerTest
{
    class Program
    {
        static IScheduledJob ScheduledJob;

        static Program()
        {
            var config = new WorkerConfig() { RunImmediately = true, DataValue = "Passed value" };
            // TODO ReadOptionsFromConfig();
            ScheduledJob = new ScheduledJob(config, new JobKey("HelloWorldJob"));
        }

        static void Main(string[] args)
        {
            ScheduledJob.Run();
        }
    }
}
