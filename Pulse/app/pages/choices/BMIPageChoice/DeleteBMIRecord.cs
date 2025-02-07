using System.Globalization;
using Pulse.utils;
using Spectre.Console;

namespace Pulse.app.pages.choices.BMIPageChoice;

public class DeleteBMIRecord : IChoiceAdapter
{
    private readonly BMIPage _page;

    public DeleteBMIRecord(BMIPage page)
    {
        _page = page;
    }
    public void Exec()
    {
        var bmiRecords = _page.GetDataModel();
        string date = AskDateInput();
        
        if (!bmiRecords.ToDict().ContainsKey(date))
        {
            _page.SetMsg(":cross_mark: [magenta2_1]That record doesn't exist[/]");
            _page.Run();
            return;
        }

        bmiRecords.RemoveRecord(date);
        bmiRecords.Save();
        _page.SetMsg($":check_mark:  [green]Record for {date} deleted successfully![/]");
        _page.Run();
    }

    private static string AskDateInput()
    {
        return AnsiConsole.Prompt(
            new TextPrompt<string>("Enter the date of the record to [bold mediumorchid1_1]delete[/] (yyyy-MM-dd):")
                .PromptStyle(Constants.PromptInputStyle)
                .ValidationErrorMessage("[red]Wrong date format, try again: (yyyy-MM-dd)[/]")
                .Validate(input =>
                    DateTime.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _)
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]Invalid date format. Use yyyy-MM-dd.[/]"))
        );
    }
}