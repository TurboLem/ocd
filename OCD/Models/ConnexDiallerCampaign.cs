namespace OCD.Models
{
    public class ConnexDiallerCampaign
    {
        public int Id
        {
            get; set;
        }
        public string Name { get; set; } = null!;
        public ICollection<ConnexDiallerSource>? ConnexDiallerSources { get; set; }
    }   
}
