using System.ComponentModel.DataAnnotations.Schema;

[Table("users")]
public class User
{
    [Column("user_id")]
    public int User_id { get; set; }
    [Column("username")]
    public required string Username { get; set; }
    [Column("email")]
    public required string Email { get; set; }
    [Column("pw_hash")]
    public required string Pw_hash { get; set; }
}