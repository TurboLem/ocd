using OCD.Models;

namespace OCD.Responses
{
    public class GetUserCampaignsResponse : BaseResponse
    {
        public IEnumerable<CampaignRequest>? Campaigns { get; set; }
    }
}
