namespace TeamAlumniNETBackend.DTOs.PostDTOs
{
    public class CommentDTO
    {
        public int PostId { get; set; }
        public string? CreatedBy { get; set; }
        public string? Image { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public DateTime LastUpdate { get; set; }
        public int? Target { get; set; }
    }
}
