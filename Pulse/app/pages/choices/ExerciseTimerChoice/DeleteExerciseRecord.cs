using Pulse.utils;
using Spectre.Console;

namespace Pulse.app.pages.choices.ExerciseTimerChoice;

public class DeleteExerciseRecord : IChoiceAdapter
{
    private readonly ExerciseTimerPage _page;

    public DeleteExerciseRecord(ExerciseTimerPage page)
    {
        _page = page;
    }
    public void Exec()
    {
        var exerciseRecords = _page.GetDataModel();
        _page.RefreshMsg($"[yellow]Deleting record...[/]");
        
        int index = AnsiConsole.Prompt(
            new TextPrompt<int>("Enter the [bold mediumorchid1_1]index[/] of the record to [bold red]delete[/]:")
                .PromptStyle(Constants.PromptInputStyle)
                .ValidationErrorMessage("[red]That's not a number.[/]")
        );
        
        var exerciseDict = exerciseRecords.ToDict();

        if (!exerciseDict.ContainsKey(index.ToString()))
        {
            _page.SetMsg($":cross_mark: [red]Record no. {index} doesn't exist[/]");
            _page.Run();
            return;
        }
        
        exerciseDict.Remove(index.ToString());

        var updatedDict = UpdateRecordIndex(exerciseDict);
        exerciseRecords.SetData(updatedDict);
        exerciseRecords.Save();
        
        _page.SetMsg($":check_mark:  [green]Record no. {index} deleted successfully![/]");
        _page.Run();
    }
    
    private Dictionary<string, List<string>> UpdateRecordIndex(Dictionary<string, List<string>> prevRecords)
    {
        var updatedRecords = new Dictionary<string, List<string>>();
        int newIndex = 1;
        foreach (var entry in prevRecords)
        {
            updatedRecords[newIndex.ToString()] = entry.Value;
            newIndex++;
        }

        return updatedRecords;
    }
}