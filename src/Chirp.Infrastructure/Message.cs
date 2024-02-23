public class Message
{
    public int message_id { get; set; }
    public int author_id { get; set; }
    public required string Text { get; set; }
    public int pub_date { get; set; }
    public int flagged { get; set; }
}