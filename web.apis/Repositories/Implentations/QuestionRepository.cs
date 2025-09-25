using data.models;
using Microsoft.EntityFrameworkCore;
using ILogger = Serilog.ILogger;

namespace web.apis
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly ILogger _logger;
        private readonly DbConn _dbConn;

        public QuestionRepository(ILogger logger, DbConn dbConn)
        {
            _logger = logger;
            _dbConn = dbConn;
        }

        public async Task<Course> Add(Course question, string userId)
        {
            try
            {
                await _dbConn.Questions.AddRangeAsync(question);
                await _dbConn.SaveChangesAsync(userId);

                return question;
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while adding a question";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }

        public Task<Course> Delete(int id, string userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Course> Get()
        {
            try
            {
                return _dbConn.Questions
                    .AsNoTracking()
                    .Where(e => e.IsActive)
                    .AsEnumerable();
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while fetching questions";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }

        public IEnumerable<Course> GetByCode(string code)
        {
            throw new NotImplementedException();
        }

        public async Task<Course> GetById(int id)
        {
            try
            {
                return await _dbConn.Questions.FindAsync(id);
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while fetching a question";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }

        public Task<Course> Update(int id, Course question, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
