﻿namespace ELK.App.Services
{
    public class ElasticsearchSettings
    {
        public string Url { get; set; } = null!;
        public string DefaultIndex { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

}
