using Spectre.Console;

namespace Pulse.app.pages.choices;

public class ChoiceExit : IChoiceAdapter
{
    public void Exec()
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[bold green]Goodbye![/]");
        Environment.Exit(0);
    }
}