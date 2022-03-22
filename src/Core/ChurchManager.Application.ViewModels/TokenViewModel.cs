namespace ChurchManager.Application.ViewModels
{
    public record TokenViewModel(bool IsAuthenticated, string AccessToken = null, string RefreshToken = null)
    {
    }
}
