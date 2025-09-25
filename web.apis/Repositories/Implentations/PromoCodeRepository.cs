using ILogger = Serilog.ILogger;
using data.models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace web.apis.Repositories.Implentations
{
	public class PromoCodeRepository : IPromoCodeRepository
    {
        private readonly DbConn _dbConn;
        private readonly ILogger _logger;

        public PromoCodeRepository(DbConn dbConn, ILogger logger)
        {
            _dbConn = dbConn;
            _logger = logger;
        }

        public async Task<bool> Activate(int id, string userId)
        {
            try
            {
                var emailTemplate = await _dbConn.PromoCodes
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
                _logger.Error("Error activating PromoCode", ex);
                throw new Exception(ex.Message);
            }
        }

        public async Task<PromoCode> Add(PromoCode promoCode, string userId)
        {
            try
            {
                await _dbConn.PromoCodes.AddAsync(promoCode);
                await _dbConn.SaveChangesAsync(userId);

                return promoCode;
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while adding a PromoCode";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }

        public async Task<PromoCode> Delete(int id, string userId)
        {
            try
            {
                var existingPromoCode = await _dbConn.PromoCodes
                    .Where(d => d.Id == id)
                    .FirstOrDefaultAsync();

                if (existingPromoCode == null)
                    return null;

                existingPromoCode.IsActive = false;
                await _dbConn.SaveChangesAsync(userId);

                return existingPromoCode;
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while deleting a PromoCode";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }

        public IEnumerable<PromoCode> Get(string userId)
        {
            try
            {
                return _dbConn.PromoCodes
                    .AsNoTracking()
                    .Include(p => p.UserPromoCode)
                    .Where(i => i.IsActive
                        && i.CreatorUserId == userId
                        && i.UserPromoCode == null)
                    .AsEnumerable();
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while fetching PromoCode(s)";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }

        public IEnumerable<PromoCode> GetAll()
        {
            try
            {
                return _dbConn.PromoCodes
                   .AsNoTracking()
                   .Include(p => p.UserPromoCode)
                .Where(i => i.IsActive);
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while fetching PromoCode(s)";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }

        public IEnumerable<PromoCode> GetList(List<int> ids, string userId)
        {
            try
            {
                return _dbConn.PromoCodes
                   .AsNoTracking()
                   .Include(p => p.UserPromoCode)
                   .Where(i => i.IsActive
                        && ids.Contains(i.Id)
                        && i.CreatorUserId == userId);
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while fetching PromoCode(s)";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }

        public async Task<PromoCode> GetSingle(int id, string userId)
        {
            try
            {
                return await _dbConn.PromoCodes
                   .AsNoTracking()
                   .Include(p => p.UserPromoCode)
                   .Where(i => i.IsActive
                        && i.Id == id
                        && i.CreatorUserId == userId)
                   .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while fetching a PromoCode";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }

        public async Task<PromoCode> Update(int id, PromoCode promoCode, string userId)
        {
            try
            {
                var existinPromoCode = await _dbConn.PromoCodes.FindAsync(id);
                if (existinPromoCode == null)
                    return null;

                existinPromoCode.IsActive = promoCode.IsActive;
                existinPromoCode.CreatorUserId = promoCode.CreatorUserId;
                existinPromoCode.UsageLimit = promoCode.UsageLimit;
                existinPromoCode.MinCartValue = promoCode.MinCartValue;
                existinPromoCode.Value = promoCode.Value;
                existinPromoCode.Code = promoCode.Code;
                existinPromoCode.Applicability = promoCode.Applicability;
                existinPromoCode.DisplayName = promoCode.DisplayName;
                existinPromoCode.EligibleForAllUsers = promoCode.EligibleForAllUsers;
                existinPromoCode.EligibleForNewUsers = promoCode.EligibleForNewUsers;

                await _dbConn.SaveChangesAsync(userId);

                return existinPromoCode;
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while updating a PromoCode";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }
    }
}

