using Pulse.app.models;
using Pulse.utils;
using Spectre.Console;

namespace Pulse.app.pages.choices.TodoPageChoice;

public class UpdateTodo : IChoiceAdapter
{
    private readonly HabitTodoPage _page;

    public UpdateTodo(HabitTodoPage page)
    {
        _page = page;
    }

    public void Exec()
    {
        var todoRecords = _page.GetDataModel();
        _page.RefreshMsg($"[yellow]Updating task status...[/]");
        
        int index = AnsiConsole.Prompt(
            new TextPrompt<int>("Enter the [bold mediumorchid1_1]index[/] of the task to [bold deepskyblue1]update[/]:")
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
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"Set a [bold mediumorchid1_1]status[/] for the task {index}:");
        AnsiConsole.WriteLine();
        
        string status = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .HighlightStyle(Style.Parse("bold #00ffff"))
                .AddChoices(TodoModel.GetAllStatus())
        );
        
        var task = todoDict[index.ToString()][0];
        todoRecords.Update(index.ToString(), [task, status]);
        todoRecords.Save();
        
        _page.SetMsg($":check_mark:  [green]Task no. {index} updated successfully![/]");
        _page.Run();
    }
}