namespace EmailFinder.Models
{
    public class VerifyResult
    {
        public string Email { get; set; }
        public bool IsValid { get; set; }
        public string Service { get; set; }
    }
}