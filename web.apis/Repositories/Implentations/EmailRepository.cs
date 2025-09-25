using System.Net;
using System.Net.Mail;
using ILogger = Serilog.ILogger;
using Microsoft.EntityFrameworkCore;

namespace web.apis
{
    public class EmailRepository : IEmailRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly DbConn _dbConn;

        public EmailRepository(IConfiguration configuration, ILogger logger, DbConn dbConn)
        {
            _configuration = configuration;
            _logger = logger;
            _dbConn = dbConn;
        }

        public async Task<bool> SendEmail(string recipientName, string recipientEmail, string templateName, string extra = "", string product = "", string amount = "", string currency = "", string emailBody = "", Dictionary<string, string> keyValuePairs = null)
        {
            try
            {
                var emailTemplate = await _dbConn.EmailTemplates.Where(e => e.Code == templateName).FirstOrDefaultAsync();
                if (emailTemplate == null)
                    return false;

                // Replace sender@example.com with your "From" address. 
                // This address must be verified with Amazon SES.
                string FROM = _configuration.GetValue<string>("SMTP:SenderEmail");
                string FROMNAME = _configuration.GetValue<string>("SMTP:SenderName");

                // Replace recipient@example.com with a "To" address. If your account 
                // is still in the sandbox, this address must be verified.
                string TO = recipientEmail;
                string ToName = recipientName;

                // Replace smtp_username with your Amazon SES SMTP user name.
                string SMTP_USERNAME = _configuration.GetValue<string>("SMTP:Username");

                // Replace smtp_password with your Amazon SES SMTP password.
                string SMTP_PASSWORD = _configuration.GetValue<string>("SMTP:Password");

                // (Optional) the name of a configuration set to use for this message.
                // If you comment out this line, you also need to remove or comment out
                // the "X-SES-CONFIGURATION-SET" header below.
                // string CONFIGSET = "ConfigSet";

                // If you're using Amazon SES in a region other than US West (Oregon), 
                // replace email-smtp.us-west-2.amazonaws.com with the Amazon SES SMTP  
                // endpoint in the appropriate AWS Region.
                string HOST = _configuration.GetValue<string>("SMTP:Endpoint");

                // The port you will connect to on the Amazon SES SMTP endpoint. We
                // are choosing port 587 because we will use STARTTLS to encrypt
                // the connection.
                int PORT = _configuration.GetValue<int>("SMTP:Port");

                // Create and build a new MailMessage object
                MailMessage message = new MailMessage();
                message.IsBodyHtml = true;
                message.From = new MailAddress(FROM, FROMNAME);
                message.To.Add(new MailAddress(TO, ToName));
                message.Subject = emailTemplate.Subject;
                message.Body = string.IsNullOrWhiteSpace(emailBody) ? emailTemplate.Message : emailBody;

                if (!string.IsNullOrWhiteSpace(message.Body))
                {
                    message.Body = message.Body.Replace("{user}", recipientName);
                    message.Body = message.Body.Replace("{code}", extra);
                    message.Body = message.Body.Replace("{Product}", product);
                    message.Body = message.Body.Replace("{Amount}", amount);
                    message.Body = message.Body.Replace("{Currency}", currency);
                    message.Body = message.Body.Replace("{Email}", recipientEmail);

                    if (keyValuePairs != null && keyValuePairs.TryGetValue("organisationname", out string organisationname))
                    {
                        message.Body = message.Body.Replace("{organisationname}", organisationname);
                    }

                    if (keyValuePairs != null && keyValuePairs.TryGetValue("invitationlink", out string invitationlink))
                    {
                        message.Body = message.Body.Replace("{invitationlink}", invitationlink);
                    }

                    if (keyValuePairs != null && keyValuePairs.TryGetValue("link", out string link))
                    {
                        keyValuePairs.TryGetValue("inviteId", out string inviteId);
                        
                        keyValuePairs.TryGetValue("expiry", out string expiry);
                        var intExpiry = int.Parse(expiry);

                        message.Body = message.Body.Replace("{link}", $"{link}?inviteId={inviteId}&email={recipientEmail}&expiry={DateTime.Now.AddHours(intExpiry).Ticks}");
                    }
                }

                #region attachment ics
                //if (!string.IsNullOrWhiteSpace(fileContentResult))
                //{
                //    System.Net.Mime.ContentType contType = new System.Net.Mime.ContentType("text/calendar");
                //    contType.Parameters.Add("method", "REQUEST");
                //    contType.Parameters.Add("name", "Task.ics");
                //
                //    MemoryStream ms = new MemoryStream(Encoding.ASCII.GetBytes(fileContentResult));
                //    message.Attachments.Add(new Attachment(ms, contType));
                //}
                #endregion

                // Comment or delete the next line if you are not using a configuration set
                // message.Headers.Add("X-SES-CONFIGURATION-SET", CONFIGSET);

                using (var client = new SmtpClient(HOST, PORT))
                {
                    // Pass SMTP credentials
                    client.Credentials = new NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);

                    // Enable SSL encryption
                    client.EnableSsl = true;

                    // Try to send the message. Show status in console.
                    try
                    {
                        _logger.Information("Attempting to send email...");
                        message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
                        await client.SendMailAsync(message);

                        Thread.Sleep(5000); // Wait for 5 seconds

                        // Check the delivery status again
                        if (message.Headers["X-MSMail-Priority"] == "Normal")
                        {
                            _logger.Error("Email was delivered successfully.");
                        }
                        else if (message.Headers["X-MSMail-Priority"] == "High")
                        {
                            _logger.Error("Email delivery failed.");
                        }

                        _logger.Information("Email sent!");
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex, $"The email was not sent. Error: {ex.Message}.");
                        return false;
                    }
                }
                return true;
            }
            catch (SmtpFailedRecipientException ex)
            {
                _logger.Error(ex, $"Failure to send email. Error message (SmtpFailedRecipientException): {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Failure to send email. Error message: {ex.Message}");
                return false;
            }
        }

        public async Task<int> SendEmails()
        {
            return 0;
        }
    }
}

