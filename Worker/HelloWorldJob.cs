using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using Quartz;

namespace Worker
{
    /// <summary>
    /// A Hello World Job
    /// </summary>
    public class HelloWorldJob : IJob
    {
        private const string fileName = "log";

        public void Execute(IJobExecutionContext context)
        {
            using (System.IO.StreamWriter fs = new System.IO.StreamWriter(context.MergedJobDataMap["FilePath"] as string, true))
            {
                var info = string.Format("{0}****{0}Job {1} fired @ {2} value next scheduled for {3}{0}***{0}",
                                                                        Environment.NewLine,
                                                                        context.JobDetail.Key,
                                                                        context.FireTimeUtc.Value.Subtract(new TimeSpan(5,0,0)).ToString("r"),
                                                                        context.NextFireTimeUtc.Value.Subtract(new TimeSpan(5,0,0)).ToString("r"));
                fs.Write(info);
                var data = string.Format("{0}***{0}Hello World! passed value:{1}{0}***{0}", Environment.NewLine, context.MergedJobDataMap["DataKey"] as string);
                fs.Write(data);
            }
        }
    }
}
