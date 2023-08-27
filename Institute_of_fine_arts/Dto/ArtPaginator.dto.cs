using Institute_of_fine_arts.Entities;

namespace Institute_of_fine_arts.Dto
{
    public class ArtPaginator
    {
        public Art[]? Data {get; set;}
        public PaginatorInfo PaginatorInfo { get; set;}
    }
}
