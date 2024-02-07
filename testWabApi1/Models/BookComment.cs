namespace testWabApi1.Models
{
    public class BookComment
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string UserId { get; set; }
        public string Comment { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
