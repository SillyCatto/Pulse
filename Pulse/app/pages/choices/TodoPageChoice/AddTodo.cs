using Pulse.app.models;
using Pulse.utils;
using Spectre.Console;

namespace Pulse.app.pages.choices.TodoPageChoice;

public class AddTodo : IChoiceAdapter
{
    private readonly HabitTodoPage _page;

    public AddTodo(HabitTodoPage page)
    {
        _page = page;
    }

    public void Exec()
    {
        var todoRecords = _page.GetDataModel().ToDict();
        var nextTaskIndex = todoRecords.Count + 1;
        _page.RefreshMsg($"[yellow]Adding task no. {nextTaskIndex}...[/]");
        
        string task = AnsiConsole.Prompt(
            new TextPrompt<string>("Write the [bold mediumorchid1_1]task[/] to add (within 150 characters):")
                .PromptStyle(Constants.PromptInputStyle)
                .ValidationErrorMessage("[red]Task must be within 150 characters.[/]")
                .Validate(w => 
                    w.Length <= 150 
                        ? ValidationResult.Success() 
                        : ValidationResult.Error("[red]Task must be within 150 characters.[/]")
                )
        );
        
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("Set a [bold mediumorchid1_1]status[/] for the task:");
        AnsiConsole.WriteLine();
        
        string status = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .HighlightStyle(Style.Parse("bold #00ffff"))
                .AddChoices(TodoModel.GetAllStatus())
        );
        
        SaveRecord(task, status);
        
        _page.SetMsg($":check_mark:  [green]Task no. {nextTaskIndex} added successfully.[/]");
        _page.Run();
    }
    
    private void SaveRecord(string task, string status)
    {
        var todoModel = _page.GetDataModel();
        var records = todoModel.ToDict();
        string newIndex = (records.Count + 1).ToString();
        todoModel.AddRecord(newIndex, [task, status]);
        todoModel.Save();
    }
}