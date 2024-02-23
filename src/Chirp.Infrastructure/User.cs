public class User
{
    public int user_id { get; set; }
    public required string username { get; set; }
    public required string email { get; set; }
    public required string pw_hash { get; set; }
}