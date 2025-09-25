using data.models;

namespace web.apis
{
	public interface ITransactionRepository
	{
        Task<Transaction> Add(Transaction transaction, string userId);
        Task<Transaction> GetByTransactionReference(string transactionReference);
        IEnumerable<Transaction> GetUserTransactions(string userId);
        Task<Transaction> Update(int id, Transaction transaction, string userId);
    }
}

