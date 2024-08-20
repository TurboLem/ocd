using OCD.Models;
using System.Text;
using System.Xml;

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

        public async Task<SendRequestResult> SendRequest(TestVDN testVDN)
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
                            <QuoteReference>NA</QuoteReference>
                            <Title></Title>
                            <Gender>M</Gender>
                            <FirstName>{testVDN.FirstName}</FirstName>
                            <Surname>{testVDN.Surname}</Surname>
                            <IDNumber></IDNumber>
                            <DateOfBirth>00:00:00</DateOfBirth>
                            <HomeNumber></HomeNumber>
                            <WorkNumber></WorkNumber>
                            <MobileNumber>{testVDN.MobileNumber}</MobileNumber>
                            <EmailAddress></EmailAddress>
                            <IncompleteLeadNo></IncompleteLeadNo>
                            <AddressLine1></AddressLine1>
                            <AddressLine2></AddressLine2>
                            <SuburbName></SuburbName>
                            <PostalCode></PostalCode>
                            <VDN>{testVDN.VDN}</VDN>
                            <BrokerCode></BrokerCode>
                            <CompanyName></CompanyName>
                            <Comments></Comments>
                            <CallBackDateTime>00:00:00</CallBackDateTime>
                            <VehicleDetails>
                                    <Description>NA</Description>
                                    <Year>1900</Year>
                                    <MNMCode></MNMCode>
                                    <Colour></Colour>
                                    <VeehicleKey></VeehicleKey>
                                    <RegistrationNumber></RegistrationNumber>
                                <DealerDetails>
                                        <DealerName>NA</DealerName>
                                        <ContactPerson></ContactPerson>
                                        <TelephoneNumber></TelephoneNumber>
                                        <EmailAddress></EmailAddress>
                                        <ChassisNumber></ChassisNumber>
                                        <EngineNumber></EngineNumber>
                                        <FinanceHouse></FinanceHouse>
                                        <DealerCode></DealerCode>
                                </DealerDetails>
                            </VehicleDetails>
                            <ErrorStatus>
                            </ErrorStatus>
                            <ErrorDescription>
                            </ErrorDescription>
                        </p_Lead>
                    </SendContact>
                </soap:Body>
            </soap:Envelope>";

            var client = _clientFactory.CreateClient();
            var content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml");

            var response = await client.PostAsync(_configuration["ContactAPI:Url"], content);

            var result = new SendRequestResult
            {
                ResponseContent = await response.Content.ReadAsStringAsync()
            };


            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(result.ResponseContent);

            var namespaceManager = new XmlNamespaceManager(xmlDoc.NameTable);
            namespaceManager.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
            namespaceManager.AddNamespace("tempuri", "http://tempuri.org/");

            var sendContactResultNode = xmlDoc.SelectSingleNode("//tempuri:SendContactResult", namespaceManager);

            if (sendContactResultNode is not null)
            {
                var errorStatusNode = sendContactResultNode.SelectSingleNode("tempuri:ErrorStatus", namespaceManager);
                var errorDescriptionNode = sendContactResultNode.SelectSingleNode("tempuri:ErrorDescription", namespaceManager);

                if (errorStatusNode is not null)
                {
                    result.ErrorStatus = errorStatusNode.InnerText;
                }

                if (errorDescriptionNode is not null)
                {

                    var stringNode = errorDescriptionNode.SelectSingleNode("string");
                    if (stringNode is not null)
                    {
                        result.ErrorDescription = stringNode.InnerText;
                    }
                    else
                    {
                        result.ErrorDescription = errorDescriptionNode.InnerText;
                    }

                    if (!string.IsNullOrEmpty(result.ErrorStatus))
                    {
                        result.IsSuccess = false;
                    }
                }
            }

            return result;
        }

        public Task<VDNTestResponse> VDNTestResponse()
        {
            throw new NotImplementedException();
        }
    }
    public class SendRequestResult
    {
        public bool IsSuccess { get; set; }
        public string ResponseContent { get; set; } = string.Empty;
        public string ErrorStatus { get; set; } = string.Empty;
        public string ErrorDescription { get; set; } = string.Empty;
    }
}
