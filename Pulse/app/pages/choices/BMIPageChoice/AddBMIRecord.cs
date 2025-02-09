using System.Globalization;
using Pulse.app.models;
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
        string today = DateTime.Now.ToString(Constants.DateStringFormat);

        var bmiRecords = _page.GetDataModel().ToDict();
        if (bmiRecords.ContainsKey(today))
        {
            _page.SetMsg($":slightly_smiling_face: [magenta2_1]You already have a record for today {today}. Come back later.[/]");
            _page.Run();
            return;
        }
        
        _page.RefreshMsg($"[yellow]Adding record for today {today}...[/]");
        
        double weight = AnsiConsole.Prompt(
            new TextPrompt<double>("Enter your [bold mediumorchid1_1]weight[/] (kg):")
                .PromptStyle(Constants.PromptInputStyle)
                .ValidationErrorMessage("[red]That's not a number.[/]")
                .Validate(w => 
                        w >= 0 ? ValidationResult.Success() 
                            : ValidationResult.Error("[red]Weight must be greater than zero.[/]")
                )
        );
        
        double height = AnsiConsole.Prompt(
            new TextPrompt<double>("Enter your [bold mediumorchid1_1]height[/] (m):")
                .PromptStyle(Constants.PromptInputStyle)
                .ValidationErrorMessage("[red]That's not a number.[/]")
                .Validate(w => 
                    w >= 0 ? ValidationResult.Success() 
                        : ValidationResult.Error("[red]Height must be greater than zero.[/]")
                )
        );

        var bmiModel = _page.GetDataModel();
        var newRecord = BMIModel.GetRecord(weight, height);
        bmiModel.AddRecord(today, newRecord);
        bmiModel.Save();
        
        _page.SetMsg($":check_mark:  [green]BMI record added successfully for {today}[/]");
        _page.Run();
    }
}