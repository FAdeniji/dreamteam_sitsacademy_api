namespace web.apis
{
    public interface IS3UploadFileRepository
	{
        Task<S3UploadViewModel> UploadFile(string keyName, IFormFile file);
        Task<S3UploadViewModel> UploadFileToS3(string keyName, IFormFile file);
        string GeneratePreSignedUrl(string fileName, bool extendLink = false);
        string ExtractFileNameFromUrl(string url);
    }
}

