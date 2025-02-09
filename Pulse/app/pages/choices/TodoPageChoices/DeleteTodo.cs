using Pulse.utils;
using Spectre.Console;

namespace Pulse.app.pages.choices.TodoPageChoices;

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

        if (!todoRecords.ToDict().ContainsKey(index.ToString()))
        {
            _page.SetMsg($":cross_mark: [red]Task no. {index} doesn't exist[/]");
            _page.Run();
            return;
        }
        
        todoRecords.RemoveRecord(index.ToString());
        todoRecords.Save();
        _page.SetMsg($":check_mark:  [green]Task no. {index} deleted successfully![/]");
        _page.Run();
        
    }
}