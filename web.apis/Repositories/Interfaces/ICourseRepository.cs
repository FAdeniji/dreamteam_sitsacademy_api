using data.models;

namespace web.apis
{
    public interface ICourseRepository
    {
        IEnumerable<Course> Get();
        IEnumerable<Course> GetByCode(string code);
        Task<Course> GetById(int id);
        Task<Course> Update(int id, Course course, string userId);
        Task<Course> Delete(int id, string userId);
        Task<Course> Add(Course course, string userId);
    }
}
