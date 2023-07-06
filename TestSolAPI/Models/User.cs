namespace TestSolAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Token { get; set; }
        public string Username { get; set; } = string.Empty;
        public TimeSpan Validity { get; set; }
        public string? RefreshToken { get; set; }
        public string EmailId { get; set; } = string.Empty;
        public Guid GuidId { get; set; }
        public DateTime ExpiredTime { get; set; }
    }
}