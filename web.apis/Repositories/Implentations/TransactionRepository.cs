using data.models;
using Microsoft.EntityFrameworkCore;
using ILogger = Serilog.ILogger;

namespace web.apis.Repositories.Implentations
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly DbConn _dbConn;
        private readonly ILogger _logger;

        public TransactionRepository(DbConn dbConn, ILogger logger)
        {
            _dbConn = dbConn;
            _logger = logger;
        }

        public async Task<Transaction> Add(Transaction transaction, string userId)
        {
            try
            {
                await _dbConn.Transactions.AddAsync(transaction);
                await _dbConn.SaveChangesAsync(userId);

                return transaction;
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while adding a Transaction";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }

        public async Task<Transaction> GetByTransactionReference(string transactionReference)
        {
            try
            {
                return await _dbConn.Transactions
                    .Where(r => r.TransationRef == transactionReference)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while fetching a Transaction";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }

        public IEnumerable<Transaction> GetUserTransactions(string userId)
        {
            try
            {
                return _dbConn.Transactions
                    .AsNoTracking()
                    .Where(r => r.UserId == userId)
                    .AsEnumerable();
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while fetching user transactions";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }

        public async Task<Transaction> Update(int id, Transaction transaction, string userId)
        {
            try
            {
                var existingTransaction = await _dbConn.Transactions.FindAsync(id);
                if (existingTransaction == null)
                    return null;

                existingTransaction.PaymentRequestAsJson = transaction.PaymentRequestAsJson;
                existingTransaction.PaymentResponseAsJson = transaction.PaymentResponseAsJson;
                existingTransaction.TransationRef = transaction.TransationRef;
                existingTransaction.Naration = transaction.Naration;
                existingTransaction.Amount = transaction.Amount;

                await _dbConn.SaveChangesAsync(userId);

                return transaction;
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while updating a Transaction";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }
    }
}

