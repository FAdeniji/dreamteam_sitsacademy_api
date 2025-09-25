using data.models;
using Microsoft.EntityFrameworkCore;
using ILogger = Serilog.ILogger;

namespace web.apis
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly DbConn _dbConn;
        private readonly ILogger _logger;
        private readonly IS3UploadFileRepository _IS3UploadFileRepository;

        public DocumentRepository(DbConn dbConn, ILogger logger, IS3UploadFileRepository IS3UploadFileService) 
        {
            _dbConn = dbConn;
            _logger = logger;
            _IS3UploadFileRepository = IS3UploadFileService;
        }

        public async Task<Document> Add(Document document, string userId)
        {
            try
            {
                await _dbConn.Documents.AddAsync(document);
                await _dbConn.SaveChangesAsync(userId);

                return document;
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while adding a Document";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }

        public IEnumerable<Document> CheckExpiry(List<Document> documents, int accessDuration)
        {
            try
            {
                foreach (var existingDocument in documents)
                {
                    var expiryDate = DateTime.UtcNow.AddHours(accessDuration);
                    if (DateTime.UtcNow > existingDocument.ExpiryDate)
                    {
                        // regenerate url
                        var documentUrl = _IS3UploadFileRepository.GeneratePreSignedUrl(existingDocument.FileName);

                        // update document
                        var document = documents.Where(d => d.Id == existingDocument.Id).FirstOrDefault();
                        if (document != null)
                        {
                            document.Path = documentUrl;
                            document.ExpiryDate = expiryDate;

                            UpdateNA(document.Id, document, document.UserId);

                            existingDocument.FileName = documentUrl;
                        }
                    }
                };

                return documents;
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while updating a Document";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }

        public async Task<Document> Delete(int id, string userId)
        {
            try
            {
                var existingDocument = await _dbConn.Documents
                    .Where(d => d.Id == id)
                    .FirstOrDefaultAsync();

                if (existingDocument == null)
                    return null;

                existingDocument.IsActive = false;
                await _dbConn.SaveChangesAsync(userId);

                return existingDocument;
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while deleting a Document";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }

        public IEnumerable<Document> Get(string userId)
        {
            try
            {
                return _dbConn.Documents
                    .AsNoTracking()
                    .Where(i => i.IsActive
                        && i.UserId == userId)
                    .AsEnumerable();
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while fetching Document(s)";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }

        public IEnumerable<Document> GetByEmailAsuserId(List<string> emails)
        {
            try
            {
                return _dbConn.Documents
                    .AsNoTracking()
                    .Where(i => i.IsActive
                        && emails.Contains(i.UserId))
                    .AsEnumerable();
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while fetching Document(s)";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }

        public IEnumerable<Document> Get(List<int> ids)
        {
            try
            {
                return _dbConn.Documents
                    .AsNoTracking()
                    .Where(i => i.IsActive
                        && ids.Contains(i.Id))
                    .AsEnumerable();
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while fetching Document(s)";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }

        public IEnumerable<Document> GetPartialByName(string name)
        {
            try
            {
                return _dbConn.Documents
                    .AsNoTracking()
                    .Where(i => i.IsActive
                           && name.Contains(name))
                    .AsEnumerable();
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while fetching Document(s)";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }

        public async Task<Document> GetSingle(int id)
        {
            try
            {
                return await _dbConn.Documents
                   .AsNoTracking()
                   .Where(i => i.IsActive
                        && i.Id == id)
                   .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while fetching a Document";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }

        public async Task<Document> Update(int id, Document Document, string userId)
        {
            try
            {
                var existingDocument = await _dbConn.Documents.FindAsync(id);
                if (existingDocument == null)
                    return null;

                existingDocument.IsActive = Document.IsActive;
                existingDocument.UserId = Document.UserId;
                existingDocument.TypeId = Document.TypeId;
                existingDocument.FileName = Document.FileName;
                existingDocument.ActualFileName = Document.ActualFileName;
                existingDocument.Path = Document.Path;
                existingDocument.Url = Document.Url;
                existingDocument.Description = Document.Description;

                if (!string.IsNullOrWhiteSpace(Document.Url))
                    existingDocument.Url = Document.Url;

                if (!string.IsNullOrWhiteSpace(Document.Description))
                    existingDocument.Description = Document.Description;

                await _dbConn.SaveChangesAsync(userId);

                return existingDocument;
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while updating a Document";
                _logger.Error(extraInfo, ex.Message);
                throw new Exception(extraInfo);
            }
        }

        public Document UpdateNA(int id, Document Document, string userId)
        {
            try
            {
                var existingDocument = _dbConn.Documents
                    .Where(d => d.Id == id)
                    .FirstOrDefault();

                if (existingDocument == null)
                    return null;

                existingDocument.IsActive = Document.IsActive;
                existingDocument.UserId = Document.UserId;
                existingDocument.TypeId = Document.TypeId;
                existingDocument.FileName = Document.FileName;
                existingDocument.ActualFileName = Document.ActualFileName;
                existingDocument.Path = Document.Path;

                _dbConn.SaveChanges(userId);

                return existingDocument;
            }
            catch (Exception ex)
            {
                var extraInfo = $"An error occurred while updating a Document";
                _logger.Error(extraInfo, ex.Message);
                // throw new Exception(extraInfo);
                return Document;
            }
        }
    }
}