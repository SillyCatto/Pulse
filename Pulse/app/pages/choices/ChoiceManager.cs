using Pulse.utils;
using Spectre.Console;

namespace Pulse.app.pages.choices;

public class ChoiceManager
{
    private readonly Dictionary<string, Func<IChoiceAdapter>> _choices = new();
    
    public void Add(string name, Func<IChoiceAdapter> choiceFactory)
    {
        _choices[name] = choiceFactory;
    }
    
    public void ShowAndExecute()
    {
        var selectedChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .HighlightStyle(Style.Parse(Constants.ChoicePromptStyle))
                .AddChoices(_choices.Keys)
        );

        _choices[selectedChoice]().Exec();
    }
}