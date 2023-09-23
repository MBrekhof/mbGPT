using mbGPT.Module.BusinessObjects;

namespace mbGPT.Module.Services
{
    public interface IMailService
    {
        Task<bool> SendAsync(MailData mailData, CancellationToken ct);
    }
}
