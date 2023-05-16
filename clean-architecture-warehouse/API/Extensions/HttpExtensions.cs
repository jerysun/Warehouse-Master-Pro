using API.DTOs;
using Newtonsoft.Json;

namespace API.Extensions;

public static class HttpExtensions
{
    public static void AddPaginationHeader(this HttpResponse response, PagedDto header)
    {
        response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(header));
        response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");
    }
}
