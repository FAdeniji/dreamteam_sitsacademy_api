using data.models;
using Microsoft.EntityFrameworkCore;
using ILogger = Serilog.ILogger;

namespace web.apis
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ILogger _logger;
        private readonly DbConn _dbConn;

        public CourseRepository(ILogger logger, DbConn dbConn)
        {
            _logger = logger;
            _dbConn = dbConn;
        }

        public async Task<Course> Add(Course course, string userId)
        {
            try
            {
                await _dbConn.Courses.AddRangeAsync(course);
                await _dbConn.SaveChangesAsync(userId);

                return course;
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while adding a course";
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
                return _dbConn.Courses
                    .AsNoTracking()
                    .Where(c => c.IsActive)
                    .AsEnumerable();
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while fetching courses";
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
                return await _dbConn.Courses.FindAsync(id);
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while fetching a course";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }

        public Task<Course> Update(int id, Course course, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
