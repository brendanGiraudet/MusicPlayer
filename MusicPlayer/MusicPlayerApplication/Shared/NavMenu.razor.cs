using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;

namespace MusicPlayerApplication.Shared;

public partial class NavMenu{
    [Inject] public IConfiguration Configuration { get; set; }

    private bool _collapseNavMenu = true;

    private string _applicationVersion;

    private string NavMenuCssClass => _collapseNavMenu ? "collapse" : null;

    private void ToggleNavMenu()
    {
        _collapseNavMenu = !_collapseNavMenu;
    }

    protected override void OnInitialized()
    {
        _applicationVersion = Configuration["ApplicationVersion"];
    }
}