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
        
        _page.RefreshMsg("[yellow]Deleting record...[/]");
        string date = AskDateInput();
        
        if (!bmiRecords.ToDict().ContainsKey(date))
        {
            _page.SetMsg(":cross_mark: [red]That record doesn't exist[/]");
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
            new TextPrompt<string>($"Enter the [bold mediumorchid1_1]date[/] of the record to [bold red]delete[/] ({Constants.DateStringFormat}):")
                .PromptStyle(Constants.PromptInputStyle)
                .Validate(input =>
                    DateTime.TryParseExact(input, Constants.DateStringFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _)
                        ? ValidationResult.Success()
                        : ValidationResult.Error($"[red]Wrong date format. Use {Constants.DateStringFormat}.[/]"))
        );
    }
}