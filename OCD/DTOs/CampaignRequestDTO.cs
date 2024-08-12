namespace OCD.DTOs
{
    public class CampaignRequestDTO
    {
        public int CampaignId { get; set; }
        public string SysAidLog { get; set; } = string.Empty;
        public int CommunicationTypeId { get; set; }
        public string CampaignRequestBrands { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string CampaignBrokerCodes { get; set; } = string.Empty;
        public int? PackType { get; set; }
        public int BusinessUnitId { get; set; }
        public int ConnexDiallerCampaignId { get; set; }
        public int ConnexDiallerSourceId { get; set; }
        public int ChannelId { get; set; }
        public int SourceId { get; set; }
        public int SourceBreakId { get; set; }
        public int MediaId { get; set; }
        public string? SharecallNumber { get; set; }
        public string? SmsNumber { get; set; }
        public int PublicationOwnerId { get; set; }
        public int PublicationId { get; set; }
        public int SlotId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? TrackingCode { get; set; }
        public string RequesterId { get; set; } = null!;
        public bool? IsActive { get; set; }
        public bool? IsPending { get; set; }
        public string? ActionedBy { get; set; }
        public DateTime? ActionedDate { get; set; }
        public DateTime RequestedDate { get; set; }
    }
}
