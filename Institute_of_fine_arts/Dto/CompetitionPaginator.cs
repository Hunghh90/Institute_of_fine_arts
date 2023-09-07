using Institute_of_fine_arts.Entities;
namespace Institute_of_fine_arts.Dto
{
    public class CompetitionPaginator
    {
        public Competition[]? Data { get; set; }
        public PaginatorInfo Pages { get; set; }

    }
}
