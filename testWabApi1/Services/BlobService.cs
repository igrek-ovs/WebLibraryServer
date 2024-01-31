using Azure.Storage.Blobs;

namespace testWabApi1.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

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

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, false);
            }

            return blobClient.Uri.ToString();
        }

        public async Task<bool> DeleteBlobAsync(int bookId)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

            var fileName = $"{bookId}.png";

            var blobClient = containerClient.GetBlobClient(fileName);

            return await blobClient.DeleteIfExistsAsync();
        }
    }
}
