namespace testWabApi1.Services
{
    public interface IBlobService
    {
        Task<string> UploadBlobAsync(/*int bookId,*/ IFormFile file);
        void DeleteBlobAsync(int bookId);
    }
}
