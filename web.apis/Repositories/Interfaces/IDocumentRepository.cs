using data.models;

namespace web.apis
{
    public interface IDocumentRepository
	{
        Task<Document> Add(Document document, string userId);
        Task<Document> Delete(int id, string userId);
        IEnumerable<Document> Get(string userId);
        IEnumerable<Document> Get(List<int> ids);
        IEnumerable<Document> GetByEmailAsuserId(List<string> emails);
        IEnumerable<Document> GetPartialByName(string name);
        Task<Document> GetSingle(int id);
        Task<Document> Update(int id, Document document, string userId);
        Document UpdateNA(int id, Document document, string userId);
        IEnumerable<Document> CheckExpiry(List<Document> documents, int accessDuration);
    }
}

