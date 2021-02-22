using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Maersk.Sorting.Api
{
    public class SortJobProcessor : ISortJobProcessor
    {
        private readonly ILogger<SortJobProcessor> _logger;

        public SortJobProcessor(ILogger<SortJobProcessor> logger)
        {
            _logger = logger;
        }

        public async Task<SortJob> Process(Queue<SortJob> job)//(SortJob job)
        {

           
            foreach (var i in job)
            {
                Console.WriteLine(i);


                //_logger.LogInformation("Processing job with ID '{JobId}'.", job.ToList().Id);
                _logger.LogInformation("Processing job with ID '{JobId}'.",i.Id);
                var stopwatch = Stopwatch.StartNew();

                //var output = job.Input.OrderBy(n => n).ToArray();
                var output = i.Input.OrderBy(n => n).ToArray();

                var output1 = from j in job
                                   .OrderBy(n => n).ToArray()
                              where j.Id == i.Id && i.Status == SortJobStatus.Pending || i.Status == SortJobStatus.Completed
                              select new
                              {
                                  id = j.Id,
                                  input = j.Input.OrderBy(n => n).ToArray(),
                                  output = j.Output,
                                  status = j.Status,
                                  duration = j.Duration
                              };

                await Task.Delay(5000); // NOTE: This is just to simulate a more expensive operation

                var duration = stopwatch.Elapsed;

                //_logger.LogInformation("Completed processing job with ID '{JobId}'. Duration: '{Duration}'.", job.Id, duration);
                _logger.LogInformation("Completed processing job with ID '{JobId}'. Duration: '{Duration}'.", i.Id, duration);
            }

            //return output1;
           
                return new SortJob(
                id: job.FirstOrDefault().Id,
                status: SortJobStatus.Completed,
                duration: job.FirstOrDefault().Duration,
                input: job.FirstOrDefault().Input,
                output: job.FirstOrDefault().Output);
        }

        public async Task<SortJob> ProcessById(Guid jobId, Queue<SortJob> job)
        {
            _logger.LogInformation("Processing job with ID '{JobId}'.", jobId);

            var stopwatch = Stopwatch.StartNew();
           
            //foreach(var j in job) { 
            //var output = j.Input
            //        .j.id equal jobId,
            //        .OrderBy(n => n).ToArray();

                var output = from j in job
                                       .OrderBy(n=>n).ToArray()
                                       where j.Id == jobId
                                       select new {                                       
                                       id=j.Id,input=j.Input,output=j.Output,status=j.Status,duration=j.Duration}
                                       ;
            
                await Task.Delay(5000); // NOTE: This is just to simulate a more expensive operation

            var duration = stopwatch.Elapsed;

            _logger.LogInformation("Completed processing job with ID '{JobId}'. Duration: '{Duration}'.", jobId, duration);
           // if (output ==null) { return;}

            return  new SortJob(
                id: output.FirstOrDefault().id,
                status: output.FirstOrDefault().status,//SortJobStatus.Completed,
                duration: output.FirstOrDefault().duration,
                input: output.FirstOrDefault().input,
                output: output.FirstOrDefault().output);
        }
    }
}
