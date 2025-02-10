using Pulse.utils;
using Spectre.Console;

namespace Pulse.app.pages.choices.TodoPageChoice;

public class DeleteTodo : IChoiceAdapter
{
    private readonly HabitTodoPage _page;

    public DeleteTodo(HabitTodoPage page)
    {
        _page = page;
    }

    public void Exec()
    {
        var todoRecords = _page.GetDataModel();
        _page.RefreshMsg($"[yellow]Deleting task...[/]");
        
        int index = AnsiConsole.Prompt(
            new TextPrompt<int>("Enter the [bold mediumorchid1_1]index[/] of the task to [bold red]delete[/]:")
                .PromptStyle(Constants.PromptInputStyle)
                .ValidationErrorMessage("[red]That's not a number.[/]")
        );
        
        var todoDict = todoRecords.ToDict();

        if (!todoDict.ContainsKey(index.ToString()))
        {
            _page.SetMsg($":cross_mark: [red]Task no. {index} doesn't exist[/]");
            _page.Run();
            return;
        }
        
        todoDict.Remove(index.ToString());

        var updatedDict = UpdateRecordIndex(todoDict);
        todoRecords.SetData(updatedDict);
        todoRecords.Save();
        
        _page.SetMsg($":check_mark:  [green]Task no. {index} deleted successfully![/]");
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