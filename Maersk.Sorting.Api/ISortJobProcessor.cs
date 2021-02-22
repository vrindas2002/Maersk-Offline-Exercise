using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maersk.Sorting.Api
{
    public interface ISortJobProcessor
    {
        //Task<SortJob> Process(SortJob job);
        Task<SortJob> ProcessById(Guid jobId, Queue<SortJob> job);
        Task<SortJob> Process(Queue<SortJob> job);
    }
}