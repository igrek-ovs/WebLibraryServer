namespace testWabApi1.Models
{
    public class Book
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public string Genre { get; set; }
        
        public string? ImagePath { get; set; }
        
        
        public int AuthorId { get; set; }
        public Author? Author { get; set; }
    }
}
