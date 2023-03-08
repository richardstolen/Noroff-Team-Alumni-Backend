using MessagePack;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace TeamAlumniNETBackend.Models
{
    public class Post
    {
        
        public int PostId { get; set; }
        
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime LastUpdate { get; set; }
        public string? TargetPost { get; set; }
        public string? TargetUser { get; set; }
        public string? TargetGroup { get; set; }
        public string? TargetTopic { get; set; }
        public string? TargetEvent { get; set; }
    }
}
