using Pulse.app.models;
using Pulse.app.pages.choices;
using Pulse.core;
using Pulse.models;
using Pulse.utils;
using Spectre.Console;

namespace Pulse.app.pages;

public class HabitTodoPage : IPageAdapter
{
    private readonly PageManager _pageManager;
    private JSONModelHandler<TodoModel> _todoModel;
    private readonly ChoiceManager _choiceManager;
    private string? _msg;

    public HabitTodoPage(PageManager pageManager)
    {
        _pageManager = pageManager;
        _todoModel = new JSONModelHandler<TodoModel>(Constants.HabitTodoPath);
        _choiceManager = new ChoiceManager();
        
        RegisterChoices();
    }

    private void RegisterChoices()
    {
        // _choiceManager.Add("Add", () => new AddBMIRecord(this));
        // _choiceManager.Add("Update", () => new AddBMIRecord(this));
        // _choiceManager.Add("Delete", () => new DeleteBMIRecord(this));
        _choiceManager.Add("Back", () => new ChoiceBackToHome(_pageManager));
        _choiceManager.Add("Exit", () => new ChoiceExit());
    }
    
    private void RecordTable()
    {
        var todoRecords = _todoModel.ToDict();
        if (todoRecords.Count == 0)
        {
            var panelText = new Panel(
                new Rows(
                    new Markup("[grey54]You don't have any records yet.[/]").Centered(),
                    new Markup("[grey54]Get started by adding some records. :rocket:[/]").Centered()
                ))
            {
                Expand = true,
                Border = BoxBorder.None
            };
            AnsiConsole.Write(panelText);
        }
        else
        {
            var todoRecordTable = new Table
            {
                Title = new TableTitle("RECORD", new Style(decoration: Decoration.Bold | Decoration.Underline)),
                Border = TableBorder.Rounded,
                Alignment = Justify.Center
            };
            
            todoRecordTable.AddColumns(
                new TableColumn("[green1]Index[/]").Centered(),
                new TableColumn("[green1]Tasks[/]").Centered(),
                new TableColumn("[green1]Status[/]").Centered()
            );

            foreach (var entry in todoRecords)
            {
                var record = entry.Value;
                todoRecordTable.AddRow(entry.Key, record[0], record[1]);
            }
            
            AnsiConsole.Write(todoRecordTable);
        }
    }
    
    public void View()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule($"[bold {Color.Orange1}]Healthy Habit Todos[/]")
        {
            Justification = Justify.Center,
            Style = Style.Parse($"bold {Color.Orange1}")
        });
        AnsiConsole.WriteLine();
        RecordTable();
        
        if (!string.IsNullOrEmpty(_msg))
        {
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"{_msg}");
            AnsiConsole.WriteLine();
            _msg = null;
        }
    }

    public void Run()
    {
        View();
        _choiceManager.ShowAndExecute();
    }
    
    public void SetMsg(string msg)
    {
        _msg = msg;
    }

    public void RefreshMsg(string msg)
    {
        SetMsg(msg);
        View();
    }
    
    public JSONModelHandler<TodoModel> GetDataModel() => _todoModel;
}