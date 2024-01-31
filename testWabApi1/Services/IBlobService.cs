namespace testWabApi1.Services
{
    public interface IBlobService
    {
        Task<string> UploadBlobAsync(/*int bookId,*/ IFormFile file);
        Task<bool> DeleteBlobAsync(int bookId);
    }
}
