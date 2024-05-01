using System.Security.Claims;

namespace BlazorAuthDemo.Client.Components.Pages
{
    public partial class Claims
    {
        private string? authMessage;
        private string? surname;
        private IEnumerable<Claim> claims = Enumerable.Empty<Claim>();

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider
                .GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity is not null && user.Identity.IsAuthenticated)
            {
                authMessage = $"{user.Identity.Name} is authenticated.";
                claims = user.Claims;
                surname = user.FindFirst(c => c.Type == ClaimTypes.Surname)?.Value;
            }
            else
            {
                authMessage = "The user is NOT authenticated.";
            }
        }
    }
}
