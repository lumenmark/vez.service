namespace UsaWeb.Service.Helper
{
    public class Environment
    {   
        public bool isEmailEnable { get; set; }
        
        public string apiUrl { get; set; }

        public string fromAddress { get; set; }
        
        public string fromName { get; set; }

        public string webApp { get; set; }

        public string webAppUrl { get; set; }

        public string saltKey { get; set; }
    }
}
