namespace Project.MVC.ViewModel
{
    public class TokenResponse
    {
        public string? AccessToken { get; set; }
        public RefreshToken? RefreshToken { get; set; }
    }

    public class RefreshToken
    {
        public string? UserName { get; set; }
        public string? TokenString { get; set; }
        public DateTime ExpireAt { get; set; }
    }

}
