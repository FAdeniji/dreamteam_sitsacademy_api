using data.models;

namespace web.apis
{
    public interface IModuleRepository
    {
        IEnumerable<Module> Get();
        IEnumerable<Module> GetByCode(string code);
        Task<Module> GetById(int id);
        Task<Module> Update(int id, Module module, string userId);
        Task<Module> Delete(int id, string userId);
        Task<Module> Add(Module module, string userId);
    }
}
