using Institute_of_fine_arts.Dto;

namespace Institute_of_fine_arts.Helpers
{
     
    public static class PaginationHelper
    {
       
        public static PaginatorInfo paginate(int totalItems, int current_page, int pageSize, int count, string url)
        {
            const string APP_URL = "http://localhost:5000/api";
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            if (current_page < 1)
            {
                current_page = 1;
            }
            else if (current_page > totalPages)
            {
                current_page = totalPages;
            }

            int startIndex = (current_page - 1) * pageSize;
            int endIndex = Math.Min(startIndex + pageSize - 1, totalItems - 1);

            return new PaginatorInfo
            {
                Total = totalItems,
                Current_page = current_page,
                Count = count,
                Last_page = totalPages,
                First_item = startIndex,
                Last_Item = endIndex,
                Per_page = pageSize,
                First_page_url = $"{APP_URL}{url}&page=1",
                Last_page_url = $"{APP_URL}{url}&page={totalPages}",
                Next_page_url = totalPages > current_page ? $"{APP_URL}{url}&page={current_page + 1}" : null,
                Prev_page_url = current_page > 1 ? $"{APP_URL}{url}&page={current_page - 1}" : null
            };
        }
    }
}
