using System.ComponentModel.DataAnnotations.Schema;

namespace Minitwit.Infrastructure
{
    [Table("follower")]
    public class Follower
    {
        [Column("who_id")]
        public int Who_id { get; set; }
        [Column("whom_id")]
        public int Whom_id { get; set; }
    }
}