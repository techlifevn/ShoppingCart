namespace DemoApi.Common.Result
{
    public class PagingRequestBase
    {
        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 20;

    }

    public class PagedResultBase : PagingRequestBase
    {
        public int TotalRecords { get; set; }

        public int PageCount
        {
            get
            {
                var pageCount = (double)TotalRecords / PageSize;
                return (int)Math.Ceiling(pageCount);
            }
        }
    }

    public class PagedResult<T> : PagedResultBase
    {
        public List<T>? Items { set; get; }
    }

    public class UpdateRequestBase
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
    }

    public class DeleteRequest : UpdateRequestBase
    {
        public string? Title { get; set; }
        public string? Caption { get; set; }
        public string? Action { get; set; }
        public string? ViewCallBack { get; set; }
    }

    public class GetPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; } = string.Empty;

    }

    public class RequestBase
    {
        public Guid UserId { get; set; }

    }
}
