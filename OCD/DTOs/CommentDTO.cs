namespace OCD.DTOs
{
    public class CommentDTO
    {
        public int CampaignRequestId { get; set; }
        public string CommentText { get; set; } = string.Empty;
        public string CommenterId { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
        public bool IsResolved { get; set; }
        public bool IsIssue { get; set; }
        public bool IsViewed { get; set; }
    }
}
