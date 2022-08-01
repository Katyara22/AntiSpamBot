
namespace DicordAntiSpamBot
{
    public class Root
    {
        public string message { get; set; }
        public bool success { get; set; }
        public bool @unsafe { get; set; }
        public string domain { get; set; }
        public string server { get; set; }
        public string content_type { get; set; }
        public int status_code { get; set; }
        public int page_size { get; set; }
        public int domain_rank { get; set; }
        public bool dns_valid { get; set; }
        public bool parking { get; set; }
        public bool spamming { get; set; }
        public bool malware { get; set; }
        public bool phishing { get; set; }
        public bool suspicious { get; set; }
        public bool adult { get; set; }
        public int risk_score { get; set; }
        public string category { get; set; }
        public DomainAge domain_age { get; set; }
        public string request_id { get; set; }
    }
}
