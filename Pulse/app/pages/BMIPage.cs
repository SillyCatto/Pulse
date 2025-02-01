using Pulse.app.models;
using Pulse.core;
using Pulse.models;
using Pulse.utils;
using Spectre.Console;

namespace Pulse.app.pages;

public class BMIPage : IPageAdapter
{
    private readonly PageManager _pageManager;
    private JSONModelHandler<BMIModel> _bmiModel;

    public BMIPage(PageManager pageManager)
    {
        _pageManager = pageManager;
        _bmiModel = new JSONModelHandler<BMIModel>(Constants.BMIDataPath);
    }

    private void RecordTable()
    {
        var bmiRecord = _bmiModel.ToDict();
        if (bmiRecord.Count == 0)
        {
            var panelText = new Panel(
                new Rows(
                    new Markup("[dim]You don't have any records yet.[/]").Centered()
                ))
            {
                Expand = true,
                Border = BoxBorder.None
            };
            AnsiConsole.Write(panelText);
        }
        else
        {
            var bmiRecordTable = new Table();
            bmiRecordTable.AddColumn(new TableColumn("Date").Centered());
            bmiRecordTable.AddColumn(new TableColumn("BMI").Centered());
            bmiRecordTable.AddColumn(new TableColumn("Verdict").Centered());

            foreach (var entry in bmiRecord)
            {
                var record = entry.Value;
                bmiRecordTable.AddRow(entry.Key, record[0], record[1]);
            }

            bmiRecordTable.Expand();
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
    }

    public void Run()
    {
        View();

        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .AddChoices("Back", "Exit")
        );

        if (choice.Equals("Back"))
        {
            _pageManager.Navigate("Home");
        }
        else
        {
            App.Exit();
        }
    }
    
    private static string GetVerdict(double bmi)
    {
        return bmi switch
        {
            < 18.5 => "Underweight",
            < 25.0 => "Normal weight",
            < 30.0 => "Pre-obesity",
            < 35.0 => "Obesity class I",
            < 40.0 => "Obesity class II",
            _ => "Obesity class III"
        };
    }
}