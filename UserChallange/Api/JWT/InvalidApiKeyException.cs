namespace JwtToken.Api.JWT
{
    public class InvalidApiKeyException: ApplicationException
    {
        public InvalidApiKeyException() : base("api key is invalid") { }
    }
}
