namespace WebApplication1.Services
{
    public class ElasticsearchSettings
    {
        public string[] Urls { get; set; } = null!;
        public string DefaultIndex { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

}
