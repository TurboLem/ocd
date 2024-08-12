using OCD.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace OCD.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int CampaignRequestId { get; set; }
        public string CommentText { get; set; } = string.Empty;
        public string CommenterId { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public bool IsResolved { get; set; } = false;
        public bool IsIssue { get; set; } = false;
        public bool IsViewed { get; set; } = false;
        [ForeignKey("CampaignRequestId")]
        public virtual CampaignRequest CampaignRequest { get; set; }
        [ForeignKey("CommenterId")]
        public virtual ApplicationUser Commenter { get; set; }
    }
}
