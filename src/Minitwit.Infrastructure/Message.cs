using System.ComponentModel.DataAnnotations.Schema;

[Table("message")]
public class Message
{
    [Column("message_id")]
    public int Message_id { get; set; }
    [Column("author_id")]
    public int Author_id { get; set; }
    [Column("text")]
    public required string Text { get; set; }
    [Column("pub_date")]
    public int Pub_date { get; set; }
    [Column("flagged")]
    public int Flagged { get; set; }
}