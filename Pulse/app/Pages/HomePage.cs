using Pulse.core;
using Spectre.Console;

namespace Pulse.Pages;

public class HomePage : IPageAdapter
{
    private readonly PageManager _pageManager;

    public HomePage(PageManager pageManager)
    {
        _pageManager = pageManager;
    }
    
    public void View()
    {
        AnsiConsole.Write(new FigletText("Pulse").Centered().Color(Color.Purple_1));
        AnsiConsole.MarkupLine("[bold blue]Your Personal Health Tracker![/]");
        AnsiConsole.WriteLine();
    }

    public void Run()
    {
        // TODO
        throw new NotImplementedException();
    }
}