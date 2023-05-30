using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace ExpertAdministration.Web.Common;

public class NavigationHistory
{
    private readonly NavigationManager _navigationManager;

    public NavigationHistory(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;

        _navigationManager.LocationChanged += OnLocationChanged;
    }

    public string CurrentPage { get; set; }

    public string PreviousPage { get; set; }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        if (PreviousPage != e.Location.Substring(Constants.HostAddress.Length))
        {
            PreviousPage = CurrentPage;
        }

        CurrentPage = e.Location.Substring(Constants.HostAddress.Length);
    }
}