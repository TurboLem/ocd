namespace OCD.Models
{
    public class ConnexDiallerSource
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int ConnexDiallerCampaignId { get; set; }
        public ConnexDiallerCampaign ConnexDiallerCampaign { get; set; }
    }
}
