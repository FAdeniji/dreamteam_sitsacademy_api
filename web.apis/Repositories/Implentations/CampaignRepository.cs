using data.models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using ILogger = Serilog.ILogger;

namespace web.apis
{
    public class CampaignRepository : ICampaignRepository
    {

        private readonly DbConn _dbConn;
        private readonly ILogger _logger;

        public CampaignRepository(DbConn dbConn, ILogger logger)
        {
            _dbConn = dbConn;
            _logger = logger;
        }

        public async Task<bool> Activate(int id, string userId)
        {
            try
            {
                var emailTemplate = await _dbConn.Campaigns
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
                _logger.Error("Error activating Campaign", ex);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Campaign> Add(Campaign campaign, string userId)
        {
            try
            {
                await _dbConn.Campaigns.AddAsync(campaign);
                await _dbConn.SaveChangesAsync(userId);

                return campaign;
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while adding a Campaign";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }

        public async Task<Campaign> Delete(int id, string userId)
        {
            try
            {
                var existingCampaign = await _dbConn.Campaigns
                    .Where(d => d.Id == id)
                    .FirstOrDefaultAsync();

                if (existingCampaign == null)
                    return null;

                existingCampaign.IsActive = false;
                await _dbConn.SaveChangesAsync(userId);

                return existingCampaign;
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while deleting a Campaign";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }

        public IEnumerable<Campaign> Get(string userId)
        {
            try
            {
                return _dbConn.Campaigns
                    .AsNoTracking()
                    .Include(p => p.PromoCodes)
                    .Where(i => i.IsActive
                        && i.CreatorUserId == userId)
                    .AsEnumerable();
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while fetching Campaign(s)";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }

        public IEnumerable<Campaign> GetAll()
        {
            try
            {
                return _dbConn.Campaigns
                    .AsNoTracking()
                .Include(p => p.PromoCodes)
                    .Where(i => i.IsActive)
                    .AsEnumerable();
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while fetching Campaign(s)";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }

        public async Task<Campaign> GetSingle(int id, string userId)
        {
            try
            {
                return await _dbConn.Campaigns
                   .AsNoTracking()
                   .Include(p => p.PromoCodes)
                   .Where(i => i.IsActive
                        && i.Id == id
                        && i.CreatorUserId == userId)
                   .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while fetching a Campaign";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }

        public async Task<Campaign> Update(int id, Campaign campaign, string userId)
        {
            try
            {
                var existinPromoCode = await _dbConn.Campaigns.FindAsync(id);
                if (existinPromoCode == null)
                    return null;

                existinPromoCode.IsActive = campaign.IsActive;
                existinPromoCode.CreatorUserId = campaign.CreatorUserId;
                existinPromoCode.Name = campaign.Name;
                existinPromoCode.Description = campaign.Description;

                if(campaign.AudienceSegment != null)
                    existinPromoCode.AudienceSegment = campaign.AudienceSegment;

                if (campaign.ConversionGoal != null)
                    existinPromoCode.ConversionGoal = campaign.ConversionGoal;

                if (!string.IsNullOrWhiteSpace(campaign.UtmParameters))
                    existinPromoCode.UtmParameters = campaign.UtmParameters;

                await _dbConn.SaveChangesAsync(userId);

                return existinPromoCode;
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while updating a Campaign";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }
    }
}

