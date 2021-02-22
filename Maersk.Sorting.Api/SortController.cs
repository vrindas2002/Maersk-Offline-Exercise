using Hangfire;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Maersk.Sorting.Api.Controllers
{
    [ApiController]
    [Route("sort")]
    //[Route("api/[controller]/[action]")]
    public class SortController : ControllerBase
    {
        private readonly ISortJobProcessor _sortJobProcessor;
        //private readonly SerialQueue queue = new SerialQueue();
        private ConcurrentQueue<Func<CancellationToken, Task>> _workItems = new ConcurrentQueue<Func<CancellationToken, Task>>();
        private SemaphoreSlim _signal = new SemaphoreSlim(0);

        private SemaphoreSlim semaphore = new SemaphoreSlim(0);
        private readonly object queueLock = new object();
        public Queue<SortJob> queue = new Queue<SortJob>();
        private static int[] values = { 2, 4, 3 };

        private Queue<SortJob> _queue = new Queue<SortJob>();

        

        public SortController(ISortJobProcessor sortJobProcessor) //, Queue<SortJob> queue)
        {
            _sortJobProcessor = sortJobProcessor;
            //_queue = queue;

        }

        
        [HttpPost("run")]
        [Obsolete("This executes the sort job asynchronously. Use the asynchronous 'EnqueueJob' instead.")]
        //public async Task<ActionResult<SortJob>> EnqueueAndRunJob(int[] values)
        //{
        //    //var pendingJob = new SortJob(
        //    //    id: Guid.NewGuid(),
        //    //    status: SortJobStatus.Pending,
        //    //    duration: null,
        //    //    input: values,
        //    //    output: null);

        //    //var completedJob = await _sortJobProcessor.Process(pendingJob);

        //    //return Ok(completedJob);
        //    return Ok();
        //}


        [AutomaticRetry(Attempts = 2, LogEvents = true, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public static void Test()
        {
            Console.WriteLine("Background Job: Hello, world!");

            //using (WebApp.Start<Startup>("http://localhost:12345"))
            //{
            //    Console.ReadLine();
            //}
        }


        //[Route("sort/createjob")]

        //[Produces("application/json")]

        //Task<ActionResult<SortJob>>

        //[Route("api/values/{ids}")]
        [HttpPost]
        //[Route("sort/EnqueueJob")]
        public IActionResult EnqueueJob([FromBody] SortJob sortjob)//(int[] values)//([FromBody] SortJob sortjob)
        {

            //int[] values = { 2, 4, 3 };

            //var job = new SortJob(
            //  id: Guid.NewGuid(),
            //  status: SortJobStatus.Pending,
            //  duration: null,
            //  input: values,//State.ENQUEUED,
            //  output: null);


            // TODO: Should enqueue a job to be processed in the background.
            //int[] values,
            //lock (queueLock)
            //{
            //  var queue = new Queue<SortJob>(job.AsEnumerable().Reverse());

            //   var item =  job;

            //var queue = new Queue<T>(job);

            //var result = await queue.Enqueue(sortjob);//queue.Enqueue(values.Sort);

            queue.Enqueue(sortjob);
            //  semaphore.Release();

            _queue = queue;

            ////foreach (var i in queue)
            ////{
            ////    // Writes a line as soon as some other Task calls queue.Enqueue(..)
            ////    Console.WriteLine(i);
            ////}

            //var data = await queue.Enqueue(sortjob);

            //await (queue.Enqueue(sortjob));

            //return CreatedAtAction(nameof(GetJobs), new { id = sortjob.Id }, sortjob);

            return CreatedAtRoute("GetJobs", new { id = sortjob.Id }, sortjob);

            // return CreatedAtRoute("GetJobs", new { id = sortjob.Id }, sortjob);

            //return CreatedAtRoute(nameof(GetJobs), new { id = sortjob.Id }, sortjob);

            // return CreatedAtAction("",new { id = sortjob.Id }, sortjob);
            //   return //CreatedAtRoute("CustomerById", new { id = sortjob.ID }, sortjob);
            // return sortjob;
            //await foreach (int i in queue)
            //{
            //    // Writes a line as soon as some other Task calls queue.Enqueue(..)
            //    Console.WriteLine(i);
            //}

            //BackgroundJob.Enqueue(() => Test());

            //}

            //, Func<CancellationToken, Task> workItem
            //Task t = new Task();
            //_workItems.Enqueue(t);


            //if (values == null)
            //{
            //    throw new ArgumentNullException(nameof(values));
            //}

            //_workItems.Enqueue(workItem);
            //_signal.Release();

            //var queue = new AsyncQueue<int>();
            //await foreach (int i in queue)
            //{
            //    // Writes a line as soon as some other Task calls queue.Enqueue(..)
            //    Console.WriteLine(i);
            //}


            //var request = new  TaskQueue();
            //var res= await  request.Enqueue(values);

            //requestQueue.Enqueue(request);

            //var result = await queue.Enqueue(values.Sort);

            // _sortJobProcessor.();
            //throw new NotImplementedException();

            // return CreatedAtRouteResult()(nameof(GetJob), new { id = sortjob.Id }, sortjob);
            //return ;
        }



        [HttpGet(Name = "GetJobs")]
        public async Task<ActionResult<SortJob[]>> GetJobs()
        {

            Queue<SortJob> queue1 = new Queue<SortJob>();
            _queue = queue;


            //await queue.EnqueueAsync(new SortJob
            //{
            //    Data = "Hello"
            //});
            // TODO: Should return all jobs that have been enqueued (both pending and completed).

            //int[] values = { 2, 4, 3 };
            //var pendingJob = new SortJob(
            //    id: Guid.NewGuid(),
            //    status: SortJobStatus.Pending,
            //    duration: null,
            //    input: values,//State.ENQUEUED,
            //    output: null);

            //var completedJob = await _sortJobProcessor.Process(pendingJob);


            var completedJob = await _sortJobProcessor.Process(queue);
            return Ok(completedJob);

            //throw new NotImplementedException();
        }

        [HttpGet("{jobId}")]
        public async Task<ActionResult<SortJob>> GetJob(Guid jobId, Queue<SortJob> q)
        {


            // TODO: Should return a specific job by ID.
            // Queue<SortJob> queue = new Queue<SortJob>();

            var processByid = await _sortJobProcessor.ProcessById(jobId, q);

            return Ok(processByid);
            // throw new NotImplementedException();
        }

    }


}


