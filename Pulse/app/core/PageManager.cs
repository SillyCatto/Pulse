using Pulse.Pages;
using Spectre.Console;

namespace Pulse.core;

public class PageManager
{
    private readonly Dictionary<string, Func<IPageAdapter>> _pages = new();

    public void RegisterPage(string pageName, Func<IPageAdapter> pageCreator)
    {
        _pages[pageName] = pageCreator;
    }

    public void Navigate(string pageName)
    {
        if (_pages.TryGetValue(pageName, out var pageCreator))
        {
            var page = pageCreator.Invoke();
            page.Run();
        }
        else
        {
            AnsiConsole.MarkupLine("[bold red]ERROR[/]  Page not found");
            AnsiConsole.WriteLine();
        }
    }

    public IEnumerable<string> GetPageNames()
    {
        return _pages.Keys;
    }
}