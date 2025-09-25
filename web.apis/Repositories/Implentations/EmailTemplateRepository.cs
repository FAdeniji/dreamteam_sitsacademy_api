using data.models;
using Microsoft.EntityFrameworkCore;
using ILogger = Serilog.ILogger;

namespace web.apis
{
    public class EmailTemplateRepository : IEmailTemplateRepository
    {
        private readonly ILogger _logger;
        private readonly DbConn _dbConn;

        public EmailTemplateRepository(ILogger logger, DbConn dbConn)
        {
            _logger = logger;
            _dbConn = dbConn;
        }

        public async Task<bool> Activate(int id, string userId)
        {
            try
            {
                var emailTemplate = await _dbConn.EmailTemplates
                    .Where(u => u.Id == id)
                    .FirstOrDefaultAsync();

                if (emailTemplate == null)
                    return false;

                emailTemplate.IsActive = true;
                await _dbConn.SaveChangesAsync(userId);

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error("Error activating category", ex);
                throw new Exception(ex.Message);
            }
        }

        public async Task<EmailTemplate> Add(EmailTemplate emailTemplate, string userId)
        {
            try
            {
                await _dbConn.EmailTemplates.AddRangeAsync(emailTemplate);
                await _dbConn.SaveChangesAsync(userId);

                return emailTemplate;
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while adding an email template";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }

        public async Task<EmailTemplate> Delete(int id, string userId)
        {
            try
            {
                var singleEmailTemplate = await _dbConn.EmailTemplates.FindAsync(id);
                if (singleEmailTemplate == null)
                    return singleEmailTemplate;

                singleEmailTemplate.IsActive = false;

                await _dbConn.SaveChangesAsync(userId);

                return singleEmailTemplate;
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while deleting an email template";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }

        public IEnumerable<EmailTemplate> Get()
        {
            try
            {
                return _dbConn.EmailTemplates
                    .AsNoTracking()
                    //.Where(e => e.IsActive)
                    .AsEnumerable();
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while fetching email templates";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }

        public IEnumerable<EmailTemplate> GetByCode(string code)
        {
            try
            {
                return _dbConn.EmailTemplates
                    .AsNoTracking()
                    .Where(e => e.Code == code)
                    .AsEnumerable();
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while fetching email templates";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }

        public async Task<EmailTemplate> GetById(int id)
        {
            try
            {
                return await _dbConn.EmailTemplates.FindAsync(id);
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while fetching an email template";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }

        public async Task<EmailTemplate> Update(int id, EmailTemplate emailTemplate, string userId)
        {
            try
            {
                var singleEmailTemplate = await _dbConn.EmailTemplates.FindAsync(id);
                if (singleEmailTemplate == null)
                    return singleEmailTemplate;

                singleEmailTemplate.Code = emailTemplate.Code;
                singleEmailTemplate.Subject = emailTemplate.Subject;
                singleEmailTemplate.Message = emailTemplate.Message;
                singleEmailTemplate.IsActive = emailTemplate.IsActive;
                //singleEmailTemplate.UpdatedAt = DateTime.Today;

                await _dbConn.SaveChangesAsync(userId);

                return singleEmailTemplate;
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while deleting an email template";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }
    }
}

