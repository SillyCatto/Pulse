using Pulse.app.pages.choices;
using Pulse.core;
using Pulse.utils;
using Spectre.Console;

namespace Pulse.app.pages;

public class HomePage : IPageAdapter
{
    private readonly PageManager _pageManager;
    private readonly ChoiceManager _choiceManager;

    public HomePage(PageManager pageManager)
    {
        _pageManager = pageManager;
        _choiceManager = new ChoiceManager();
        
        RegisterChoices();
    }
    
    private void RegisterChoices()
    {
        _choiceManager.Add("Home", () => new ChoiceNavigatePage(_pageManager, "Home"));
        _choiceManager.Add("BMI Calculator", () => new ChoiceNavigatePage(_pageManager, "BMI Calculator"));
        _choiceManager.Add("Healthy Habit Todos", () => new ChoiceNavigatePage(_pageManager, "Healthy Habit Todos"));
        _choiceManager.Add("Mood Tracker", () => new ChoiceNavigatePage(_pageManager, "Mood Tracker"));
        _choiceManager.Add("Exit", () => new ChoiceExit());
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
        AnsiConsole.MarkupLine("[grey54]Use ↑/↓ to navigate, Enter to select an option.[/]");
        
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[turquoise2]Choose an option:[/]");
    }

    public void Run()
    {
        View();

        _choiceManager.ShowAndExecute();
    }
}