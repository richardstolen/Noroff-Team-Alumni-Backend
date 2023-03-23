namespace TeamAlumniNETBackend.DTOs.PostDTOs
{
    public class DirectMessagesDTO
    {
        public Guid? SenderID { get; set; }
        public string? Sender { get; set; }
        public string? Image { get; set; }
        public List<CommentDTO> Messages { get; set; } = new List<CommentDTO>();
    }
}
