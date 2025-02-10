using Pulse.app.models;
using Pulse.utils;
using Spectre.Console;

namespace Pulse.app.pages.choices.MoodPageChoice;

public class AddMoodRecord : IChoiceAdapter
{
    private readonly MoodTracker _page;

    public AddMoodRecord(MoodTracker page)
    {
        _page = page;
    }
    public void Exec()
    {
        string today = DateTime.Now.ToString(Constants.DateStringFormat);

        var bmiRecords = _page.GetDataModel().ToDict();
        if (bmiRecords.ContainsKey(today))
        {
            _page.SetMsg($":slightly_smiling_face: [magenta2_1]You already have a mood record for today {today}. Come back later.[/]");
            _page.Run();
            return;
        }
        
        _page.RefreshMsg($"[yellow]Adding record for today {today}...[/]");
        
        int stress = AnsiConsole.Prompt(
            new TextPrompt<int>("How much [bold mediumorchid1_1]stressed[/] are you today? (type a number from 1 to 5 for stress level):")
                .PromptStyle(Constants.PromptInputStyle)
                .ValidationErrorMessage("[red]That's not a number.[/]")
                .Validate(w => 
                    w is >= 1 and <= 5 
                        ? ValidationResult.Success() 
                        : ValidationResult.Error("[red]Choose a stress level between 1 to 5.[/]")
                )
        );
        
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("How are you [bold mediumorchid1_1]feeling[/] today? :");
        AnsiConsole.WriteLine();
        
        string mood = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .PageSize(6)
                .HighlightStyle(Style.Parse("bold #00ffff"))
                .AddChoices(MoodModel.GetAllMoods())
        );

        var moodModel = _page.GetDataModel();
        moodModel.AddRecord(today, [stress.ToString(), mood]);
        moodModel.Save();
        
        _page.SetMsg($":check_mark:  [green]Mood record added successfully for {today}[/]");
        _page.Run();
    }
}