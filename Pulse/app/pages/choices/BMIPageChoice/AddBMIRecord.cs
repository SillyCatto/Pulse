using System.Globalization;
using Pulse.utils;
using Spectre.Console;

namespace Pulse.app.pages.choices.BMIPageChoice;

public class AddBMIRecord : IChoiceAdapter
{
    private readonly BMIPage _page;

    public AddBMIRecord(BMIPage page)
    {
        _page = page;
    }
    public void Exec()
    {
        string today = DateTime.Now.ToString("yyyy-MM-dd");

        var bmiRecords = _page.GetDataModel().ToDict();
        if (bmiRecords.ContainsKey(today))
        {
            _page.SetMsg($":slightly_smiling_face: [magenta2_1]You already have a record for today {today}. Come back later.[/]");
            _page.Run();
            return;
        }
        
        AnsiConsole.WriteLine();
        double weight = AnsiConsole.Prompt(
            new TextPrompt<double>("Enter your [bold mediumorchid1_1]weight[/] (kg):")
                .PromptStyle(Constants.PromptInputStyle)  // Makes user input green
                .ValidationErrorMessage("[red]That's not a number.[/]") // Custom error for non-numeric input
                .Validate(w => 
                        w >= 0 ? ValidationResult.Success() 
                            : ValidationResult.Error("[red]Weight must be positive.[/]") // Custom error for invalid numbers
                )
        );
    }
}