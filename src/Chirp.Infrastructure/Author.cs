public class Author
{
    public Guid AuthorId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required List<Cheep> Cheeps { get; set; }
    // Following and Followers should be Sets
    public List<Author> Following { get; set; } = new List<Author>();
    public List<Author> Followers { get; set; } = new List<Author>();
}