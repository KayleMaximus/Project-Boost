using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;

public static class AuthService
{
    public static async Task LoginAnonymously()
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        
    }
}