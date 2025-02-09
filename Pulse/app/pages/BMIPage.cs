using Pulse.app.models;
using Pulse.app.pages.choices;
using Pulse.app.pages.choices.BMIPageChoice;
using Pulse.core;
using Pulse.models;
using Pulse.utils;
using Spectre.Console;

namespace Pulse.app.pages;

public class BMIPage : IPageAdapter
{
    private readonly PageManager _pageManager;
    private JSONModelHandler<BMIModel> _bmiModel;
    private readonly ChoiceManager _choiceManager;
    private string? _msg;

    public BMIPage(PageManager pageManager)
    {
        _pageManager = pageManager;
        _bmiModel = new JSONModelHandler<BMIModel>(Constants.BMIDataPath);
        _choiceManager = new ChoiceManager();
        
        RegisterChoices();
    }

    private void RegisterChoices()
    {
        _choiceManager.Add("Add", () => new AddBMIRecord(this));
        _choiceManager.Add("Delete", () => new DeleteBMIRecord(this));
        _choiceManager.Add("Back", () => new ChoiceBackToHome(_pageManager));
        _choiceManager.Add("Exit", () => new ChoiceExit());
    }

    private void RecordTable()
    {
        var bmiRecord = _bmiModel.ToDict();
        if (bmiRecord.Count == 0)
        {
            var panelText = new Panel(
                new Rows(
                    new Markup("[grey54]You don't have any bmi records yet.[/]").Centered(),
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
            var bmiRecordTable = new Table
            {
                Title = new TableTitle("RECORD", new Style(decoration: Decoration.Bold | Decoration.Underline)),
                Border = TableBorder.Rounded,
                Alignment = Justify.Center,
                Width = 60
            };
            
            bmiRecordTable.AddColumns(
                new TableColumn("[green1]Date[/]").Centered(),
                new TableColumn("[green1]BMI[/]").Centered(),
                new TableColumn("[green1]Verdict[/]").Centered()
            );

            foreach (var entry in bmiRecord)
            {
                var date = entry.Key;
                var bmi = entry.Value[0];
                var verdict = entry.Value[1];

                bmiRecordTable.AddRow(
                    new Markup($"[bold fuchsia]{date}[/]"),
                    new Markup($"{bmi}"),
                    new Markup($"{verdict}")
                );
            }
            
            AnsiConsole.Write(bmiRecordTable);
        }
    }

    public void View()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule($"[bold {Color.Orange1}]BMI Calculator[/]")
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
    
    public JSONModelHandler<BMIModel> GetDataModel() => _bmiModel;
}