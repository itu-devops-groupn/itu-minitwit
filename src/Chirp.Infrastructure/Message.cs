public class Message
{
    public int Message_id { get; set; }
    public int Author_id { get; set; }
    public required string Text { get; set; }
    public int Pub_date { get; set; }
    public int Flagged { get; set; }
    public User User { get; set; }
}