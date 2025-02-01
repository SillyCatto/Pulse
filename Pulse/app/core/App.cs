using Pulse.app.models.record;
using Pulse.app.pages;
using Pulse.models;
using Pulse.utils;
using Spectre.Console;

namespace Pulse.core;

public class App
{
    private readonly PageManager _pageManager;

    public App()
    {
        _pageManager = new PageManager();
        
        // register pages
        _pageManager.RegisterPage("Home", () => new HomePage(_pageManager));
        _pageManager.RegisterPage("BMI Calculator", () => new BMIPage(_pageManager));
        _pageManager.RegisterPage("Healthy Habit Todos", () => new HabitTodoPage());
        _pageManager.RegisterPage("Mental Health Tracker", () => new MentalHealthPage());
        _pageManager.RegisterPage("Report", () => new ReportPage());
        
    }

    public void Run()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        //Test();
        _pageManager.Navigate("Home");
    }

    public static void Exit()
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[bold green]Goodbye![/]");
        Environment.Exit(0);
    }

    private void Test()
    {
        // Dictionary<string, object> data = new Dictionary<string, object>
        // {
        //     {
        //         "user", new Dictionary<string, object>
        //         {
        //             { "name", "Alice" } 
        //             
        //         } 
        //         
        //     }
        // };
        //
        // var writer = new JSONFileWriter(FilePath.UserDataPath);
        // writer.Write(data);
        // writer.UpdateValue("user.age", 10);
        // Console.WriteLine(new JSONFileReader(FilePath.UserDataPath).Read());

        var bmiModel = new JSONModelHandler<BMIModel>(Constants.BMIDataPath);
        bmiModel.AddRecord(DateTime.Now.ToString(Constants.DateStringFormat), new BMIRecord(25));
        bmiModel.Save();
    }
}