using OCD.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Threading.Channels;

namespace OCD.Models
{
    public class CampaignRequest
    {
        [Key]
        public int Id { get; set; }
        public string SysAidLog { get; set; } = string.Empty;
        public int CampaignId { get; set; }
        public int BusinessUnitId { get; set; }
        public int CommunicationTypeId { get; set; }
        public int? PackTypeId { get; set; }
        public string? Description { get; set; }
        public int ConnexDiallerCampaignId { get; set; }
        public int ConnexDiallerSourceId { get; set; }
        public int ChannelId { get; set; }
        public int SourceId { get; set; }
        public int SourceBreakId { get; set; }
        public int MediaId { get; set; }
        public int? CampaignRequestMainId { get; set; }
        public string? SharecallNumber { get; set; }
        public string? SmsNumber { get; set; }
        public string CampaignBrokerCodes { get; set; } = string.Empty;
        public string CampaignRequestBrands { get; set; } = string.Empty;
        public int PublicationOwnerId { get; set; }
        public int PublicationId { get; set; }
        public int SlotId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? TrackingCode { get; set; }
        public string RequesterId { get; set; } = null!;
        public bool IsActive { get; set; }
        public bool  IsPending { get; set; }
        public string? ActionedBy { get; set; }
        public DateTime? ActionedDate { get; set; }
        public DateTime RequestedDate { get; set; }

        [ForeignKey("RequesterId")]
        public virtual ApplicationUser Requester { get; set; }

        [ForeignKey("ActionedBy")]
        public virtual ApplicationUser Actioner { get; set; }
        [ForeignKey("CampaignId")]
        public virtual Campaign Campaign { get; set; }

        [ForeignKey("PackTypeId")]
        public virtual PackType PackType { get; set; }

        [ForeignKey("BusinessUnitId")]
        public virtual BusinessUnit BusinessUnit { get; set; }

        [ForeignKey("ChannelId")]
        public virtual Channel Channel { get; set; }

        [ForeignKey("SourceId")]
        public virtual Source Source { get; set; }

        [ForeignKey("MediaId")]
        public virtual Media Media { get; set; }

        [ForeignKey("PublicationOwnerId")]
        public virtual PublicationOwner PublicationOwner { get; set; }

        [ForeignKey("PublicationId")]
        public virtual Publication Publication { get; set; }

        [ForeignKey("SlotId")]
        public virtual Slot Slot { get; set; }
        [ForeignKey("ConnexDiallerCampaignId")]
        public virtual ConnexDiallerCampaign ConnexDiallerCampaign { get; set; } 
        [ForeignKey("ConnexDiallerSourceId")]
        public virtual ConnexDiallerSource ConnexDiallerSource { get; set; }
       
        [ForeignKey("CommunicationTypeId")]
        public virtual CommunicationType CommunicationType { get; set; }
       
        [ForeignKey("SourceBreakId")]
        public virtual SourceBreak SourceBreak { get; set; }
        [ForeignKey("CampaignRequestMainId")]
        public virtual CampaignRequest CampaignRequestMain { get; set; }
        public virtual ICollection<CampaignRequest> SubCampaignRequests { get; set; } 
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    }
}
