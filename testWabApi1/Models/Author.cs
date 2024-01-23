using Microsoft.Extensions.Hosting;

namespace testWabApi1.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Book> Books { get; } = new List<Book>();
    }
}
