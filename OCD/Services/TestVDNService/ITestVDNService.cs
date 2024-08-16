using OCD.Models;

namespace OCD.Services.TestVDNService
{
    public interface ITestVDNService
    {
        Task<string> SendRequest(TestVDN testVDN);
        Task<VDNTestResponse> VDNTestResponse();

    }
}
