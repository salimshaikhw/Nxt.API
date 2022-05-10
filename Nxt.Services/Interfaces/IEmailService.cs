using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nxt.Services.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(IEnumerable<string> to, string subject, string content,
            IEnumerable<string> attachmentFiles = null, bool isHtml = true);
    }
}
