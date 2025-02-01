using Pulse.core;
using Spectre.Console;

namespace Pulse.app.pages;

public class BMIPage : IPageAdapter
{
    private readonly PageManager _pageManager;

    public BMIPage(PageManager pageManager)
    {
        _pageManager = pageManager;
    }

    public void View()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule($"[bold {Color.Orange1}]BMI Calculator[/]")
        {
            Justification = Justify.Center,
            Style = Style.Parse($"bold {Color.Orange1}")
        });
        AnsiConsole.WriteLine();
    }

    public void Run()
    {
        View();

        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .AddChoices("Back", "Exit")
        );

        if (choice.Equals("Back"))
        {
            _pageManager.Navigate("Home");
        }
        else
        {
            App.Exit();
        }
    }
}