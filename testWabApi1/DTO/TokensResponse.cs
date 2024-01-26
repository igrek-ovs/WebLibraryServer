using testWabApi1.Models;

namespace testWabApi1.DTO
{
    public class TokensResponse
    {
        public string AccessToken { get; set; }
        public RefreshToken RefreshToken { get; set; }
    }
}
