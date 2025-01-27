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
        View();

        string choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose an option:")
                .AddChoices(_pageManager.GetPageNames().Append("Exit"))
        );

        if (choice.Equals("Exit"))
        {
            AnsiConsole.MarkupLine("[bold green]Goodbye![/]");
            return;
        }
        
        _pageManager.Navigate(choice);
    }
}