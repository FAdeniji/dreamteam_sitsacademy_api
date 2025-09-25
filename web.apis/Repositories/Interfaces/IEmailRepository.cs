namespace web.apis
{
	public interface IEmailRepository
	{
		Task<bool> SendEmail(string recipientName, string recipientEmail, string templateName, string extra = "", string product = "", string amount = "", string currency = "", string emailBody = "", Dictionary<string, string> keyValuePairs = null);

		Task<int> SendEmails();
	}
}

