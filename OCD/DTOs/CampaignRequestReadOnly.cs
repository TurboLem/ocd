namespace OCD.DTOs
{
    public class CampaignRequestReadOnly
    {
        public int Campaign { get; set; }  // map from CampaignId
        public string SysAidLog { get; set; } = string.Empty;
        public int Company { get; set; } // map from CompanyId
        public int CommunicationType { get; set; } // map from CommunicationTypeId
        public string CampaignRequestBrands { get; set; } = string.Empty; // map from BrandId
        public string? Description { get; set; }
        public string CampaignBrokerCodes { get; set; } = string.Empty; // map from BrokerCodeId
        public int? ConnexDiallerCampaign { get; set; }
        public int? ConnexDiallerSource { get; set; }
        public int? PackType { get; set; } // map from PackTypeId
        public int? BusinessUnit { get; set; } // map from BusinessUnitId
        public int Channel { get; set; }
        public int Source { get; set; }
        public int SourceBreak { get; set; }
        public int Media { get; set; }
        public string? SharecallNumber { get; set; }
        public string? SmsNumber { get; set; }
        public int PublicationOwner { get; set; }
        public int Publication { get; set; }
        public int Slot { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? TrackingCode { get; set; }
        public string Requester { get; set; } = null!; // the owner of the request. String interpolate user's name and surname
        public bool? IsActive { get; set; }
        public bool? IsPending { get; set; }
        public string? ActionedBy { get; set; }
        public DateTime? ActionedDate { get; set; }
        public DateTime RequestedDate { get; set; }
    }
}
