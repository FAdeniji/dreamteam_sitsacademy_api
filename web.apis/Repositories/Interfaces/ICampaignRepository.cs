using data.models;

namespace web.apis
{
    public interface ICampaignRepository
    {
        Task<bool> Activate(int id, string userId);
        Task<Campaign> Add(Campaign campaign, string userId);
        Task<Campaign> Delete(int id, string userId);
        IEnumerable<Campaign> Get(string userId);
        Task<Campaign> GetSingle(int id, string userId);
        IEnumerable<Campaign> GetAll();
        Task<Campaign> Update(int id, Campaign campaign, string userId);
    }
}

