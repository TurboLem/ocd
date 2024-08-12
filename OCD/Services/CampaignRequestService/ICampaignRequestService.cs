using OCD.DTOs;
using OCD.Models;
using OCD.Responses;

namespace OCD.Services.CampaignRequestService
{
    public interface ICampaignRequestService
    {
        Task<GetCampaignRequestsResponse> GetCampaignRequestsAsync();
        Task<GetCampaignRequestsResponse> GetCampaignRequestsByTrackingCode(string trackingCode);
        Task<GetCampaignRequestsResponse> GetOnePackCampaignRequests();
        Task<GetCampaignRequestsResponse> GetThreePackCampaignRequests();
        Task<OCD.Models.CampaignRequest> GetCampaignRequestByIdAsync(int id);
        Task<GetUserCampaignsResponse> GetUserCampaigns(string userId);
        Task<BaseResponse> AddCampaignRequest(CampaignRequestDTO campaignRequestDTO);
        Task<BaseResponse> UpdateCampaignRequest(CampaignRequest campaignRequest);



    }
}
