using System.Text.Json;
using API.Helpers;
using Microsoft.AspNetCore.Http;

namespace API._Extensions
{
    public static class AddingPaginationHeaderExtension
    {
        public static void AddPaginationHeader(this HttpResponse response, int currentPage,
         int itemPerPage, int totalItems, int totalPages)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var pagination = new PaginationHeader(currentPage, itemPerPage, totalItems, totalPages);
            response.Headers.Add("Pagination", JsonSerializer.Serialize(pagination, options));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");


        }

    }
}