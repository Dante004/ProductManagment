using System.Collections.Generic;

namespace ProductManagment.Api.Helpers
{
    public class PaginationResult<T> : Result
    {
        public IEnumerable<T> Items { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
        public int Count { get; set; }
    }
}
