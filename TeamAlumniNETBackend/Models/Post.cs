using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace TeamAlumniNETBackend.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        public Guid? UserId { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public DateTime LastUpdate { get; set; }
        public int? TargetPost { get; set; }
        public Guid? TargetUser { get; set; }
        public int? TargetGroup { get; set; }
        public int? TargetTopic { get; set; }
        public int? TargetEvent { get; set; }
    }
}
