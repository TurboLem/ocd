namespace OCD.Models
{
    public class DatanetDiallerSource
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int DatanetDiallerCampaignId { get; set; }
        public DatanetDiallerCampaign DatanetDiallerCampaign { get; set; }
    }
}
