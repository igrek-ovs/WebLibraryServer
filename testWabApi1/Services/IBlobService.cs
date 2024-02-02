namespace testWabApi1.Services
{
    public interface IBlobService
    {
        Task<string> UploadBlobAsync(IFormFile file);
        Task<bool> DeleteBlobAsync(string imagePath);
    }
}
