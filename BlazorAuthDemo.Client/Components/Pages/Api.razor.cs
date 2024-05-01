namespace BlazorAuthDemo.Client.Components.Pages
{
    public partial class Api
    {
        private string? _claims;

        protected override async Task OnInitializedAsync()
        {
            _claims = await apiService.GetHomeAsync(); ;
        }
    }
}
