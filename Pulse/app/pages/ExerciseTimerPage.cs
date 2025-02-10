using Pulse.app.models;
using Pulse.app.pages.choices;
using Pulse.app.pages.choices.ExerciseTimerChoice;
using Pulse.core;
using Pulse.models;
using Pulse.utils;
using Spectre.Console;

namespace Pulse.app.pages;

public class ExerciseTimerPage : IPageAdapter
{
    private readonly PageManager _pageManager;
    private JSONModelHandler<ExerciseModel> _exerciseModel;
    private readonly ChoiceManager _choiceManager;
    private string? _msg;

    public ExerciseTimerPage(PageManager pageManager)
    {
        _pageManager = pageManager;
        _exerciseModel = new JSONModelHandler<ExerciseModel>(Constants.ExerciseDataPath);
        _choiceManager = new ChoiceManager();
        
        RegisterChoices();
    }

    private void RegisterChoices()
    {
        _choiceManager.Add("Start Timer", () => new StartTimer(this));
        _choiceManager.Add("Delete", () => new DeleteExerciseRecord(this));
        _choiceManager.Add("Back", () => new ChoiceBackToHome(_pageManager));
    }

    private void RecordTable()
    {
        var exerciseRecord = _exerciseModel.ToDict();
        if (exerciseRecord.Count == 0)
        {
            var panelText = new Panel(
                new Rows(
                    new Markup("[grey54]You don't have any exercise records yet.[/]").Centered(),
                    new Markup("[grey54]:rocket: Let's get grinding.[/]").Centered()
                ))
            {
                Expand = true,
                Border = BoxBorder.None
            };
            AnsiConsole.Write(panelText);
        }
        else
        {
            var exerciseRecordTable = new Table
            {
                Title = new TableTitle("RECORD", new Style(decoration: Decoration.Bold | Decoration.Underline)),
                Border = TableBorder.Rounded,
                Alignment = Justify.Center,
                Width = 60
            };
            
            exerciseRecordTable.AddColumns(
                new TableColumn("[green1]Index[/]").Centered(),
                new TableColumn("[green1]Exercise[/]").Centered(),
                new TableColumn("[green1]Time[/]").Centered()
            );

            foreach (var entry in exerciseRecord)
            {
                var index = entry.Key;
                var exercise = entry.Value[0];
                var time = entry.Value[1];

                exerciseRecordTable.AddRow(
                    new Markup($"[bold fuchsia]{index}[/]"),
                    new Markup($"{exercise}"),
                    new Markup($"{time}")
                );
            }
            
            AnsiConsole.Write(exerciseRecordTable);
        }
    }

    public void View()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule($"[bold {Color.Orange1}]Exercise Timer[/]")
        {
            Justification = Justify.Center,
            Style = Style.Parse($"bold {Color.Orange1}")
        });
        AnsiConsole.WriteLine();
        RecordTable();
        AnsiConsole.WriteLine();
        
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
    
    public JSONModelHandler<ExerciseModel> GetDataModel() => _exerciseModel;
}