using data.models;
using Microsoft.EntityFrameworkCore;
using ILogger = Serilog.ILogger;

namespace web.apis
{
    public class ModuleRepository : IModuleRepository
    {
        private readonly ILogger _logger;
        private readonly DbConn _dbConn;

        public ModuleRepository(ILogger logger, DbConn dbConn)
        {
            _logger = logger;
            _dbConn = dbConn;
        }

        public async Task<Module> Add(Module module, string userId)
        {
            try
            {
                await _dbConn.Modules.AddRangeAsync(module);
                await _dbConn.SaveChangesAsync(userId);

                return module;
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while adding a module";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }

        public Task<Module> Delete(int id, string userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Module> Get()
        {
            try
            {
                return _dbConn.Modules
                    .AsNoTracking()
                    .Where(e => e.IsActive)
                    .AsEnumerable();
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while fetching modules";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }

        public IEnumerable<Module> GetByCode(string code)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Module> GetByProductCode(string productcode)
        {
            try
            {
                return _dbConn.Modules
                    .AsNoTracking()
                    .Where(e => e.IsActive
                        && e.ProductCode == productcode)
                    .AsEnumerable();
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while fetching modules";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }

        public async Task<Module> GetById(int id)
        {
            try
            {
                return await _dbConn.Modules.FindAsync(id);
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while fetching a module";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }

        public Task<Module> Update(int id, Module module, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
