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

    private void DrawTitle()
    {
        var titlePanel = new Panel(
            new Rows(
                new FigletText("Pulse").Centered().Color(Color.Red),
                new Markup($":fire: [bold {Color.Orange1}]Your Personal Health Tracker![/]").Centered()
            ))
        {
            Expand = true,
            Border = BoxBorder.None
        };
        AnsiConsole.Write(titlePanel);
    }

    private void DrawQuote()
    {
        var quotePanel = new Panel(
            new Rows(
                new Markup(":sparkles: [italic #00ffff]Health is wealth[/]").Centered()
            ))
        {
            Expand = true,
            Border = BoxBorder.Rounded
        };
        AnsiConsole.WriteLine();
        AnsiConsole.Write(quotePanel);
    }
    
    public void View()
    {
        Console.Clear();
        DrawTitle();
        DrawQuote();
        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[grey]Use ↑/↓ to navigate, Enter to select an option.[/]");
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[yellow]Choose an option:[/]");
    }

    public void Run()
    {
        View();

        string choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .AddChoices(_pageManager
                    .GetPageNames() // add: .Where(name => name != "Home") here to remove "Home"
                    .Append("Exit")
                )
        );

        if (choice.Equals("Exit"))
        {
            AnsiConsole.MarkupLine("[bold green]Goodbye![/]");
            return;
        }
        
        _pageManager.Navigate(choice);
    }
}