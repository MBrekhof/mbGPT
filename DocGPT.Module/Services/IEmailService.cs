using DocGPT.Module.BusinessObjects;

namespace DocGPT.Module.Services
{
    public interface IMailService
    {
        Task<bool> SendAsync(MailData mailData, CancellationToken ct);
    }
}
