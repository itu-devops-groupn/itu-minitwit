public class Cheep
{
    public Guid CheepId { get; set; }
    public Guid AuthorId { get; set; }
    public required Author Author { get; set; }
    public required string Text { get; set; }
    public DateTime TimeStamp { get; set; }
}