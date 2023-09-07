namespace Institute_of_fine_arts.Dto
{
    public class PaginatorInfo
    {
        public int? count { get; set; }
        public int? first_item { get; set; }
        public int? current_page { get; set; }
        public int? last_Item { get; set; }
        public int? last_page { get; set; }
        public int? per_page { get; set; }
        public int? total { get; set; }
        public string? first_page_url { get; set; }
        public string? last_page_url { get; set; }
        public string? next_page_url { get; set; }
        public string? prev_page_url { get; set; }
    }
}
