using Pulse.Pages;

namespace Pulse.core;

public class App
{
    private readonly PageManager _pageManager;

    public App()
    {
        _pageManager = new PageManager();
        
        // register pages
        _pageManager.RegisterPage("Home", () => new HomePage(_pageManager));
        
    }
}