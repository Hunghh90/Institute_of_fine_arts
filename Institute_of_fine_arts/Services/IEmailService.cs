using Institute_of_fine_arts.Dto;

namespace Institute_of_fine_arts.Services
{
    public interface IEmailService
    {
        void SendEmail(EmailDto request);
    }
}
