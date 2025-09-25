using data.models;

namespace web.apis
{
    public interface IEmailTemplateRepository
	{
        Task<bool> Activate(int id, string userId);
        IEnumerable<EmailTemplate> Get();
        IEnumerable<EmailTemplate> GetByCode(string code);
        Task<EmailTemplate> GetById(int id);
        Task<EmailTemplate> Update(int id, EmailTemplate emailTemplate, string userId);
        Task<EmailTemplate> Delete(int id, string userId);
        Task<EmailTemplate> Add(EmailTemplate emailTemplate, string userId);
    }
}

