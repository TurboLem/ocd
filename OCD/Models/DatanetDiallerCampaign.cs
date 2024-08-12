namespace OCD.Models
{
    public class DatanetDiallerCampaign
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<DatanetDiallerSource>? DatanetDiallerSources { get; set; }
    }
}
