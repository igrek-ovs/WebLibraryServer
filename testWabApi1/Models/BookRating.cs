namespace testWabApi1.Models
{
    public class BookRating
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string UserId { get; set; }
        public int Rating { get; set; }
    }
}
