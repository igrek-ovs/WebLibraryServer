using Azure.Storage.Blobs;

namespace testWabApi1.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;
        private readonly string _blobUrl = "https://librarystor.blob.core.windows.net/bookpictures-cont/";

        public BlobService(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("BlobStorageConnection");
            _blobServiceClient = new BlobServiceClient(connectionString);
            _containerName = configuration["BlobStorage:ContainerName"];
        }

        public async Task<string> UploadBlobAsync(IFormFile file)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

            if (!containerClient.Exists())
            {
                containerClient.Create();
            }

            var uniqueFileName = $"{Guid.NewGuid()}{file.FileName}";

            var blobClient = containerClient.GetBlobClient(uniqueFileName);

            await using var stream = file.OpenReadStream();

            await blobClient.UploadAsync(stream, false);

            return blobClient.Uri.ToString();
        }

        public async Task<bool> DeleteBlobAsync(string imagePath)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

            if (imagePath == null)
                return false;

            var fileName = imagePath.Replace(_blobUrl, "");

            var blobClient = containerClient.GetBlobClient(fileName);

            return await blobClient.DeleteIfExistsAsync();
        }
    }
}
