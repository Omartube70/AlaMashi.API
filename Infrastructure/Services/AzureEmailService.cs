using Application.Common.Interfaces;
using Application.Interfaces;
using Azure;
using Azure.Communication.Email;
using Microsoft.Extensions.Options;
using System.Runtime;
using System.Threading.Tasks;

public class AzureEmailService : IEmailService
{
    private readonly AzureEmailSettings _emailSettings;

    public AzureEmailService(IOptions<AzureEmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
    {
        var emailClient = new EmailClient(_emailSettings.ConnectionString);

        var emailContent = new EmailContent(subject)
        {
            Html = htmlBody
        };

        var recipients = new EmailRecipients(new List<EmailAddress> {
            new EmailAddress(toEmail)
        });

        var emailMessage = new EmailMessage(
            senderAddress: _emailSettings.SenderAddress,
            recipients: recipients,
            content: emailContent);

        try
        {
            await emailClient.SendAsync(WaitUntil.Completed, emailMessage);
        }
        catch (RequestFailedException ex)
        {
            throw;
        }
    }

}
