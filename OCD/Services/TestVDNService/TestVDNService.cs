using OCD.Models;
using System.Text;

namespace OCD.Services.TestVDNService
{
    public class TestVDNService : ITestVDNService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public TestVDNService(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
        }

        public async Task<string> SendRequest(TestVDN testVDN)
        {

            var username = _configuration["ContactAPI:Username"];
            var password = _configuration["CONTACT_API_PASSWORD"] ?? _configuration["ContactAPI:Password"];
           

            var soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
        <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                        xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                        xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
            <soap:Header>
                <AuthenticationHeader xmlns=""http://tempuri.org/"">
                    <Username>{username}</Username>
                    <Password>{password}</Password>
                </AuthenticationHeader>
            </soap:Header>
            <soap:Body>
                <SendContact xmlns=""http://tempuri.org/"">
                    <p_Lead>
                        <FirstName>{testVDN.FirstName}</FirstName>
                        <Surname>{testVDN.Surname}</Surname>
                        <MobileNumber>{testVDN.MobileNumber}</MobileNumber>
                        <VDN>{testVDN.VDN}</VDN>
                    </p_Lead>
                </SendContact>
            </soap:Body>
        </soap:Envelope>";
            var client = _clientFactory.CreateClient();
            var content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml");

            var response = await client.PostAsync(_configuration["ContactAPI:Url"], content);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();


           return responseContent;
        }

        public Task<VDNTestResponse> VDNTestResponse()
        {
            throw new NotImplementedException();
        }
    }
}
