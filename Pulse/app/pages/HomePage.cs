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
        var quote = Quotes.GetRandom();
        var quotePanel = new Panel(
            new Rows(
                new Markup($":sparkles: [italic #00ffff]{quote}[/]").Centered()
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
        AnsiConsole.Clear();
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

        // later add: .Where(name => name != "Home") here to remove "Home"
        var choices = _pageManager.GetPageNames().Append("Exit").ToList();

        string choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .AddChoices(choices)
        );

        if (choice.Equals("Exit"))
        {
            App.Exit();
        }
        
        _pageManager.Navigate(choice);
    }
}