using Microsoft.EntityFrameworkCore;
using OCD.Data;
using OCD.DTOs;
using OCD.Models;
using OCD.Responses;

namespace OCD.Services.CampaignRequestService
{
    public class CampaignRequestService : ICampaignRequestService
    {
        private readonly IDbContextFactory<DataContext> _dataContext;


        public CampaignRequestService(IDbContextFactory<DataContext> dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<BaseResponse> ActivateCampaignRequest(CampaignRequest campaignRequest)
        {
            var response = new BaseResponse();
            using (var context = await _dataContext.CreateDbContextAsync())
            {
                try
                {
                    context.CampaignRequests.Attach(campaignRequest);

                    context.Entry(campaignRequest).Property(r => r.IsActive).IsModified = true;
                    context.Entry(campaignRequest).Property(r => r.IsPending).IsModified = true;
                    var entries = context.ChangeTracker.Entries();
                    foreach (var entry in entries)
                    {
                        Console.WriteLine($"{entry.Entity.GetType().Name} - {entry.State}");
                    }
                    var result = await context.SaveChangesAsync();
                    if (result > 0)
                    {
                        response.StatusCode = 200;
                        response.Message = "Campaign Request has successfully been activated";
                    }
                    else
                    {
                        response.StatusCode = 500;
                        response.Message = "Error activating campaign request";
                    }
                }
                catch (Exception ex)
                {
                    response.StatusCode = 500;
                    response.Message = $"Error activating campaign request: {ex.Message}";
                }
            }
            return response;
        }

        public async Task<BaseResponse> AddCampaignRequest(CampaignRequestDTO campaignRequestDTO)
        {
            var response = new BaseResponse();
            try
            {
                using (var context = await _dataContext.CreateDbContextAsync())
                {
                    var newCampaignRequest = new OCD.Models.CampaignRequest
                    {
                        CampaignId = campaignRequestDTO.CampaignId,
                        CampaignBrokerCodes = campaignRequestDTO.CampaignBrokerCodes,
                        CampaignRequestBrands = campaignRequestDTO.CampaignRequestBrands,
                        BusinessUnitId = campaignRequestDTO.BusinessUnitId,
                        CommunicationTypeId = campaignRequestDTO.CommunicationTypeId,
                        Description = campaignRequestDTO.Description,
                        ConnexDiallerCampaignId = campaignRequestDTO.ConnexDiallerCampaignId,
                        ConnexDiallerSourceId = campaignRequestDTO.ConnexDiallerSourceId,
                        ChannelId = campaignRequestDTO.ChannelId,
                        SourceId = campaignRequestDTO.SourceId,
                        SourceBreakId = campaignRequestDTO.SourceBreakId,
                        MediaId = campaignRequestDTO.MediaId,
                        SharecallNumber = null,
                        SmsNumber = null,
                        PublicationOwnerId = campaignRequestDTO.PublicationOwnerId,
                        PublicationId = campaignRequestDTO.PublicationId,
                        SlotId = campaignRequestDTO.SlotId,
                        StartDate = campaignRequestDTO.StartDate,
                        EndDate = campaignRequestDTO.EndDate,
                        TrackingCode = null,
                        RequesterId = campaignRequestDTO.RequesterId,
                        IsActive = false,
                        IsPending = false,
                        ActionedBy = null,
                        ActionedDate = null

                    };
                    context.CampaignRequests.Add(newCampaignRequest);
                    var result = await context.SaveChangesAsync();
                    if (result == 1)
                    {
                        // TODO: Send email to requesters manager
                        response.StatusCode = 200;
                        response.Message = "Campaign Request added successfully";
                    }
                    else
                    {
                        response.StatusCode = 500;
                        response.Message = "Error adding campaign request";
                    }
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = $"Error adding campaign request: {ex.Message}";
            }
            return response;
        }

        public async Task<OCD.Models.CampaignRequest> GetCampaignRequestByIdAsync(int id)
        {
            using (var context = await _dataContext.CreateDbContextAsync())
            {
                var campaign = await context.CampaignRequests.FirstOrDefaultAsync(c => c.Id == id);
                if (campaign is null)
                {
                    return new();
                }
                else
                {
                    return campaign;
                }
            }
        }
        #region GetCampaignRequests
        public async Task<GetCampaignRequestsResponse> GetCampaignRequestsAsync()
        {
           
            var response = new GetCampaignRequestsResponse();
            try
            {
                using (var context = await _dataContext.CreateDbContextAsync())
                {
                    var campaignRequests = await context.CampaignRequests
                        .OrderByDescending(rd => rd.RequestedDate)
                        .AsNoTracking()
                        .ToListAsync();

                    var processedCampaignRequests = await BuildCampaignRequest(campaignRequests);

                    response.CampaignRequests = processedCampaignRequests;
                    
                    response.StatusCode = 200;
                    response.Message = "Campaign Requests retrieved successfully";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = $"Error retrieving campaign requests: {ex.Message}";
                response.CampaignRequests = null;
            }
            return response;
        }
        #endregion
       

        public async Task<GetCampaignRequestsResponse> GetCampaignRequestsByTrackingCode(string trackingCode)
        {
            var response = new GetCampaignRequestsResponse();
            try
            {
                using (var context = await _dataContext.CreateDbContextAsync())
                {
                    // Perform a case-insensitive partial search on trackingCode
                    var campaignRequests = await context.CampaignRequests
                        .Where(c => EF.Functions.Like(c.TrackingCode, $"%{trackingCode}%"))
                        .AsNoTracking()
                        .ToListAsync();

                    response.CampaignRequests = campaignRequests;
                    response.StatusCode = 200;
                    response.Message = "Campaign Requests retrieved successfully";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = $"Error retrieving campaign requests: {ex.Message}";
                response.CampaignRequests = null;
            }
            return response;
        }

        public async Task<GetCampaignRequestsResponse> GetMultibrandCampaignRequests()
        {
            using (var context = await _dataContext.CreateDbContextAsync())
            {
                var response = new GetCampaignRequestsResponse();
                try
                {
                    var campaignRequests = await context.CampaignRequests
                        .Where(p => p.PackTypeId == null)
                        .AsNoTracking()
                        .ToListAsync();

                    var processedCampaignRequests = await BuildCampaignRequest(campaignRequests);

                    response.CampaignRequests = processedCampaignRequests;
                    response.StatusCode = 200;
                    response.Message = "Campaign Requests retrieved successfully";
                }
                catch (Exception ex)
                {
                    response.StatusCode = 500;
                    response.Message = $"Error retrieving campaign requests: {ex.Message}";
                    response.CampaignRequests = null;
                }
                return response;
            }
        }

        #region GetOnePackCampaignRequests
        public async Task<GetCampaignRequestsResponse> GetOnePackCampaignRequests()
        {
            using (var context = await _dataContext.CreateDbContextAsync())
            {
                var response = new GetCampaignRequestsResponse();
                try
                {
                    var campaignRequests = await context.CampaignRequests
                        .Where(c => c.PackTypeId == 1)
                        .AsNoTracking()
                        .ToListAsync();

                    var processedCampaignRequests = await BuildCampaignRequest(campaignRequests);

                    response.CampaignRequests = processedCampaignRequests;
                    response.StatusCode = 200;
                    response.Message = "Campaign Requests retrieved successfully";
                }
                catch (Exception ex)
                {
                    response.StatusCode = 500;
                    response.Message = $"Error retrieving campaign requests: {ex.Message}";
                    response.CampaignRequests = null;
                }
                return response;
            }
        }
        #endregion

        #region GetThreePackCampaignRequests
        public async Task<GetCampaignRequestsResponse> GetThreePackCampaignRequests()
        {
            using (var context = await _dataContext.CreateDbContextAsync())
            {
                var response = new GetCampaignRequestsResponse();
                try
                {
                    var campaignRequests = await context.CampaignRequests
                        .Where(c => c.PackTypeId == 2)
                        .AsNoTracking()
                        .ToListAsync();
                   
                    var processedCampaignRequests = await BuildCampaignRequest(campaignRequests);

                    response.CampaignRequests = processedCampaignRequests;
                    response.StatusCode = 200;
                    response.Message = "Campaign Requests retrieved successfully";
                }
                catch (Exception ex)
                {
                    response.StatusCode = 500;
                    response.Message = $"Error retrieving campaign requests: {ex.Message}";
                    response.CampaignRequests = null;
                }
                return response;
            }
        }
        #endregion
        public async Task<GetUserCampaignsResponse> GetUserCampaigns(string userId)
        {
            var response = new GetUserCampaignsResponse();
            try
            {
                using (var context = await _dataContext.CreateDbContextAsync())
                {
                    response.Campaigns = await context.CampaignRequests.Where(c => c.RequesterId.Equals(userId))
                        .AsNoTracking()
                        .ToListAsync();

                    response.StatusCode = 200;
                    response.Message = "Campaign Requests retrieved successfully";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = $"Error retrieving campaign requests: {ex.Message}";
                response.Campaigns = null;
            }

            return response;
        }

        public async Task<BaseResponse> UpdateCampaignRequest(CampaignRequest campaignRequest)
        {
            var response = new BaseResponse();
            using (var context = await _dataContext.CreateDbContextAsync())
            {
                try
                {
                    context.CampaignRequests.Update(campaignRequest);
                    var result = await context.SaveChangesAsync();
                    if (result > 0)
                    {
                        response.StatusCode = 200;
                        response.Message = "Campaign Request updated successfully";
                    }
                    else
                    {
                        response.StatusCode = 500;
                        response.Message = "Error updating campaign request";
                    }
                }
                catch (Exception ex)
                {
                    response.StatusCode = 500;
                    response.Message = $"Error updating campaign request: {ex.Message}";
                }

            }

            return response;
        }

        private async Task<IEnumerable<CampaignRequest>> BuildCampaignRequest(IEnumerable<CampaignRequest> campaignRequests)
        {

            using (var context = await _dataContext.CreateDbContextAsync())
            {
                var businessUnitIds = campaignRequests.Select(c => c.BusinessUnitId).Distinct().ToList();
                var businessUnits = await context.BusinessUnits.Where(b => businessUnitIds.Contains(b.Id)).ToListAsync();
                var communicationTypeIds = campaignRequests.Select(c => c.CommunicationTypeId).Distinct().ToList();
                var communicationTypes = await context.CommunicationTypes.Where(c => communicationTypeIds.Contains(c.Id)).ToListAsync();
                var channelIds = campaignRequests.Select(c => c.ChannelId).Distinct().ToList();
                var channels = await context.Channels.Where(c => channelIds.Contains(c.Id)).ToListAsync();
                var sourceIds = campaignRequests.Select(c => c.SourceId).Distinct().ToList();
                var sources = await context.Sources.Where(s => sourceIds.Contains(s.Id)).ToListAsync();
                var sourceBreakIds = campaignRequests.Select(c => c.SourceBreakId).Distinct().ToList();
                var sourceBreaks = await context.SourceBreaks.Where(sb => sourceBreakIds.Contains(sb.Id)).ToListAsync();
                var mediaIds = campaignRequests.Select(c => c.MediaId).Distinct().ToList();
                var media = await context.Media.Where(m => mediaIds.Contains(m.Id)).ToListAsync();
                var publicationOwnerIds = campaignRequests.Select(c => c.PublicationOwnerId).Distinct().ToList();
                var publicationOwners = await context.PublicationOwners.Where(po => publicationOwnerIds.Contains(po.Id)).ToListAsync();
                var publicationIds = campaignRequests.Select(c => c.PublicationId).Distinct().ToList();
                var publications = await context.Publications.Where(p => publicationIds.Contains(p.Id)).ToListAsync();
                var slotIds = campaignRequests.Select(c => c.SlotId).Distinct().ToList();
                var slots = await context.Slots.Where(s => slotIds.Contains(s.Id)).ToListAsync();
                var campaignIds = campaignRequests.Select(c => c.CampaignId).Distinct().ToList();
                var campaigns = await context.Campaigns.Where(c => campaignIds.Contains(c.Id)).ToListAsync();
                var connexDiallerCampaignIds = campaignRequests.Select(c => c.ConnexDiallerCampaignId).Distinct().ToList();
                var connexDiallerCampaigns = await context.ConnexDiallerCampaigns.Where(c => connexDiallerCampaignIds.Contains(c.Id)).ToListAsync();
                var connexDiallerSourceIds = campaignRequests.Select(c => c.ConnexDiallerSourceId).Distinct().ToList();
                var connexDiallerSources = await context.ConnexDiallerSources.Where(c => connexDiallerSourceIds.Contains(c.Id)).ToListAsync();
                var actionerIds = campaignRequests.Select(c => c.ActionedBy).Distinct().ToList();
                var actioners = await context.Users.Where(u => actionerIds.Contains(u.Id)).ToListAsync();
                var requesterIds = campaignRequests.Select(c => c.RequesterId).Distinct().ToList();
                var requesters = await context.Users.Where(u => requesterIds.Contains(u.Id)).AsNoTracking().ToListAsync();


                foreach (var campaignRequest in campaignRequests)
                {
                    campaignRequest.Requester = requesters.FirstOrDefault(r => r.Id == campaignRequest.RequesterId) ?? new();
                    campaignRequest.BusinessUnit = businessUnits.FirstOrDefault(b => b.Id == campaignRequest.BusinessUnitId) ?? new();
                    campaignRequest.Campaign = campaigns.FirstOrDefault(c => c.Id == campaignRequest.CampaignId) ?? new();
                    campaignRequest.Slot = slots.FirstOrDefault(s => s.Id == campaignRequest.SlotId) ?? new();
                    campaignRequest.Media = media.FirstOrDefault(m => m.Id == campaignRequest.MediaId) ?? new();
                    campaignRequest.Source = sources.FirstOrDefault(s => s.Id == campaignRequest.SourceId) ?? new();
                    campaignRequest.SourceBreak = sourceBreaks.FirstOrDefault(sb => sb.Id == campaignRequest.SourceBreakId) ?? new();
                    campaignRequest.Publication = publications.FirstOrDefault(p => p.Id == campaignRequest.PublicationId) ?? new();
                    campaignRequest.PublicationOwner = publicationOwners.FirstOrDefault(po => po.Id == campaignRequest.PublicationOwnerId) ?? new();
                    campaignRequest.Channel = channels.FirstOrDefault(ch => ch.Id == campaignRequest.ChannelId) ?? new();
                    campaignRequest.CommunicationType = communicationTypes.FirstOrDefault(ct => ct.Id == campaignRequest.CommunicationTypeId) ?? new();
                    campaignRequest.ConnexDiallerCampaign = connexDiallerCampaigns.FirstOrDefault(cdc => cdc.Id == campaignRequest.ConnexDiallerCampaignId) ?? new();
                    campaignRequest.ConnexDiallerSource = connexDiallerSources.FirstOrDefault(cds => cds.Id == campaignRequest.ConnexDiallerSourceId) ?? new();
                    campaignRequest.Actioner = actioners.FirstOrDefault(u => u.Id == campaignRequest.ActionedBy) ?? new();
                }
            }
            return campaignRequests.ToList();
        }
    }
}
