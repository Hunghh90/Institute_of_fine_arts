namespace Institute_of_fine_arts.Dto
{
    public class PaginatorInfo
    {
        public int? Count { get; set; }
        public int? First_item { get; set; }
        public int? Current_page { get; set; }
        public int? Last_Item { get; set; }
        public int? Last_page { get; set; }
        public int? Per_page { get; set; }
        public int? Total { get; set; }
        public string? First_page_url { get; set; }
        public string? Last_page_url { get; set; }
        public string? Next_page_url { get; set; }
        public string? Prev_page_url { get; set; }
    }
}
