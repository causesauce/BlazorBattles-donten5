using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorBattles.Client
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _locaStorageService;

        public CustomAuthStateProvider(ILocalStorageService locaStorageService)
        {
            _locaStorageService = locaStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            Console.WriteLine(1);
            var state = new AuthenticationState(new ClaimsPrincipal());
            if (await _locaStorageService.GetItemAsync<bool>("isAuthenticated"))
            {
                Console.WriteLine(2);

                var identity = new ClaimsIdentity(new[] {
                            new Claim(ClaimTypes.Name, "Patrick")
                }, "test auth type");

                var user = new ClaimsPrincipal(identity);
                state = new AuthenticationState(user);

            }
            NotifyAuthenticationStateChanged(Task.FromResult(state));
            return state;
        }
    }
}
