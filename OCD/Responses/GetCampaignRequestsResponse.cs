using OCD.Models;

namespace OCD.Responses
{
    public class GetCampaignRequestsResponse : BaseResponse
    {
        public IEnumerable<CampaignRequest>? CampaignRequests { get; set; }
    }
}
