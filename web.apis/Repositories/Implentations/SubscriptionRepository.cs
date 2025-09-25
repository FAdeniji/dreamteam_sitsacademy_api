using AutoMapper;
using common.data.Enums;
using data.models;
using Microsoft.EntityFrameworkCore;
using web.apis.ViewModels;
using ILogger = Serilog.ILogger;

namespace web.apis
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly DbConn _dbConn;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public SubscriptionRepository(DbConn dbConn, ILogger logger, IMapper mapper)
        {
            _dbConn = dbConn;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<bool> Activate(int id, string userId)
        {
            try
            {
                var subscription = await _dbConn.Subscriptions
                    .Where(u => u.Id == id)
                    .FirstOrDefaultAsync();

                if (subscription == null)
                    return false;

                subscription.IsActive = true;
                await _dbConn.SaveChangesAsync(userId);

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error("Error activating subscription", ex);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Subscription> Add(Subscription subscription, string userId)
        {
            try
            {
                await _dbConn.Subscriptions.AddAsync(subscription);
                await _dbConn.SaveChangesAsync(userId);

                return subscription;
            }
            catch (Exception ex)
            {
                _logger.Error("Error adding subscription", ex);
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserSubscription> AddUserSubscription(Subscription subscription, string userId, string loggedInUserId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(loggedInUserId))
                    loggedInUserId = "System";

                var userSubscription = new UserSubscription(subscription.Id, userId, DateTime.UtcNow.AddMonths(subscription.ExpiryInMonths), subscription.Price);
                await _dbConn.UserSubscriptions.AddAsync(userSubscription);
                await _dbConn.SaveChangesAsync(loggedInUserId);

                return userSubscription;
            }
            catch (Exception ex)
            {
                _logger.Error("Error adding subscription", ex);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Subscription> Delete(int id, Subscription subscription, string userId)
        {
            try
            {
                var deletedSubscription = await _dbConn.Subscriptions.FindAsync(id);
                if (deletedSubscription == null)
                    return null;

                deletedSubscription.IsActive = false;
                await _dbConn.SaveChangesAsync(userId);

                return subscription;
            }
            catch (Exception ex)
            {
                _logger.Error("Error deleting subscription", ex);
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<Subscription> Get()
        {
            try
            {
                return _dbConn.Subscriptions
                    .AsNoTracking()
                    .AsEnumerable();
            }
            catch (Exception ex)
            {
                _logger.Error("Error fetching subscription", ex);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Subscription> GetInvestorSubscription()
        {
            return await _dbConn.Subscriptions
                .AsNoTracking()
                .Where(s => s.Topic == "Investors Subscription")
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetNoIfIdeasForSubscriptionByTopic(string name)
        {
            try
            {
                return await _dbConn.Subscriptions
                    .AsNoTracking()
                    .Where(u => u.Topic == name)
                    .Select(s => s.NoOfIdeas)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.Error("Error adding user subscription", ex);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Subscription> GetSingle(int id)
        {
            return await _dbConn.Subscriptions.FindAsync(id);
        }

        public async Task<Subscription> GetLISubscription()
        {
            return await _dbConn.Subscriptions.Where(s => s.Topic == "Education Subscription").FirstOrDefaultAsync();
        }

        public async Task<UserSubscription> GetUserSubscription(string userId, bool ant = true)
        {
            try
            {
                var currentDate = DateTime.UtcNow;

                var user = await _dbConn.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();

                var query = _dbConn.UserSubscriptions
                    .Include(s => s.Subscription)
                    .Where(u => u.UserId == userId);

                if (ant)
                    query = query.AsNoTracking().OrderByDescending(u => u.DateAdded);

                var res = await query
                    .FirstOrDefaultAsync();

                // check if its expired and default to free subscription
                if (res == null && user != null && user.UserRoleEnum == common.data.Enums.UserRoleEnum.Administrator)
                {
                    var userSub = new UserSubscription(1, userId, currentDate.AddMonths(1), 0);
                    await _dbConn.UserSubscriptions.AddAsync(userSub);
                    await _dbConn.SaveChangesAsync(userId);

                    res = userSub;
                }
                else if (res == null && user != null)
                {
                    if (res == null)
                    {
                        var userSub = new UserSubscription(1, userId, currentDate.AddMonths(1), 0);
                        await _dbConn.UserSubscriptions.AddAsync(userSub);
                        await _dbConn.SaveChangesAsync(userId);

                        res = userSub;
                    }
                }
                else if (currentDate >= res.ExpiryDate)
                {
                    query = _dbConn.UserSubscriptions
                    .Include(s => s.Subscription)
                    .Where(u => u.UserId == userId
                        && u.SubscriptionId == 1);

                    if (ant)
                        query = query.AsNoTracking().OrderByDescending(u => u.DateAdded);

                    res = await query
                        .FirstOrDefaultAsync();

                    if (res == null)
                    {
                        var userSub = new UserSubscription(1, userId, currentDate.AddMonths(1), 0);
                        await _dbConn.UserSubscriptions.AddAsync(userSub);
                        await _dbConn.SaveChangesAsync(userId);

                        res = userSub;
                    }
                }

                return res;
            }
            catch (Exception ex)
            {
                _logger.Error("Error adding user subscription", ex);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Subscription> Update(Subscription subscription, string userId)
        {
            try
            {
                var existingSubscription = await _dbConn.Subscriptions
                    .FindAsync(subscription.Id);

                if (existingSubscription == null)
                    return null;

                existingSubscription.Description = subscription.Description;
                existingSubscription.ExpiryInMonths = subscription.ExpiryInMonths;
                existingSubscription.NoOfIdeas = subscription.NoOfIdeas;
                existingSubscription.Price = subscription.Price;
                existingSubscription.IsActive = subscription.IsActive;
                existingSubscription.ColourCode = subscription.ColourCode;

                await _dbConn.SaveChangesAsync(userId);

                return existingSubscription;
            }
            catch (Exception ex)
            {
                _logger.Error("Error adding updating subscription", ex);
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserSubscription> UpdateUserSubscription(Subscription subscription, string userId, string loggedInUserId)
        {
            try
            {
                var usub = await _dbConn.UserSubscriptions
                    .Where(u => u.UserId == userId)
                    .FirstOrDefaultAsync();

                if (usub == null)
                    return null;

                usub.SubscriptionId = subscription.Id;

                usub.ExpiryDate = usub.ExpiryDate.AddMonths(subscription.ExpiryInMonths);
                usub.Subscription = subscription;

                await _dbConn.SaveChangesAsync(loggedInUserId);

                return usub;
            }
            catch (Exception ex)
            {
                _logger.Error("Error updating user subscription", ex);
                throw new Exception(ex.Message);
            }
        }
    }
}

