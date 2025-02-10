using Pulse.app.models;
using Pulse.app.pages.choices;
using Pulse.app.pages.choices.MoodPageChoice;
using Pulse.core;
using Pulse.models;
using Pulse.utils;
using Spectre.Console;

namespace Pulse.app.pages;

public class MoodTracker : IPageAdapter
{
    private readonly PageManager _pageManager;
    private JSONModelHandler<MoodModel> _moodModel;
    private readonly ChoiceManager _choiceManager;
    private string? _msg;

    public MoodTracker(PageManager pageManager)
    {
        _pageManager = pageManager;
        _moodModel = new JSONModelHandler<MoodModel>(Constants.MoodDataPath);
        _choiceManager = new ChoiceManager();
        
        RegisterChoices();
    }

    private void RegisterChoices()
    {
        _choiceManager.Add("Add", () => new AddMoodRecord(this));
        _choiceManager.Add("Delete", () => new DeleteMoodRecord(this));
        _choiceManager.Add("Back", () => new ChoiceBackToHome(_pageManager));
    }

    private void RecordTable()
    {
        var moodRecord = _moodModel.ToDict();
        if (moodRecord.Count == 0)
        {
            var panelText = new Panel(
                new Rows(
                    new Markup("[grey54]You don't have any mood records yet.[/]").Centered(),
                    new Markup("[grey54]:rocket: Get started by adding.[/]").Centered()
                ))
            {
                Expand = true,
                Border = BoxBorder.None
            };
            AnsiConsole.Write(panelText);
        }
        else
        {
            var moodRecordTable = new Table
            {
                Title = new TableTitle("RECORD", new Style(decoration: Decoration.Bold | Decoration.Underline)),
                Border = TableBorder.Rounded,
                Alignment = Justify.Center,
                Width = 60
            };
            
            moodRecordTable.AddColumns(
                new TableColumn("[green1]Date[/]").Centered(),
                new TableColumn("[green1]Stress level[/]").Centered(),
                new TableColumn("[green1]Mood[/]").Centered()
            );

            foreach (var entry in moodRecord)
            {
                var date = entry.Key;
                var stress = entry.Value[0];
                var mood = entry.Value[1];

                moodRecordTable.AddRow(
                    new Markup($"[bold fuchsia]{date}[/]"),
                    new Markup($"{stress}"),
                    new Markup($"{mood}")
                );
            }
            
            AnsiConsole.Write(moodRecordTable);
        }
    }

    public void View()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule($"[bold {Color.Orange1}]Mood Tracker[/]")
        {
            Justification = Justify.Center,
            Style = Style.Parse($"bold {Color.Orange1}")
        });
        AnsiConsole.WriteLine();
        RecordTable();
        AnsiConsole.WriteLine();
        
        if (!string.IsNullOrEmpty(_msg))
        {
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
    
    public JSONModelHandler<MoodModel> GetDataModel() => _moodModel;
}