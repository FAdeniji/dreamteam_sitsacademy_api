using data.models;

namespace web.apis
{
    public interface IQuestionRepository
    {
        IEnumerable<Course> Get();
        IEnumerable<Course> GetByCode(string code);
        Task<Course> GetById(int id);
        Task<Course> Update(int id, Course question, string userId);
        Task<Course> Delete(int id, string userId);
        Task<Course> Add(Course question, string userId);
    }
}
