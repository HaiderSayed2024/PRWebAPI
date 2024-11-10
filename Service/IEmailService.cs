using PRWebAPI.Models;

namespace PRWebAPI.Service
{
    public interface IEmailService
    {
        Task SendEmailAsync(Mailrequest mailrequest);
    }
}
