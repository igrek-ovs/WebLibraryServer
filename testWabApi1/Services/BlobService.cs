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

        public async Task<string> UploadBlobAsync(/*int bookId,*/ IFormFile file)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

            if (!containerClient.Exists())
            {
                containerClient.Create();
            }

            //var uniqueFileName = $"{bookId}.png";
            var uniqueFileName = $"{Guid.NewGuid()}{file.FileName}";
            var blobClient = containerClient.GetBlobClient(uniqueFileName);

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, false);
            }

            return blobClient.Uri.ToString();
        }

        public void DeleteBlobAsync(int bookId)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

            

            var fileName = $"{bookId}.png";

            var blobClient = containerClient.GetBlobClient(fileName);

            blobClient.DeleteIfExistsAsync();
        }
    }
}
