using OCD.Models;

namespace OCD.Services.TestVDNService
{
    public interface ITestVDNService
    {
        Task<SendRequestResult> SendRequest(TestVDN testVDN);
        Task<VDNTestResponse> VDNTestResponse();

    }
}
