using data.models;

namespace web.apis
{
	public interface IPromoCodeRepository
	{
        Task<bool> Activate(int id, string userId);
        Task<PromoCode> Add(PromoCode promoCode, string userId);
        Task<PromoCode> Delete(int id, string userId);
        IEnumerable<PromoCode> Get(string userId);
        Task<PromoCode> GetSingle(int id, string userId);
        IEnumerable<PromoCode> GetList(List<int> ids, string userId);
        IEnumerable<PromoCode> GetAll();
        Task<PromoCode> Update(int id, PromoCode promoCode, string userId);
    }
}