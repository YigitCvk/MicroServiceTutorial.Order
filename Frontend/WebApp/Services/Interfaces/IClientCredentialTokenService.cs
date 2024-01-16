namespace WebApp.Services.Interfaces
{
    public interface IClientCredentialTokenService
    {
        Task<string> GetTokenAsync(string token);
    }
}
