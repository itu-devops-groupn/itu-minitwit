public class User
{
    public int User_id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Pw_hash { get; set; }
}