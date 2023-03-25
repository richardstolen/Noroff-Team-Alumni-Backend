using TeamAlumniNETBackend.Models;

namespace TeamAlumniNETBackend.DTOs.PostDTOs
{
    public class PostGetDTO
    {
        public int PostId { get; set; }
        public string? CreatedBy { get; set; }
        public string? Image { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public DateTime LastUpdate { get; set; }
        public string? Target { get; set; }
        public ICollection<CommentDTO>? Comments { get; set; } = new List<CommentDTO>();
    }
}
