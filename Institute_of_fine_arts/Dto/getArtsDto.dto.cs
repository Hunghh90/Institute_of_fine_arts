namespace Institute_of_fine_arts.Dto
{
    public class getArtsDto : paginationAGRSDto
    {
        public string? OrderBy { get; set; }
        public string? SortedBy { get; set; }
        public string? SearchJoin { get; set; }
        public string? Search { get; set; }

    }
}
