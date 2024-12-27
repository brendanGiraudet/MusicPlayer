using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;

namespace MusicPlayerApplication.Components.Layout;

public partial class MainLayout
{
    [Inject]public IConfiguration Configuration { get; set; }
    private bool sidebarExpanded = false;
}